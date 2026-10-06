using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Authorization;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Request;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Response;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes.Item;

/// <summary>
/// WithRouteItemRequestBuilderのテスト
/// </summary>
public class WithRouteItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithRouteItemRequestBuilder> _mockLogger;

    public WithRouteItemRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<WithRouteItemRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<WithRouteItemRequestBuilder>().Returns(_mockLogger);
    }

    private static Dictionary<string, object> CreatePathParameters() =>
        new()
        {
            { "baseurl", "https://api.example.com" },
            { "serviceId", Guid.NewGuid() },
            { "routeId", Guid.NewGuid() }
        };

    private static RouteDetail CreateValidRouteDetail() =>
        new()
        {
            Name = "test-route",
            Path = "/test",
            Methods = [HTTPMethod.GET],
            Protocols = Route_protocols.Https,
        };

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのPathParametersで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullPathParameters_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        Dictionary<string, object>? nullPath = null;
        Assert.Throws<ArgumentNullException>(() => new WithRouteItemRequestBuilder(nullPath, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WithRouteItemRequestBuilder(CreatePathParameters(), null, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder($"https://api.example.com/services/{Guid.NewGuid()}/routes/{Guid.NewGuid()}", requestAdapter, _mockLoggerFactory);
        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのrawUrlで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRawUrl_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        string? nullUrl = null;
        Assert.Throws<ArgumentNullException>(() => new WithRouteItemRequestBuilder(nullUrl, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WithRouteItemRequestBuilder($"https://api.example.com/services/{Guid.NewGuid()}/routes/{Guid.NewGuid()}", null, _mockLoggerFactory));
    }

    /// <summary>
    /// Authorizationプロパティが正しいAuthorizationRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Authorization_ReturnsAuthorizationRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var auth = builder.Authorization;
        Assert.NotNull(auth);
        Assert.IsType<AuthorizationRequestBuilder>(auth);
    }

    /// <summary>
    /// Requestプロパティが正しいRequestRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Request_ReturnsRequestRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var request = builder.Request;
        Assert.NotNull(request);
        Assert.IsType<RequestRequestBuilder>(request);
    }

    /// <summary>
    /// Responseプロパティが正しいResponseRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Response_ReturnsResponseRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var response = builder.Response;
        Assert.NotNull(response);
        Assert.IsType<ResponseRequestBuilder>(response);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("routes", requestInfo.UrlTemplate);
        Assert.Contains("routeId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// GetAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var response = new WithRouteGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.GetAsync();
        Assert.NotNull(result);
        Assert.IsType<WithRouteGetResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncがrequestConfigurationを伝播することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_PropagatesRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithRouteGetResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.GetAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncがCancellationTokenを伝播することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_PropagatesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithRouteGetResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.GetAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            token);
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "route-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートの詳細情報取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, route\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートの詳細情報の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToDeleteRequestInformation();
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToDeleteRequestInformation();
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("routes", requestInfo.UrlTemplate);
        Assert.Contains("routeId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToDeleteRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToDeleteRequestInformation();
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// DeleteAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        await builder.DeleteAsync();
        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// DeleteAsyncがrequestConfigurationを伝播することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_PropagatesRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.DeleteAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// DeleteAsyncがCancellationTokenを伝播することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_PropagatesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.DeleteAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            token);
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "route-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルート削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, route\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationがnull bodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null));
    }

    /// <summary>
    /// ToPutRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("routes", requestInfo.UrlTemplate);
        Assert.Contains("routeId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// PutAsyncがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
    }

    /// <summary>
    /// PutAsyncのリクエストボディのNameがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Name = name
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Nameが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのNameに無効な文字が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidNameCharacters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Name = "invalid route"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Nameの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのMethodsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullMethods_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Methods = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Methodsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのMethods要素に無効なEnum値が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidMethodsElement_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Methods = [(HTTPMethod)999]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Methods", exception.Message);
        Assert.Contains("Enum", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのHttpsRedirectStatusCodeが許可値以外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidHttpsRedirectStatusCode_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            HttpsRedirectStatusCode = 999
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("HttpsRedirectStatusCode", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのPathが255文字を超過する場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithPathExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Path = "/" + new string('a', 255)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Pathは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのPathの形式が不正な場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidPathFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Path = "invalid-path"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Pathの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのProtocolsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullProtocols_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Protocols = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Protocolsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのRegexPriorityが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRegexPriorityOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            RegexPriority = 256
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("RegexPriorityの値は0以上255以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのTags要素が255文字を超過する場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithTagElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Tags = [new string('a', 256)]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Tagsの各要素は1文字以上255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのTags要素に無効な文字が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithTagElementInvalidCharacters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Tags = ["invalid tag"]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Tagsの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのIpRestrictionConfig.Ipsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullIpRestrictionIps_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Ips = null
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Ipsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのIpRestrictionConfig.Ipsが空配列の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithEmptyIpRestrictionIps_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Ips = [],
                Protocols = IpRestrictionConfig_protocols.Https,
                RestrictedBy = IpRestrictionConfig_restrictedBy.AllowIps,
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Ipsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのIpRestrictionConfig.Ips要素の形式が不正な場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidIpRestrictionIpsElement_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Ips = ["999.0.0.1"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Ipsの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのIpRestrictionConfig.Protocolsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullIpRestrictionProtocols_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Protocols = null
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Protocolsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのIpRestrictionConfig.RestrictedByがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullIpRestrictionRestrictedBy_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                RestrictedBy = null
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("RestrictedByが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PutAsyncがrequestConfigurationを伝播することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_PropagatesRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.PutAsync(body, config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PutAsyncがCancellationTokenを伝播することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_PropagatesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.PutAsync(body, cancellationToken: token);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            token);
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Name = "test-route",
            Methods = [HTTPMethod.GET],
            Protocols = Route_protocols.Https,
        };
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "route-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートの詳細情報更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, route\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Name = "test-route",
            Methods = [HTTPMethod.GET],
            Protocols = Route_protocols.Https,
        };
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.PutAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートの詳細情報の更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが正しい新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var newBuilder = builder.WithUrl($"https://api.example.com/services/{Guid.NewGuid()}/routes/{Guid.NewGuid()}");
        Assert.NotNull(newBuilder);
        Assert.IsType<WithRouteItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// GetAsyncが成功時に開始ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_Success_LogsStartMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithRouteGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートの詳細情報の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが成功時に完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_Success_LogsCompletionMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithRouteGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithRouteGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートの詳細情報の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncが成功時に開始ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_Success_LogsStartMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncが成功時に完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_Success_LogsCompletionMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが成功時に開始ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_Success_LogsStartMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^指定したルートの詳細情報の更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが成功時に完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_Success_LogsCompletionMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithRouteItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^指定したルートの詳細情報の更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが不正なserviceIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidServiceId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["serviceId"] = "invalid-guid";
        var builder = new WithRouteItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// GetAsyncが不正なrouteIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidRouteId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["routeId"] = "invalid-guid";
        var builder = new WithRouteItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// DeleteAsyncが不正なserviceIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidServiceId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["serviceId"] = "invalid-guid";
        var builder = new WithRouteItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());
    }

    /// <summary>
    /// DeleteAsyncが不正なrouteIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidRouteId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["routeId"] = "invalid-guid";
        var builder = new WithRouteItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());
    }

    /// <summary>
    /// PutAsyncが不正なserviceIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidServiceId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["serviceId"] = "invalid-guid";
        var builder = new WithRouteItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(CreateValidRouteDetail()));
    }

    /// <summary>
    /// PutAsyncが不正なrouteIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidRouteId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["routeId"] = "invalid-guid";
        var builder = new WithRouteItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(CreateValidRouteDetail()));
    }
}
