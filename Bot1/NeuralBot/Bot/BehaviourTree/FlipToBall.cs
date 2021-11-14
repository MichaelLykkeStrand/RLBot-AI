using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System.Numerics;

namespace Bot.BehaviourTree
{
    public class FlipToBall : ActionNode
    {
        public override NodeResult Update(Bot agent, Packet packet, ref Controller output)
        {
            Physics carPhysics = packet.Players[agent.Index].Physics;
            Vector3 carLocation = carPhysics.Location;
            Vector3 ballLocation = packet.Ball.Physics.Location;
            if (packet.Players[agent.Index].Boost != 0)
            {
                output.Boost = true;
                NodeResult jmpResult = new NodeResult
                {
                    controller = output,
                    nodeState = State.SUCCESS
                };
                return jmpResult;
            }

            NodeResult tmpResult = new NodeResult
            {
                controller = output,
                nodeState = State.FAILURE
            };

            tmpResult.controller.Boost = false;
            return tmpResult;
        }
    }
}