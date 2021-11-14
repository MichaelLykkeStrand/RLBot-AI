using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

//https://github.com/ItsCodeRed/RedUtils
namespace Bot.Objects
{
	/// <summary>Contains info on a collision between the ball and a car</summary>
	public class BallTouch
	{
		/// <summary>The time at which point this collision happened</summary>
		public readonly float Time;
		/// <summary>The location of this collision</summary>
		public readonly Vector3 Location;
		/// <summary>The normal of this collision</summary>
		public readonly Vector3 Normal;
		/// <summary>The name of the player who collided with the ball</summary>
		public readonly string PlayerName;
		/// <summary>The index of the player who collided with the ball</summary>
		public readonly int PlayerIndex;
		/// <summary>The team of the player who collided with the ball</summary>
		public readonly int Team;

		/// <summary>Initializes a new ball touch with data from the packet</summary>
		public BallTouch(rlbot.flat.Touch touch)
		{
			Time = touch.GameSeconds;
			Location = new Vector3(touch.Location.Value.X, touch.Location.Value.Y, touch.Location.Value.Z);
			Normal = new Vector3(touch.Normal.Value.X, touch.Normal.Value.Y, touch.Normal.Value.Z);
			PlayerName = touch.PlayerName;
			PlayerIndex = touch.PlayerIndex;
			Team = touch.Team;
		}
	}
}
