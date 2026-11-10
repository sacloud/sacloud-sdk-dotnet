using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedRangeAttributeのテスト
/// </summary>
public class LocalizedRangeAttributeTests
{
    /// <summary>
    /// IsValidが範囲の境界値を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void IsValid_WithBoundaryValue_ReturnsTrue(int value)
    {
        var attribute = new LocalizedRangeAttribute(1, 10);

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが範囲外の値を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void IsValid_WithOutOfRangeValue_ReturnsFalse(int value)
    {
        var attribute = new LocalizedRangeAttribute(1, 10);

        Assert.False(attribute.IsValid(value));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の範囲エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedRangeAttribute(1, 10);

        Assert.Equal("値の値は1以上10以下で入力してください。", attribute.FormatErrorMessage("値"));
    }
}