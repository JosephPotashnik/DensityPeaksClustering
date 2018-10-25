namespace DensityPeaksClustering
{
    internal class DensityPeaksClusteringArgs
    {
    }

    internal class MultiManifoldClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int k;
        public readonly int M;

        public MultiManifoldClusteringArgs(int k, int M)
        {
            this.k = k;
            this.M = M;
        }
    }

    internal class RodriguezAndLaioDPCClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int distanceCutoff;

        public RodriguezAndLaioDPCClusteringArgs(int dc)
        {
            distanceCutoff = dc;
        }
    }


    internal class KNNClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int k;

        public KNNClusteringArgs(int k)
        {
            this.k = k;
        }
    }
}