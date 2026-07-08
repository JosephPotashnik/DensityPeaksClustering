using System.Globalization;

namespace DensityPeaksClustering.Tests;

public class DatasetGoldenTests
{
    public static TheoryData<string, AlgorithmKind, int, int, double, string> KnownDatasetCases()
    {
        return new TheoryData<string, AlgorithmKind, int, int, double, string>
        {
            { "flame.csv", AlgorithmKind.Knn, 10, 0, 0, "1:0-1,2:2-153,1:154-162,2:163-163,1:164-239" },
            { "flame.csv", AlgorithmKind.MultiManifold, 10, 10, 0, "1:0-1,2:2-239" },
            { "flame.csv", AlgorithmKind.DensityPeaks, 0, 0, 2, "0:0-60,1:61-61,0:62-64,1:65-66,0:67-67,1:68-70,0:71-81,2:82-84,0:85-165,3:166-166,0:167-170,3:171-171,0:172-176,3:177-181,0:182-183,3:184-189,0:190-192,3:193-193,0:194-206,3:207-208,0:209-213,3:214-222,0:223-225,3:226-232,0:233-233,3:234-237,0:238-239" },

            { "Jain.csv", AlgorithmKind.Knn, 10, 0, 0, "1:0-109,2:110-112,1:113-128,2:129-372" },
            { "Jain.csv", AlgorithmKind.MultiManifold, 10, 10, 0, "1:0-96,2:97-372" },
            { "Jain.csv", AlgorithmKind.DensityPeaks, 0, 0, 2, "0:0-125,1:126-127,0:128-153,1:154-154,0:155-160,1:161-162,0:163-187,1:188-188,0:189-191,1:192-198,0:199-204,1:205-207,0:208-210,1:211-211,0:212-212,1:213-213,0:214-214,1:215-217,0:218-218,1:219-220,0:221-228,1:229-230,0:231-253,2:254-254,0:255-281,2:282-284,0:285-372" },

            { "pathBased.csv", AlgorithmKind.Knn, 10, 0, 0, "1:0-33,2:34-69,3:70-201,1:202-299" },
            { "pathBased.csv", AlgorithmKind.MultiManifold, 10, 10, 0, "1:0-69,2:70-201,1:202-299" },
            { "pathBased.csv", AlgorithmKind.DensityPeaks, 0, 0, 2, "0:0-1,1:2-8,0:9-9,1:10-54,0:55-55,1:56-56,0:57-57,1:58-69,0:70-70,2:71-73,0:74-74,2:75-76,0:77-78,2:79-79,0:80-80,2:81-89,0:90-92,2:93-97,0:98-98,2:99-99,0:100-102,2:103-108,0:109-109,2:110-110,0:111-113,2:114-118,0:119-119,2:120-142,0:143-144,2:145-170,0:171-171,2:172-190,0:191-191,2:192-196,0:197-198,2:199-200,0:201-203,1:204-204,0:205-205,1:206-206,0:207-207,1:208-265,0:266-266,1:267-269,0:270-270,1:271-293,0:294-295,1:296-299" },

            { "spiral.csv", AlgorithmKind.Knn, 10, 0, 0, "1:0-105,2:106-206,1:207-212,2:213-311" },
            { "spiral.csv", AlgorithmKind.MultiManifold, 10, 10, 0, "1:0-4,2:5-105,3:106-206,4:207-311" },
            { "spiral.csv", AlgorithmKind.DensityPeaks, 0, 0, 2, "0:0-66,1:67-105,0:106-179,2:180-204,0:205-270,1:271-278,0:279-279,2:280-311" },

            { "zahn.csv", AlgorithmKind.Knn, 10, 0, 0, "1:0-141,2:142-398" },
            { "zahn.csv", AlgorithmKind.MultiManifold, 10, 10, 0, "1:0-21,2:22-37,1:38-49,0:50-382,3:383-398" },
            { "zahn.csv", AlgorithmKind.DensityPeaks, 0, 0, 2, "1:0-141,2:142-398" }
        };
    }

    [Theory]
    [MemberData(nameof(KnownDatasetCases))]
    public void KnownDatasets_ProduceStableClusterPartitions(
        string datasetName,
        AlgorithmKind algorithm,
        int k,
        int m,
        double cutoffDistance,
        string expectedRuns)
    {
        var samples = ReadSamples(datasetName);

        var labels = algorithm switch
        {
            AlgorithmKind.Knn => DensityPeaksClusteringAlgorithms.KNN(new KNNAlgorithmParams
            {
                Samples = samples,
                k = k,
                TuningType = ClusterCentersTuningType.FineTuning
            }),
            AlgorithmKind.MultiManifold => DensityPeaksClusteringAlgorithms.MultiManifold(new MultiManifoldAlgorithmParams
            {
                Samples = samples,
                k = k,
                m = m,
                TuningType = ClusterCentersTuningType.FineTuning
            }),
            AlgorithmKind.DensityPeaks => DensityPeaksClusteringAlgorithms.DPClustering(new DensityPeaksAlgorithmParams
            {
                Samples = samples,
                CutoffDistance = cutoffDistance
            }),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null)
        };

        Assert.Equal(samples.Length, labels.Length);
        Assert.Equal(expectedRuns, ToNormalizedRuns(labels));
    }

    private static float[][] ReadSamples(string datasetName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", datasetName);

        return File.ReadLines(path)
            .Skip(1)
            .Select(line =>
            {
                var parts = line.Split(',');
                return new[]
                {
                    float.Parse(parts[0], CultureInfo.InvariantCulture),
                    float.Parse(parts[1], CultureInfo.InvariantCulture)
                };
            })
            .ToArray();
    }

    private static string ToNormalizedRuns(int[] labels)
    {
        var normalized = Normalize(labels);
        var runs = new List<string>();
        var start = 0;

        for (var i = 1; i <= normalized.Length; i++)
        {
            if (i < normalized.Length && normalized[i] == normalized[start])
                continue;

            runs.Add($"{normalized[start]}:{start}-{i - 1}");
            start = i;
        }

        return string.Join(",", runs);
    }

    private static int[] Normalize(int[] labels)
    {
        var nextLabel = 1;
        var map = new Dictionary<int, int>();

        return labels.Select(label =>
        {
            if (label == 0)
                return 0;

            if (!map.TryGetValue(label, out var normalized))
            {
                normalized = nextLabel++;
                map[label] = normalized;
            }

            return normalized;
        }).ToArray();
    }

    public enum AlgorithmKind
    {
        Knn,
        MultiManifold,
        DensityPeaks
    }
}
