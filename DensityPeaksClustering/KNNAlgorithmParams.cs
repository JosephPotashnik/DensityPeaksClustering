﻿using System.Text.Json.Serialization;

namespace DensityPeaksClustering
{
    public class KNNAlgorithmParams
    {
        public float[][] Samples { get; set; }
        public int k { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ClusterCentersTuningType TuningType { get; set; }

    }
}
