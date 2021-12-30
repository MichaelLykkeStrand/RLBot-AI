using Bot.MathUtils;
using Bot.Objects;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree.Actions
{
    public class Wait : ActionNode
    {
        private const float DISTANCE = 512;
        private const float DEVIATION = 20;

        public override State Update(Bot agent, Packet packet)
        {
            Vector3 myGoal = Field.GetMyGoal(agent);
            Player me = packet.Players[agent.Index];

            float targetY = Remapper.Lerp(myGoal.Y, packet.Ball.Physics.Location.Y, 0.5f);
            float targetX = Remapper.Lerp(myGoal.X, packet.Ball.Physics.Location.X, 0.5f);
            Vector3 target = new Vector3(targetX, targetY, 0f);

            Controller controller = Game.OutoutControls;
            float distance = Vector3.Distance(me.Physics.Location, target);

            controller.Throttle = Math.Min(distance / DISTANCE, 1f);
            Console.WriteLine(controller.Throttle);

            var relativeLocation = Orientation.RelativeLocation(me.Physics.Location, target, me.Physics.Rotation);
            agent.Renderer.DrawLine3D(System.Drawing.Color.BlueViolet, me.Physics.Location, target);

            if (relativeLocation.Y > DEVIATION)
            {
                controller.Steer = 1;
            }
            else if (relativeLocation.Y < -DEVIATION)
            {
                controller.Steer = -1;
            }

            controller.Boost = false;
            controller.Jump = false;

            Game.OutoutControls = controller;

            float distanceToMyGoal = Vector3.Distance(packet.Ball.Physics.Location, myGoal);
            if (distanceToMyGoal < Field.Length / 2f)
            {
                _state = State.FAILURE;
            }
            else
            {
                _state = State.RUNNING;
            }
            return _state;
        }
    }
}
