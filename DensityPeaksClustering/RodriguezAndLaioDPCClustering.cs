using System;

namespace DensityPeaksClustering
{
    internal class RodriguezAndLaioDPCClustering : DensityPeaksClusteringBase
    {
        public override DistanceMatrix ComputeRho(DistanceMatrix dMatrix,
            SampleClusteringVariables[] samplesClusteringVars, DensityPeaksClusteringArgs args)
        {
            throw new NotImplementedException();
            //eventually return dMatrix (the original distance matrix of the data,
            //in other implementation the distance matrix is manipulated)
        }


        public override void PostProcessing(DensityPeaksClusteringArgs args)
        {
            //TODO fix cluster counter - not working now.
            //clustr counter is computed at FindClusterCenters() - pass it down here.
            var clusterCounter = 0;
            AnalyzeNoise(clusterCounter);
        }


        private void AnalyzeNoise(int clusterCounter)
        {
            var maximumborderDensity = FindMaximumDensitiesInBorderRegion(clusterCounter);

            //all points there are less than maximum border density in the cluster 
            //are re-assigned as noise/suitable to be noise.
            ReassignPointsAsNoise(maximumborderDensity);
        }

        private void ReassignPointsAsNoise(double[] maximumborderDensity)
        {
            //for (int i = 0; i < numberOfSamples - 1; i++)
            //{
            //    var currentCluster = samplesClusteringVars[i].ClusterIndex;

            //    if (samplesClusteringVars[i].Rho < maximumborderDensity[currentCluster])
            //        samplesClusteringVars[i].ClusterIndex = 0; //0 is the noise cluster.
            //}
        }

        private double[] FindMaximumDensitiesInBorderRegion(int clusterCounter)
        {
            //var args1 = args as RodriguezAndLaioDPCClusteringArgs;

            var maximumborderDensity = new double[clusterCounter + 1];
            for (var i = 0; i < clusterCounter + 1; i++)
                maximumborderDensity[i] = 0;

            //for (int i = 0; i < numberOfSamples - 1; i++)
            //{
            //    for (int j = i + 1; j < numberOfSamples; j++)
            //    {
            //        if (deltaDistanceMatrix[i, j] < args1.distanceCutoff)
            //        {
            //            if (samplesClusteringVars[i].ClusterIndex != samplesClusteringVars[j].ClusterIndex)
            //            {
            //                if (samplesClusteringVars[i].Rho > maximumborderDensity[samplesClusteringVars[i].ClusterIndex])
            //                    maximumborderDensity[samplesClusteringVars[i].ClusterIndex] = samplesClusteringVars[i].Rho;

            //                if (samplesClusteringVars[j].Rho > maximumborderDensity[samplesClusteringVars[j].ClusterIndex])
            //                    maximumborderDensity[samplesClusteringVars[j].ClusterIndex] = samplesClusteringVars[j].Rho;

            //            }
            //        }
            //    }
            //}

            return maximumborderDensity;
        }
    }
}