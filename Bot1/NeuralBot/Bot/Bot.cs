using System.Drawing;
using System.Numerics;
using Bot.AnalysisUtils;
using Bot.Utilities.Processed.BallPrediction;
using Bot.Utilities.Processed.FieldInfo;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;

namespace Bot
{
    // We want to our bot to derive from Bot, and then implement its abstract methods.
    class Bot : RLBotDotNet.Bot
    {
        // We want the constructor for our Bot to extend from RLBotDotNet.Bot, but we don't want to add anything to it.
        // You might want to add logging initialisation or other types of setup up here before the bot starts.
        public Bot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex) { }

        public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
        {
            // We process the gameTickPacket and convert it to our own internal data structure.
            Packet packet = new Packet(gameTickPacket);

            // Get the data required to drive to the ball.
            Vector3 ballLocation = packet.Ball.Physics.Location;
            Vector3 carLocation = packet.Players[Index].Physics.Location;
            Orientation carRotation = packet.Players[Index].Physics.Rotation;

            PredictionSlice? ballInFuture = BallSimulation.FindSliceAtTime(this.GetBallPrediction(), packet.GameInfo.SecondsElapsed+2f);
            if(ballInFuture != null)
            {
                var ballFutureLocation = (Vector3)ballInFuture?.Physics.Location;
                Renderer.DrawLine3D(Color.Gray, ballLocation, ballFutureLocation);
                Renderer.DrawString3D("Future", Color.Black, ballFutureLocation, 1, 1);
            }
            PredictionSlice? goalInFuture = BallSimulation.PredictFutureGoal(this.GetBallPrediction());
            if (goalInFuture != null)
            {
                var goalInFutureLocation = (Vector3)goalInFuture?.Physics.Location;
                Renderer.DrawString3D("Goal", Color.LightGreen, goalInFutureLocation, 2, 2);
            }
            PredictionSlice? groundInFuture = BallSimulation.FindSliceWhereBallIsGrounded(this.GetBallPrediction(), packet.GameInfo.SecondsElapsed + 2f);
            if (groundInFuture != null)
            {
                var groundInFutureLocation = (Vector3)groundInFuture?.Physics.Location;
                Renderer.DrawString3D("Grounded", Color.LightGreen, groundInFutureLocation, 2, 2);
            }
            // Find where the ball is relative to us.
            Vector3 ballRelativeLocation = Orientation.RelativeLocation(carLocation, ballLocation, carRotation);

            // Decide which way to steer in order to get to the ball.
            // If the ball is to our left, we steer left. Otherwise we steer right.
            float steer;
            if (ballRelativeLocation.Y > 0)
                steer = 1;
            else
                steer = -1;
            
            // Examples of rendering in the game
            Renderer.DrawString3D("Ball", Color.Black, ballLocation, 1, 1);
            Renderer.DrawString3D(steer > 0 ? "Right" : "Left", Color.Red, carLocation, 1, 1);
            Renderer.DrawLine3D(Color.Red, carLocation, ballLocation);
            
            // This controller will contain all the inputs that we want the bot to perform.
            return new Controller
            {
                // Set the throttle to 1 so the bot can move.
                Throttle = 1,
                Steer = steer
            };
        }
        
        // Hide the old methods that return Flatbuffers objects and use our own methods that
        // use processed versions of those objects instead.
        internal new FieldInfo GetFieldInfo() => new FieldInfo(base.GetFieldInfo());
        internal new BallPrediction GetBallPrediction() => new BallPrediction(base.GetBallPrediction());
    }
}