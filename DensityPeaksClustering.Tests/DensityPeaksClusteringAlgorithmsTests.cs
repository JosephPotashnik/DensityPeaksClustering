namespace DensityPeaksClustering.Tests;

public class DensityPeaksClusteringAlgorithmsTests
{
    [Fact]
    public void DPClustering_ReturnsOneClusterLabelPerSample()
    {
        var samples = TwoCompactGroups();

        var labels = DensityPeaksClusteringAlgorithms.DPClustering(new DensityPeaksAlgorithmParams
        {
            Samples = samples,
            CutoffDistance = 2
        });

        Assert.Equal(samples.Length, labels.Length);
        Assert.All(labels, label => Assert.True(label >= 0));
    }

    [Fact]
    public void DPClusteringFromDistanceMatrix_ReturnsSameResultAsSampleBasedApiForEquivalentInput()
    {
        var samples = TwoCompactGroups();
        var distanceMatrix = new DistanceMatrix(samples, new EuclideanDistanceFunction());

        var fromSamples = DensityPeaksClusteringAlgorithms.DPClustering(new DensityPeaksAlgorithmParams
        {
            Samples = samples,
            CutoffDistance = 2
        });
        var fromMatrix = DensityPeaksClusteringAlgorithms.DPClusteringFromDistanceMatrix(distanceMatrix, 2);

        Assert.Equal(fromSamples, fromMatrix);
    }

    [Fact]
    public void DPClustering_WithExplicitClusterCenters_UsesProvidedCenters()
    {
        var samples = TwoCompactGroups();

        var labels = DensityPeaksClusteringAlgorithms.DPClustering(new DensityPeaksAlgorithmParams
        {
            Samples = samples,
            CutoffDistance = 2,
            ClusterCenterIndices = new[] { 0, 3 },
            UseHaloProcessing = false
        });

        Assert.Equal(new[] { 1, 1, 1, 2, 2, 2 }, labels);
    }

    [Fact]
    public void DPClusteringFromDistanceMatrix_WithExplicitClusterCenters_MatchesSampleBasedApi()
    {
        var samples = TwoCompactGroups();
        var distanceMatrix = new DistanceMatrix(samples, new EuclideanDistanceFunction());

        var fromSamples = DensityPeaksClusteringAlgorithms.DPClustering(new DensityPeaksAlgorithmParams
        {
            Samples = samples,
            CutoffDistance = 2,
            ClusterCenterIndices = new[] { 0, 3 },
            UseHaloProcessing = false
        });
        var fromMatrix = DensityPeaksClusteringAlgorithms.DPClusteringFromDistanceMatrix(
            distanceMatrix,
            2,
            clusterCenterIndices: new[] { 0, 3 },
            useHaloProcessing: false);

        Assert.Equal(fromSamples, fromMatrix);
    }

    [Fact]
    public void DPClustering_WithThresholdsMatchingNoCenters_ReturnsOnlyNoise()
    {
        var samples = TwoCompactGroups();

        var labels = DensityPeaksClusteringAlgorithms.DPClustering(new DensityPeaksAlgorithmParams
        {
            Samples = samples,
            CutoffDistance = 2,
            RhoMin = 100,
            DeltaMin = 100,
            UseHaloProcessing = false
        });

        Assert.Equal(new[] { 0, 0, 0, 0, 0, 0 }, labels);
    }

    [Fact]
    public void KNN_ReturnsOneClusterLabelPerSample()
    {
        var samples = TwoCompactGroups();

        var labels = DensityPeaksClusteringAlgorithms.KNN(new KNNAlgorithmParams
        {
            Samples = samples,
            k = 2,
            TuningType = ClusterCentersTuningType.FineTuning
        });

        Assert.Equal(samples.Length, labels.Length);
        Assert.All(labels, label => Assert.True(label >= 0));
    }

    [Fact]
    public void MultiManifold_ReturnsOneClusterLabelPerSample()
    {
        var samples = TwoCompactGroups();

        var labels = DensityPeaksClusteringAlgorithms.MultiManifold(new MultiManifoldAlgorithmParams
        {
            Samples = samples,
            k = 2,
            m = 2,
            TuningType = ClusterCentersTuningType.FineTuning
        });

        Assert.Equal(samples.Length, labels.Length);
        Assert.All(labels, label => Assert.True(label >= 0));
    }

    [Fact]
    public void KNN_DoesNotMutateInputSamples()
    {
        var samples = TwoCompactGroups();
        var original = samples.Select(row => row.ToArray()).ToArray();

        DensityPeaksClusteringAlgorithms.KNN(new KNNAlgorithmParams
        {
            Samples = samples,
            k = 2,
            TuningType = ClusterCentersTuningType.FineTuning
        });

        for (var i = 0; i < samples.Length; i++)
            Assert.Equal(original[i], samples[i]);
    }

    private static float[][] TwoCompactGroups()
    {
        return new[]
        {
            new[] { 0f, 0f },
            new[] { 0.1f, 0f },
            new[] { 0f, 0.1f },
            new[] { 10f, 10f },
            new[] { 10.1f, 10f },
            new[] { 10f, 10.1f }
        };
    }
}
