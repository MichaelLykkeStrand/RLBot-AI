using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.AnalysisUtils;
using Bot.Utilities.Processed.FieldInfo;
using RLBotDotNet.Renderer;
using Color = System.Drawing.Color;
using Physics = Bot.Utilities.Processed.Packet.Physics;
using PredictionSlice = Bot.Utilities.Processed.BallPrediction.PredictionSlice;
using Vector3 = System.Numerics.Vector3;
using Bot.Objects;
using Ball = Bot.Objects.Ball;
using Bot.MathUtils;

namespace Bot.BehaviourTree
{
    class BezierDrive : ActionNode
    {
        const float correctionDiff = 0.5f;
        const float failureDistance = 1000f;
        const float maxDistance = 5000f;
        protected List<Vector3> _path;
        private float latestTouchMemory = float.MinValue;
        private float lastUpdate = 0;

        public override State Update(Bot agent, Packet packet)
        {
            float time = Game.Time;
            Physics carPhysics = packet.Players[agent.Index].Physics;
            Vector3 carLocation = carPhysics.Location;
            Vector3 ballLocation = packet.Ball.Physics.Location;
            Orientation carRotation = packet.Players[agent.Index].Physics.Rotation;
            Vector3 ballRelativeLocation = Orientation.RelativeLocation(carLocation, ballLocation, carRotation);
            float distance = Vector3.Distance(carLocation, ballLocation);
            float timeoffset = distance.Remap(0, maxDistance, 0, 4);
            PredictionSlice? ballInFuture = BallSimulation.FindSliceAtTime(agent.GetBallPrediction(), time + timeoffset);

            if (ballInFuture != null)
            {
                var ballFutureLocation = (Vector3)ballInFuture?.Physics.Location;
                var distanceToFuture = Vector3.Distance(carLocation, ballFutureLocation);
                
                if (distanceToFuture < failureDistance) //Move to other node
                {
                    return State.FAILURE;
                }

                Vector3 goalPos = Field.GetOpponentGoal(agent);
                try
                {
                    if (Ball.LatestTouch.Time != latestTouchMemory || lastUpdate+1.5f < Game.Time)
                    {
                        latestTouchMemory = Ball.LatestTouch.Time;
                        lastUpdate = Game.Time;
                        _path = CarSimulation.GetBezierCurve(carPhysics, ballInFuture?.Physics, goalPos, agent.Renderer);
                    }
                }
                catch (Exception)
                {
                    _path = CarSimulation.GetBezierCurve(carPhysics, ballInFuture?.Physics, goalPos, agent.Renderer);
                }


                // Find where the ball is relative to us.

                List<Vector3> removeList = new List<Vector3>();
                foreach (Vector3 position in _path)
                {
                    //Check if the point position is behind the car or if passed point already
                    float x = Orientation.RelativeLocation(carLocation, position, carRotation).X;
                    if (Vector3.Distance(carLocation, position) < 500f || x < 0)
                    {
                        removeList.Add(position);
                    }
                }
                foreach (var location in removeList) _path.Remove(location);

                Vector3 targetPos = ballLocation;
                try
                {
                    targetPos = _path.First();
                }
                catch (Exception) //Refactor
                {
                    _path = CarSimulation.GetBezierCurve(carPhysics, ballInFuture?.Physics, goalPos, agent.Renderer);
                }

                RenderPath(_path, agent.Renderer);

                agent.Renderer.DrawString3D("NextMove", Color.White, targetPos, 2, 2);
                ballRelativeLocation = Orientation.RelativeLocation(carLocation, targetPos, carRotation);
                agent.Renderer.DrawLine3D(Color.Gray, ballLocation, ballFutureLocation);
                agent.Renderer.DrawString3D("Future", Color.Black, ballFutureLocation, 1, 1);
            }
            //Move the movement code to new follower script - movement should not be here
            Controller controller = new Controller();
            if (ballRelativeLocation.Y > correctionDiff)
                controller.Steer = 1;
            else if(ballRelativeLocation.Y < -correctionDiff)
                controller.Steer = -1;
            controller.Throttle = 1;

            //TODO set controller on game
            Game.OutoutControls = controller;

            return State.SUCCESS;
        }

        private void RenderPath(List<Vector3> path, Renderer r)
        {
            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    r.DrawLine3D(Color.White, path[i], path[i+1]);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
