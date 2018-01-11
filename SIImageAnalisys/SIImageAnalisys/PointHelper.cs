using System;
using System.Collections.Generic;
using System.Linq;

namespace SIImageAnalisys
{
    public class PointHelper
    {
        public float GetDistance(Point p1, Point p2)
        {
            return (float)Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        public int GetCohesion(IEnumerable<Tuple<Pair, float>> closesNeibhoursA, IEnumerable<Tuple<Pair, float>> closesNeibhoursB)
        {
            var matchedPoints = 0;
            foreach (var pair in closesNeibhoursA)
            {
                if (closesNeibhoursB.Any(x => x.Item1.MatchPair(pair.Item1)))
                {
                    matchedPoints++;
                }
            }

            return matchedPoints;
        }
    }
}