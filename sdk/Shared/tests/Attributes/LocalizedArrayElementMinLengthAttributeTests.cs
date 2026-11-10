using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementMinLengthAttributeのテスト
/// </summary>
public class LocalizedArrayElementMinLengthAttributeTests
{
    /// <summary>
    /// IsValidが最小文字数以上の要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithElementsAtOrAboveMinimum_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementMinLengthAttribute(2);

        Assert.True(attribute.IsValid(new[] { "ab", "longer" }));
    }

    /// <summary>
    /// IsValidが最小文字数未満の要素を含むコレクションを無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("a")]
    public void IsValid_WithElementBelowMinimum_ReturnsFalse(string invalidValue)
    {
        var attribute = new LocalizedArrayElementMinLengthAttribute(2);

        Assert.False(attribute.IsValid(new[] { "ab", invalidValue }));
    }

    /// <summary>
    /// IsValidが最小文字数を大きく超える要素も有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithVeryLongElement_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementMinLengthAttribute(1);

        Assert.True(attribute.IsValid(new[] { new string('a', 4096) }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementMinLengthAttribute(1);

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の最小文字数エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedArrayElementMinLengthAttribute(2);

        Assert.Equal("名前の各要素は2文字以上で入力してください。", attribute.FormatErrorMessage("名前"));
    }
}