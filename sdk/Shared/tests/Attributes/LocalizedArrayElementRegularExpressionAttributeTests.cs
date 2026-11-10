using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementRegularExpressionAttributeのテスト
/// </summary>
public class LocalizedArrayElementRegularExpressionAttributeTests
{
    /// <summary>
    /// IsValidが正規表現に完全一致する要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithFullMatchElements_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementRegularExpressionAttribute("[a-z]+");

        Assert.True(attribute.IsValid(new[] { "abc", "xyz" }));
    }

    /// <summary>
    /// IsValidが正規表現に部分一致するだけの要素を含むコレクションを無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("xabc")]
    [InlineData("abcx")]
    public void IsValid_WithPartialMatchElement_ReturnsFalse(string invalidValue)
    {
        var attribute = new LocalizedArrayElementRegularExpressionAttribute("abc");

        Assert.False(attribute.IsValid(new[] { "abc", invalidValue }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementRegularExpressionAttribute("abc");

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }
}