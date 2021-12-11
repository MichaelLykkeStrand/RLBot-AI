using Bot.AnalysisUtils;
using Bot.MathUtils;
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
    public class Dribble : ActionNode
    {
        const float correctionDiff = 0.5f;
        const float successDistance = 1000f;
        const float speedDistance = 500f;

        public override State Update(Bot agent, Packet packet)
        {
            Physics carPhysics = packet.Players[agent.Index].Physics;
            Vector3 ballLocation = Objects.Ball.Location;
            Vector3 carLocation = carPhysics.Location;
            Orientation carRotation = packet.Players[agent.Index].Physics.Rotation;
            Vector3 ballRelativeLocation = Orientation.RelativeLocation(carLocation, ballLocation, carRotation);
            Vector3 oponentGoal = Field.GetOpponentGoal(agent);

            var distance = Vector3.Distance(carLocation, ballLocation);

            if (distance < successDistance) //TODO check if ball is near ground
            {
                Controller closeControls = Game.OutoutControls;
                if (ballRelativeLocation.Y > correctionDiff)
                {
                    closeControls.Steer = 1;
                }  
                else if (ballRelativeLocation.Y < -correctionDiff)
                {
                    closeControls.Steer = -1;
                }
                else
                {
                    closeControls.Boost = true;
                }

                var prediction = BallSimulation.FindSliceWhereBallIsGrounded(Objects.Ball.Prediction,0.5f);
                var nextGround = prediction?.Physics.Location;
                if (nextGround != null || ballLocation.Z < 120)
                {
                    closeControls.Throttle = 1;
                    closeControls.Boost = false;
                }
                else
                {
                    float throttle = distance.Remap(0, successDistance, 0, 1);
                    closeControls.Throttle = throttle;
                    
                }
                
                Game.OutoutControls = closeControls;
                _state = State.SUCCESS;
                return _state;
            }
            _state = State.FAILURE;
            return _state;

        }
    }
}
