using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// SortOrder Enumのテスト
/// </summary>
public class SortOrderTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void SortOrder_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(SortOrder));

        Assert.Equal(2, values.Length);
        Assert.Contains(SortOrder.Asc, values.Cast<SortOrder>());
        Assert.Contains(SortOrder.Desc, values.Cast<SortOrder>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void SortOrder_ToString_ReturnsCorrectName()
    {
        Assert.Equal("Asc", SortOrder.Asc.ToString());
        Assert.Equal("Desc", SortOrder.Desc.ToString());
    }
}
