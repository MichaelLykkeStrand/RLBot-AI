using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    public class ActionNode : Node
    {
        //Signature for the action, probably a good idea to add a param
        public delegate NodeStates ActionNodeDelegate();

        private ActionNodeDelegate m_action;

        //Use delegate to do action
        public ActionNode(ActionNodeDelegate action)
        {
            m_action = action;
        }

        public override NodeStates Evaluate()
        {
            switch (m_action())
            {
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
                default:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
            }
        }
    }
}
