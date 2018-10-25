namespace DensityPeaksClustering
{
    public class KNNClustering : DensityPeaksClusteringBase
    {
        public override void ComputeRho(DistanceMatrix dMatrix, SampleClusteringVariables[] samplesClusteringVars)
        {
            var numberOfSamples = dMatrix.NumberOfSamples;

            var k = GetKFromNumberOfSamples(numberOfSamples);
            var nearestNeighbourGraph = new KNearestNeighborsGraph(dMatrix);
            var distanceMatrixOfKNN = nearestNeighbourGraph.GetKNearestNeighbors(k);

            DensityFunction densityFunction = new KNNDistanceDensityFunction();
            args = new KNNClusteringArgs(k);
            densityFunction.ComputeLocalDensity(distanceMatrixOfKNN, samplesClusteringVars, args);
            //samplesClusteringVars.Rho values are now calculated.
        }

        public override void PostProcessing(DensityPeaksClusteringArgs args)
        {
        }
    }
}