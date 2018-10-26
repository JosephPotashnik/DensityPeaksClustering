namespace DensityPeaksClustering
{
    public class DensityPeaksClusteringAlgorithms
    {
        //Functions From Data (Distance is computed here).

        /// <summary>
        ///     Density Peaks clustering based on multi-manifold clustering variant
        ///     see http://www.win.tue.nl/~mpechen/publications/pubs/ZhangSAC2016.pdf
        /// </summary>
        /// <param name="matrix"> data for clustering, in arr[][] format </param>
        /// <see cref="float [number of samples][ dimension of sample]" />
        /// <param name="k"> k for k-nearest-neighbors. Default value: 0 (in this case, k is taken to be log(number of samples)</param>
        /// <see cref="int" />
        /// <param name="M"> M for global cohesion,Default Value: 0 (in this case, M is taken to be 0.02*number of samples)</param>
        /// <see cref="int" />
        /// <param name="distanceType"> distance type between samples, default is Euclidean distance</param>
        /// <returns>
        ///     array of cluster indices, with length as the number of samples cluster 0 = noise.
        ///     <see cref="int []" />
        /// </returns>
        public static int[] MultiManifold(float[][] matrix, int k = 0, int M = 0,
            DistanceFunctionType distanceType = DistanceFunctionType.EuclideanDistance)
        {
            var distanceFunction = DistanceFunctionFactory.CreateDistanceFunction(distanceType);
            var dMatrix = new DistanceMatrix(matrix, distanceFunction);
            var manifoldArgs = new MultiManifoldClusteringArgs(k, M);
            DensityPeaksClusteringBase clusterizer = new MultiManifoldClustering();
            return clusterizer.Clusterize(dMatrix, manifoldArgs);
        }

        /// <summary>
        ///     Density Peaks clustering based on KNN clustering variant
        ///     see https://ieeexplore.ieee.org/abstract/document/7420716
        /// </summary>
        /// <param name="matrix"> data for clustering, in arr[][] format </param>
        /// <see cref="float [number of samples][ dimension of sample]" />
        /// <param name="k"> k for k-nearest-neighbors. Default value: 0 (in this case, k is taken to be log(number of samples)</param>
        /// <see cref="int" />
        /// <param name="distanceType"> distance type between samples, default is Euclidean distance</param>
        /// <returns>
        ///     array of cluster indices, with length as the number of samples cluster 0 = noise.
        ///     <see cref="int []" />
        /// </returns>
        public static int[] KNN(float[][] matrix, int k = 0,
            DistanceFunctionType distanceType = DistanceFunctionType.EuclideanDistance)
        {
            var distanceFunction = DistanceFunctionFactory.CreateDistanceFunction(distanceType);
            var dMatrix = new DistanceMatrix(matrix, distanceFunction);
            var knnArgs = new KNNClusteringArgs(k);
            DensityPeaksClusteringBase clusterizer = new KNNClustering();
            return clusterizer.Clusterize(dMatrix, knnArgs);
        }

        /*
        //the matrix matrix is expected to be in the following structure: array of arrays, such that:
        //float[numberOfSamples][numberOfDimensions] 
        public static int[] DPClustering(float[][] matrix, double cutoffDistance,
            DistanceFunctionType distanceType = DistanceFunctionType.EuclideanDistance)
        {
            var distanceFunction = DistanceFunctionFactory.CreateDistanceFunction(distanceType);
            var dMatrix = new DistanceMatrix(matrix, distanceFunction);
            DensityPeaksClusteringBase clusterizer = new RodriguezAndLaioDPCClustering();
            return clusterizer.Clusterize(dMatrix, new RodriguezAndLaioDPCClusteringArgs((int) cutoffDistance));
        }

        //Functions From Distance Matrix:
        public static int[] DPClusteringFromDistanceMatrix(DistanceMatrix dMatrix, double cutoffDistance)
        {
            DensityPeaksClusteringBase clusterizer = new RodriguezAndLaioDPCClustering();
            return clusterizer.Clusterize(dMatrix, new RodriguezAndLaioDPCClusteringArgs((int) cutoffDistance));
        }

        public static int[] DPClusteringWithKNNFromDistanceMatrix(DistanceMatrix dMatrix)
        {
            DensityPeaksClusteringBase clusterizer = new KNNClustering();
            return clusterizer.Clusterize(dMatrix, new DensityPeaksClusteringArgs());
        }

        public static int[] DPClusteringWithMultiManifoldFromDistanceMatrix(DistanceMatrix dMatrix)
        {
            DensityPeaksClusteringBase clusterizer = new MultiManifoldClustering();
            return clusterizer.Clusterize(dMatrix, new DensityPeaksClusteringArgs());
        }
        */
    }
}