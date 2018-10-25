namespace DensityPeaksClustering
{
    public class SampleClusteringVariables
    {
        public double Rho { get; set; }
        public int NearestNeighborWithHigherDensity { get; set; }
        public double Delta { get; set; }
        public double Gamma { get; set; }
        public int ClusterIndex { get; set; }
    }

    //we need to sort density (Rho) by descending order, and keep track of the original index.
    public class Density
    {
        public double Value { get; set; }
        public int SampleIndex { get; set; }
    }

    //we need to sort Gamma (Rho*Delta) by descending order, and keep track of the original index.
    public class Gamma
    {
        public double Value { get; set; }
        public int SampleIndex { get; set; }
    }
}