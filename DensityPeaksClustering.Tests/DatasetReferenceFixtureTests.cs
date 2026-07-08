using System.Globalization;
using System.Text.Json;

namespace DensityPeaksClustering.Tests;

public class DatasetReferenceFixtureTests
{
    public static TheoryData<string, string, int, int> GroundTruthCases()
    {
        return new TheoryData<string, string, int, int>
        {
            { "Jain.csv", "jain.txt", 373, 2 },
            { "flame.csv", "flame.txt", 240, 2 },
            { "pathBased.csv", "pathbased.txt", 300, 3 },
            { "spiral.csv", "spiral.txt", 312, 3 },
            { "zahn.csv", "Compound.txt", 399, 6 }
        };
    }

    [Theory]
    [MemberData(nameof(GroundTruthCases))]
    public void GroundTruthFixtures_MatchSampleFilesAndExpectedClusterCounts(
        string sampleFile,
        string groundTruthFile,
        int expectedPoints,
        int expectedClusters)
    {
        var samples = ReadSamples(sampleFile);
        var groundTruth = ReadGroundTruth(groundTruthFile);

        Assert.Equal(expectedPoints, samples.Length);
        Assert.Equal(samples.Length, groundTruth.Length);
        Assert.Equal(expectedClusters, groundTruth.Distinct().Count());
    }

    [Fact]
    public void ComparisonParameterMetadata_RecordsRequiredDpcReproducibilityParameters()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "ReferenceData", "dataset-comparison-parameters.json");
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        var root = document.RootElement;

        Assert.Equal("Euclidean", root.GetProperty("distanceMetric").GetString());

        var requiredParameters = root
            .GetProperty("recommendedExternalDpcComparisonParameters")
            .GetProperty("additionalParametersRequired")
            .EnumerateArray()
            .Select(x => x.GetString())
            .ToArray();

        Assert.Contains("dc", requiredParameters);
        Assert.Contains("k", requiredParameters);
        Assert.Contains("distanceMetric", requiredParameters);
        Assert.Contains("densityDefinition", requiredParameters);
        Assert.Contains("centerSelection", requiredParameters);
        Assert.Contains("haloProcessing", requiredParameters);
        Assert.Contains("tieHandling", requiredParameters);
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

    private static int[] ReadGroundTruth(string datasetName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "ReferenceData", "SipuGroundTruth", datasetName);
        var tokens = File.ReadAllText(path)
            .Split(null as char[], StringSplitOptions.RemoveEmptyEntries);

        var labels = new List<int>();
        for (var i = 2; i < tokens.Length; i += 3)
            labels.Add(int.Parse(tokens[i], CultureInfo.InvariantCulture));

        return labels.ToArray();
    }
}
