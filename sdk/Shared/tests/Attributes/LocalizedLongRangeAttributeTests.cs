using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedLongRangeAttributeのテスト
/// </summary>
public class LocalizedLongRangeAttributeTests
{
    /// <summary>
    /// IsValidがintの範囲を超えるlong値の境界を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(5000000000L)]
    [InlineData(5000000010L)]
    public void IsValid_WithBoundaryValue_ReturnsTrue(long value)
    {
        var attribute = new LocalizedLongRangeAttribute(5000000000L, 5000000010L);

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidがlongの範囲外の値を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(4999999999L)]
    [InlineData(5000000011L)]
    public void IsValid_WithOutOfRangeValue_ReturnsFalse(long value)
    {
        var attribute = new LocalizedLongRangeAttribute(5000000000L, 5000000010L);

        Assert.False(attribute.IsValid(value));
    }
}