using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Management;
using Sacloud.Sdk.MonitoringSuite.Management.Limits;
using Sacloud.Sdk.MonitoringSuite.Management.Provisioning;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Management;

/// <summary>
/// ManagementRequestBuilderのテスト
/// </summary>
public class ManagementRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public ManagementRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        var builder = new ManagementRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var builder = new ManagementRequestBuilder("https://api.example.com/management", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// LimitsプロパティがLimitsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Limits_ReturnsLimitsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ManagementRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Limits);
        Assert.IsType<LimitsRequestBuilder>(builder.Limits);
    }

    /// <summary>
    /// ProvisioningプロパティがProvisioningRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Provisioning_ReturnsProvisioningRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ManagementRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Provisioning);
        Assert.IsType<ProvisioningRequestBuilder>(builder.Provisioning);
    }
}
