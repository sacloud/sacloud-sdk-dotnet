using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services;
using Sacloud.Sdk.APIGateway.Services.Item;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Services;

/// <summary>
/// ServicesRequestBuilderのテスト
/// </summary>
public class ServicesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ServicesRequestBuilder> _mockLogger;

    public ServicesRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ServicesRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ServicesRequestBuilder>().Returns(_mockLogger);
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
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new ServicesRequestBuilder(nullPath, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        Assert.Throws<ArgumentNullException>(() => new ServicesRequestBuilder(pathParameters, null, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ServicesRequestBuilder("https://api.example.com/services", requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new ServicesRequestBuilder(nullUrl, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ServicesRequestBuilder("https://api.example.com/services", null, _mockLoggerFactory));
    }

    /// <summary>
    /// インデクサーが正しいWithServiceItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsCorrectItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var itemBuilder = builder[Guid.NewGuid()];
        Assert.NotNull(itemBuilder);
        Assert.IsType<WithServiceItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが異なるIDで異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithDifferentIds_ReturnsDifferentInstances()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var itemBuilder1 = builder[Guid.NewGuid()];
        var itemBuilder2 = builder[Guid.NewGuid()];
        Assert.NotNull(itemBuilder1);
        Assert.NotNull(itemBuilder2);
        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.NotNull(requestInfo);
        Assert.Contains("services", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// GetAsyncがRequestAdapterのSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var response = new ServicesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.GetAsync();
        Assert.NotNull(result);
        Assert.IsType<ServicesGetResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesGetResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.GetAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesGetResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.GetAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 404
        };
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
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^サービス一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, service\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^サービス一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "test-service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest
            {
                Id = Guid.NewGuid()
            }
        };
        var requestInfo = builder.ToPostRequestInformation(body);
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがnull bodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null));
    }

    /// <summary>
    /// ToPostRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsCorrectUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "test-service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest
            {
                Id = Guid.NewGuid()
            }
        };
        var requestInfo = builder.ToPostRequestInformation(body);
        Assert.NotNull(requestInfo);
        Assert.Contains("services", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPostRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "test-service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest
            {
                Id = Guid.NewGuid()
            }
        };
        var requestInfo = builder.ToPostRequestInformation(body);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// PostAsyncがnull bodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null));
    }

    /// <summary>
    /// PostAsyncがRequestAdapterのSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest()
        {
            Name = "Test-Service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest()
            {
                Id = new Guid()
            }
        };
        var response = new ServicesPostResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.PostAsync(body);
        Assert.NotNull(result);
        Assert.IsType<ServicesPostResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncがrequestConfigurationを伝播することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_PropagatesRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "test-service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest
            {
                Id = Guid.NewGuid()
            }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesPostResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.PostAsync(body, config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncがCancellationTokenを伝播することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_PropagatesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "test-service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest
            {
                Id = Guid.NewGuid()
            }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesPostResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.PostAsync(body, cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "test-service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest
            {
                Id = Guid.NewGuid()
            }
        };
        var expectedException = new ApiError
        {
            ResponseStatusCode = 404
        };
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
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^サービス登録のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, service\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "test-service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest
            {
                Id = Guid.NewGuid()
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^サービスの登録中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいServicesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewServicesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newBuilder = builder.WithUrl("https://test-url");
        Assert.NotNull(newBuilder);
        Assert.IsType<ServicesRequestBuilder>(newBuilder);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのSubscriptionがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullSubscription_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest { Subscription = null };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Contains("Subscriptionが入力されていません", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのNameがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = name
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Nameが入力されていません", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのNameが256文字以上の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNameExceeds255Length_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = new string('a', 256)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Nameは1文字以上255文字以下で入力してください", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのNameが無効な文字を含む場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNameInvalidCharacters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Name = "invalid name"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Nameの形式が正しくありません", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのProtocolがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullProtocol_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Protocol = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Protocolが入力されていません", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのHostがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullHost_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Host = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Hostが入力されていません", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのHostが不正形式の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidHost_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Host = "invalid host!",
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Hostの形式が正しくありません", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのSubscription.Idがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullSubscriptionId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Subscription = new ServiceSubscriptionRequest
            {
                Id = null
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Idが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのHostが253文字を超過する場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithHostExceeding253Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Host = new string('a', 300) + ".jp"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Hostは253文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのPathが255文字を超過する場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithPathExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Path = "/" + new string('a', 255)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Pathは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのPath形式が不正な場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidPathFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Path = "invalid-path"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Pathの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのConnectTimeoutが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithConnectTimeoutOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            ConnectTimeout = 0
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("ConnectTimeoutの値は1以上2147483646以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのPortが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithPortOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Port = 65536
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Portの値は0以上65535以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのReadTimeoutが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithReadTimeoutOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            ReadTimeout = 0
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("ReadTimeoutの値は1以上2147483646以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのRetriesが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithRetriesOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Retries = 32768
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Retriesの値は0以上32767以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのWriteTimeoutが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithWriteTimeoutOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            WriteTimeout = 0
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("WriteTimeoutの値は1以上2147483646以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのTags要素が255文字を超過する場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithTagElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Tags = [new string('a', 256)]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Tagsの各要素は1文字以上255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのTags要素に無効な文字が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithTagElementInvalidCharacters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            Tags = ["invalid tag"]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Contains("Tagsの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのCorsConfig.AccessControlAllowMethods要素に無効なEnum値が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidCorsAccessControlAllowMethodsElement_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest
        {
            CorsConfig = new CorsConfig
            {
                AccessControlAllowMethods = [(HTTPMethod)999]
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("AccessControlAllowMethods", exception.Message);
        Assert.Contains("Enum", exception.Message);
    }

    /// <summary>
    /// GetAsyncが成功時に開始ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_Success_LogsStartMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^サービス一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^サービス一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが成功時に開始ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_Success_LogsStartMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest()
        {
            Name = "Test-Service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest()
            {
                Id = new Guid()
            }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesPostResponse());

        await builder.PostAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^サービスの登録を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが成功時に完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_Success_LogsCompletionMessage()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServiceDetailRequest()
        {
            Name = "Test-Service",
            Protocol = Service_protocol.Https,
            Host = "0.0.0.0",
            Subscription = new ServiceSubscriptionRequest()
            {
                Id = new Guid()
            }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ServicesPostResponse());

        await builder.PostAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^サービスの登録が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
