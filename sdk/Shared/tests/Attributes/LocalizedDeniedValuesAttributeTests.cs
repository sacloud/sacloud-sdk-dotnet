using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedDeniedValuesAttributeのテスト
/// </summary>
public class LocalizedDeniedValuesAttributeTests
{
    /// <summary>
    /// IsValidが拒否値を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDeniedValue_ReturnsFalse()
    {
        var attribute = new LocalizedDeniedValuesAttribute("blocked", "disabled");

        Assert.False(attribute.IsValid("blocked"));
    }

    /// <summary>
    /// IsValidが拒否値以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithAllowedValue_ReturnsTrue()
    {
        var attribute = new LocalizedDeniedValuesAttribute("blocked", "disabled");

        Assert.True(attribute.IsValid("active"));
    }

    /// <summary>
    /// IsValidがnullを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullValue_ReturnsTrue()
    {
        var attribute = new LocalizedDeniedValuesAttribute("blocked");

        Assert.True(attribute.IsValid(null));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedDeniedValuesAttribute("blocked");

        Assert.Equal("状態には許可された値を入力してください。", attribute.FormatErrorMessage("状態"));
    }
}