using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.IAM.ServicePrincipals.Oauth2;

namespace Sacloud.Sdk.IAM.Tests.ServicePrincipals.Oauth2;

/// <summary>
/// Oauth2RequestBuilderのテスト
/// </summary>
public class Oauth2RequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<Oauth2RequestBuilder> _mockLogger;

    public Oauth2RequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<Oauth2RequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<Oauth2RequestBuilder>().Returns(_mockLogger);
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

        var builder = new Oauth2RequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/service-principals/oauth2";

        var builder = new Oauth2RequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタにnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        Assert.Throws<ArgumentNullException>(() =>
            new Oauth2RequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタにnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var rawUrl = "https://api.example.com/service-principals/oauth2";

        Assert.Throws<ArgumentNullException>(() =>
            new Oauth2RequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// Tokenプロパティが正しいTokenRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Token_ReturnsTokenRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new Oauth2RequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var tokenBuilder = builder.Token;

        Assert.NotNull(tokenBuilder);
    }
}
