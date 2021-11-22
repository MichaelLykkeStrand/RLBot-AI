using Bot.Objects;
using RLBotDotNet;
using RLBotDotNet.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scenario
{
    public static class ScenarioUtils
    {
        private static readonly Random random = new Random();
        public static CarState GetRandomCarState()
        {
            float rotation = random.Next(0, (int)Math.PI * 10000) / 10000;
            CarState carState = new CarState
            {
                PhysicsState = new PhysicsState(
                location: new DesiredVector3(Field.GetRandomPosition()),
                velocity: new DesiredVector3(100, null, 100),
                rotation: new DesiredRotator(0, rotation, 0),
                angularVelocity: new DesiredVector3(0, 0, 0)
    )
            };
            return carState;
        }
    }
}
