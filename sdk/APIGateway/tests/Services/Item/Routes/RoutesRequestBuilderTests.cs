using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes;

/// <summary>
/// RoutesRequestBuilderのテスト
/// </summary>
public class RoutesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<RoutesRequestBuilder> _mockLogger;

    public RoutesRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<RoutesRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<RoutesRequestBuilder>().Returns(_mockLogger);
    }

    private static Dictionary<string, object> CreatePathParameters() =>
        new()
        {
            { "baseurl", "https://api.example.com" },
            { "serviceId", Guid.NewGuid() }
        };

    private static RouteDetail CreateValidRouteDetail() =>
        new()
        {
            Name = "test-route",
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new RoutesRequestBuilder(nullPath, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RoutesRequestBuilder(CreatePathParameters(), null, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder($"https://api.example.com/services/{Guid.NewGuid()}/routes", requestAdapter, _mockLoggerFactory);
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
        Assert.Throws<ArgumentNullException>(() => new RoutesRequestBuilder(nullUrl, requestAdapter, _mockLoggerFactory));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RoutesRequestBuilder($"https://api.example.com/services/{Guid.NewGuid()}/routes", null, _mockLoggerFactory));
    }

    /// <summary>
    /// インデクサーが正しいWithRouteItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsCorrectItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var itemBuilder = builder[Guid.NewGuid()];
        Assert.NotNull(itemBuilder);
        Assert.IsType<WithRouteItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが異なるIDで異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithDifferentIds_ReturnsDifferentInstances()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var requestInfo = builder.ToGetRequestInformation();
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("routes", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var response = new RoutesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);
        var result = await builder.GetAsync();
        Assert.NotNull(result);
        Assert.IsType<RoutesGetResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesGetResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.GetAsync(config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesGetResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.GetAsync(cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したサービスのルート一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, service\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したサービスのルート一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null));
    }

    /// <summary>
    /// ToPostRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        var requestInfo = builder.ToPostRequestInformation(body);
        Assert.False(string.IsNullOrEmpty(requestInfo.UrlTemplate));
        Assert.Contains("routes", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPostRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        var requestInfo = builder.ToPostRequestInformation(body);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// PostAsyncがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null));
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Name = name
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Nameが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのNameに無効な文字が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidNameCharacters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Name = "invalid route"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Nameの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのMethodsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullMethods_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Methods = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Methodsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのMethods要素に無効なEnum値が含まれる場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidMethodsElement_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Methods = [(HTTPMethod)999]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Methods", exception.Message);
        Assert.Contains("Enum", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのHttpsRedirectStatusCodeが許可値以外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidHttpsRedirectStatusCode_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            HttpsRedirectStatusCode = 999
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("HttpsRedirectStatusCode", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのPathが255文字を超過する場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithPathExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Path = "/" + new string('a', 255)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Pathは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのPathの形式が不正な場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidPathFormat_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Path = "invalid-path"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Pathの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのProtocolsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullProtocols_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Protocols = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Protocolsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのRegexPriorityが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithRegexPriorityOutOfRange_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            RegexPriority = 256
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("RegexPriorityの値は0以上255以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのTags要素が255文字を超過する場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithTagElementExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Tags = ["invalid tag"]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Tagsの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのIpRestrictionConfig.Ipsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullIpRestrictionIps_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Ips = null
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Ipsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのIpRestrictionConfig.Ipsが空配列の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithEmptyIpRestrictionIps_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Ips = []
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Ipsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのIpRestrictionConfig.Ips要素の形式が不正な場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidIpRestrictionIpsElement_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Ips = ["999.0.0.1"]
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Ipsの要素の形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのIpRestrictionConfig.Protocolsがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullIpRestrictionProtocols_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Protocols = null
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("Protocolsが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのIpRestrictionConfig.RestrictedByがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullIpRestrictionRestrictedBy_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            IpRestrictionConfig = new IpRestrictionConfig
            {
                RestrictedBy = null
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("RestrictedByが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        var response = new RoutesPostResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.IsType<RoutesPostResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesPostResponse());

        const string customHeader = "X-Custom-Header";
        const string customValue = "custom-value";

        await builder.PostAsync(body, config =>
        {
            config.Headers.Add(customHeader, customValue);
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.Headers.ContainsKey(customHeader)),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesPostResponse());

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.PostAsync(body, cancellationToken: token);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            token);
    }

    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
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
            { "error_msg", "service-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したサービスへのルート登録のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, service\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = new RouteDetail
        {
            Name = "test-route",
            Methods = [HTTPMethod.GET],
            Protocols = Route_protocols.Https,
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(() => builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString() ?? string.Empty,
                @"^指定したサービスへのルートの登録中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var newBuilder = builder.WithUrl($"https://api.example.com/services/{Guid.NewGuid()}/routes");
        Assert.NotNull(newBuilder);
        Assert.IsType<RoutesRequestBuilder>(newBuilder);
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスのルート一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesGetResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString(),
                @"^指定したサービスのルート一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesPostResponse());

        await builder.PostAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^指定したサービスへのルートの登録を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new RoutesRequestBuilder(CreatePathParameters(), requestAdapter, _mockLoggerFactory);
        var body = CreateValidRouteDetail();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RoutesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new RoutesPostResponse());

        await builder.PostAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(
                state.ToString() ?? string.Empty,
                @"^指定したサービスへのルートの登録が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
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
        var builder = new RoutesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// PostAsyncが不正なserviceIdでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidServiceId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = CreatePathParameters();
        pathParameters["serviceId"] = "invalid-guid";
        var builder = new RoutesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(CreateValidRouteDetail()));
    }
}
