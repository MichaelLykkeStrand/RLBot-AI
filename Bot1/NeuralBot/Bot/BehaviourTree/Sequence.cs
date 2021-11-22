﻿using Bot.Utilities.Processed.Packet;
using Newtonsoft.Json.Linq;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree
{
    class Sequence : CompositeNode
    {
        public Sequence() : base () { }
        public Sequence(List<Node> nodes) : base (nodes) { }

        public override NodeResult Update(Bot agent, Packet packet,ref Controller output)
        {
            Controller tmpOutput = output;
            bool hasRunningChild = false;
            foreach (var node in m_nodes)
            {
                NodeResult nodeUpdate = node.Update(agent, packet, ref output);
                switch (nodeUpdate.nodeState)
                {
                    case State.FAILURE:
                        _state = State.FAILURE;
                        return nodeUpdate;
                    case State.SUCCESS:
                        tmpOutput = nodeUpdate.controller;
                        continue;
                    case State.RUNNING:
                        hasRunningChild = true;
                        tmpOutput = nodeUpdate.controller;
                        continue;
                    default:
                        _state = State.SUCCESS;
                        return nodeUpdate;
                }
            }
            _state = hasRunningChild ? State.RUNNING : State.SUCCESS;
            var tmpResult = new NodeResult
            {
                nodeState = _state,
                controller = tmpOutput
            };
            return tmpResult;
        }
    }
}
