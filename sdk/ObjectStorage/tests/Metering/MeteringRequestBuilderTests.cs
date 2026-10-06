using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Metering;
using Sacloud.Sdk.ObjectStorage.Metering.Buckets;

namespace Sacloud.Sdk.ObjectStorage.Tests.Metering;

/// <summary>
/// MeteringRequestBuilderのテスト
/// </summary>
public class MeteringRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<MeteringRequestBuilder> _mockLogger;

    public MeteringRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<MeteringRequestBuilder>>();
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

        var builder = new MeteringRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new MeteringRequestBuilder("https://api.example.com/metering", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Bucketsプロパティが正しいBucketsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Buckets_ReturnsBucketsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MeteringRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Buckets);
        Assert.IsType<BucketsRequestBuilder>(builder.Buckets);
    }
}
