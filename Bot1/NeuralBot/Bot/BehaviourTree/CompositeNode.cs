using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        private List<Node> nodes = new List<Node>();

        public CompositeNode()
        {
        }

        public CompositeNode(List<Node> nodes)
        {
            Nodes = nodes;
        }

        public List<Node> Nodes { get => nodes; set => nodes = value; }

        public override void Deserialize(JObject source)
        {
            Nodes = source["Children"].Select(x => Serialization.DeserializeObject<Node>((JObject)x)).ToList();
        }

        public override JObject Serialize()
        {
            return new JObject()
            {
                { "Children", new JArray(Nodes.Select(x => Serialization.SerializeObject(x)).ToArray()) }
            };
        }
    }
}
