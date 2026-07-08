namespace DensityPeaksClustering.Tests;

public class QuickSelectTests
{
    [Fact]
    public void QuickSelectSmallest_PlacesKSmallestItemsBeforeRequestedIndex()
    {
        var values = new[] { 9, 1, 7, 3, 5, 3, 2 };
        var k = 4;

        ArrayExtension.QuickSelectSmallest(values, k);

        var firstFour = values.Take(k).OrderBy(x => x).ToArray();
        Assert.Equal(new[] { 1, 2, 3, 3 }, firstFour);
        Assert.All(values.Take(k), selected =>
            Assert.All(values.Skip(k), unselected => Assert.True(selected <= unselected)));
    }

    [Fact]
    public void StandardDeviation_ReturnsPopulationStandardDeviation()
    {
        var deviation = new[] { 2d, 4d, 4d, 4d, 5d, 5d, 7d, 9d }.StandardDeviation();

        Assert.Equal(2d, deviation, precision: 10);
    }
}
