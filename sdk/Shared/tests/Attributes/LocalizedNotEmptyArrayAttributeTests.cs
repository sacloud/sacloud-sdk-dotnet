using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedNotEmptyArrayAttributeのテスト
/// </summary>
public class LocalizedNotEmptyArrayAttributeTests
{
    /// <summary>
    /// IsValidが要素のあるコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNonEmptyCollection_ReturnsTrue()
    {
        var attribute = new LocalizedNotEmptyArrayAttribute();

        Assert.True(attribute.IsValid(new[] { "value" }));
    }

    /// <summary>
    /// IsValidが空のコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithEmptyCollection_ReturnsFalse()
    {
        var attribute = new LocalizedNotEmptyArrayAttribute();

        Assert.False(attribute.IsValid(Array.Empty<string>()));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedNotEmptyArrayAttribute();

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }
}