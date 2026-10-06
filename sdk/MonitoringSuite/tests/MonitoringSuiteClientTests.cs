using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Alerts;
using Sacloud.Sdk.MonitoringSuite.Dashboards;
using Sacloud.Sdk.MonitoringSuite.Logs;
using Sacloud.Sdk.MonitoringSuite.Management;
using Sacloud.Sdk.MonitoringSuite.Metrics;
using Sacloud.Sdk.MonitoringSuite.Publishers;
using Sacloud.Sdk.MonitoringSuite.Traces;

namespace Sacloud.Sdk.MonitoringSuite.Tests;

/// <summary>
/// MonitoringSuiteClientのテスト
/// </summary>
public class MonitoringSuiteClientTests
{
    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client);
    }

    /// <summary>
    /// コンストラクタがBaseUrlを設定することを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsDefaultBaseUrl_WhenBaseUrlIsEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/zone/is1a/api/monitoring/1.0", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタが既存のBaseUrlを保持することを確認
    /// </summary>
    [Fact]
    public void Constructor_PreservesExistingBaseUrl_WhenBaseUrlIsNotEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var customBaseUrl = "https://custom.api.example.com";
        requestAdapter.BaseUrl = customBaseUrl;

        new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// AlertsプロパティがAlertsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Alerts_ReturnsAlertsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client.Alerts);
        Assert.IsType<AlertsRequestBuilder>(client.Alerts);
    }

    /// <summary>
    /// DashboardsプロパティがDashboardsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Dashboards_ReturnsDashboardsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client.Dashboards);
        Assert.IsType<DashboardsRequestBuilder>(client.Dashboards);
    }

    /// <summary>
    /// LogsプロパティがLogsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Logs_ReturnsLogsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client.Logs);
        Assert.IsType<LogsRequestBuilder>(client.Logs);
    }

    /// <summary>
    /// ManagementプロパティがManagementRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Management_ReturnsManagementRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client.Management);
        Assert.IsType<ManagementRequestBuilder>(client.Management);
    }

    /// <summary>
    /// MetricsプロパティがMetricsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Metrics_ReturnsMetricsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client.Metrics);
        Assert.IsType<MetricsRequestBuilder>(client.Metrics);
    }

    /// <summary>
    /// PublishersプロパティがPublishersRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Publishers_ReturnsPublishersRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client.Publishers);
        Assert.IsType<PublishersRequestBuilder>(client.Publishers);
    }

    /// <summary>
    /// TracesプロパティがTracesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Traces_ReturnsTracesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new MonitoringSuiteClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client.Traces);
        Assert.IsType<TracesRequestBuilder>(client.Traces);
    }
}
