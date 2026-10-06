using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Metering.Buckets;
using Sacloud.Sdk.ObjectStorage.Metering.Buckets.Item;

namespace Sacloud.Sdk.ObjectStorage.Tests.Metering.Buckets;

/// <summary>
/// BucketsRequestBuilder（Metering）のテスト
/// </summary>
public class MeteringBucketsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<BucketsRequestBuilder> _mockLogger;

    public MeteringBucketsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<BucketsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（pathParameters版）が正常に初期化されることを確認
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

        var builder = new BucketsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new BucketsRequestBuilder("https://api.example.com/metering/buckets", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithNameItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithName_ReturnsWithNameItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new BucketsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item = builder["my-bucket"];

        Assert.NotNull(item);
        Assert.IsType<WithNameItemRequestBuilder>(item);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstanceOnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new BucketsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item1 = builder["my-bucket"];
        var item2 = builder["my-bucket"];

        Assert.NotSame(item1, item2);
    }
}
