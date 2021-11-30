using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Bot.MathUtils
{
    public class VectorUtils
    {
        public static Vector3 NormalizeDiff(Vector3 loc1, Vector3 loc2)
        {
            var tmp = Vector3.Subtract(loc1, loc2);
            var tmp2 = Vector3.Normalize(tmp);
            return tmp2;
        }

        public static Vector3 Clamp(Vector3 dir, Vector3 start, Vector3 end)
        {
            var isRight = Vector3.Dot(dir, Vector3.Cross(end, new Vector3(0, 0, -1))) < 0;
            var isLeft = Vector3.Dot(dir, Vector3.Cross(start, new Vector3(0, 0, -1))) > 0;
            if (Vector3.Dot(end, Vector3.Cross(start, new Vector3(0, 0, -1))) > 0
                ? (isRight && isLeft)
                : (isRight || isLeft))
                return dir;
            if (Vector3.Dot(start, dir) < Vector3.Dot(end, dir))
                return end;

            return start;
        }

        public static Vector3 Flatten(Vector3 v)
        {
            return new Vector3(v.X, v.Y, 0);
        }

        public static float Angle(Vector3 v1, Vector3 v2)
        {
            var dot = Vector3.Dot(v1, v2);
            var v1Magnitude = Magnitude(v1);
            var v2Magnitude = Magnitude(v2);
            var angle = (dot / (v1Magnitude * v2Magnitude));
            return (float)angle;
        }

        private static double Magnitude(Vector3 v)
        {
            return Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2) + Math.Pow(v.Z, 2));
        }
    }
}