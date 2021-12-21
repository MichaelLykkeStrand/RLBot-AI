using Bot.Objects;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Numerics;

namespace Bot.BehaviourTree.Actions
{
    class Kickoff : ActionNode
    {
        enum KickOffPosition { L, CL, C, R, CR, ERROR }
        int frameCount = 0;

        public override State Update(Bot agent, Packet packet)
        {
            Physics carPhysics = packet.Players[agent.Index].Physics;
            Vector3 carLocation = carPhysics.Location;
            Vector3 ballLocation = packet.Ball.Physics.Location;
            Orientation carRotation = packet.Players[agent.Index].Physics.Rotation;

            Controller output = new Controller();

            //Simple universal kickoff
            if (Game.IsKickoffPause)
            {
                Vector3 target = ballLocation + new Vector3(0, 400 * (float)(agent.Team - 0.5), 0);
                Vector3 targetRelativeLocation = Orientation.RelativeLocation(carLocation, target, carRotation);
                float correctionDiff = 0.5f;

                if (targetRelativeLocation.Length() > 650)
                {
                    output.Throttle = 1;
                    output.Boost = true;
                    if (targetRelativeLocation.Y > correctionDiff)
                    {
                        output.Steer = 0.1f;
                    }
                    else if (targetRelativeLocation.Y < -correctionDiff)
                    {
                        output.Steer = -0.1f;
                    }
                }
                else
                {
                    Vector3 ballRelativeLocation = Orientation.RelativeLocation(carLocation, ballLocation, carRotation);
                    frameCount++;
                    output.Throttle = 1;
                    if (frameCount < 5)
                    {
                        if (ballRelativeLocation.Y > 0)
                        {
                            output.Steer = 1;
                        }
                        else
                        {
                            output.Steer = -1;
                        }
                        output.Jump = true;
                        output.Pitch = -1;
                    }
                    else if (frameCount < 25)
                    {
                        if (ballRelativeLocation.Y > 0)
                        {
                            output.Steer = 1;
                        }
                        else
                        {
                            output.Steer = -1;
                        }
                        output.Pitch = -1;
                        output.Jump = false;
                    }
                    else if (frameCount < 30)
                    {
                        if (ballRelativeLocation.Y > 0)
                        {
                            output.Steer = 1;
                        }
                        else
                        {
                            output.Steer = -1;
                        }
                        output.Pitch = -1;
                        output.Jump = true;
                    }
                    else if (frameCount < 40)
                    {
                        frameCount = 0;
                        return State.SUCCESS;
                    }

                }
            }
            else
            {
                return State.FAILURE;
            }
            Game.OutoutControls = output;
            return State.RUNNING;

        }
    }
}
