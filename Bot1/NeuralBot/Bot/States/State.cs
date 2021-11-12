using Bot.BehaviourTree;
using static Bot.BehaviourTree.ActionNode;

namespace Bot.States
{
    public abstract class State
    {
        protected Node m_node;
        public Node Node { get => m_node; }
        protected ActionNodeDelegate m_delegate;

        public State()
        {
            m_delegate = new ActionNodeDelegate(Evaluate);
            m_node = new ActionNode(m_delegate);
        }
        public abstract NodeStates Evaluate();
    }
}
