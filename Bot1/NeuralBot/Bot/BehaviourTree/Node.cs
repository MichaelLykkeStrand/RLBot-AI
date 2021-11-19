using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;


namespace Bot.BehaviourTree
{
    public enum State
    {
        FAILURE,
        SUCCESS,
        RUNNING
    }

    [Serializable]
    public abstract class Node
    {
        protected State _state;

        public State State
        {
            get { return _state; }
        }

        public abstract State Update(Bot agent, Packet packet);

    }
}
