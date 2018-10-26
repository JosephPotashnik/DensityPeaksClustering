namespace DensityPeaksClustering
{
    internal class KNNClustering : DensityPeaksClusteringBase
    {
        public override DistanceMatrix ComputeRho(DistanceMatrix dMatrix,
            SampleClusteringVariables[] samplesClusteringVars, DensityPeaksClusteringArgs args)
        {
            var numberOfSamples = dMatrix.NumberOfSamples;

            var k = 0;
            if (args is KNNClusteringArgs knnArgs) k = knnArgs.k;

            if (k == 0)
                k = GetKFromNumberOfSamples(numberOfSamples);

            var nearestNeighborGraph = new KNearestNeighborsGraph(dMatrix);
            var distanceMatrixOfKNN = nearestNeighborGraph.GetKNearestNeighbors(k);

            DensityFunction densityFunction = new KNNDistanceDensityFunction();
            args = new KNNClusteringArgs(k);
            densityFunction.ComputeLocalDensity(distanceMatrixOfKNN, samplesClusteringVars, args);
            //samplesClusteringVars.Rho values are now calculated.

            return dMatrix;
        }

        public override void PostProcessing(DensityPeaksClusteringArgs args)
        {
        }
    }
}