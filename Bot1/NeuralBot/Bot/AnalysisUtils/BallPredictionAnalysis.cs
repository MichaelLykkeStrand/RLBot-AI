using Bot.Utilities.Processed.BallPrediction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.AnalysisUtils
{
    public static class BallPredictionAnalysis
    {
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
        
    }
}
