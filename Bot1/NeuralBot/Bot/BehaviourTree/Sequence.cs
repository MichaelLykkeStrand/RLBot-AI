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

        public override NodeStates Evaluate()
        {
            bool hasRunningChild = false;
            foreach (var node in m_nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.FAILURE:
                        m_nodeState = NodeStates.FAILURE;
                        return m_nodeState;
                    case NodeStates.SUCCESS:
                        continue;
                    case NodeStates.RUNNING:
                        hasRunningChild = true;
                        continue;
                    default:
                        m_nodeState = NodeStates.SUCCESS;
                        return m_nodeState;
                }
            }
            m_nodeState = hasRunningChild ? NodeStates.RUNNING : NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
}
