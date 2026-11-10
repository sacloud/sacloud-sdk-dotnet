using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedMinLengthAttributeのテスト
/// </summary>
public class LocalizedMinLengthAttributeTests
{
    /// <summary>
    /// IsValidが最小文字数以上の文字列を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("abc")]
    [InlineData("abcd")]
    public void IsValid_WithLengthAtOrAboveMinimum_ReturnsTrue(string value)
    {
        var attribute = new LocalizedMinLengthAttribute(3);

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが最小文字数未満の文字列を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public void IsValid_WithLengthBelowMinimum_ReturnsFalse(string value)
    {
        var attribute = new LocalizedMinLengthAttribute(3);

        Assert.False(attribute.IsValid(value));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedMinLengthAttribute(3);

        Assert.Equal("名前は3文字以上で入力してください。", attribute.FormatErrorMessage("名前"));
    }
}