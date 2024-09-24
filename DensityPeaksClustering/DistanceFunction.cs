using System;

namespace DensityPeaksClustering
{
    public enum DistanceFunctionType
    {
        EuclideanDistance = 0,
        ManhattanDistance = 1,
        PearsonDistance = 2
    }

    public interface IDistanceFunction
    {
        double GetDistance(float[][] m, int i, int j, int dimension);
    }

    public class EuclideanDistanceFunction : IDistanceFunction
    {
        public double GetDistance(float[][] m, int i, int j, int dimension)
        {
            double sum = 0;
            for (var k = 0; k < dimension; k++)
                sum += Math.Pow(m[i][k] - m[j][k], 2);

            return Math.Pow(sum, 0.5);
        }
    }

    public class ManhattanDistanceFunction : IDistanceFunction
    {
        public double GetDistance(float[][] m, int i, int j, int dimension)
        {
            double sum = 0;
            for (var k = 0; k < dimension; k++)
                sum += Math.Abs(m[i][k] - m[j][k]);

            return sum;
        }
    }

    public class PearsonDistanceFunction : IDistanceFunction
    {
        public double GetDistance(float[][] m, int i, int j, int dimension)
        {
            throw new NotImplementedException();
        }
    }


    public class CosineDistanceFunction : IDistanceFunction
    {
        public double GetDistance(float[][] m, int i, int j, int dimension)
        {
            throw new NotImplementedException();
        }
    }
}