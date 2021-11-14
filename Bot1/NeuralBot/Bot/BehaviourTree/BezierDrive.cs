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


namespace Bot.BehaviourTree
{
    class BezierDrive : ActionNode
    {
        public override NodeResult Update(Bot agent, Packet packet, ref Controller output)
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
                // Find where the ball is relative to us.
                Vector3 targetPos = Vector3.One;
                if (agent.Team == 0)
                {
                    targetPos = CarSimulation.GetBezierCurve(carPhysics, ballInFuture?.Physics,
                        agent.GetFieldInfo().Goals[1].Location, agent.Renderer)[0];
                }
                else
                {
                    targetPos = CarSimulation.GetBezierCurve(carPhysics, ballInFuture?.Physics,
                        agent.GetFieldInfo().Goals[0].Location, agent.Renderer)[0];  
                }
                
                
                agent.Renderer.DrawString3D("NextMove", Color.White, targetPos, 2, 2);
                ballRelativeLocation = Orientation.RelativeLocation(carLocation, targetPos, carRotation);
                agent.Renderer.DrawLine3D(Color.Gray, ballLocation, ballFutureLocation);
                agent.Renderer.DrawString3D("Future", Color.Black, ballFutureLocation, 1, 1);
            }
            
            float steer;
            if (ballRelativeLocation.Y > 0)
                output.Steer = 1;
            else
                output.Steer = -1;

            output.Throttle = 1;

            NodeResult tmpResult = new NodeResult
            {
                nodeState = State.SUCCESS,
                controller = output
            };

            return tmpResult;
        }
    }
}
