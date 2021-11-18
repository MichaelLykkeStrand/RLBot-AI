using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//https://github.com/ItsCodeRed/RedUtils
namespace Bot.Objects
{
	/// <summary>Contains static properties related to the field, like boost pads, goals, etc</summary>
	public static class Field
	{
		/// <summary>The length of the field, from one goal to the other</summary>
		public const float Length = 10240;
		/// <summary>The width of the field, from one side wall to the other</summary>
		public const float Width = 8192;
		/// <summary>The total height of the field</summary>
		public const float Height = 1950;

		/// <summary>The corner planes intersect the axes at ±8064</summary>
		public const float CornerIntersection = 8064;
		/// <summary>The wall length of the corners</summary>
		public const float CornerLength = 1629;
		/// <summary>The length of the corners on the x axis or y axis</summary>
		public const float CornerWidth = 1152;

		private static Random random = new Random();

		public static Vector3 GetRandomPosition()
        {
			
			float x = random.Next((int)-Field.Width / 2, (int)Field.Width / 2);
			float y = random.Next((int)-Field.Length / 2, (int)Field.Length / 2);
			float z = 0.2f;

			return new Vector3(x, y, z);
		}
	}
}
