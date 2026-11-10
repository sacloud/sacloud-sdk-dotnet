using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementSpecificCountAttributeのテスト
/// </summary>
public class LocalizedArrayElementSpecificCountAttributeTests
{
    /// <summary>
    /// IsValidが指定要素数のコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithExactCount_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementSpecificCountAttribute(2);

        Assert.True(attribute.IsValid(new[] { 1, 2 }));
    }

    /// <summary>
    /// IsValidが指定要素数と異なるコレクションを無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public void IsValid_WithDifferentCount_ReturnsFalse(int count)
    {
        var attribute = new LocalizedArrayElementSpecificCountAttribute(2);

        Assert.False(attribute.IsValid(Enumerable.Range(0, count).ToArray()));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementSpecificCountAttribute(2);

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }
}