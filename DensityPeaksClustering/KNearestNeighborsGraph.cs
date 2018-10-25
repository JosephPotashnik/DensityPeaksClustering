using System;
using System.Collections.Generic;

namespace DensityPeaksClustering
{
    public class Neighbor : IComparable<Neighbor>
    {
        public double Distance { get; set; }
        public int SampleIndex { get; set; }

        public int CompareTo(Neighbor other)
        {
            return Distance.CompareTo(other.Distance);
        }
    }

    public class KNearestNeighborsGraph
    {
        private HashSet<int>[] mutualNeighbors;
        public Neighbor[][] neighborMatrix;

        public KNearestNeighborsGraph(DistanceMatrix dMatrix)
        {
            var numOfSamples = dMatrix.NumberOfSamples;
            neighborMatrix = new Neighbor[numOfSamples][];

            for (var i = 0; i < numOfSamples; i++)
            {
                neighborMatrix[i] = new Neighbor[numOfSamples];
                for (var j = 0; j < numOfSamples; j++)
                    neighborMatrix[i][j] = new Neighbor
                    {
                        Distance = dMatrix[i, j],
                        SampleIndex = j
                    };
            }
        }

        public DistanceMatrix GetKNearestNeighbors(int k)
        {
            var numOfSamples = neighborMatrix.Length;
            var neighbors = new HashSet<int>[numOfSamples];

            var dMatrix = DistanceMatrix.CreateDistanceMatrixWithInfiniteValues(numOfSamples);

            for (var i = 0; i < neighborMatrix.Length; i++)
            {
                ArrayExtension.QuickSelectSmallest(neighborMatrix[i], k);
                neighbors[i] = new HashSet<int>();
                for (var j = 0; j < k; j++)
                {
                    var neighborSampleIndex = neighborMatrix[i][j].SampleIndex;
                    dMatrix[i][neighborSampleIndex] = neighborMatrix[i][j].Distance;
                    neighbors[i].Add(neighborSampleIndex);
                }
            }

            mutualNeighbors = new HashSet<int>[numOfSamples];

            for (var i = 0; i < neighborMatrix.Length; i++)
            {
                mutualNeighbors[i] = new HashSet<int>();

                foreach (var neighborSampleIndex in neighbors[i])
                    //if the the current sample i is a neighbor of its neighbor (mutual),
                    //add to the list of mutual neighbors
                    if (neighbors[neighborSampleIndex].Contains(i))
                        mutualNeighbors[i].Add(neighborSampleIndex);
            }

            return dMatrix;
        }

        //assumption: for each sample , the neighbor matrix has only at most k neighbors.
        //which are the first k elements.
        //in other words, call to GetKNearestNeighbors(k) before calling ToAdjacencyList
        //the purpose of ToAdjacencyList() is to help with the representation of sparse matrix only!
        public List<int>[] ToAdjacencyList()
        {
            var adjList = new List<int>[neighborMatrix.Length];
            for (var i = 0; i < neighborMatrix.Length; i++)
            {
                adjList[i] = new List<int>();
                foreach (var neighborIndex in mutualNeighbors[i])
                    adjList[i].Add(neighborIndex);
            }

            return adjList;
        }
    }
}