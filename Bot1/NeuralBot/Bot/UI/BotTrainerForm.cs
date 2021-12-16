using Bot.BehaviourTree.Selectors;
using Bot.Objects;
using Bot.Scenario;
using Bot.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Bot.UI
{
    public partial class BotTrainerForm : Form
    {
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


        private void InitializeStateButtons()
        {
            NeuralSelector neuralSelector = GetNeuralSelector();
            foreach (var child in neuralSelector.Nodes)
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

        private string GetStorePath() => Path.Combine(Directory.GetCurrentDirectory(), REL_STORE_PATH) + Guid.NewGuid() + ".json";

        private void button1_Click(object sender, EventArgs e) // deal with it
        {
            try
            {
                var datasets = _collector.ToDataSets();
                NeuralNetworkIO.StoreDataSets(datasets, GetStorePath());
                _collector.Reset();
            }catch (Exception exc)
            {
                Console.WriteLine(exc.Message + ", " + exc.StackTrace);
            }
        }
    }
}
