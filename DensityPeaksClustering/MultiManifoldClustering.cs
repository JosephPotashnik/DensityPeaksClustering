namespace DensityPeaksClustering
{
    public class MultiManifoldClustering : DensityPeaksClusteringBase
    {
        private static int GetMFromNumberOfSamples(int numberOfSamples)
        {
            return (int) (0.02 * numberOfSamples);
        }

        private DistanceMatrix LeaveOnlyMutualNeighbors(DistanceMatrix dMatrix)
        {
            var numberOfSamples = dMatrix.NumberOfSamples;
            for (var i = 0; i < numberOfSamples; i++)
            for (var j = 0; j < numberOfSamples; j++)
                //if j is not a nearest neighbor of i, or i is not a nearest neighbor of j,
                //then they are not mutual neighbors. so the weight between them is infinite.
                if (double.IsPositiveInfinity(dMatrix[i, j]) || double.IsPositiveInfinity(dMatrix[j, i]))
                    dMatrix[i, j] = dMatrix[j, i] = double.PositiveInfinity;

            return dMatrix;
        }

        public override void ComputeRho(DistanceMatrix dMatrix, SampleClusteringVariables[] samplesClusteringVars)
        {
            var numberOfSamples = dMatrix.NumberOfSamples;

            var M = GetMFromNumberOfSamples(numberOfSamples);
            var k = GetKFromNumberOfSamples(numberOfSamples);

            //compute manifold distance matrix.
            var nearestNeighborGraph = new KNearestNeighborsGraph(dMatrix);
            var distanceMatrixOfKNN = nearestNeighborGraph.GetKNearestNeighbors(k);
            var distanceMatrixOfMutualKNN = LeaveOnlyMutualNeighbors(distanceMatrixOfKNN);
            var adjacencyList = nearestNeighborGraph.ToAdjacencyList();
            var manifoldDistanceMatrix =
                ShortestDistanceBetweenSamplesMatrix.DijkstraFromAllVertices(adjacencyList, distanceMatrixOfMutualKNN);

            //Get closest M neighbors 
            //TODO: you could copy nearestNeighborGraph after the first line above instead of re-computing.
            var nearestNeighborGraphForMNeighbors = new KNearestNeighborsGraph(dMatrix);
            nearestNeighborGraphForMNeighbors.GetKNearestNeighbors(M);


            for (var i = 0; i < numberOfSamples; i++)
            {
                double sum = 0;
                for (var j = 0; j < M; j++)
                {
                    var neighborIndex = nearestNeighborGraphForMNeighbors.neighborMatrix[i][j].SampleIndex;
                    var val = manifoldDistanceMatrix[i][neighborIndex];

                    if (!double.IsPositiveInfinity(val))
                        sum += val;
                }

                samplesClusteringVars[i].Rho = M / sum;
            }

            //samplesClusteringVars.Rho values are now calculated.
        }

        public override void PostProcessing(DensityPeaksClusteringArgs args)
        {
        }
    }
}