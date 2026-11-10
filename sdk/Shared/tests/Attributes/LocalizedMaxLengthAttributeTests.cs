using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedMaxLengthAttributeのテスト
/// </summary>
public class LocalizedMaxLengthAttributeTests
{
    /// <summary>
    /// IsValidが最大文字数以下の文字列を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    public void IsValid_WithLengthAtOrBelowMaximum_ReturnsTrue(string value)
    {
        var attribute = new LocalizedMaxLengthAttribute(3);

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが最大文字数を超える文字列を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithLengthAboveMaximum_ReturnsFalse()
    {
        var attribute = new LocalizedMaxLengthAttribute(3);

        Assert.False(attribute.IsValid("abcd"));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedMaxLengthAttribute(3);

        Assert.Equal("名前は3文字以下で入力してください。", attribute.FormatErrorMessage("名前"));
    }
}