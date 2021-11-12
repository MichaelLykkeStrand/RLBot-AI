using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bot.BezierUtils
{
    public class Bezier
    {
        public Vector3 CubicBezier(float t, Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint)
        {
            var l1 = Vector3.Lerp(startPoint, controlPoint1, t);
            var l2 = Vector3.Lerp(controlPoint1, controlPoint2, t);
            var l3 = Vector3.Lerp(controlPoint2, endPoint, t);
            var a = Vector3.Lerp(l1, l2, t);
            var b = Vector3.Lerp(l2, l3, t);
            var cubic = Vector3.Lerp(a, b, t);
            return cubic;
        }

        public Vector3 QuadBezier(float t, Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint)
        {
            var l1 = Vector3.Lerp(startPoint, controlPoint, t);
            var l2 = Vector3.Lerp(controlPoint, endPoint, t);
            var quad = Vector3.Lerp(l1, l2, t);
            return quad;
        }
    }
}
