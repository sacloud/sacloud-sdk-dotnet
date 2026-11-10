using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementLengthRangeAttributeのテスト
/// </summary>
public class LocalizedArrayElementLengthRangeAttributeTests
{
    /// <summary>
    /// IsValidが文字数範囲内の要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithElementsInLengthRange_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementLengthRangeAttribute(2, 4);

        Assert.True(attribute.IsValid(new[] { "ab", "abcd" }));
    }

    /// <summary>
    /// IsValidが文字数範囲外の要素を含むコレクションを無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("a")]
    [InlineData("abcde")]
    public void IsValid_WithElementOutsideLengthRange_ReturnsFalse(string invalidValue)
    {
        var attribute = new LocalizedArrayElementLengthRangeAttribute(2, 4);

        Assert.False(attribute.IsValid(new[] { "abc", invalidValue }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementLengthRangeAttribute(2, 4);

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の要素長エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedArrayElementLengthRangeAttribute(2, 4);

        Assert.Equal("名前の各要素は2文字以上4文字以下で入力してください。", attribute.FormatErrorMessage("名前"));
    }
}