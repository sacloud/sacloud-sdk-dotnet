using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementRangeAttributeのテスト
/// </summary>
public class LocalizedArrayElementRangeAttributeTests
{
    /// <summary>
    /// IsValidが範囲内の要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithElementsInRange_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementRangeAttribute(1, 10);

        Assert.True(attribute.IsValid(new[] { 1, 5, 10 }));
    }

    /// <summary>
    /// IsValidが範囲外の要素を含むコレクションを無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void IsValid_WithElementOutsideRange_ReturnsFalse(int invalidValue)
    {
        var attribute = new LocalizedArrayElementRangeAttribute(1, 10);

        Assert.False(attribute.IsValid(new[] { 5, invalidValue }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementRangeAttribute(1, 10);

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の要素範囲エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedArrayElementRangeAttribute(1, 10);

        Assert.Equal("値の各要素は1以上10以下で入力してください。", attribute.FormatErrorMessage("値"));
    }
}