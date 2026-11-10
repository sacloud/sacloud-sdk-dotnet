using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedIpv4Addressのテスト
/// </summary>
public class LocalizedIpv4AddressTests
{
    /// <summary>
    /// IsValidが有効なIPv4アドレスを有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("0.0.0.0")]
    [InlineData("192.168.1.1")]
    [InlineData("255.255.255.255")]
    public void IsValid_WithValidIpv4Address_ReturnsTrue(string value)
    {
        var attribute = new LocalizedIpv4Address();

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidが不正なIPv4形式を無効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("256.1.1.1")]
    [InlineData("192.168.1")]
    [InlineData("192.168.001.1")]
    [InlineData("2001:db8::1")]
    [InlineData("192.168.1.1extra")]
    public void IsValid_WithInvalidIpv4Address_ReturnsFalse(string value)
    {
        var attribute = new LocalizedIpv4Address();

        Assert.False(attribute.IsValid(value));
    }

    /// <summary>
    /// IsValidがnullおよび空白値を有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_WithMissingValue_ReturnsTrue(string? value)
    {
        var attribute = new LocalizedIpv4Address();

        Assert.True(attribute.IsValid(value));
    }

    /// <summary>
    /// FormatErrorMessageが日本語のエラーメッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessage()
    {
        var attribute = new LocalizedIpv4Address();

        Assert.Equal("IPアドレスには有効なIPv4アドレス形式を入力してください。", attribute.FormatErrorMessage("IPアドレス"));
    }
}