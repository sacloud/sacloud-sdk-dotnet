using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Logs.Storages.Item.Stats;
using Sacloud.Sdk.MonitoringSuite.Logs.Storages.Item.Stats.Daily;
using Sacloud.Sdk.MonitoringSuite.Logs.Storages.Item.Stats.Monthly;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Logs.Storages.Item.Stats;

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

    private static Dictionary<string, object> PathParams() => new()
    {
        { "baseurl", "https://api.example.com" },
        { "logResourceId", 1L }
    };

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var builder = new StatsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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

        var builder = new StatsRequestBuilder("https://api.example.com/logs/storages/1/stats", requestAdapter, _mockLoggerFactory);

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
        var builder = new StatsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new StatsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Monthly);
        Assert.IsType<MonthlyRequestBuilder>(builder.Monthly);
    }
}
