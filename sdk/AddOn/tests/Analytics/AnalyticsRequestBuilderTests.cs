using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.AddOn.Analytics;
using Sacloud.Sdk.AddOn.Analytics.Datalake;
using Sacloud.Sdk.AddOn.Analytics.Dwh;
using Sacloud.Sdk.AddOn.Analytics.Etl;
using Sacloud.Sdk.AddOn.Analytics.Query;
using Sacloud.Sdk.AddOn.Analytics.Search;
using Sacloud.Sdk.AddOn.Analytics.Streaming;

namespace Sacloud.Sdk.AddOn.Tests.Analytics;

/// <summary>
/// AnalyticsRequestBuilderのテスト
/// </summary>
public class AnalyticsRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;

    public AnalyticsRequestBuilderTests()
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

        var builder = new AnalyticsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/analytics";

        var builder = new AnalyticsRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Datalakeプロパティが正しくDatalakeRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Datalake_ReturnsDatalakeRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AnalyticsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Datalake;

        Assert.NotNull(result);
        Assert.IsType<DatalakeRequestBuilder>(result);
    }

    /// <summary>
    /// Dwhプロパティが正しくDwhRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Dwh_ReturnsDwhRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AnalyticsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Dwh;

        Assert.NotNull(result);
        Assert.IsType<DwhRequestBuilder>(result);
    }

    /// <summary>
    /// Etlプロパティが正しくEtlRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Etl_ReturnsEtlRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AnalyticsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Etl;

        Assert.NotNull(result);
        Assert.IsType<EtlRequestBuilder>(result);
    }

    /// <summary>
    /// Queryプロパティが正しくQueryRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Query_ReturnsQueryRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AnalyticsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Query;

        Assert.NotNull(result);
        Assert.IsType<QueryRequestBuilder>(result);
    }

    /// <summary>
    /// Searchプロパティが正しくSearchRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Search_ReturnsSearchRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AnalyticsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Search;

        Assert.NotNull(result);
        Assert.IsType<SearchRequestBuilder>(result);
    }

    /// <summary>
    /// Streamingプロパティが正しくStreamingRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Streaming_ReturnsStreamingRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AnalyticsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Streaming;

        Assert.NotNull(result);
        Assert.IsType<StreamingRequestBuilder>(result);
    }
}
