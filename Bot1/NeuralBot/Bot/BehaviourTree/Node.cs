using Bot.Utilities.Processed.Packet;
using Newtonsoft.Json.Linq;
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
    public abstract class Node : ISerializable
    {
        protected State _state;

        public State State
        {
            get { return _state; }
        }

        public abstract State Update(Bot agent, Packet packet);

        public virtual void Deserialize(JObject source)
        {
        }

        public virtual JObject Serialize()
        {
            return new JObject();
        }
    }
}
