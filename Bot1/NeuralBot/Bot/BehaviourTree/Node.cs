using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    [Serializable]
    public abstract class Node
    {
        public delegate NodeStates NodeReturn();

        protected NodeStates m_nodeState;

        public NodeStates nodeState
        {
            get { return m_nodeState; }
        }

        public abstract NodeStates Evaluate();
    }
}
