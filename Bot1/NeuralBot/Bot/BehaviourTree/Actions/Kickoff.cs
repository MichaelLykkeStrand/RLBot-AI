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



        public override State Update(Bot agent, Packet packet)
        {
            
            Physics carPhysics = packet.Players[agent.Index].Physics;
            Vector3 carLocation = carPhysics.Location;
            Vector3 ballLocation = packet.Ball.Physics.Location;
            Orientation carRotation = packet.Players[agent.Index].Physics.Rotation;

            KickOffPosition kickOffPosition;

            if(Game.IsKickoffPause && isNewKickoff)
            {
                kickOffPosition = SetKickOffPosition(carLocation);
                Console.WriteLine("Kickoff");
                isNewKickoff = false;

                if(kickOffPosition == KickOffPosition.L)
                {
                    Console.WriteLine("Position L");
                }
                if (kickOffPosition == KickOffPosition.CL)
                {
                    Console.WriteLine("Position CL");
                }
                if (kickOffPosition == KickOffPosition.C)
                {
                    Console.WriteLine("Position C");
                }
                if (kickOffPosition == KickOffPosition.CR)
                {
                    Console.WriteLine("Position CR");
                }
                if (kickOffPosition == KickOffPosition.R)
                {
                    Console.WriteLine("Position R");
                }




            }
            
            if(Game.IsKickoffPause == false)
            {
                isNewKickoff = true;
            }




            Controller output = new Controller();
            output.Boost = true;
            output.Throttle = 1;

            Game.OutoutControls = output;
            return State.RUNNING;

        }

        private KickOffPosition SetKickOffPosition(Vector3 carLocation)
        {
            Vector3 L = new Vector3(-2047.87f, 2559.87f, 17.01f);
            Vector3 CL = new Vector3(-256, 3839.82f, 17.01f);
            Vector3 C = new Vector3(0, 4607.82f, 17.01f);
            Vector3 CR = new Vector3(256, 3839.82f, 17.01f);
            Vector3 R = new Vector3(2048, 2560, 17.01f);

            if(carLocation == L)
            {
                return KickOffPosition.L;
            }
            if (carLocation == CL)
            {
                return KickOffPosition.CL;
            }
            if (carLocation == C)
            {
                return KickOffPosition.C;
            }
            if (carLocation == CR)
            {
                return KickOffPosition.CR;
            }
            if (carLocation == R)
            {
                return KickOffPosition.R;
            }

            return KickOffPosition.ERROR;

        }
    }
}
