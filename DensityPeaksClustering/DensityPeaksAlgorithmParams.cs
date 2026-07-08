namespace DensityPeaksClustering
{
    public class DensityPeaksAlgorithmParams
    {
        public float[][] Samples { get; set; }
        public double CutoffDistance { get; set; }
        public int[] ClusterCenterIndices { get; set; }
        public double? RhoMin { get; set; }
        public double? DeltaMin { get; set; }
        public bool UseHaloProcessing { get; set; } = true;
    }

}
