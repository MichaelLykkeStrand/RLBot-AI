using Bot.Objects;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System.Numerics;

namespace Bot.BehaviourTree
{
    public class FlipToBall : ActionNode
    {
        public override State Update(Bot agent, Packet packet)
        {
            Physics carPhysics = packet.Players[agent.Index].Physics;
            Vector3 carLocation = carPhysics.Location;
            Vector3 ballLocation = packet.Ball.Physics.Location;

            Controller output = new Controller();
            if (packet.Players[agent.Index].Boost != 0)
            {
                output.Boost = true;
                Game.OutoutControls = output;
                return State.SUCCESS;
            }

            output.Boost = false;
            Game.OutoutControls = output;
            return State.FAILURE;
        }
    }
}