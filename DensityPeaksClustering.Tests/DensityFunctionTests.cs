namespace DensityPeaksClustering.Tests;

public class DensityFunctionTests
{
    [Fact]
    public void CutoffDensity_UsesFullDoubleCutoffDistance()
    {
        var matrix = DistanceMatrix.CreateDistanceMatrixWithInfiniteValues(3);
        matrix[0, 1] = matrix[1, 0] = 1.25;
        matrix[0, 2] = matrix[2, 0] = 1.75;
        matrix[1, 2] = matrix[2, 1] = 2.25;
        var variables = new[]
        {
            new SampleClusteringVariables(),
            new SampleClusteringVariables(),
            new SampleClusteringVariables()
        };

        new PointsUnderDistanceCutoffDistanceDensityFunction().ComputeLocalDensity(
            matrix, variables, new RodriguezAndLaioDPCClusteringArgs(1.5));

        Assert.Equal(1d, variables[0].Rho);
        Assert.Equal(1d, variables[1].Rho);
        Assert.Equal(0d, variables[2].Rho);
    }
}
