﻿using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    class PrioritySelector : Selector
    {

        public PrioritySelector(List<Node> nodes) : base(nodes) { }

        //Evaluate state of child nodes
        public override State Update(Bot agent, Packet packet)
        {
            foreach (Node node in Nodes)
            {
                switch (node.Update(agent, packet))
                {
                    case State.FAILURE:
                        continue;
                    case State.SUCCESS:
                        _state = State.SUCCESS;
                        return _state;
                    case State.RUNNING:
                        _state = State.RUNNING;
                        return _state;
                    default:
                        continue;
                }
            }
            _state = State.FAILURE;
            return _state;
        }
    }
}
