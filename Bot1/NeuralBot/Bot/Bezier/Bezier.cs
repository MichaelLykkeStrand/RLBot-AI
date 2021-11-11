using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Bezier
{
    public class Bezier
    {
        public static Vector3 CubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var l1 = Vector3.Lerp(p0, p1, t);
            var l2 = Vector3.Lerp(p1, p2, t);
            var l3 = Vector3.Lerp(p2, p3, t);
            var a = Vector3.Lerp(l1, l2, t);
            var b = Vector3.Lerp(l2, l3, t);
            var cubic = Vector3.Lerp(a, b, t);
            return cubic;
        }
    }
}
