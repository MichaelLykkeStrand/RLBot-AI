using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    class Sequence : Node
    {
        private List<Node> m_nodes = new List<Node>();
        public Sequence(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        public override State Update(Bot agent, Packet packet,ref Controller output)
        {
            bool hasRunningChild = false;
            foreach (var node in m_nodes)
            {
                switch (node.Update(agent,packet,ref output))
                {
                    case State.FAILURE:
                        _state = State.FAILURE;
                        return _state;
                    case State.SUCCESS:
                        continue;
                    case State.RUNNING:
                        hasRunningChild = true;
                        continue;
                    default:
                        _state = State.SUCCESS;
                        return _state;
                }
            }
            _state = hasRunningChild ? State.RUNNING : State.SUCCESS;
            return _state;
        }
    }
}
