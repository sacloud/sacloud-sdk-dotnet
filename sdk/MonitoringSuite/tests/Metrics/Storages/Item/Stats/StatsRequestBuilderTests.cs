using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Metrics.Storages.Item.Stats;
using Sacloud.Sdk.MonitoringSuite.Metrics.Storages.Item.Stats.Daily;
using Sacloud.Sdk.MonitoringSuite.Metrics.Storages.Item.Stats.Monthly;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Metrics.Storages.Item.Stats;

/// <summary>
/// StatsRequestBuilderのテスト
/// </summary>
public class StatsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public StatsRequestBuilderTests()
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
            { "baseurl", "https://api.example.com" },
            { "metricsResourceId", 123L }
        };

        var builder = new StatsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new StatsRequestBuilder("https://api.example.com/metrics/storages/123/stats", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// DailyプロパティがDailyRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Daily_ReturnsDailyRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "metricsResourceId", 123L }
        };
        var builder = new StatsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Daily);
        Assert.IsType<DailyRequestBuilder>(builder.Daily);
    }

    /// <summary>
    /// MonthlyプロパティがMonthlyRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Monthly_ReturnsMonthlyRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "metricsResourceId", 123L }
        };
        var builder = new StatsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Monthly);
        Assert.IsType<MonthlyRequestBuilder>(builder.Monthly);
    }
}
