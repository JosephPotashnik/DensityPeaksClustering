using System.Text.Json.Serialization;


namespace DensityPeaksClustering
{
    public class MultiManifoldAlgorithmParams
    {
        public float[][] Samples { get; set; }
        public int k { get; set; }
        public int m { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ClusterCentersTuningType TuningType { get; set; }
    }
}
