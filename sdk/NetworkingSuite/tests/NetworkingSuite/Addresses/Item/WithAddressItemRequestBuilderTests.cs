using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.Models;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Addresses.Item;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.NetworkingSuite.Tests.NetworkingSuite.Addresses.Item;

/// <summary>
/// WithAddressItemRequestBuilderのテスト
/// </summary>
public class WithAddressItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithAddressItemRequestBuilder> _mockLogger;

    public WithAddressItemRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithAddressItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithAddressItemRequestBuilder>().Returns(_mockLogger);
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
            { "addressid", "test-addressid" },
        };

        var builder = new WithAddressItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };

        Assert.Throws<ArgumentNullException>(() => new WithAddressItemRequestBuilder(pathParameters, _mockRequestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/networking-suite/addresses/test-addressid";

        var builder = new WithAddressItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/networking-suite/addresses/test-addressid";

        Assert.Throws<ArgumentNullException>(() => new WithAddressItemRequestBuilder(rawUrl, _mockRequestAdapter, null));
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("addressid"));
        Assert.Equal("test-addressid", requestInfo.PathParameters["addressid"]);
    }

    /// <summary>
    /// GetAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new AddressResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<AddressResponse>(result);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス詳細の取得を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス詳細の取得が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncで無効なアドレスIDを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAsync_WithNullOrEmptyOrWhitespaceAddressId_ThrowsValidationException(string? invalidAddressId)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", invalidAddressId },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("アドレスIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アドレスIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncが"XXX"キーに汎用ApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new AddressResponse());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x =>
                x.ContainsKey("XXX")
                && x["XXX"] == ApiError.CreateFromDiscriminatorValue),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncで汎用ApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<AddressResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス詳細取得のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<AddressResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス詳細の取得中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "addressid", "test-addressid" },
        };
        var builder = new WithAddressItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/networking-suite/addresses/new-addressid";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WithAddressItemRequestBuilder>(newBuilder);
    }
}
