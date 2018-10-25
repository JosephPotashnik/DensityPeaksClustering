namespace DensityPeaksClustering
{
    public abstract class DensityFunction
    {
        public abstract void ComputeLocalDensity(DistanceMatrix distanceMatrix,
            SampleClusteringVariables[] sampleClusteringVars, DensityPeaksClusteringArgs args);
    }

    public class KNNDistanceDensityFunction : DensityFunction
    {
        public override void ComputeLocalDensity(DistanceMatrix distanceMatrix,
            SampleClusteringVariables[] sampleClusteringVars, DensityPeaksClusteringArgs args)
        {
            var numberOfSamples = sampleClusteringVars.Length;

            var args1 = args as KNNClusteringArgs;

            for (var i = 0; i < numberOfSamples; i++)
            {
                double sum = 0;
                for (var j = 0; j < numberOfSamples; j++)
                {
                    var val = distanceMatrix[i][j];
                    if (!double.IsPositiveInfinity(val))
                        sum += val;
                }

                sampleClusteringVars[i].Rho = args1.NearestKNeighbours / sum;
            }
        }
    }

    public class PointsUnderDistanceCutoffDistanceDensityFunction : DensityFunction
    {
        public override void ComputeLocalDensity(DistanceMatrix distanceMatrix,
            SampleClusteringVariables[] sampleClusteringVars, DensityPeaksClusteringArgs args)
        {
            var args1 = args as RodriguezAndLaioDPCClusteringArgs;
            var numberOfSamples = distanceMatrix.NumberOfSamples;

            for (var i = 0; i < numberOfSamples - 1; i++)
            for (var j = i + 1; j < numberOfSamples; j++)
                if (distanceMatrix[i, j] < args1.distanceCutoff)
                {
                    sampleClusteringVars[i].Rho++;
                    sampleClusteringVars[j].Rho++;
                }
        }
    }
}