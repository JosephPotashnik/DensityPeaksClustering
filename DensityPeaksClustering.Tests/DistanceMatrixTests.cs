namespace DensityPeaksClustering.Tests;

public class DistanceMatrixTests
{
    [Fact]
    public void Constructor_BuildsSymmetricMatrixWithZeroDiagonal()
    {
        var samples = new[]
        {
            new[] { 0f, 0f },
            new[] { 3f, 4f },
            new[] { 6f, 8f }
        };

        var matrix = new DistanceMatrix(samples, new EuclideanDistanceFunction());

        Assert.Equal(3, matrix.NumberOfSamples);
        Assert.Equal(0d, matrix[0, 0]);
        Assert.Equal(0d, matrix[1, 1]);
        Assert.Equal(0d, matrix[2, 2]);
        Assert.Equal(5d, matrix[0, 1], precision: 10);
        Assert.Equal(matrix[0, 1], matrix[1, 0]);
        Assert.Equal(10d, matrix[0, 2], precision: 10);
        Assert.Equal(matrix[0, 2], matrix[2, 0]);
        Assert.Equal(5d, matrix[1, 2], precision: 10);
        Assert.Equal(matrix[1, 2], matrix[2, 1]);
    }

    [Fact]
    public void CreateDistanceMatrixWithInfiniteValues_FillsAllCellsWithInfinity()
    {
        var matrix = DistanceMatrix.CreateDistanceMatrixWithInfiniteValues(3);

        Assert.Equal(3, matrix.NumberOfSamples);
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            Assert.True(double.IsPositiveInfinity(matrix[i, j]));
    }

    [Fact]
    public void Max_IgnoresPositiveInfinity()
    {
        var matrix = DistanceMatrix.CreateDistanceMatrixWithInfiniteValues(3);
        matrix[0, 1] = matrix[1, 0] = 4;
        matrix[1, 2] = matrix[2, 1] = 7;

        Assert.Equal(7d, matrix.Max);
    }
}
