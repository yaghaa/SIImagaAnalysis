using System;
using System.Collections.Generic;
using System.Linq;

namespace SIImageAnalisys
{
    public class ImageModel
    {
        public List<Point> ImageA { get; set; }
        public List<Point> ImageB { get; set; }

        public List<Pair> MatchPairs()
        {
            var pairs = new List<Pair>();
            var reversedPairs = new List<Pair>();
            
            foreach (var pointA in ImageA)
            {
                var pairPoint = new Pair {FirstImage = pointA};

                var minD = float.MaxValue;
                var chosenPointB = 0;
                var currentPoint = 0;

                foreach (var pointB in ImageB)
                {
                    var currentD = 0f;
                    for (int i = 0; i < pointA.Q.Count; i++)
                    {
                        currentD += (pointA.Q[i] - pointB.Q[i]) * (pointA.Q[i] - pointB.Q[i]);

                        if (currentD > minD)
                        {
                            break;
                        }
                    }

                    if (currentD < minD)
                    {
                        chosenPointB = currentPoint;
                        minD = currentD;
                    }
                    currentPoint++;
                }
                pairPoint.SecondImage = ImageB[chosenPointB];
                pairPoint.D = (float) Math.Sqrt(minD);
                pairs.Add(pairPoint);
            }

            foreach (var pointB in ImageB)
            {
                var pairPoint = new Pair { SecondImage = pointB };

                var minD = float.MaxValue;
                var chosenPointA = 0;
                var currentPoint = 0;

                foreach (var pointA in ImageA)
                {
                    var currentD = 0f;
                    for (int i = 0; i < pointA.Q.Count; i++)
                    {
                        currentD += (pointB.Q[i] - pointA.Q[i]) * (pointB.Q[i] - pointA.Q[i]);

                        if (currentD > minD)
                        {
                            break;
                        }
                    }

                    if (currentD < minD)
                    {
                        chosenPointA = currentPoint;
                        minD = currentD;
                    }
                    currentPoint++;
                }
                pairPoint.FirstImage = ImageA[chosenPointA];
                pairPoint.D = (float)Math.Sqrt(minD);
                reversedPairs.Add(pairPoint);
            }

            var result = new List<Pair>();
            foreach (var pair in pairs)
            {
                if (reversedPairs.Any(reversedPair => pair.MatchPair(reversedPair)))
                {
                    result.Add(pair);
                }
            }
            return result;
            //return pairs.Where(x=> reversedPairs.Any(y=> y.MatchPair(x))).ToList();
        }
    }
}