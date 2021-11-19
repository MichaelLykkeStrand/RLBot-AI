using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot.UI
{
    public partial class BotTrainerForm : Form
    {
        private Bot _bot;
        public BotTrainerForm(Bot bot)
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
                RadioButton rb = new RadioButton();
                rb.Text = node.GetType().Name;
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
            RadioButton button = statePanel.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);
            if(button != null)
            {
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
