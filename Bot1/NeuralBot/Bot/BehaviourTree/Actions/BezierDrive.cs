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

namespace Bot.BehaviourTree
{
    class BezierDrive : ActionNode
    {
        const float correctionDiff = 0.5f;
        const float failureDistance = 1000f;
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
            PredictionSlice? ballInFuture = BallSimulation.FindSliceAtTime(agent.GetBallPrediction(), time+0.2f);
            
            if (ballInFuture != null)
            {
                
                var wht = agent.GetFieldInfo().Goals[agent.Team].Location;
                var ballFutureLocation = (Vector3)ballInFuture?.Physics.Location;
                var distance = Vector3.Distance(carLocation, ballFutureLocation);
                
                if (distance < failureDistance) //Move to other node
                {
                    Controller closeControls = new Controller();
                    if (ballRelativeLocation.Y > correctionDiff)
                        closeControls.Steer = 1;
                    else if (ballRelativeLocation.Y < -correctionDiff)
                        closeControls.Steer = -1;
                    closeControls.Throttle = 1;
                    Game.OutoutControls = closeControls;
                    return State.SUCCESS;
                }
                
                Vector3 goalPos = Vector3.Zero;
                if (agent.Team == 0) //move this logic to own class or move it up to parent class + it is duplicate?
                {
                    goalPos = agent.GetFieldInfo().Goals[1].Location;
                }
                else
                {
                    goalPos = agent.GetFieldInfo().Goals[0].Location;
                }


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
                    if (Vector3.Distance(carLocation, position) < 500f)
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
                
                agent.Renderer.DrawString3D("NextMove", Color.White, targetPos, 2, 2);
                ballRelativeLocation = Orientation.RelativeLocation(carLocation, targetPos, carRotation);
                agent.Renderer.DrawLine3D(Color.Gray, ballLocation, ballFutureLocation);
                agent.Renderer.DrawString3D("Future", Color.Black, ballFutureLocation, 1, 1);
            }

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
    }
}
