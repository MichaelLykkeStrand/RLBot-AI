using Bot.Utilities.Processed.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree.Selectors
{
    public class NeuralSelector : Selector
    {
        public NeuralSelector(List<Node> nodes) : base(nodes) { }

        public override State Update(Bot agent, Packet packet)
        {
            throw new NotImplementedException();
        }
    }
}
