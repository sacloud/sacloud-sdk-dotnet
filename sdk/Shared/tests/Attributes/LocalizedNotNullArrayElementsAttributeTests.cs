using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedNotNullArrayElementsAttributeのテスト
/// </summary>
public class LocalizedNotNullArrayElementsAttributeTests
{
    /// <summary>
    /// IsValidがnullでない要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNonNullElements_ReturnsTrue()
    {
        var attribute = new LocalizedNotNullArrayElementsAttribute();

        Assert.True(attribute.IsValid(new object[] { "first", 2 }));
    }

    /// <summary>
    /// IsValidがnull要素を含むコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullElement_ReturnsFalse()
    {
        var attribute = new LocalizedNotNullArrayElementsAttribute();
        object?[] values = { "first", null, "last" };

        Assert.False(attribute.IsValid(values));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedNotNullArrayElementsAttribute();

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }

    /// <summary>
    /// IsValidが空のコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithEmptyCollection_ReturnsTrue()
    {
        var attribute = new LocalizedNotNullArrayElementsAttribute();

        Assert.True(attribute.IsValid(Array.Empty<object>()));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedNotNullArrayElementsAttribute();

        Assert.Equal("値の各要素はnullにできません。", attribute.FormatErrorMessage("値"));
    }
}