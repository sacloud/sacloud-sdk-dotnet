using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item;
using Sacloud.Sdk.APIGateway.Services.Item.Routes;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item;

/// <summary>
/// WithServiceItemRequestBuilderのテスト
/// </summary>
public class WithServiceItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithServiceItemRequestBuilder> _mockLogger;

    public WithServiceItemRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<WithServiceItemRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<WithServiceItemRequestBuilder>().Returns(_mockLogger);
    }

    private static Dictionary<string, object> CreatePathParameters() =>
        new()
        {
            { "baseurl", "https://api.example.com" },
            { "serviceId", Guid.NewGuid() }
        };

    private static ServiceDetail CreateValidServiceDetail()
    {
        return new ServiceDetail
        {
            Name = "Test-Service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
        };
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new WithServiceItemRequestBuilder(nullPath, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WithServiceItemRequestBuilder(CreatePathParameters(), null, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder($"https://api.example.com/services/{Guid.NewGuid()}", requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new WithServiceItemRequestBuilder(nullUrl, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WithServiceItemRequestBuilder($"https://api.example.com/services/{Guid.NewGuid()}", null, _mockLoggerFactory));
    }

    /// <summary>
    /// Routesプロパティが正しいRoutesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Routes_ReturnsRoutesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var routes = builder.Routes;
        Assert.NotNull(routes);
        Assert.IsType<RoutesRequestBuilder>(routes);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("services", requestInfo.UrlTemplate);
        Assert.Contains("serviceId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var response = new WithServiceGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.GetAsync();
        Assert.NotNull(result);
        Assert.IsType<WithServiceGetResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithServiceGetResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.GetAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithServiceGetResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.GetAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "service-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したサービスの詳細情報取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, service\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したサービスの詳細情報の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToDeleteRequestInformation();
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("services", requestInfo.UrlTemplate);
        Assert.Contains("serviceId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToDeleteRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "service-not-found" },
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
                @"^指定したサービス削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, service\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
                @"^指定したサービスの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail();
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail();
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("services", requestInfo.UrlTemplate);
        Assert.Contains("serviceId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail();
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
    }

    /// <summary>
    /// PutAsyncがNameがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Name = name
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Name", exception.Message);
    }

    /// <summary>
    /// PutAsyncがName文字数超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNameExceeds255Length_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Name = new string('a', 256)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Name", exception.Message);
    }

    /// <summary>
    /// PutAsyncがName不正文字でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNameInvalidCharacters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Name = "invalid name"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Name", exception.Message);
    }

    /// <summary>
    /// PutAsyncがProtocol未指定でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullProtocol_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Protocol = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Protocol", exception.Message);
    }

    /// <summary>
    /// PutAsyncがHost未指定でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullHost_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Host = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Host", exception.Message);
    }

    /// <summary>
    /// PutAsyncがHost不正形式でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidHost_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Host = "invalid host"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Host", exception.Message);
    }

    /// <summary>
    /// PutAsyncがHost文字数超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithHostExceeds253Length_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Host = new string('a', 300) + ".jp"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Hostは253文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがPath文字数超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithPathExceeds255Length_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Path = "/" + new string('a', 255)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Pathは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがPath不正形式でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidPath_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Path = "invalid-path"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Pathの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがConnectTimeout範囲外でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithConnectTimeoutOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            ConnectTimeout = 0
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("ConnectTimeoutの値は1以上2147483646以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがPort範囲外でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithPortOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Port = 65536
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Portの値は0以上65535以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがReadTimeout範囲外でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithReadTimeoutOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            ReadTimeout = 0
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("ReadTimeoutの値は1以上2147483646以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRetries範囲外でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithRetriesOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Retries = 32768
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Retriesの値は0以上32767以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがWriteTimeout範囲外でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithWriteTimeoutOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            WriteTimeout = 0
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("WriteTimeoutの値は1以上2147483646以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがTags要素文字数超過でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithTagElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Tags = [new string('a', 256)]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Tagsの各要素は1文字以上255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがTags要素不正形式でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithTagElementInvalidCharacters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            Host = "0.0.0.0",
            Tags = ["invalid tag"]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("Tagsの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PutAsyncがCorsConfig.AccessControlAllowMethods要素に無効なEnum値が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidCorsAccessControlAllowMethodsElement_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetail
        {
            CorsConfig = new CorsConfig
            {
                AccessControlAllowMethods = [(HTTPMethod)999]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Contains("AccessControlAllowMethods", exception.Message);
        Assert.Contains("Enum", exception.Message);
    }

    /// <summary>
    /// PutAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidServiceDetail();
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidServiceDetail();

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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidServiceDetail();

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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidServiceDetail();
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "service-not-found" },
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
                @"^指定したサービスの詳細情報更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, service\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidServiceDetail();
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
                @"^指定したサービスの詳細情報の更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var newBuilder = builder.WithUrl($"https://api.example.com/services/{Guid.NewGuid()}");
        Assert.NotNull(newBuilder);
        Assert.IsType<WithServiceItemRequestBuilder>(newBuilder);
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithServiceGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスの詳細情報の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithServiceGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithServiceGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスの詳細情報の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidServiceDetail();

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスの詳細情報の更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithServiceItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidServiceDetail();

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスの詳細情報の更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
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
        var builder = new WithServiceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new WithServiceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new WithServiceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(CreateValidServiceDetail()));
    }
}
