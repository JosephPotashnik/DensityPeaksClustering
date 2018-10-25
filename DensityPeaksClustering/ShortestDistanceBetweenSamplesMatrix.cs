using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DensityPeaksClustering
{
    public class ShortestDistanceBetweenSamplesMatrix
    {
        private static double[] Dijkstra(int sourceVertex, List<int>[] adjacencyList, DistanceMatrix dMatrix)
        {
            var dist = Enumerable.Repeat(double.PositiveInfinity, dMatrix.NumberOfSamples).ToArray();

            dist[sourceVertex] = 0;

            // ReSharper disable once UseObjectOrCollectionInitializer
            var heap = new SortedDictionary<int, double>();
            heap[sourceVertex] = 0;

            while (heap.Any())
            {
                var u = heap.First().Key;
                heap.Remove(u);

                foreach (var v in adjacencyList[u])
                {
                    var alt = dist[u] + dMatrix[u, v];

                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        if (heap.ContainsKey(v))
                            heap[v] -= alt;
                        else
                            heap[v] = alt;
                    }
                }
            }

            return dist;
        }

        public static DistanceMatrix DijkstraFromAllVertices(List<int>[] adjacencyList, DistanceMatrix dMatrix)
        {
            var shortestDistances = DistanceMatrix.CreateDistanceMatrixWithInfiniteValues(dMatrix.NumberOfSamples);
            var numberOfSamples = adjacencyList.Length;

            Parallel.For(0, numberOfSamples, i => shortestDistances[i] = Dijkstra(i, adjacencyList, dMatrix));

            return shortestDistances;
        }
    }
}