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
            bot.scenarioController.OnNewScenarioReady += ScenarioController_OnNewScenarioReady;
            InitializeComponent();
        }

        private void ScenarioController_OnNewScenarioReady(object sender, EventArgs e)
        {
            foreach (var node in _bot.Nodes)
            {
                RadioButton rb = new RadioButton();
                rb.Text = node.GetType().Name;
                groupBoxStates.Controls.Add(rb);
            }
        }

    }
}
