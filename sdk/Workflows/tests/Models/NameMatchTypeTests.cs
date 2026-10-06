using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// NameMatchType Enumのテスト
/// </summary>
public class NameMatchTypeTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void NameMatchType_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(NameMatchType));

        Assert.Equal(2, values.Length);
        Assert.Contains(NameMatchType.Partial, values.Cast<NameMatchType>());
        Assert.Contains(NameMatchType.Prefix, values.Cast<NameMatchType>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void NameMatchType_ToString_ReturnsCorrectName()
    {
        Assert.Equal("Partial", NameMatchType.Partial.ToString());
        Assert.Equal("Prefix", NameMatchType.Prefix.ToString());
    }
}
