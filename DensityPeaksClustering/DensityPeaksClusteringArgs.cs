namespace DensityPeaksClustering
{
    public class DensityPeaksClusteringArgs
    {
    }

    public class RodriguezAndLaioDPCClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int distanceCutoff;

        public RodriguezAndLaioDPCClusteringArgs(int dc)
        {
            distanceCutoff = dc;
        }
    }


    public class KNNClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int NearestKNeighbours;

        public KNNClusteringArgs(int k)
        {
            NearestKNeighbours = k;
        }
    }
}