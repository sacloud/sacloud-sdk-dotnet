using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementCountAttributeのテスト
/// </summary>
public class LocalizedArrayElementCountAttributeTests
{
    /// <summary>
    /// IsValidが最大要素数以下のコレクションを有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void IsValid_WithCountAtOrBelowMaximum_ReturnsTrue(int count)
    {
        var attribute = new LocalizedArrayElementCountAttribute(2);

        Assert.True(attribute.IsValid(Enumerable.Range(0, count).ToArray()));
    }

    /// <summary>
    /// IsValidが最大要素数を超えるコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithCountAboveMaximum_ReturnsFalse()
    {
        var attribute = new LocalizedArrayElementCountAttribute(2);

        Assert.False(attribute.IsValid(new[] { 1, 2, 3 }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementCountAttribute(2);

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }
}