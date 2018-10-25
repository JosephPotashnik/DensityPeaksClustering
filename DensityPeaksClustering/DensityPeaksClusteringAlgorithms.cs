using System;

namespace DensityPeaksClustering
{
    public class DensityPeaksClusteringAlgorithms
    {
        //Functions From Data (Distance is computed here).

        //the matrix matrix is expected to be in the following structure: array of arrays, such that:
        //float[numberOfSamples][numberOfDimensions] 
        public static int[] MultiManifold(float[][] matrix, int k = 0, int M = 0,
            DistanceFunctionType distanceType = DistanceFunctionType.EuclideanDistance)
        {
            var distanceFunction = DistanceFunctionFactory.CreateDistanceFunction(distanceType);
            var dMatrix = new DistanceMatrix(matrix, distanceFunction);
            var manifoldArgs = new MultiManifoldClusteringArgs(k,M);
            DensityPeaksClusteringBase clusterizer = new MultiManifoldClustering();
            return clusterizer.Clusterize(dMatrix, manifoldArgs);
        }

        //the matrix matrix is expected to be in the following structure: array of arrays, such that:
        //float[numberOfSamples][numberOfDimensions] 
        public static int[] KNN(float[][] matrix, int k = 0,
            DistanceFunctionType distanceType = DistanceFunctionType.EuclideanDistance)
        {
            var distanceFunction = DistanceFunctionFactory.CreateDistanceFunction(distanceType);
            var dMatrix = new DistanceMatrix(matrix, distanceFunction);
            var knnArgs = new KNNClusteringArgs(k);
            DensityPeaksClusteringBase clusterizer = new KNNClustering();
            return clusterizer.Clusterize(dMatrix, knnArgs);
        }

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
    }
}
