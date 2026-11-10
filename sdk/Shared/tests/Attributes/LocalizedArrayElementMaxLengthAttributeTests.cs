using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementMaxLengthAttributeのテスト
/// </summary>
public class LocalizedArrayElementMaxLengthAttributeTests
{
    /// <summary>
    /// IsValidが最大文字数以下の要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithElementsAtOrBelowMaximum_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementMaxLengthAttribute(3);

        Assert.True(attribute.IsValid(new[] { "", "abc" }));
    }

    /// <summary>
    /// IsValidが最大文字数を超える要素を含むコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithElementAboveMaximum_ReturnsFalse()
    {
        var attribute = new LocalizedArrayElementMaxLengthAttribute(3);

        Assert.False(attribute.IsValid(new[] { "abc", "abcd" }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementMaxLengthAttribute(3);

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の要素長エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedArrayElementMaxLengthAttribute(3);

        Assert.Equal("名前の各要素は3文字以下で入力してください。", attribute.FormatErrorMessage("名前"));
    }
}