using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    public abstract class Selector : CompositeNode
    {
        public Selector() : base() { }
        public Selector(List<Node> nodes) : base(nodes) { }
    }
}
