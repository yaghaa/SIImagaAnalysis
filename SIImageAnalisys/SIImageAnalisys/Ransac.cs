using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIImageAnalisys
{
    public class RANSAC
    {
        public int amountOfPoints()
        {
            return 3;
        }

        public Matrix<double> Evaluate(List<Pair> samplePairs)
        {
            List<Pair> sampleList = samplePairs.ToList();
            Matrix<double> model = null;
            Point kpX1 = sampleList[0].FirstImage;
            Point kpU1 = sampleList[0].SecondImage;

            Point kpX2 = sampleList[1].FirstImage;
            Point kpU2 = sampleList[1].SecondImage;

            Point kpX3 = sampleList[2].FirstImage;
            Point kpU3 = sampleList[2].SecondImage;

            var matrix1 = CreateMatrix.DenseOfArray<double>(new double[,] {
                {kpX1.X, kpX1.Y, 1, 0, 0, 0},
                {kpX2.X, kpX2.Y, 1, 0, 0, 0},
                {kpX3.X, kpX3.Y, 1, 0, 0, 0},
                {0, 0, 0, kpX1.X, kpX1.Y, 1},
                {0, 0, 0, kpX2.X, kpX2.Y, 1},
                {0, 0, 0, kpX3.X, kpX3.Y, 1}
            });

            var matrix2 = CreateMatrix.DenseOfArray<double>(new double[,] {
                {kpU1.X},
                {kpU2.X},
                {kpU3.X},
                {kpU1.Y},
                {kpU2.Y},
                {kpU3.Y}
            });

            if (matrix1.Determinant() != 0)
            {
                var equationResult = matrix1.Inverse().Multiply(matrix2);
                model = CreateMatrix.DenseOfArray<double>(new double[,] {
                    {equationResult[0,0], equationResult[1,0], equationResult[2, 0]},
                    {equationResult[3,0], equationResult[4,0], equationResult[5,0]},
                    {0,0,1}
                });
            }

            return model;
        }

        public double evaluateError(Matrix<double> model, Point keyPoint1,  Point keyPoint2)
        {
            Matrix<double> secondMatrix = CreateMatrix.DenseOfArray<double>(
                new double[,] {
                    {(double)keyPoint1.X},
                    {(double)keyPoint2.Y},
                    {1.0}
                }
            );
            var timedMatrix = model.Multiply(secondMatrix);
            double estimatedX = timedMatrix[0, 0];
            double estimatedY = timedMatrix[1, 0];

            return Math.Sqrt(Math.Pow(keyPoint2.X - estimatedX, 2) + Math.Pow(keyPoint2.Y - estimatedY, 2));
        }


        private List<Pair> getSamples(List<Pair> keyPointsPairs)
        {
            var copy = keyPointsPairs.ToList();
            var result = new List<Pair>();
            Random random = new Random();

            for (int i = 0; i < amountOfPoints(); i++)
            {
                int index = random.Next(copy.Count);
                result.Add(copy[index]);
                copy.RemoveAt(index);
            }

            return result;
        }

        public List<Pair> transform(List<Pair> pairs, double imageSize, int iterations, int maxError)
        {
            int bestScore = 0;
            var bestFilteredPairs = new List<Pair>();
            List<Pair> filteredPairs;
            var samplePairs = new List<Pair>();
            Matrix<double> model;
            Point kp1;
            Point kp2;
            double error;
            for (int i = 0; i < iterations; i++)
            {
                model = null;
                var score = 0;
                filteredPairs = new List<Pair>();
                while (model == null && pairs.Count != 0)
                {
                    samplePairs = getSamples(pairs);
                    model = Evaluate(samplePairs);
                }
                List<Pair> pairsList = pairs.ToList();
                for (int j = 0; j < pairs.Count(); j++)
                {
                    kp1 = pairsList[j].FirstImage;
                    kp2 = pairsList[j].SecondImage;
                    error = evaluateError(model, kp1, kp2);
                    if (error < maxError)
                    {
                        score++;
                        filteredPairs.Add(pairsList[j]);
                    }
                }
                if (score > bestScore)
                {
                    bestScore = score;
                    bestFilteredPairs = filteredPairs;
                }
            }
            return bestFilteredPairs;
        }
    }
}