using System.Collections.Generic;

namespace DensityPeaksClustering.Tests;

public class ShortestDistanceBetweenSamplesMatrixTests
{
    [Fact]
    public void DijkstraFromAllVertices_ComputesShortestPathsAndLeavesDisconnectedVerticesInfinite()
    {
        var matrix = DistanceMatrix.CreateDistanceMatrixWithInfiniteValues(4);
        matrix[0, 1] = 10;
        matrix[0, 2] = 1;
        matrix[2, 1] = 1;

        var adjacencyList = new[]
        {
            new List<int> { 1, 2 },
            new List<int>(),
            new List<int> { 1 },
            new List<int>()
        };

        var shortest = ShortestDistanceBetweenSamplesMatrix.DijkstraFromAllVertices(adjacencyList, matrix);

        Assert.Equal(0d, shortest[0, 0]);
        Assert.Equal(2d, shortest[0, 1]);
        Assert.Equal(1d, shortest[0, 2]);
        Assert.True(double.IsPositiveInfinity(shortest[0, 3]));
        Assert.True(double.IsPositiveInfinity(shortest[3, 0]));
    }
}
