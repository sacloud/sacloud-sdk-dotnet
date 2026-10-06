using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.KMS.Kms;
using Sacloud.Sdk.KMS.Kms.Keys;

namespace Sacloud.Sdk.KMS.Tests.Kms;

/// <summary>
/// KmsRequestBuilderのテスト
/// </summary>
public class KmsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public KmsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(Substitute.For<ILogger>());
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        Assert.Throws<ArgumentNullException>(() => new KmsRequestBuilder(pathParameters, requestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/kms";

        Assert.Throws<ArgumentNullException>(() => new KmsRequestBuilder(rawUrl, requestAdapter, null!));
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

        var builder = new KmsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/kms";

        var builder = new KmsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Keysプロパティが正しいKeysRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Keys_ReturnsKeysRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KmsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var keysBuilder = builder.Keys;

        Assert.NotNull(keysBuilder);
        Assert.IsType<KeysRequestBuilder>(keysBuilder);
    }

    /// <summary>
    /// Keysプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Keys_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KmsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var keysBuilder1 = builder.Keys;
        var keysBuilder2 = builder.Keys;

        Assert.NotSame(keysBuilder1, keysBuilder2);
    }
}
