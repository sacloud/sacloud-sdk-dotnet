using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedRegularExpressionAttributeのテスト
/// </summary>
public class LocalizedRegularExpressionAttributeTests
{
    private sealed class NullStringValue
    {
        public override string? ToString()
        {
            return null;
        }
    }

    /// <summary>
    /// IsValidが正規表現に完全一致する値を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    public void IsValid_WithFullMatch_ReturnsTrue(string value)
    {
        var attribute = new LocalizedRegularExpressionAttribute("[a-z]*");

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが部分一致のみの値を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("xabc")]
    [InlineData("abcx")]
    public void IsValid_WithPartialMatch_ReturnsFalse(string value)
    {
        var attribute = new LocalizedRegularExpressionAttribute("abc");

        Assert.False(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidがnullを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullValue_ReturnsTrue()
    {
        var attribute = new LocalizedRegularExpressionAttribute("abc");

        Assert.True(attribute.IsValid(null));
    }

    /// <summary>
    /// IsValidが文字列変換結果がnullの値を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullStringConversion_ReturnsTrue()
    {
        var attribute = new LocalizedRegularExpressionAttribute("abc");

        Assert.True(attribute.IsValid(new NullStringValue()));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedRegularExpressionAttribute("abc");

        Assert.Equal("名前の形式が正しくありません。", attribute.FormatErrorMessage("名前"));
    }
}