namespace DensityPeaksClustering.Tests;

public class DistanceFunctionTests
{
    [Fact]
    public void EuclideanDistance_ReturnsExpectedDistance()
    {
        var samples = new[]
        {
            new[] { 0f, 0f },
            new[] { 3f, 4f }
        };

        var distance = new EuclideanDistanceFunction().GetDistance(samples, 0, 1, 2);

        Assert.Equal(5d, distance, precision: 10);
    }

    [Fact]
    public void ManhattanDistance_ReturnsExpectedDistance()
    {
        var samples = new[]
        {
            new[] { -1f, 2f, 5f },
            new[] { 3f, -2f, 1f }
        };

        var distance = new ManhattanDistanceFunction().GetDistance(samples, 0, 1, 3);

        Assert.Equal(12d, distance, precision: 10);
    }

    [Theory]
    [InlineData(DistanceFunctionType.EuclideanDistance, typeof(EuclideanDistanceFunction))]
    [InlineData(DistanceFunctionType.ManhattanDistance, typeof(ManhattanDistanceFunction))]
    [InlineData(DistanceFunctionType.PearsonDistance, typeof(PearsonDistanceFunction))]
    public void Factory_ReturnsExpectedDistanceFunction(DistanceFunctionType type, Type expectedType)
    {
        var distanceFunction = DistanceFunctionFactory.CreateDistanceFunction(type);

        Assert.IsType(expectedType, distanceFunction);
    }

    [Fact]
    public void Factory_FallsBackToEuclideanDistanceForUnknownDistanceType()
    {
        var distanceFunction = DistanceFunctionFactory.CreateDistanceFunction((DistanceFunctionType)999);

        Assert.IsType<EuclideanDistanceFunction>(distanceFunction);
    }
}
