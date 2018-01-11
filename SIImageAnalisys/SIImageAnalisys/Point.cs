using System.Collections.Generic;

namespace SIImageAnalisys
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public List<float> Q { get; set; }

        public Point()
        {
            Q = new List<float>();
        }

        public bool Equals(Point obj)
        {
            return X == obj.X && Y == obj.Y;
        }
    }
}