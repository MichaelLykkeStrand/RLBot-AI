using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    class PrioritySelector : Selector
    {
        public PrioritySelector() : base() { }
        public PrioritySelector(List<Node> nodes) : base(nodes) { }

        //Evaluate state of child nodes
        public override NodeResult Update(Bot agent, Packet packet,ref Controller output)
        {
            foreach (Node node in m_nodes)
            {
                NodeResult nodeUpdate = node.Update(agent, packet, ref output);
                switch (nodeUpdate.nodeState)
                {
                    case State.FAILURE:
                        continue;
                    case State.SUCCESS:
                        _state = State.SUCCESS;
                        return nodeUpdate;
                    case State.RUNNING:
                        _state = State.RUNNING;
                        return nodeUpdate;
                    default:
                        continue;
                }
            }
            _state = State.FAILURE;
            var tmpResult = new NodeResult
            {
                nodeState = State.FAILURE
            };
            return tmpResult;
        }
    }
}
