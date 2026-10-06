using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Order Enumのテスト
/// </summary>
public class OrderTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Order_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(Order));

        Assert.Equal(2, values.Length);
        Assert.Contains(Order.Asc, values.Cast<Order>());
        Assert.Contains(Order.Desc, values.Cast<Order>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void Order_ToString_ReturnsCorrectName()
    {
        Assert.Equal("Asc", Order.Asc.ToString());
        Assert.Equal("Desc", Order.Desc.ToString());
    }
}
