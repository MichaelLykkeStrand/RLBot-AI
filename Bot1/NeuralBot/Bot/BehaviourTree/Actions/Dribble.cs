using Bot.AnalysisUtils;
using Bot.MathUtils;
using Bot.Objects;
using Bot.Utilities.Processed.BallPrediction;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using rlbot.flat;
using Orientation = Bot.Utilities.Processed.Packet.Orientation;
using Physics = Bot.Utilities.Processed.Packet.Physics;
using Vector3 = System.Numerics.Vector3;

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
            /*
            PredictionSlice? futureBallPrediciton = BallSimulation.FindSliceAtTime(agent.GetBallPrediction(), Game.Time+4f);
            PredictionSlice futureBall;
            if(futureBallPrediciton != null)
            {
                futureBall =  (PredictionSlice)futureBallPrediciton;
                if(futureBall.Physics.Location.Z > 0)
                {
                    _state = State.FAILURE;
                    return _state;
                }
            }
            if (BallSimulation.PredictFutureGoal(agent.GetBallPrediction()) != null)
            {
                _state = State.FAILURE;
                return _state;
            }
            */
            
            var distance = Vector3.Distance(carLocation, ballLocation);

            var targetGoalLeftPos = (agent.Team == 0) ? new Vector3(800, 5213, 321.3875f) : new Vector3(800, -5213, 321.3875f);
            var targetGoalRightPos = (agent.Team == 0) ? new Vector3(-800, 5213, 321.3875f) : new Vector3(-800, -5213, 321.3875f);
            
            if (distance < successDistance) //TODO check if ball is near ground
            {
                Controller closeControls = new Controller();
                //AIM DRIBBLE IMPLEMENTATION START
                var carToBall = Vector3.Subtract(ballLocation, carLocation);
                var carToBallDirection = Vector3.Normalize(carToBall);
                var ballToLeftTargetDirection = VectorUtils.NormalizeDiff(targetGoalLeftPos, ballLocation);
                var ballToRightTargetDirection = VectorUtils.NormalizeDiff(targetGoalRightPos, ballLocation);
                var aimDirection = VectorUtils.Clamp(carToBallDirection, ballToLeftTargetDirection,
                    ballToRightTargetDirection);
                //Calculate ball offset.
                var ballPosOffset = Vector3.Subtract(ballLocation, Vector3.Multiply(aimDirection, 92.75f));
                //Align direction with ball
                var approachSide = Sign(aimDirection, ballLocation, carLocation);
                var carToBallPerpendicular =
                    Vector3.Normalize(Vector3.Cross(carToBall, new Vector3(0, 0, approachSide)));

                var adjustment =
                    Abs(VectorUtils.Angle(VectorUtils.Flatten(carToBall), VectorUtils.Flatten(aimDirection)) * 2560);
                var finalTarget = Vector3.Add(ballPosOffset, Vector3.Multiply(carToBallPerpendicular, adjustment));
                //Control
                var targetRelativeLocation = Orientation.RelativeLocation(carLocation, finalTarget, carRotation);
                //This assumes no boost - dumb
                //var approximateRemainingDistance = VectorUtils.Magnitude(Vector3.Subtract(finalTarget, carLocation));
                //var timeRemaining = 
                //if (expr)
                //{
                //    
                //}
                //AIM DRIBBLE IMPLEMENTATION END
                //TODO REDO THE FOLLOWING CONTROL CODE TO BETTER MATCH AIMING IMPLEMENTATION :)
                if (targetRelativeLocation.Y > correctionDiff)
                    closeControls.Steer = 1;
                else if (targetRelativeLocation.Y < -correctionDiff)
                    closeControls.Steer = -1;
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
                    if(targetRelativeLocation.Z < 30)
                    {
                        closeControls.Handbrake = true;
                    }
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

        private int Sign(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var product = Vector3.Dot(Vector3.Cross(v1, new Vector3(0, 0, -1)), Vector3.Subtract(v2, v3));
            if (product == 0)
            {
                return 0;
            }

            return (product < 0) ? -1 : 1;
        }

        private float Abs(float x)
        {
            return (x < 0) ? x * -1 : x;
        }
    }
}
