using Bot.Objects;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BehaviourTree.Actions
{
    class Kickoff : ActionNode
    {
        enum KickOffPosition {L,CL,C,R,CR,ERROR}
        bool isNewKickoff = true;
        bool kickOffRunning = false;
        int frameCount = 0;
        KickOffPosition kickOffPosition;



        public override State Update(Bot agent, Packet packet)
        {
            
            Physics carPhysics = packet.Players[agent.Index].Physics;
            Vector3 carLocation = carPhysics.Location;
            Vector3 ballLocation = packet.Ball.Physics.Location;
            Orientation carRotation = packet.Players[agent.Index].Physics.Rotation;

            
            //Detects if it's a new kickoff and updates the kickoff position
            if(Game.IsKickoffPause && isNewKickoff)
            {
                kickOffPosition = SetKickOffPosition(carLocation);
                Console.WriteLine("Kickoff");
                isNewKickoff = false;

                kickOffRunning = true;
                frameCount = 0;
            }
            else //If it's not a new kickoff and we not currently running a kick off return FAILURE
            {
                if(kickOffRunning == false)
                {
                    return State.FAILURE;
                }
            }
            
            if(Game.IsKickoffPause == false)
            {
                isNewKickoff = true;
            }

            Controller output = new Controller();

            
            if(kickOffRunning)
            {
                if(frameCount > 1000)
                {
                    kickOffRunning = false;
                    return State.SUCCESS;
                }
                else
                {
                    frameCount++;
                    switch (kickOffPosition)
                    {
                        case KickOffPosition.L:
                            output = KickOffL();
                            break;
                        case KickOffPosition.CL:
                            output = KickOffCL();
                            break;
                        case KickOffPosition.C:
                            output = KickOffC();
                            break;
                        case KickOffPosition.CR:
                            output = KickOffCR();
                            break;
                        case KickOffPosition.R:
                            output = kickOffR();
                            break;                      
                    }
                    
                }



            }           

            Game.OutoutControls = output;
            return State.RUNNING;

        }

        private Controller kickOffR()
        {
            Controller output = new Controller();
            output.Boost = true;
            output.Throttle = 1;
            Console.WriteLine("Position R");
            return output;
        }

        private Controller KickOffCR()
        {
            Controller output = new Controller();
            output.Boost = true;
            output.Throttle = 1;
            Console.WriteLine("Position CR");
            return output;
        }

        private Controller KickOffC()
        {
            Controller output = new Controller();
            output.Boost = true;
            output.Throttle = 1;
            Console.WriteLine("Position C");
            return output;
        }

        private Controller KickOffCL()
        {
            Controller output = new Controller();
            output.Boost = true;
            output.Throttle = 1;
            Console.WriteLine("Position CL");
            return output;
        }

        private Controller KickOffL()
        {
            Controller output = new Controller();
            output.Boost = true;
            output.Throttle = 1;
            Console.WriteLine("Position L");
            return output;
        }

        private KickOffPosition SetKickOffPosition(Vector3 carLocation)
        {
            Vector3 L = new Vector3(-2047.87f, 2559.87f, 17.01f);
            Vector3 CL = new Vector3(-256, 3839.82f, 17.01f);
            Vector3 C = new Vector3(0, 4607.82f, 17.01f);
            Vector3 CR = new Vector3(256, 3839.82f, 17.01f);
            Vector3 R = new Vector3(2048, 2560, 17.01f);

            if(ApproximatelyEquals(carLocation,L,100))
            {
                return KickOffPosition.L;
            }
            if (ApproximatelyEquals(carLocation, CL, 100))
            {
                return KickOffPosition.CL;
            }
            if (ApproximatelyEquals(carLocation, C, 100))
            {
                return KickOffPosition.C;
            }
            if (ApproximatelyEquals(carLocation, CR, 100))
            {
                return KickOffPosition.CR;
            }
            if (ApproximatelyEquals(carLocation, R, 100))
            {
                return KickOffPosition.R;
            }

            return KickOffPosition.ERROR;

        }

        static bool ApproximatelyEquals(Vector3 vector1, Vector3 vector2, double acceptableDifference)
        {
            return (vector2 - vector1).Length() < acceptableDifference;
        }
    }
}
