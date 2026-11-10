using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedUniqueArrayAttributeのテスト
/// </summary>
public class LocalizedUniqueArrayAttributeTests
{
    /// <summary>
    /// IsValidが重複のないコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithUniqueItems_ReturnsTrue()
    {
        var attribute = new LocalizedUniqueArrayAttribute();

        Assert.True(attribute.IsValid(new[] { "first", "second" }));
    }

    /// <summary>
    /// IsValidが重複要素のあるコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDuplicateItems_ReturnsFalse()
    {
        var attribute = new LocalizedUniqueArrayAttribute();

        Assert.False(attribute.IsValid(new[] { "duplicate", "duplicate" }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedUniqueArrayAttribute();

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }
}