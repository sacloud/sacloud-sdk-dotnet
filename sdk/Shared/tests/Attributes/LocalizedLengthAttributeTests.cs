using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedLengthAttributeのテスト
/// </summary>
public class LocalizedLengthAttributeTests
{
    /// <summary>
    /// IsValidが指定文字数と一致する文字列を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithExactLength_ReturnsTrue()
    {
        var attribute = new LocalizedLengthAttribute(3);

        Assert.True(attribute.IsValid("abc"));
    }

    /// <summary>
    /// IsValidが指定文字数と異なる文字列を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("abcd")]
    public void IsValid_WithDifferentLength_ReturnsFalse(string value)
    {
        var attribute = new LocalizedLengthAttribute(3);

        Assert.False(attribute.IsValid(value));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedLengthAttribute(3);

        Assert.Equal("名前は3文字で入力してください。", attribute.FormatErrorMessage("名前"));
    }
}