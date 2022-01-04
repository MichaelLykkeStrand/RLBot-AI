using Bot.ANN;
using Bot.BehaviourTree;
using Bot.BehaviourTree.Selectors;
using Bot.Objects;
using Bot.Scenario;
using Bot.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Bot.UI
{
    public partial class BotTrainerForm : Form
    {
        private const int TRAINER_EPOCHS = 1000;

        private const string REL_STORE_PATH = "/datasets/";
        
        private Bot _bot;
        private ScenarioDataCollector _collector;

        public BotTrainerForm(Bot bot) //Replace bot reference with ScenarioController reference
        {
            _bot = bot;
            InitializeComponent();
            bot.scenarioController.OnNewScenarioReady += ScenarioController_OnNewScenarioReady;
            _bot.scenarioController.Generate();
            InitializeStateButtons();

            _collector = new ScenarioDataCollector(_bot);
        }

        private NeuralSelector GetNeuralSelector ()
        {
            return (NeuralSelector)_bot.RootNode.RecursiveFindNode(x => x.GetType() == typeof (NeuralSelector));
        }

        private PrioritySelector GetPrioritySelector()
        {
            return (PrioritySelector)_bot.RootNode.RecursiveFindNode(x => x.GetType() == typeof(PrioritySelector));
        }


        private void InitializeStateButtons()
        {
            Selector selector = GetNeuralSelector();
            if(selector == null) selector = GetPrioritySelector();
            foreach (var child in selector.Nodes)
            {
                RadioButton rb = new RadioButton();
                rb.Text = child.GetType().Name;
                statePanel.Controls.Add(rb);
            }
        }

        private void ScenarioController_OnNewScenarioReady(object sender, EventArgs e)
        {
            RadioButton button = statePanel.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);
            if (button != null && button.Checked)
            {
                button.Checked = false;
            }
        }


        private void SubmitButton_Click(object sender, EventArgs e)
        {
            var buttons = statePanel.Controls.OfType<RadioButton>();
            Game.tickPacket.Players(_bot.Index);
            _collector.Collect(buttons.Select(x => x.Checked ? 1d : 0d).ToArray());
            _bot.scenarioController.Generate();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            RadioButton[] buttons = statePanel.Controls.OfType<RadioButton>().ToArray();
            NeuralSelector selector = GetNeuralSelector();
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Checked)
                {
                    selector.ForceFirstNode(selector.Nodes[i]);
                }
            }

            _bot.scenarioController.PlayScenario();
        }

        private string GetBasePath() => Path.Combine(Directory.GetCurrentDirectory(), REL_STORE_PATH);
        private string GetStorePath() => GetBasePath() + Guid.NewGuid() + ".json";

        private void button1_Click(object sender, EventArgs e) // deal with it
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = GetBasePath();
                dialog.DefaultExt = "json";
                dialog.FileOk += (s, cea) =>
                {
                    var datasets = _collector.ToDataSets();
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), REL_STORE_PATH));
                    NeuralNetworkIO.StoreDataSets(datasets, GetStorePath());
                    _collector.Reset();
                };

                dialog.ShowDialog();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message + ", " + exc.StackTrace);
            }
        }

        private void TrainAI_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = GetBasePath();
            dialog.DefaultExt = "json";
            dialog.FileOk += (s, cea) =>
            {
                IEnumerable<DataSet> dataSets = dialog.FileNames.SelectMany(x => NeuralNetworkIO.LoadDataSets(x));
                Trainer trainer = new Trainer(dataSets, TRAINER_EPOCHS);
                trainer.TrainAFucker();
                ANN.ANN ann = trainer.NeuralNetwork;

                SaveFileDialog svd = new SaveFileDialog();
                svd.InitialDirectory = GetBasePath();
                svd.DefaultExt = "bin";

                svd.FileOk += (s2, cea2) =>
                {
                    NeuralNetworkIO.StoreBrain(ann, svd.FileName);
                    svd.Dispose();
                };

                svd.ShowDialog();
                dialog.Dispose();
            };

            dialog.ShowDialog();
        }

        private void LoadAI_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = GetBasePath();
            dialog.DefaultExt = "bin";
            dialog.FileOk += (s, cea) =>
            {
                GetNeuralSelector().NeuralNetwork = NeuralNetworkIO.LoadBrain(dialog.FileName);
                dialog.Dispose();
            };
            dialog.ShowDialog();
        }
    }
}
