using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Compat.Users;
using Sacloud.Sdk.IAM.Compat.Users.Item;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users;

/// <summary>
/// UsersRequestBuilderのテスト
/// </summary>
public class UsersRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<UsersRequestBuilder> _mockLogger;

    public UsersRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<UsersRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<UsersRequestBuilder>().Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new UsersRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/compat/users";

        Assert.Throws<ArgumentNullException>(() => new UsersRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/compat/users";

        var builder = new UsersRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサー（long版）が正しいWithUser_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithLong_ReturnsWithUser_ItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var userId = 12345L;

        var itemBuilder = builder[userId];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithUser_ItemRequestBuilder>(itemBuilder);
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var userId = 12345L;

        var itemBuilder1 = builder[userId];
        var itemBuilder2 = builder[userId];

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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// QueryParametersのOrderingプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_CanBeSet()
    {
        var queryParams = new UsersRequestBuilder.UsersRequestBuilderGetQueryParameters
        {
            Ordering = GetOrderingQueryParameterType.Code
        };

        Assert.Equal(GetOrderingQueryParameterType.Code, queryParams.Ordering);
    }

    /// <summary>
    /// QueryParametersのPageプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanBeSet()
    {
        var queryParams = new UsersRequestBuilder.UsersRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        Assert.Equal(1, queryParams.Page);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanBeSet()
    {
        var queryParams = new UsersRequestBuilder.UsersRequestBuilderGetQueryParameters
        {
            PerPage = 50
        };

        Assert.Equal(50, queryParams.PerPage);
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = "user001",
            Name = "Test User"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = "user001",
            Name = "Test User"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// GetAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new UsersGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザ一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザ一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがnullのrequestConfigurationでRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithNullRequestConfiguration_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new UsersGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync(null);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PostAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = "user001",
            Description = "テストユーザ",
            Name = "Test User",
            Password = "P@ssword1"
        };
        var expectedResponse = new User();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<User>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<User>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());    }

    /// <summary>
    /// PostAsyncがnullのbodyで例外をスローすることを確認
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null));
    }

    /// <summary>
    /// PostAsyncがnullのrequestConfigurationでRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullRequestConfiguration_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = "user001",
            Description = "テストユーザ",
            Name = "Test User",
            Password = "P@ssword1"
        };
        var expectedResponse = new User();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<User>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body, null);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<User>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
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
                @"^ユーザ一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
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
                @"^ユーザ一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = "user001",
            Description = "テストユーザ",
            Name = "Test User",
            Password = "P@ssword1"
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
            Arg.Any<ParsableFactory<User>>(),
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
                @"^ユーザ作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = "user001",
            Description = "テストユーザ",
            Name = "Test User",
            Password = "P@ssword1"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<User>>(),
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
                @"^ユーザの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://api.example.com/compat/users";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<UsersRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new UsersGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync(null);

        Assert.NotNull(result);
    }

    /// <summary>
    /// UsersRequestBuilderGetQueryParametersのプロパティに[QueryParameter]属性が付いていることを確認
    /// </summary>
    [Fact]
    public void UsersRequestBuilderGetQueryParameters_Properties_HaveQueryParameterAttribute()
    {
        var pageProperty = typeof(UsersRequestBuilder.UsersRequestBuilderGetQueryParameters)
            .GetProperty("Page");
        var perPageProperty = typeof(UsersRequestBuilder.UsersRequestBuilderGetQueryParameters)
            .GetProperty("PerPage");

        Assert.NotNull(pageProperty);
        Assert.NotEmpty(pageProperty.GetCustomAttributes(typeof(QueryParameterAttribute), false));
        Assert.NotNull(perPageProperty);
        Assert.NotEmpty(perPageProperty.GetCustomAttributes(typeof(QueryParameterAttribute), false));
    }

    /// <summary>
    /// UsersRequestBuilderGetQueryParametersのすべてのプロパティが機能することを確認
    /// </summary>
    [Fact]
    public async Task UsersRequestBuilderGetQueryParameters_AllPropertiesWork()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new UsersGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UsersGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 5;
            config.QueryParameters.PerPage = 20;
        });

        Assert.NotNull(result);
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを正しく処理することを確認
    /// </summary>
    [Theory]
    [InlineData(1, 10, GetOrderingQueryParameterType.Code)]
    [InlineData(2, 20, GetOrderingQueryParameterType.Minus_code)]
    [InlineData(null, null, null)]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly(
        int? page, int? perPage, GetOrderingQueryParameterType? ordering)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Page = page;
            config.QueryParameters.PerPage = perPage;
            config.QueryParameters.Ordering = ordering;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
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
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Ordering = (GetOrderingQueryParameterType)999;
            });
        });

        Assert.Equal("orderingはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("orderingはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Code")]
    [InlineData("Description")]
    [InlineData("Name")]
    [InlineData("Password")]
    public async Task PostAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = invalidParameterName == "Code" ? null : "user001",
            Description = invalidParameterName == "Description" ? null : "テストユーザ",
            Name = invalidParameterName == "Name" ? null : "Test User",
            Password = invalidParameterName == "Password" ? null : "P@ssword1"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの複数パラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMultipleInvalidParams_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new UsersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UsersPostRequestBody
        {
            Code = null,
            Description = null,
            Name = null,
            Password = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Contains("Codeが入力されていません。", exception.Message);
        Assert.Contains("Descriptionが入力されていません。", exception.Message);
        Assert.Contains("Nameが入力されていません。", exception.Message);
        Assert.Contains("Passwordが入力されていません。", exception.Message);
    }
}
