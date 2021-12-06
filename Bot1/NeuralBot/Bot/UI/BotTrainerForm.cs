using Bot.BehaviourTree.Selectors;
using Bot.Objects;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Bot.UI
{
    public partial class BotTrainerForm : Form
    {
        private Bot _bot;
        public BotTrainerForm(Bot bot) //Replace bot reference with ScenarioController reference
        {
            _bot = bot;
            InitializeComponent();
            bot.scenarioController.OnNewScenarioReady += ScenarioController_OnNewScenarioReady;
            _bot.scenarioController.Generate();
            InitializeStateButtons();
        }

        private void InitializeStateButtons()
        {
            foreach (var node in _bot.Nodes)
            {
                if(node.GetType() == typeof(NeuralSelector))
                {
                    NeuralSelector neuralSelector = (NeuralSelector)node;
                    foreach (var child in neuralSelector.Nodes)
                    {
                        RadioButton rb = new RadioButton();
                        rb.Text = child.GetType().Name;
                        statePanel.Controls.Add(rb);
                    }
                }
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
            RadioButton button = statePanel.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);
            if(button != null)
            {
                Game.tickPacket.Players(_bot.Index);
                //Save dataset
                _bot.scenarioController.Generate();
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            _bot.scenarioController.PlayScenario();
        }
    }
}
