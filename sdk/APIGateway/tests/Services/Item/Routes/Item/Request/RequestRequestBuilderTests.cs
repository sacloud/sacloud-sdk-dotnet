using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Request;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes.Item.Request;

/// <summary>
/// RequestRequestBuilderのテスト
/// </summary>
public class RequestRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<RequestRequestBuilder> _mockLogger;

    public RequestRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<RequestRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<RequestRequestBuilder>().Returns(_mockLogger);
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new RequestRequestBuilder(nullPath, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RequestRequestBuilder(CreatePathParameters(), null, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder("https://api.example.com/services/x/routes/y/request", requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new RequestRequestBuilder(nullUrl, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RequestRequestBuilder("https://api.example.com/services/x/routes/y/request", null, _mockLoggerFactory));
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsCorrectUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.Contains("request", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// GetAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var response = new RequestGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.GetAsync();
        Assert.NotNull(result);
        Assert.IsType<RequestGetResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RequestGetResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.GetAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RequestGetResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.GetAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートのリクエスト変換情報取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したルートのリクエスト変換情報の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.Contains("request", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    ///  PutAsyncがnull bodyでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
    }

    /// <summary>
    /// PutAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();

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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();

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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();
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
                @"^指定したルートのリクエスト変換情報登録のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();
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
                @"^指定したルートのリクエスト変換情報の登録中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var newBuilder = builder.WithUrl("https://api.example.com/services/x/routes/y/request");
        Assert.NotNull(newBuilder);
        Assert.IsType<RequestRequestBuilder>(newBuilder);
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RequestGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのリクエスト変換情報の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RequestGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RequestGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのリクエスト変換情報の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのリクエスト変換情報の登録を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation();

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したルートのリクエスト変換情報の登録が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
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
        var builder = new RequestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new RequestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new RequestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new RequestTransformation()));
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
        var builder = new RequestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new RequestTransformation()));
    }

    /// <summary>
    /// PutAsyncが不正なHttpMethodでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidHttpMethod_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            HttpMethod = (HTTPMethod)999
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("HttpMethodはEnumに含まれる値を入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAllow.Body要素の長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAllowBodyElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Allow = new RequestAllowDetail
            {
                Body = [new string('a', 256)]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Bodyの各要素は255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAllow.Body要素の形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAllowBodyElementInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Allow = new RequestAllowDetail
            {
                Body = ["invalid key"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Bodyの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.Body要素の長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveBodyElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Remove = new RequestRemoveDetail
            {
                Body = [new string('a', 256)]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Bodyの各要素は255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.Body要素の形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveBodyElementInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Remove = new RequestRemoveDetail
            {
                Body = ["invalid key"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Bodyの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.HeaderKeys要素の長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveHeaderKeysElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Remove = new RequestRemoveDetail
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
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Remove = new RequestRemoveDetail
            {
                HeaderKeys = ["invalid key"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("HeaderKeysの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.QueryParams要素の長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveQueryParamsElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Remove = new RequestRemoveDetail
            {
                QueryParams = [new string('a', 256)]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("QueryParamsの各要素は255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRemove.QueryParams要素の形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRemoveQueryParamsElementInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Remove = new RequestRemoveDetail
            {
                QueryParams = ["invalid query"]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("QueryParamsの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Body.Keyの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddBodyKeyExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Add = new RequestModificationDetail
            {
                Body = [new RequestModificationDetail_body { Key = new string('a', 256), Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAdd.Body.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAddBodyKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Add = new RequestModificationDetail
            {
                Body = [new RequestModificationDetail_body { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAppend.Headers.Keyの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAppendHeadersKeyExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Append = new RequestModificationDetail
            {
                Headers = [new RequestModificationDetail_headers { Key = new string('a', 256), Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAppend.Headers.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAppendHeadersKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Append = new RequestModificationDetail
            {
                Headers = [new RequestModificationDetail_headers { Key = "invalid key", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAppend.Headers.Valueの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAppendHeadersValueExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Append = new RequestModificationDetail
            {
                Headers = [new RequestModificationDetail_headers { Key = "X-Test", Value = new string('a', 256) }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Valueは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがAppend.Headers.Valueの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithAppendHeadersValueInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Append = new RequestModificationDetail
            {
                Headers = [new RequestModificationDetail_headers { Key = "X-Test", Value = "値" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Valueの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.QueryParams.Keyの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceQueryParamsKeyExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Replace = new RequestModificationDetail
            {
                QueryParams = [new RequestModificationDetail_queryParams { Key = new string('a', 256), Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.QueryParams.Keyの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceQueryParamsKeyInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Replace = new RequestModificationDetail
            {
                QueryParams = [new RequestModificationDetail_queryParams { Key = "invalid query", Value = "value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Keyの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.QueryParams.Valueの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceQueryParamsValueExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Replace = new RequestModificationDetail
            {
                QueryParams = [new RequestModificationDetail_queryParams { Key = "page", Value = new string('a', 256) }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Valueは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReplace.QueryParams.Valueの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReplaceQueryParamsValueInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Replace = new RequestModificationDetail
            {
                QueryParams = [new RequestModificationDetail_queryParams { Key = "page", Value = "invalid value" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Valueの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Body.Fromの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameBodyFromExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Body = [new RequestRenameDetail_body { From = new string('a', 256), To = "valid.key" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Body.Fromの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameBodyFromInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Body = [new RequestRenameDetail_body { From = "invalid key", To = "valid.key" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Body.Toの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameBodyToExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Body = [new RequestRenameDetail_body { From = "valid.key", To = new string('a', 256) }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Body.Toの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameBodyToInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Body = [new RequestRenameDetail_body { From = "valid.key", To = "invalid key" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Headers.Fromの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameHeadersFromExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Headers = [new RequestRenameDetail_headers { From = new string('a', 256), To = "X-New" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Headers.Fromの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameHeadersFromInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Headers = [new RequestRenameDetail_headers { From = "invalid key", To = "X-New" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Headers.Toの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameHeadersToExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Headers = [new RequestRenameDetail_headers { From = "X-Old", To = new string('a', 256) }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.Headers.Toの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameHeadersToInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                Headers = [new RequestRenameDetail_headers { From = "X-Old", To = "invalid key" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.QueryParams.Fromの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameQueryParamsFromExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                QueryParams = [new RequestRenameDetail_queryParams { From = new string('a', 256), To = "page" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.QueryParams.Fromの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameQueryParamsFromInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                QueryParams = [new RequestRenameDetail_queryParams { From = "invalid query", To = "page" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Fromの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.QueryParams.Toの長さ超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameQueryParamsToExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                QueryParams = [new RequestRenameDetail_queryParams { From = "page", To = new string('a', 256) }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRename.QueryParams.Toの形式不正でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRenameQueryParamsToInvalidFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RequestRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RequestTransformation
        {
            Rename = new RequestRenameDetail
            {
                QueryParams = [new RequestRenameDetail_queryParams { From = "page", To = "invalid query" }]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Contains("Toの形式が正しくありません。", exception.Message);
    }
}
