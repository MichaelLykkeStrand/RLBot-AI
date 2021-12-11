using Bot.Objects;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree.Actions
{
    public class Recover : ActionNode
    {
        public override State Update(Bot agent, Packet packet)
        {
            Player player = packet.Players[agent.Index];
            Physics carPhysics = player.Physics;
            if (!player.HasWheelContact)
            {
                //Use old controller and overwrite the rotation outputs.
                Controller controller = Game.OutoutControls;
                if (carPhysics.Rotation.Roll < 0.1)
                    controller.Roll = 1;
                else if (carPhysics.Rotation.Roll > 0.1)
                    controller.Roll = -1;

                if (carPhysics.Rotation.Pitch < 0.1)
                    controller.Pitch = 0.5f;
                else if (carPhysics.Rotation.Pitch > 0.1)
                    controller.Pitch = -0.5f;

                Game.OutoutControls = controller;
                this._state = State.SUCCESS;
                return _state;
            }
            _state = State.FAILURE;
            return _state;
            
        }
    }
}
