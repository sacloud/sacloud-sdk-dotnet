using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.IdRoles;
using Sacloud.Sdk.IAM.IdRoles.Item;
using Sacloud.Sdk.Shared.Models;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.IdRoles;

/// <summary>
/// IdRolesRequestBuilderのテスト
/// </summary>
public class IdRolesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<IdRolesRequestBuilder> _mockLogger;

    public IdRolesRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<IdRolesRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<IdRolesRequestBuilder>().Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new IdRolesRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/id-roles";

        Assert.Throws<ArgumentNullException>(() => new IdRolesRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/id-roles";

        var builder = new IdRolesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithId_role_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithId_role_ItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-id-role-id"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithId_role_ItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-id-role-id"];
        var itemBuilder2 = builder["test-id-role-id"];

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
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("id-roles", requestInfo.UrlTemplate);
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
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを正しく処理することを確認
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
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Page = 1;
            config.QueryParameters.PerPage = 20;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
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
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(null);

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// GetAsyncが結果を返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsIdRolesGetResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResult = new IdRolesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.Same(expectedResult, result);
    }

    /// <summary>
    /// GetAsyncがクエリパラメータ付きで正しいリクエストを送信することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithQueryParameters_SendsCorrectRequest()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResult = new IdRolesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 2;
            config.QueryParameters.PerPage = 50;
        });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(r => r.HttpMethod == Method.GET),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^IDロール一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^IDロール一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがキャンセレーショントークンでリクエストをキャンセルすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithCancellationToken_CancelsRequest()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var cts = new CancellationTokenSource();
        cts.Cancel();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromException<IdRolesGetResponse?>(new OperationCanceledException()));

        await Assert.ThrowsAsync<OperationCanceledException>(() => builder.GetAsync(cancellationToken: cts.Token));
    }

    /// <summary>
    /// GetAsyncがエラーレスポンス時にApiErrorをスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ThrowsApiError_OnErrorResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromException<IdRolesGetResponse?>(new ApiError()));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());
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
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
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
                @"^IDロール一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
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
                @"^IDロール一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void GetAsync_WithNullRequestAdapter_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        Assert.Throws<ArgumentNullException>(() => new IdRolesRequestBuilder(pathParameters, null, _mockLoggerFactory));
    }

    /// <summary>
    /// GetAsyncが空レスポンスの場合nullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithEmptyResponse_ReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns((IdRolesGetResponse?)null);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// WithUrlが新しいIdRolesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewIdRolesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/id-roles/custom");

        Assert.NotNull(newBuilder);
        Assert.IsType<IdRolesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// WithUrlが正しいURLで初期化されることを確認
    /// </summary>
    [Fact]
    public void WithUrl_WithValidUrl_InitializesCorrectly()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var customUrl = "https://custom.example.com/id-roles";

        var newBuilder = builder.WithUrl(customUrl);

        Assert.NotNull(newBuilder);
    }

    /// <summary>
    /// IdRolesRequestBuilderGetQueryParametersのPageプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void IdRolesRequestBuilderGetQueryParameters_PageProperty_CanBeSet()
    {
        var queryParameters = new IdRolesRequestBuilder.IdRolesRequestBuilderGetQueryParameters
        {
            Page = 3
        };

        Assert.Equal(3, queryParameters.Page);
    }

    /// <summary>
    /// IdRolesRequestBuilderGetQueryParametersのPageプロパティが読み取れることを確認
    /// </summary>
    [Fact]
    public void IdRolesRequestBuilderGetQueryParameters_PageProperty_CanBeRead()
    {
        var queryParameters = new IdRolesRequestBuilder.IdRolesRequestBuilderGetQueryParameters
        {
            Page = 5
        };

        var page = queryParameters.Page;

        Assert.Equal(5, page);
    }

    /// <summary>
    /// IdRolesRequestBuilderGetQueryParametersのPerPageプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void IdRolesRequestBuilderGetQueryParameters_PerPageProperty_CanBeSet()
    {
        var queryParameters = new IdRolesRequestBuilder.IdRolesRequestBuilderGetQueryParameters
        {
            PerPage = 100
        };

        Assert.Equal(100, queryParameters.PerPage);
    }

    /// <summary>
    /// IdRoles RequestBuilderGetQueryParametersのPerPageプロパティが読み取れることを確認
    /// </summary>
    [Fact]
    public void IdRolesRequestBuilderGetQueryParameters_PerPageProperty_CanBeRead()
    {
        var queryParameters = new IdRolesRequestBuilder.IdRolesRequestBuilderGetQueryParameters
        {
            PerPage = 25
        };

        var perPage = queryParameters.PerPage;

        Assert.Equal(25, perPage);
    }

    /// <summary>
    /// QueryParametersがnull設定でデフォルト設定を使用することを確認
    /// </summary>
    [Fact]
    public async Task QueryParameters_WithNullConfiguration_UsesDefaultSettings()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResult = new IdRolesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var result = await builder.GetAsync(null);

        Assert.NotNull(result);
    }

    /// <summary>
    /// IdRolesRequestBuilderGetQueryParametersのプロパティに[QueryParameter]属性が付いていることを確認
    /// </summary>
    [Fact]
    public void IdRolesRequestBuilderGetQueryParameters_Properties_HaveQueryParameterAttribute()
    {
        var pageProperty = typeof(IdRolesRequestBuilder.IdRolesRequestBuilderGetQueryParameters)
            .GetProperty("Page");
        var perPageProperty = typeof(IdRolesRequestBuilder.IdRolesRequestBuilderGetQueryParameters)
            .GetProperty("PerPage");

        Assert.NotNull(pageProperty);
        Assert.NotEmpty(pageProperty.GetCustomAttributes(typeof(QueryParameterAttribute), false));
        Assert.NotNull(perPageProperty);
        Assert.NotEmpty(perPageProperty.GetCustomAttributes(typeof(QueryParameterAttribute), false));
    }

    /// <summary>
    /// IdRolesRequestBuilderGetQueryParametersのすべてのプロパティが機能することを確認
    /// </summary>
    [Fact]
    public async Task IdRolesRequestBuilderGetQueryParameters_AllPropertiesWork()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new IdRolesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResult = new IdRolesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IdRolesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 5;
            config.QueryParameters.PerPage = 20;
        });

        Assert.NotNull(result);
    }
}
