using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// SortBy Enumのテスト
/// </summary>
public class SortByTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void SortBy_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(SortBy));

        Assert.Equal(3, values.Length);
        Assert.Contains(SortBy.CreatedAt, values.Cast<SortBy>());
        Assert.Contains(SortBy.UpdatedAt, values.Cast<SortBy>());
        Assert.Contains(SortBy.Id, values.Cast<SortBy>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void SortBy_ToString_ReturnsCorrectName()
    {
        Assert.Equal("CreatedAt", SortBy.CreatedAt.ToString());
        Assert.Equal("UpdatedAt", SortBy.UpdatedAt.ToString());
        Assert.Equal("Id", SortBy.Id.ToString());
    }
}
