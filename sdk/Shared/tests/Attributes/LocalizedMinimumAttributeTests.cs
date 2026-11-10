using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedMinimumAttributeのテスト
/// </summary>
public class LocalizedMinimumAttributeTests
{
    /// <summary>
    /// IsValidが最小値以上の値を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    public void IsValid_WithValueAtOrAboveMinimum_ReturnsTrue(int value)
    {
        var attribute = new LocalizedMinimumAttribute(-10);

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが最小値を下回る値を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithValueBelowMinimum_ReturnsFalse()
    {
        var attribute = new LocalizedMinimumAttribute(-10);

        Assert.False(attribute.IsValid(-11));
    }

    /// <summary>
    /// FormatErrorMessageが日本語の最小値エラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedMinimumAttribute(-10);

        Assert.Equal("値の値は-10以上で入力してください。", attribute.FormatErrorMessage("値"));
    }
}