﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DensityPeaksClustering
{
    public abstract class DensityPeaksClusteringBase
    {
        protected DensityPeaksClusteringArgs args;

        public int[] Clusterize(DistanceMatrix dMatrix, DensityPeaksClusteringArgs args)
        {
            var numberOfSamples = dMatrix.NumberOfSamples;
            var samplesClusteringVars = new SampleClusteringVariables[numberOfSamples];

            for (var i = 0; i < numberOfSamples; i++)
                samplesClusteringVars[i] = new SampleClusteringVariables();

            //to cluster, execute the steps in the following order:

            ////2. Compute rho, local densities
            ComputeRho(dMatrix, samplesClusteringVars);

            var rhoDescending = samplesClusteringVars
                .Select((x, index) => new Density {Value = x.Rho, SampleIndex = index}).OrderByDescending(x => x.Value)
                .ToArray();

            ////3. compute delta, the distance to the nearest neighbor with higher density
            ComputeDelta(dMatrix, samplesClusteringVars, rhoDescending);

            ////4. find cluster centers.
            var clusterCounter = FindClusterCenters(samplesClusteringVars, rhoDescending);

            ////no clusters found at all - return now with an array of cluster indices all equal to 0.
            if (clusterCounter == 0) return new int[numberOfSamples];

            ////5. assign points to the cluster of their nearest neighbor with higher density.
            AssignPointsToTheirNearestHigherDensityNeighbor(samplesClusteringVars, rhoDescending);

            ////6. after all clusters have been found, postprocessing
            //in the original DPC algorithm, we assign noise in border regions.
            PostProcessing(args);

            return samplesClusteringVars.Select(x => x.ClusterIndex).ToArray();
        }

        public abstract void ComputeRho(DistanceMatrix dMatrix, SampleClusteringVariables[] samplesClusteringVars);
        public abstract void PostProcessing(DensityPeaksClusteringArgs args);

        private void ComputeDelta(DistanceMatrix dMatrix, SampleClusteringVariables[] samplesClusteringVars,
            Density[] rhoDescending)
        {
            samplesClusteringVars[rhoDescending[0].SampleIndex].Delta = dMatrix.MaxValue;
            samplesClusteringVars[rhoDescending[0].SampleIndex].NearestNeighborWithHigherDensity = -1;
            samplesClusteringVars[rhoDescending[0].SampleIndex].Gamma = dMatrix.MaxValue * rhoDescending[0].Value;
            var numberOfSamples = dMatrix.NumberOfSamples;

            for (var i = 1; i < numberOfSamples; i++)
            {
                var minDist = dMatrix.MaxValue;
                var nearestNeighbor = -1;
                var currentSample = samplesClusteringVars[rhoDescending[i].SampleIndex];

                for (var j = 0; j < i; j++)
                    if (rhoDescending[j].Value >= rhoDescending[i].Value)
                    {
                        var dist = dMatrix[rhoDescending[i].SampleIndex, rhoDescending[j].SampleIndex];

                        if (dist < minDist)
                        {
                            nearestNeighbor = rhoDescending[j].SampleIndex;
                            minDist = dist;
                        }
                    }

                currentSample.Delta = minDist;
                currentSample.NearestNeighborWithHigherDensity = nearestNeighbor;
                currentSample.Gamma = minDist * rhoDescending[i].Value;
            }
        }

        private void AssignPointsToTheirNearestHigherDensityNeighbor(SampleClusteringVariables[] samplesClusteringVars,
            Density[] rhoDescending)
        {
            var numberOfSamples = samplesClusteringVars.Length;

            for (var i = 0; i < numberOfSamples; i++)
            {
                var currentSample = samplesClusteringVars[rhoDescending[i].SampleIndex];

                if (currentSample.ClusterIndex == 0)
                {
                    var nearestNeighbor = currentSample.NearestNeighborWithHigherDensity;

                    if (nearestNeighbor != -1)
                    {
                        var clusterOfNearestNeighbor = samplesClusteringVars[nearestNeighbor].ClusterIndex;
                        currentSample.ClusterIndex = clusterOfNearestNeighbor;
                    }
                }
            }
        }

        private int FindClusterCenters(SampleClusteringVariables[] samplesClusteringVars, Density[] rhoDescending)
        {
            var clusterCounter = 0;

            var potentialClusterCenters = CoarseTuning(samplesClusteringVars, rhoDescending);
            //potentialClusterCenters = potentialClusterCenters.Union(FineTuning(samplesClusteringVars)).ToList();
  
            //assign clusters indices to cluster centers.
            foreach (var i in potentialClusterCenters)
                samplesClusteringVars[i].ClusterIndex = ++clusterCounter;

            return clusterCounter;
        }

        private List<int> CoarseTuning(SampleClusteringVariables[] samplesClusteringVars, Density[] rhoDescending)
        {
            var numberOfSamples = samplesClusteringVars.Length;

            var potentialClusterCenters = new List<int>();
            var delta = samplesClusteringVars.Select(x => x.Delta).ToArray();
            var rho = samplesClusteringVars.Select(x => x.Rho).ToArray();

            var upperBoundDelta = delta.Average() + 3 * delta.StandardDeviation();
            var upperBoundRho = rho.Average() + 1 * rho.StandardDeviation();

            for (var i = 0; i < numberOfSamples; i++)
            {
                var currentIndex = rhoDescending[i].SampleIndex;

                //a new cluster center only if the delta is sufficiently high.
                if (delta[currentIndex] > upperBoundDelta)
                    potentialClusterCenters.Add(currentIndex);

                //cluster centers are only those whose density is sufficiently high.
                if (rhoDescending[i].Value < upperBoundRho)
                    break;
            }

            return potentialClusterCenters;
        }

        private List<int> FineTuning(SampleClusteringVariables[] samplesClusteringVars)
        {
            var potentialClusterCenters = new List<int>();
            var gammaDescending = samplesClusteringVars
                .Select((x, index) => new Gamma {Value = x.Gamma, SampleIndex = index}).OrderByDescending(x => x.Value)
                .ToArray();
            var numberOfSamples = samplesClusteringVars.Length;

            var derivative = new double[numberOfSamples];
            var secondDerivative = new double[numberOfSamples];

            var turningPoint = 0;
            double maxSecondDerivative = 0;

            for (var i = 1; i < numberOfSamples; i++)
            {
                derivative[i] = gammaDescending[i].Value - gammaDescending[i - 1].Value;
                secondDerivative[i] = derivative[i] - derivative[i - 1];

                if (secondDerivative[i] > maxSecondDerivative)
                {
                    maxSecondDerivative = secondDerivative[i];
                    turningPoint = i;
                }
            }

            for (var i = 0; i < turningPoint; i++)
                potentialClusterCenters.Add(gammaDescending[i].SampleIndex);

            return potentialClusterCenters;
        }

        protected static int GetKFromNumberOfSamples(int numberOfSamples)
        {
            return (int) Math.Log(numberOfSamples, 2);
        }
    }
}