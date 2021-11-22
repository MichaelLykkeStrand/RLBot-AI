using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    public abstract class Selector : Node
    {
        //Child nodes
        protected List<Node> m_nodes = new List<Node>();

        public Selector()
        {
        }

        public Selector(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        public override void Deserialize(JObject source)
        {
            m_nodes = source["Children"].Select(x => Serialization.DeserializeObject<Node>((JObject)x)).ToList();
        }

        public override JObject Serialize()
        {
            return new JObject()
            {
                { "Children", new JArray(m_nodes.Select(x => Serialization.SerializeObject(x)).ToArray()) }
            };
        }
    }
}
