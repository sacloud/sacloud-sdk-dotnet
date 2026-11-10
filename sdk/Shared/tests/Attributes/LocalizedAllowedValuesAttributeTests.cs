using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedAllowedValuesAttributeのテスト
/// </summary>
public class LocalizedAllowedValuesAttributeTests
{
    /// <summary>
    /// IsValidが許可値を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("active")]
    [InlineData("inactive")]
    public void IsValid_WithAllowedValue_ReturnsTrue(string value)
    {
        var attribute = new LocalizedAllowedValuesAttribute("active", "inactive");

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが許可されていない値を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDisallowedValue_ReturnsFalse()
    {
        var attribute = new LocalizedAllowedValuesAttribute("active", "inactive");

        Assert.False(attribute.IsValid("unknown"));
    }

    /// <summary>
    /// IsValidがnullを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullValue_ReturnsTrue()
    {
        var attribute = new LocalizedAllowedValuesAttribute("active");

        Assert.True(attribute.IsValid(null));
    }

    /// <summary>
    /// FormatErrorMessageが許可値を含む日本語メッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessageWithAllowedValues()
    {
        var attribute = new LocalizedAllowedValuesAttribute("active", "inactive");

        Assert.Equal("状態には許可された値（active, inactive）を入力してください。", attribute.FormatErrorMessage("状態"));
    }
}