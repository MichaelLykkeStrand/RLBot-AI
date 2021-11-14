using Bot.Utilities.Processed.BallPrediction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

//https://github.com/ItsCodeRed/RedUtils
namespace Bot.Objects
{
	/// <summary>A ball object. Also contains static properties relating the the "main" ball on the field</summary>
	public class Ball
	{
		public const float MaxSpeed = 6000;
		public const float Radius = 93.15f;

		/// <summary>An instance of the current state of the ball in the game
		/// <para>Unless you manipulate this object in some way, it is advised to use the static properties to access info on the current ball state</para>
		/// </summary>
		public static Ball MainBall { get { return new Ball(Location, Velocity, AngularVelocity); } }
		/// <summary>The current location of the ball in the game</summary>
		public static Vector3 Location { get; private set; }
		/// <summary>The current velocity of the ball in the game</summary>
		public static Vector3 Velocity { get; private set; }
		/// <summary>The current angular velocity of the ball in the game</summary>
		public static Vector3 AngularVelocity { get; private set; }
		/// <summary>Information on the last touch the ball had with a car</summary>
		public static BallTouch LatestTouch { get; private set; }
		/// <summary>A list of slices containing the future positions and velocities of the ball 
		/// <para>Each slice is 1/60 of a second into the future. Can go 6 seconds into the future max, so a total of 360 slices</para>
		/// </summary>
		public static BallPrediction Prediction { get; private set; }

		/// <summary>The location of this ball instance
		/// <para>it should be noted that this variable doesn't follow normal naming conventions, because there is already a static variable with the correct namen</para>
		/// </summary>
		public Vector3 location;
		/// <summary>The velocity of this ball instance
		/// <para>it should be noted that this variable doesn't follow normal naming conventions, because there is already a static variable with the correct namen</para>
		/// </summary>
		public Vector3 velocity;
		/// <summary>The angular velocity of this ball instance. 
		/// <para>it should be noted that this variable doesn't follow normal naming conventions, because there is already a static variable with the correct namen</para>
		/// </summary>
		public Vector3 angularVelocity;

		static Ball()
		{
			Location = Vector3.Zero;
			Velocity = Vector3.Zero;
			AngularVelocity = Vector3.Zero;
		}

		/// <summary>Initializes a new ball instance</summary>
		public Ball(Vector3 location, Vector3 velocity)
		{
			this.location = location;
			this.velocity = velocity;
			angularVelocity = Vector3.Zero;
		}

		/// <summary>Initializes a new ball instance</summary>
		public Ball(Vector3 location, Vector3 velocity, Vector3 angularVelocity)
		{
			this.location = location;
			this.velocity = velocity;
			this.angularVelocity = angularVelocity;
		}

		/// <summary>Updates the static properties with new info from the packet</summary>
		public static void Update(Bot bot, rlbot.flat.BallInfo ballInfo)
		{
			Location = ballInfo.Physics.Value.Location.HasValue ? new Vector3(ballInfo.Physics.Value.Location.Value.X, ballInfo.Physics.Value.Location.Value.Y, ballInfo.Physics.Value.Location.Value.Z) : Location;
			Velocity = ballInfo.Physics.Value.Velocity.HasValue ? new Vector3(ballInfo.Physics.Value.Velocity.Value.X, ballInfo.Physics.Value.Velocity.Value.Y, ballInfo.Physics.Value.Velocity.Value.Z) : Velocity;
			AngularVelocity = ballInfo.Physics.Value.AngularVelocity.HasValue ? new Vector3(ballInfo.Physics.Value.AngularVelocity.Value.X, ballInfo.Physics.Value.AngularVelocity.Value.Y, ballInfo.Physics.Value.AngularVelocity.Value.Z) : AngularVelocity;
			LatestTouch = ballInfo.LatestTouch.HasValue ? new BallTouch(ballInfo.LatestTouch.Value) : null;
			Prediction = bot.GetBallPrediction();
		}

		/// <summary>Predicts the state of this ball instance. Note that this ignores walls and cars</summary>
		public Ball Predict(float time)
		{
			return new Ball(location + velocity * time + Game.Gravity * 0.5f * time * time, velocity + Game.Gravity * time);
		}

		/// <summary>Predicts the location of this ball instance. Note that this ignores walls and cars</summary>
		public Vector3 PredictLocation(float time)
		{
			return location + velocity * time + Game.Gravity * 0.5f * time * time;
		}

		/// <summary>Predicts the velocity of this ball instance. Note that this ignores walls and cars</summary>
		public Vector3 PredictVelocity(float time)
		{
			return velocity + Game.Gravity * time;
		}
	}
}
