using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DensityPeaksClustering
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DistanceMatrix
    {
        [JsonProperty] public double[][] Distances;

        public DistanceMatrix()
        {
        }
        

        public DistanceMatrix(float[][] m, IDistanceFunction distanceFunction)
        {
            NumberOfSamples = m.Length;
            Distances = new double[NumberOfSamples][];

            for (var i = 0; i < NumberOfSamples; i++)
                Distances[i] = new double[NumberOfSamples];

            int dimension = m[0].Length;

            double dist;
            for (var i = 0; i < NumberOfSamples - 1; i++)
            for (var j = i + 1; j < NumberOfSamples; j++)
            {
                //compute distance according to distance function between point i and point j in data.
                dist = distanceFunction.GetDistance(m, i, j, dimension);
                Distances[j][i] = Distances[i][j] = dist;

            }
        }

        public int NumberOfSamples { get; set; }

        public double this[int i, int j]
        {
            get => Distances[i][j];
            set => Distances[i][j] = value;
        }

        //single indexer = returns a vector of distances of all samples from sample i.
        public double[] this[int i]
        {
            get => Distances[i];
            set => Distances[i] = value;
        }

        internal double Max
        {
            get
            {

                NumberOfSamples = Distances.GetLength(0);

                double max = double.MinValue;

                double dist;
                for (var i = 0; i < NumberOfSamples - 1; i++)
                    for (var j = i + 1; j < NumberOfSamples; j++)
                    {
                        dist = Distances[i][j];
                        if (!double.IsPositiveInfinity(dist) && dist > max)
                            max = dist;
                    }

                return max;
            }
        }

        public static DistanceMatrix CreateDistanceMatrixWithInfiniteValues(int numberOfSamples)
        {
            var dMatrix = new DistanceMatrix
            {
                NumberOfSamples = numberOfSamples,
                Distances = new double[numberOfSamples][]
            };

            for (var i = 0; i < numberOfSamples; i++)
                dMatrix.Distances[i] = Enumerable.Repeat(double.PositiveInfinity, numberOfSamples).ToArray();

            return dMatrix;
        }
    }
}