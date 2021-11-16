using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scenario
{
    public class ScenarioController
    {
        public event EventHandler OnNewScenarioReady;
        public ScenarioController()
        {

        }

        public void Generate()
        {
            OnNewScenarioReady?.Invoke(this, EventArgs.Empty);
        }
    }
}
