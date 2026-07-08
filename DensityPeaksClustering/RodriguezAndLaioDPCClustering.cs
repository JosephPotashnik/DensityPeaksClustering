namespace DensityPeaksClustering
{
    internal class RodriguezAndLaioDPCClustering : DensityPeaksClusteringBase
    {
        public override DistanceMatrix ComputeRho(DistanceMatrix dMatrix,
            SampleClusteringVariables[] samplesClusteringVars, DensityPeaksClusteringArgs args)
        {
            DensityFunction densityFunction = new PointsUnderDistanceCutoffDistanceDensityFunction();
            densityFunction.ComputeLocalDensity(dMatrix, samplesClusteringVars, args);
            return dMatrix;
        }

        public override void PostProcessing(DistanceMatrix dMatrix,
            SampleClusteringVariables[] samplesClusteringVars, DensityPeaksClusteringArgs args, int clusterCounter)
        {
            var dpcArgs = args as RodriguezAndLaioDPCClusteringArgs;

            if (!dpcArgs.UseHaloProcessing)
                return;

            var maximumBorderDensity = FindMaximumDensitiesInBorderRegion(
                dMatrix, samplesClusteringVars, clusterCounter, dpcArgs.CutoffDistance);

            ReassignPointsAsNoise(samplesClusteringVars, maximumBorderDensity);
        }

        private void ReassignPointsAsNoise(SampleClusteringVariables[] samplesClusteringVars,
            double[] maximumBorderDensity)
        {
            for (var i = 0; i < samplesClusteringVars.Length; i++)
            {
                var currentCluster = samplesClusteringVars[i].ClusterIndex;

                if (currentCluster > 0 && samplesClusteringVars[i].Rho < maximumBorderDensity[currentCluster])
                    samplesClusteringVars[i].ClusterIndex = 0;
            }
        }

        private double[] FindMaximumDensitiesInBorderRegion(DistanceMatrix dMatrix,
            SampleClusteringVariables[] samplesClusteringVars, int clusterCounter, double cutoffDistance)
        {
            var maximumBorderDensity = new double[clusterCounter + 1];
            var numberOfSamples = dMatrix.NumberOfSamples;

            for (var i = 0; i < numberOfSamples - 1; i++)
            for (var j = i + 1; j < numberOfSamples; j++)
            {
                var firstCluster = samplesClusteringVars[i].ClusterIndex;
                var secondCluster = samplesClusteringVars[j].ClusterIndex;

                if (firstCluster <= 0 || secondCluster <= 0 || firstCluster == secondCluster)
                    continue;

                if (dMatrix[i, j] >= cutoffDistance)
                    continue;

                if (samplesClusteringVars[i].Rho > maximumBorderDensity[firstCluster])
                    maximumBorderDensity[firstCluster] = samplesClusteringVars[i].Rho;

                if (samplesClusteringVars[j].Rho > maximumBorderDensity[secondCluster])
                    maximumBorderDensity[secondCluster] = samplesClusteringVars[j].Rho;
            }

            return maximumBorderDensity;
        }
    }
}
