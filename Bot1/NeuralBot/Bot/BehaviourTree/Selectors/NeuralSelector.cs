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
        private List<Node> _baseNodes;

        public ANN.ANN NeuralNetwork { get; set; }

        public NeuralSelector(List<Node> nodes) : base(nodes) 
        {
            _baseNodes = nodes;

            _internalSelector = new PrioritySelector(Nodes);
        }

        public override State Update(Bot agent, Packet packet)
        {
            AISortNodes(agent, packet);
            _internalSelector.Nodes = Nodes; // Might not be neccesary but not taking chances.
            return _internalSelector.Update(agent, packet);
        }

        private void AISortNodes (Bot agent, Packet packet)
        {
            if (NeuralNetwork != null)
            {
                int ourIndex = agent.Index;
                int enemyIndex = ourIndex == 0 ? 1 : 0;

                Player player = packet.Players[ourIndex];
                Player enemy = packet.Players[enemyIndex];
                Ball ball = packet.Ball;

                double[] values = new double[] {
                player.Physics.Location.X,
                player.Physics.Location.Y,
                player.Physics.Location.Z,
                enemy.Physics.Location.X,
                enemy.Physics.Location.Y,
                enemy.Physics.Location.Z,
                ball.Physics.Location.X,
                ball.Physics.Location.Y,
                ball.Physics.Location.Z
                };

                double[] results = NeuralNetwork.Compute(values);
                Node[] nodes = _baseNodes.ToArray();

                Array.Sort(results, nodes);
                Nodes = nodes.ToList(); // bunch of copying but should be far from enough for a performance impact
            }
        }

        public void ForceFirstNode (Node node)
        {
            Nodes.Remove(node);
            Nodes.Insert(0, node);
        }
    }
}
