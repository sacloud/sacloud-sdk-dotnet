using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedRequiredAttributeのテスト
/// </summary>
public class LocalizedRequiredAttributeTests
{
    /// <summary>
    /// IsValidがnullまたは空白文字列を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_WithMissingValue_ReturnsFalse(string? value)
    {
        var attribute = new LocalizedRequiredAttribute();

        Assert.False(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが値のある文字列を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithValue_ReturnsTrue()
    {
        var attribute = new LocalizedRequiredAttribute();

        Assert.True(attribute.IsValid("value"));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedRequiredAttribute();

        Assert.Equal("名前が入力されていません。", attribute.FormatErrorMessage("名前"));
    }
}