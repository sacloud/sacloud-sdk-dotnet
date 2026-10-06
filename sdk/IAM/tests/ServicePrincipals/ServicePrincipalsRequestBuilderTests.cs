using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.ServicePrincipals;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.ServicePrincipals;

/// <summary>
/// ServicePrincipalsRequestBuilderのテスト
/// </summary>
public class ServicePrincipalsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ServicePrincipalsRequestBuilder> _mockLogger;

    public ServicePrincipalsRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ServicePrincipalsRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ServicePrincipalsRequestBuilder>().Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/service-principals";

        Assert.Throws<ArgumentNullException>(() => new ServicePrincipalsRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/service-principals";

        var builder = new ServicePrincipalsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithService_principal_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithService_principal_ItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[12345L];

        Assert.NotNull(itemBuilder);
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder[12345L];
        var itemBuilder2 = builder[67890L];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// Oauth2プロパティが正しいOauth2RequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Oauth2_ReturnsOauth2RequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var oauth2Builder = builder.Oauth2;

        Assert.NotNull(oauth2Builder);
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("service-principals", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいAcceptヘッダーを設定することを確認
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToGetRequestInformationがQueryParametersを正しく設定できることを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Page = 1;
            config.QueryParameters.PerPage = 10;
            config.QueryParameters.ProjectId = 12345L;
        });

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// ToGetRequestInformationがnull設定で動作することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithNullConfiguration_WorksCorrectly()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(null);

        Assert.NotNull(requestInfo);
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Name = "Test Service Principal",
            Description = "Test Description",
            ProjectId = 12345L
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Name = "Test Service Principal"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Contains("service-principals", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Name = "Test Service Principal"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// GetAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsServicePrincipalsGetResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePrincipalsGetResponse
        {
            Items = []
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipalsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパル一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパル一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがCancellationTokenを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_AcceptsCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePrincipalsGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipalsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var cancellationToken = new CancellationToken();

        var result = await builder.GetAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
    }

    /// <summary>
    /// GetAsyncがrequestConfigurationを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_AcceptsRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePrincipalsGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipalsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 1;
        });

        Assert.NotNull(result);
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 401
        };
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
            Arg.Any<ParsableFactory<ServicePrincipalsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパル一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipalsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパル一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsServicePrincipal()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Description = "Test Description",
            Name = "Test Service Principal",
            ProjectId = 12345L
        };
        var expectedResponse = new ServicePrincipal
        {
            Name = "Test Service Principal"
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipal>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.Equal("Test Service Principal", result.Name);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパルの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパルの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがCancellationTokenを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_AcceptsCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Description = "Test Description",
            Name = "Test Service Principal",
            ProjectId = 12345L
        };
        var expectedResponse = new ServicePrincipal();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipal>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var cancellationToken = new CancellationToken();

        var result = await builder.PostAsync(body, cancellationToken: cancellationToken);

        Assert.NotNull(result);
    }

    /// <summary>
    /// PostAsyncがrequestConfigurationを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_AcceptsRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Description = "Test Description",
            Name = "Test Service Principal",
            ProjectId = 12345L
        };
        var expectedResponse = new ServicePrincipal();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipal>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body, config => { });

        Assert.NotNull(result);
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(async () => await builder.PostAsync(null));
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Description = "Test Description",
            Name = "Test Service Principal",
            ProjectId = 12345L
        };
        var expectedException = new ApiError
        {
            ResponseStatusCode = 401
        };
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
            Arg.Any<ParsableFactory<ServicePrincipal>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパル作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Description = "Test Description",
            Name = "Test Service Principal",
            ProjectId = 12345L
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePrincipal>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパルの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのOrderingに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidOrdering_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.Ordering = (GetOrderingQueryParameterType)999;
        }));

        Assert.Equal("orderingはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("orderingはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncにnullが含まれるリクエストボディが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Description")]
    [InlineData("Name")]
    [InlineData("ProjectId")]
    public async Task PostAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ServicePrincipalsPostRequestBody
        {
            Description = nullParam == "Description" ? null : "test-description",
            Name = nullParam == "Name" ? null : "test-name",
            ProjectId = nullParam == "ProjectId" ? (long?)null : 12345L
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Equal($"{nullParam}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{nullParam}が入力されていません。");
    }

    /// <summary>
    /// WithUrlが正しいServicePrincipalsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewServicePrincipalsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/service-principals";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
    }

    /// <summary>
    /// WithUrlが異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsDifferentInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePrincipalsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/service-principals";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのOrderingプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_CanSetAndGet()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            Ordering = GetOrderingQueryParameterType.Name
        };

        Assert.Equal(GetOrderingQueryParameterType.Name, queryParams.Ordering);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのOrderingプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_CanSetNull()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            Ordering = null
        };

        Assert.Null(queryParams.Ordering);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのPageプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanSetAndGet()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        Assert.Equal(1, queryParams.Page);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのPageプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanSetNull()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            Page = null
        };

        Assert.Null(queryParams.Page);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのPerPageプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanSetAndGet()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            PerPage = 10
        };

        Assert.Equal(10, queryParams.PerPage);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのPerPageプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanSetNull()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            PerPage = null
        };

        Assert.Null(queryParams.PerPage);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのProjectIdプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ProjectId_CanSetAndGet()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            ProjectId = 12345L
        };

        Assert.Equal(12345L, queryParams.ProjectId);
    }

    /// <summary>
    /// ServicePrincipalsRequestBuilderGetQueryParametersのProjectIdプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ProjectId_CanSetNull()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            ProjectId = null
        };

        Assert.Null(queryParams.ProjectId);
    }

    /// <summary>
    /// QueryParametersのOrderingプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters.Ordering));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters.Page));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters.PerPage));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのProjectIdプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ProjectId_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters.ProjectId));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのすべてのプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_AllProperties_CanBeSet()
    {
        var queryParams = new ServicePrincipalsRequestBuilder.ServicePrincipalsRequestBuilderGetQueryParameters
        {
            Ordering = GetOrderingQueryParameterType.Name,
            Page = 1,
            PerPage = 10,
            ProjectId = 12345L
        };

        Assert.Equal(GetOrderingQueryParameterType.Name, queryParams.Ordering);
        Assert.Equal(1, queryParams.Page);
        Assert.Equal(10, queryParams.PerPage);
        Assert.Equal(12345L, queryParams.ProjectId);
    }
}
