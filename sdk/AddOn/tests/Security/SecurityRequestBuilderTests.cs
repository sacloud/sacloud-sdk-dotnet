using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.AddOn.Security.Ddos;
using Sacloud.Sdk.AddOn.Security.Vulnerability;
using Sacloud.Sdk.AddOn.Security.Waf;
using Sacloud.Sdk.AddOn.Security;

namespace Sacloud.Sdk.AddOn.Tests.Security;

/// <summary>
/// SecurityRequestBuilderのテスト
/// </summary>
public class SecurityRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;

    public SecurityRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };

        var builder = new SecurityRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/security";

        var builder = new SecurityRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Ddosプロパティが正しくDdosRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Ddos_ReturnsDdosRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SecurityRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Ddos;

        Assert.NotNull(result);
        Assert.IsType<DdosRequestBuilder>(result);
    }

    /// <summary>
    /// Vulnerabilityプロパティが正しくVulnerabilityRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Vulnerability_ReturnsVulnerabilityRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SecurityRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Vulnerability;

        Assert.NotNull(result);
        Assert.IsType<VulnerabilityRequestBuilder>(result);
    }

    /// <summary>
    /// Wafプロパティが正しくWafRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Waf_ReturnsWafRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SecurityRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Waf;

        Assert.NotNull(result);
        Assert.IsType<WafRequestBuilder>(result);
    }
}
