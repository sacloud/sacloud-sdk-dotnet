using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedMaximumAttributeのテスト
/// </summary>
public class LocalizedMaximumAttributeTests
{
    /// <summary>
    /// IsValidが最大値以下の値を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void IsValid_WithValueAtOrBelowMaximum_ReturnsTrue(int value)
    {
        var attribute = new LocalizedMaximumAttribute(10);

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが最大値を超える値を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithValueAboveMaximum_ReturnsFalse()
    {
        var attribute = new LocalizedMaximumAttribute(10);

        Assert.False(attribute.IsValid(11));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の最大値エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedMaximumAttribute(10);

        Assert.Equal("値の値は10以下で入力してください。", attribute.FormatErrorMessage("値"));
    }
}