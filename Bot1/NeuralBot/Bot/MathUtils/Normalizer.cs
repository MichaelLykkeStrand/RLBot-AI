using System.Numerics;

namespace Bot.MathUtils
{
    public static class Normalizer
    {
        public static Vector3 NormalizeDiff(Vector3 loc1, Vector3 loc2)
        {
            var tmp = Vector3.Subtract(loc1, loc2);
            var tmp2 = Vector3.Normalize(tmp);
            return tmp2;
        }
    }
}