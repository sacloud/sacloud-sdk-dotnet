using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Metrics;
using Sacloud.Sdk.MonitoringSuite.Metrics.Routings;
using Sacloud.Sdk.MonitoringSuite.Metrics.Storages;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Metrics;

/// <summary>
/// MetricsRequestBuilderのテスト
/// </summary>
public class MetricsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public MetricsRequestBuilderTests()
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

        var builder = new MetricsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new MetricsRequestBuilder("https://api.example.com/metrics", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// RoutingsプロパティがRoutingsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Routings_ReturnsRoutingsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MetricsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Routings);
        Assert.IsType<RoutingsRequestBuilder>(builder.Routings);
    }

    /// <summary>
    /// StoragesプロパティがStoragesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Storages_ReturnsStoragesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MetricsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Storages);
        Assert.IsType<StoragesRequestBuilder>(builder.Storages);
    }
}
