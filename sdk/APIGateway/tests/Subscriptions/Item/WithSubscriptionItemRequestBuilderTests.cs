using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Subscriptions.Item;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.APIGateway.Tests.Subscriptions.Item;

/// <summary>
/// WithSubscriptionItemRequestBuilderのテスト
/// </summary>
public class WithSubscriptionItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithSubscriptionItemRequestBuilder> _mockLogger;

    public WithSubscriptionItemRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<WithSubscriptionItemRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<WithSubscriptionItemRequestBuilder>().Returns(_mockLogger);
    }

    private Dictionary<string, object> CreatePathParameters() =>
        new()
        {
            { "baseurl", "https://api.example.com" },
            { "subscriptionId", Guid.NewGuid() }
        };

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new WithSubscriptionItemRequestBuilder(nullPath, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WithSubscriptionItemRequestBuilder(CreatePathParameters(), null, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder($"https://api.example.com/subscriptions/{Guid.NewGuid()}", requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new WithSubscriptionItemRequestBuilder(nullUrl, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        var rawUrl = $"https://api.example.com/subscriptions/{Guid.NewGuid()}";
        Assert.Throws<ArgumentNullException>(() => new WithSubscriptionItemRequestBuilder(rawUrl, null, _mockLoggerFactory));
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToDeleteRequestInformation();
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("subscriptions", requestInfo.UrlTemplate);
        Assert.Contains("subscriptionId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);

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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "subscription-not-found" },
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
                @"^指定した契約の解約のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, subscription\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
                @"^指定した契約の解約中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("subscriptions", requestInfo.UrlTemplate);
        Assert.Contains("subscriptionId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var response = new WithSubscriptionGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.GetAsync();
        Assert.NotNull(result);
        Assert.IsType<WithSubscriptionGetResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithSubscriptionGetResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.GetAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithSubscriptionGetResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.GetAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "subscription-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定した契約の詳細情報取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, subscription\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定した契約の詳細情報の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null));
    }

    /// <summary>
    /// ToPutRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
        var requestInfo = builder.ToPutRequestInformation(body);
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("subscriptions", requestInfo.UrlTemplate);
        Assert.Contains("subscriptionId", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };

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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };

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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
        var expectedException = new ApiError { ResponseStatusCode = 400 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "bad_request" },
            { "error_msg", "invalid-subscription-update" },
            { "is_fatal", false },
            { "serial", "1234567890" },
            { "status", "400 Bad Request" }
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
                @"^指定した契約更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 400\], \[error_code, bad_request\], \[error_msg, invalid\-subscription\-update\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 400 Bad Request\]$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
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
                @"^指定した契約の更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var rawUrl = $"https://api.example.com/subscriptions/{Guid.NewGuid()}";
        var newBuilder = builder.WithUrl(rawUrl);
        Assert.NotNull(newBuilder);
        Assert.IsType<WithSubscriptionItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate
        {
            Name = name
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Contains("Nameが入力されていません", exception.Message);
    }

    /// <summary>
    /// PutAsyncが成功時に開始ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_Success_LogsStartMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^指定した契約の更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate { Name = "updated-subscription" };
        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^指定した契約の更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^指定した契約の解約を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^指定した契約の解約が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが成功時に開始ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_Success_LogsStartMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithSubscriptionGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^指定した契約の詳細情報を取得します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithSubscriptionItemRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithSubscriptionGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new WithSubscriptionGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^指定した契約の詳細情報の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが不正なsubscriptionIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidSubscriptionId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["subscriptionId"] = "invalid-guid";
        var builder = new WithSubscriptionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// DeleteAsyncが不正なsubscriptionIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidSubscriptionId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["subscriptionId"] = "invalid-guid";
        var builder = new WithSubscriptionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());
    }

    /// <summary>
    /// PutAsyncが不正なsubscriptionIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidSubscriptionId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["subscriptionId"] = "invalid-guid";
        var builder = new WithSubscriptionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SubscriptionUpdate
        {
            Name = "updated-subscription"
        };

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
    }
}
