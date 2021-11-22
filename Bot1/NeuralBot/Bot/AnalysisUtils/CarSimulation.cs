using Bot.BezierUtils;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet.Renderer;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bot.AnalysisUtils
{
    public static class CarSimulation
    {
        //Refactor to return list of positions
        public static List<Vector3> GetBezierCurve(Physics car, Physics ball, Vector3 target, Renderer r)
        {
            List<Vector3> path = new List<Vector3>();
            Vector3 min = new Vector3(-8192, -5120, -2044);
            Vector3 max = min * -1;
            Bezier bezier = new Bezier();
            Vector3 carPos = car.Location;
            Vector3 ballPos = ball.Location;
            Vector3 targetControl = new Vector3(target.X, ballPos.Y + (ballPos.Y - target.Y), target.Z);
            targetControl = Vector3.Clamp(targetControl, min, max);
            Vector3 carControl = new Vector3(ballPos.X + (ballPos.X - carPos.X), carPos.Y, ballPos.Z);
            carControl = Vector3.Clamp(carControl, min, max);

            r.DrawString3D("TargetControl", Color.White, targetControl, 2, 2);
            r.DrawString3D("Target", Color.White, target, 2, 2);
            r.DrawString3D("CarControl", Color.White, carControl, 2, 2);

            Vector3 bezierControl1 = carPos + car.Velocity;
            r.DrawString3D("BC1",Color.White, bezierControl1,2,2);
            Vector3 bezierControl2 = new Vector3(carControl.X, targetControl.Y, carControl.Z);
            r.DrawString3D("BC2", Color.White, bezierControl2, 2, 2);
            float time = 0;
            while(time < 0.9f)
            {
                time += 0.1f;
                Vector3 p1 = bezier.CubicBezier(time, carPos, bezierControl1, bezierControl2, ballPos);
                //Vector3 p2 = bezier.CubicBezier(time + 0.1f, carPos, bezierControl1, bezierControl2, ballPos);
                path.Add(p1);
            }
            return path;
        }
    }
}
