using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Response;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes.Item.Response;

/// <summary>
/// ResponseRequestBuilderのテスト
/// </summary>
public class ResponseRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ResponseRequestBuilder> _mockLogger;

    public ResponseRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ResponseRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ResponseRequestBuilder>().Returns(_mockLogger);
    }

    private static Dictionary<string, object> CreatePathParameters() =>
        new()
        {
            { "baseurl", "https://api.example.com" },
            { "serviceId", Guid.NewGuid() },
            { "routeId", Guid.NewGuid() }
        };

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new ResponseRequestBuilder(nullPath, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ResponseRequestBuilder(CreatePathParameters(), null, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder("https://api.example.com/services/x/routes/y/response", requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new ResponseRequestBuilder(nullUrl, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ResponseRequestBuilder("https://api.example.com/services/x/routes/y/response", null, _mockLoggerFactory));
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.Contains("response", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// GetAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var response = new ResponseGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.GetAsync();
        Assert.NotNull(result);
        Assert.IsType<ResponseGetResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ResponseGetResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.GetAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ResponseGetResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.GetAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 401 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "unauthorized" },
            { "error_msg", "error-unauthorized" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "401 Unauthorized" }
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートのレスポンス変換情報取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートのレスポンス変換情報の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null));
    }

    /// <summary>
    /// ToPutRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsCorrectUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.Contains("response", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// PutAsyncがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
    }

    /// <summary>
    /// PutAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();

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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();

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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();
        var expectedException = new ApiError { ResponseStatusCode = 401 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "unauthorized" },
            { "error_msg", "error-unauthorized" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "401 Unauthorized" }
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
                @"^指定したルートのレスポンス変換情報登録のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();
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
                @"^指定したルートのレスポンス変換情報の登録中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var newBuilder = builder.WithUrl("https://api.example.com/services/x/routes/y/response");
        Assert.NotNull(newBuilder);
        Assert.IsType<ResponseRequestBuilder>(newBuilder);
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ResponseGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのレスポンス変換情報の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ResponseGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ResponseGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのレスポンス変換情報の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのレスポンス変換情報の登録を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation();

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのレスポンス変換情報の登録が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
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
        var builder = new ResponseRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new ResponseRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
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
        var builder = new ResponseRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new ResponseTransformation()));
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
        var builder = new ResponseRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new ResponseTransformation()));
    }

    /// <summary>
    /// PutAsyncがAllow.JsonKeys要素の長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAllowJsonKeysElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Allow = new ResponseAllowDetail
            {
                JsonKeys = [new string('a', 256)]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("JsonKeysの各要素は255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAllow.JsonKeys要素の形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAllowJsonKeysElementInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Allow = new ResponseAllowDetail
            {
                JsonKeys = ["invalid key"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("JsonKeysの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.HeaderKeys要素の長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveHeaderKeysElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Remove = new ResponseRemoveDetail
            {
                HeaderKeys = [new string('a', 256)]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("HeaderKeysの各要素は255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.HeaderKeys要素の形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveHeaderKeysElementInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Remove = new ResponseRemoveDetail
            {
                HeaderKeys = ["invalid key"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("HeaderKeysの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.JsonKeys要素の長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveJsonKeysElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Remove = new ResponseRemoveDetail
            {
                JsonKeys = [new string('a', 256)]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("JsonKeysの各要素は255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.JsonKeys要素の形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveJsonKeysElementInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Remove = new ResponseRemoveDetail
            {
                JsonKeys = ["invalid key"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("JsonKeysの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.IfStatusCode要素の範囲外値でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveIfStatusCodeOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Remove = new ResponseRemoveDetail
            {
                IfStatusCode = [99]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("IfStatusCodeの各要素は100以上999以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Headers.Keyの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddHeadersKeyExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Add = new ResponseModificationDetail
            {
                Headers = [new ResponseModificationDetail_headers { Key = new string('a', 256), Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Headers.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddHeadersKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Add = new ResponseModificationDetail
            {
                Headers = [new ResponseModificationDetail_headers { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Headers.Valueの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddHeadersValueExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Add = new ResponseModificationDetail
            {
                Headers = [new ResponseModificationDetail_headers { Key = "X-Test", Value = new string('a', 256) }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Valueは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Headers.Valueの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddHeadersValueInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Add = new ResponseModificationDetail
            {
                Headers = [new ResponseModificationDetail_headers { Key = "X-Test", Value = "値" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Valueの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Json.Keyの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddJsonKeyExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Add = new ResponseModificationDetail
            {
                Json = [new ResponseModificationDetail_json { Key = new string('a', 256), Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Json.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddJsonKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Add = new ResponseModificationDetail
            {
                Json = [new ResponseModificationDetail_json { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.IfStatusCode要素の範囲外値でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddIfStatusCodeOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Add = new ResponseModificationDetail
            {
                IfStatusCode = [1000]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("IfStatusCodeの各要素は100以上999以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAppend.Headers.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAppendHeadersKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Append = new ResponseModificationDetail
            {
                Headers = [new ResponseModificationDetail_headers { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAppend.Json.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAppendJsonKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Append = new ResponseModificationDetail
            {
                Json = [new ResponseModificationDetail_json { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Headers.Fromの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameHeadersFromInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Rename = new ResponseRenameDetail
            {
                Headers = [new ResponseRenameDetail_headers { From = "invalid key", To = "X-New" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Headers.Toの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameHeadersToInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Rename = new ResponseRenameDetail
            {
                Headers = [new ResponseRenameDetail_headers { From = "X-Old", To = "invalid key" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Json.Fromの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameJsonFromInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Rename = new ResponseRenameDetail
            {
                Json = [new ResponseRenameDetail_json { From = "invalid key", To = "newKey" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Json.Toの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameJsonToInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Rename = new ResponseRenameDetail
            {
                Json = [new ResponseRenameDetail_json { From = "oldKey", To = "invalid key" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.IfStatusCode要素の範囲外値でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameIfStatusCodeOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Rename = new ResponseRenameDetail
            {
                IfStatusCode = [1000]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("IfStatusCodeの各要素は100以上999以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.Headers.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceHeadersKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Replace = new ResponseReplaceDetail
            {
                Headers = [new ResponseReplaceDetail_headers { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.Headers.Valueの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceHeadersValueInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Replace = new ResponseReplaceDetail
            {
                Headers = [new ResponseReplaceDetail_headers { Key = "X-Test", Value = "値" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Valueの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.Json.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceJsonKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Replace = new ResponseReplaceDetail
            {
                Json = [new ResponseReplaceDetail_json { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.IfStatusCode要素の範囲外値でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceIfStatusCodeOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ResponseRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ResponseTransformation
        {
            Replace = new ResponseReplaceDetail
            {
                IfStatusCode = [1000]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("IfStatusCodeの各要素は100以上999以下で入力してください。", exception.Message);
    }
}
