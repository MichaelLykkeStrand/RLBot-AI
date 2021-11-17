using RLBotDotNet.GameState;
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
        public event EventHandler OnPlayScenario;
        public GameState currentState;
        private Bot _bot;
        Random random = new Random();
        public ScenarioController(Bot bot)
        {
            this._bot = bot;
        }

        public void Generate()
        {
            //Do scenario generation
            GameState gameState = new GameState();
            CarState carState = new CarState
            {
                Boost = (float)random.NextDouble() * 100,
                PhysicsState = new PhysicsState(
                    location: new DesiredVector3((float)random.NextDouble()*2000, (float)random.NextDouble()*2000, 0),
                    velocity: new DesiredVector3(100, null, 100),
                    rotation: new DesiredRotator(0, (float)Math.PI / 2, 0),
                    angularVelocity: new DesiredVector3(0, 0, 0)
                )
            };

            BallState ballState = new BallState
            {
                PhysicsState = new PhysicsState(
                    location: new DesiredVector3((float)random.NextDouble() * 2000, (float)random.NextDouble() * 2000, 0),
                    velocity: new DesiredVector3(null, null, 500),
                    rotation: new DesiredRotator((float)Math.PI / 2, 0, 0),
                    angularVelocity: new DesiredVector3(0, 0, 0)
                )
            };
            gameState.SetCarState(_bot.Index,carState);

            gameState.GameInfoState.Paused = true;

            currentState = gameState;

            OnNewScenarioReady?.Invoke(this, EventArgs.Empty);
        }

        public void PlayScenario()
        {
            OnPlayScenario?.Invoke(this, EventArgs.Empty);
        }
    }
}
