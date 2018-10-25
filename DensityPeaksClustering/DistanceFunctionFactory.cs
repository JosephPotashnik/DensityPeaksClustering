namespace DensityPeaksClustering
{
    public class DistanceFunctionFactory
    {
        public static IDistanceFunction CreateDistanceFunction(DistanceFunctionType distanceType)
        {
            switch (distanceType)
            {
                case DistanceFunctionType.EuclideanDistance:
                    return new EuclideanDistanceFunction();
                case DistanceFunctionType.ManhattanDistance:
                    return new ManhattanDistanceFunction();
                case DistanceFunctionType.PearsonDistance:
                    return new PearsonDistanceFunction();
                default:
                    return new EuclideanDistanceFunction();
            }
        }
    }
}