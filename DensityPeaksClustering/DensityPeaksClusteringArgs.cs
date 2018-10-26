using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DensityPeaksClustering
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum ClusterCentersTuningType
    {
        FineTuning = 1, 
        CoarseTuning = 2
    }

    internal class DensityPeaksClusteringArgs
    {
        public ClusterCentersTuningType TuningType { get; set; }

        public DensityPeaksClusteringArgs(ClusterCentersTuningType tuningType = ClusterCentersTuningType.FineTuning)
        {
            TuningType = tuningType;
        }
    }

    internal class MultiManifoldClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int k;
        public readonly int M;

        public MultiManifoldClusteringArgs(int k, int M, ClusterCentersTuningType tuningType = ClusterCentersTuningType.FineTuning) : base(tuningType)
        {
            this.k = k;
            this.M = M;
        }
    }

    internal class RodriguezAndLaioDPCClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int distanceCutoff;

        public RodriguezAndLaioDPCClusteringArgs(int dc, ClusterCentersTuningType tuningType = ClusterCentersTuningType.FineTuning) : base(tuningType)
        {
            
            distanceCutoff = dc;
        }
    }


    internal class KNNClusteringArgs : DensityPeaksClusteringArgs
    {
        public readonly int k;

        public KNNClusteringArgs(int k, ClusterCentersTuningType tuningType = ClusterCentersTuningType.FineTuning) : base(tuningType)
        {
            this.k = k;
        }
    }
}