using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedLengthRangeAttributeのテスト
/// </summary>
public class LocalizedLengthRangeAttributeTests
{
    /// <summary>
    /// IsValidが最小および最大文字数の境界を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("ab")]
    [InlineData("abcd")]
    public void IsValid_WithBoundaryLength_ReturnsTrue(string value)
    {
        var attribute = new LocalizedLengthRangeAttribute(2, 4);

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが指定範囲外の文字数を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("a")]
    [InlineData("abcde")]
    public void IsValid_WithLengthOutsideRange_ReturnsFalse(string value)
    {
        var attribute = new LocalizedLengthRangeAttribute(2, 4);

        Assert.False(attribute.IsValid(value));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の範囲エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedLengthRangeAttribute(2, 4);

        Assert.Equal("名前は2文字以上4文字以下で入力してください。", attribute.FormatErrorMessage("名前"));
    }
}