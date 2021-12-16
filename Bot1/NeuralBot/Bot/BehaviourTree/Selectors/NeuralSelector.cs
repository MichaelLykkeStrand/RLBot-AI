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
        private PrioritySelector _internalSelector;

        public NeuralSelector(List<Node> nodes) : base(nodes) 
        {
            _internalSelector = new PrioritySelector(Nodes);
        }

        public override State Update(Bot agent, Packet packet)
        {
            AISortNodes(agent, packet);
            _internalSelector.Nodes = Nodes;
            return _internalSelector.Update(agent, packet);
        }

        private void AISortNodes (Bot agent, Packet packet)
        {
            // TODO: Implement AI thingies.
        }

        public void ForceFirstNode (Node node)
        {
            Nodes.Remove(node);
            Nodes.Insert(0, node);
        }
    }
}
