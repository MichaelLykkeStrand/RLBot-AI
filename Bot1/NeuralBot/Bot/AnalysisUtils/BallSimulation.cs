using Bot.Utilities.Processed.BallPrediction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.AnalysisUtils
{
    public static class BallSimulation
    {

        const int GOAL_THRESHOLD = 5235;
        const float BALL_SIZE = 92.75f;
        public static PredictionSlice? FindSliceAtTime(BallPrediction ballPrediction, float time)
        {
            float startTime = ballPrediction.Slices[0].GameSeconds;
            int approxIndex = (int)(time - startTime) * 60;
            if(0 <= approxIndex && approxIndex < ballPrediction.Slices.Length)
            {
                return ballPrediction.Slices[approxIndex];
            }
            return null;
        }

        public static PredictionSlice? FindSliceWhereBallIsGrounded(BallPrediction ballPrediction, float time)
        {
            float startTime = ballPrediction.Slices[0].GameSeconds;
            int approxIndex = (int)(time - startTime) * 60;
            if (0 <= approxIndex && approxIndex < ballPrediction.Slices.Length)
            {
                for (int i = 0; i < approxIndex; i++)
                {
                    var slice = ballPrediction.Slices[i];
                    if (slice.Physics.Location.Z <= BALL_SIZE)
                    {
                        return ballPrediction.Slices[i];
                    }
                }
            }
            return null;
        }

        public static PredictionSlice? PredictFutureGoal(BallPrediction ballPrediction)
        {
            for (int coarseIndex = 0; coarseIndex < ballPrediction.Slices.Length; coarseIndex++)
            {
                var slice = ballPrediction.Slices[coarseIndex];
                if (Math.Abs(slice.Physics.Location.Y) >= GOAL_THRESHOLD)
                {
                    return slice;
                }
            }
            return null;
        }
        
    }
}
