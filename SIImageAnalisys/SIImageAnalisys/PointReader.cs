using System.Collections.Generic;

namespace SIImageAnalisys
{
    public static class PointReader
    {
        public static List<Point> ReadPoints(string[] line)
        {
            var points = new List<Point>();

            for (int i = 2; i < line.Length; i++)
            {
                var point = ReadPoint(line[i]);
                points.Add(point);
            }

            return points;
        }
        public static Point ReadPoint(string line)
        {
            var point = new Point();

            var data = line.Replace('.',',').Split(' ');

            point.X = float.Parse(data[0]);
            point.Y = float.Parse(data[1]);

            for (int i = 5; i < data.Length; i++)
            {
                point.Q.Add(float.Parse(data[i]));
            }

            return point;
        }
    }
}