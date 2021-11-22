using Bot.Objects;
using Bot.Utilities.Processed.FieldInfo;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree.Actions
{
    public class CollectBoost : ActionNode
    {
        const float deviation = 50f;
        const float maxDeviationAngle = 0.5f;
        const float maxDistance = 2000f;
        public override State Update(Bot agent, Packet packet)
        {
            Player player = packet.Players[agent.Index];
            Physics carPhysics = player.Physics;
            Vector3 carLocation = carPhysics.Location;
            Orientation carRotation = packet.Players[agent.Index].Physics.Rotation;
            FieldInfo field = agent.GetFieldInfo();
            if(player.Boost < 30  && player.HasWheelContact)
            {
                foreach (var boostPad in field.BoostPads)
                {
                    float padDistance = Vector3.Distance(carLocation, boostPad.Location);
                    float ballDistance = Vector3.Distance(carLocation, Objects.Ball.Location);
                    if (boostPad.IsFullBoost && padDistance < maxDistance && padDistance < ballDistance) 
                    {
                        Controller controls = Game.OutoutControls;
                        var relativeLocation = Orientation.RelativeLocation(carLocation, boostPad.Location, carRotation);
                        if (relativeLocation.Y > deviation)
                        {
                            controls.Steer = 1;
                        } else if (relativeLocation.Y < -deviation)
                        {
                            controls.Steer = -1;
                        }
                        agent.Renderer.DrawString2D("Y: " + relativeLocation.Y, Color.Yellow,new Vector2(20,100), 1, 1);
                        Game.OutoutControls = controls;
                        return State.SUCCESS;
                    }
                }
            }
            return State.FAILURE;
        }
    }
}
