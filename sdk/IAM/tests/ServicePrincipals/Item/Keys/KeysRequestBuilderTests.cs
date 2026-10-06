using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.ServicePrincipals.Item.Keys;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.ServicePrincipals.Item.Keys;

/// <summary>
/// KeysRequestBuilderのテスト
/// </summary>
public class KeysRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<KeysRequestBuilder> _mockLogger;

    public KeysRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<KeysRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<KeysRequestBuilder>().Returns(_mockLogger);
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };

        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/service-principals/12345/keys";

        var builder = new KeysRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };

        Assert.Throws<ArgumentNullException>(() =>
            new KeysRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/service-principals/12345/keys";

        Assert.Throws<ArgumentNullException>(() =>
            new KeysRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// インデクサでWithService_principal_key_ItemRequestBuilderが返されることを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithService_principal_key_ItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[Guid.NewGuid()];

        Assert.NotNull(itemBuilder);
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("keys", requestInfo.UrlTemplate);
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// GetAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsKeysGetResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new KeysGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<KeysGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパルキー一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパルキー一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new KeysGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<KeysGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var cancellationToken = new CancellationToken();

        var result = await builder.GetAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパルキー一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^サービスプリンシパルキー一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<KeysGetResponse>>(),
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
                @"^サービスプリンシパルキー一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<KeysGetResponse>>(),
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
                @"^サービスプリンシパルキー一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncに不正なパスパラメータが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("service_principal_id", "")]
    [InlineData("service_principal_id", " ")]
    [InlineData("service_principal_id", "\u3000")]
    public async Task GetAsync_WithInvalidPathParameters_ThrowsValidationException(string key, string value)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { key, value }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", "12345" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.Ordering = (GetOrderingQueryParameterType)999;
        }));

        Assert.Equal("orderingはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("orderingはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// WithUrlが正しいKeysRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewKeysRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/service-principals/12345/keys";

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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/service-principals/12345/keys";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParameters.Orderingプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_SetsCorrectly()
    {
        var queryParameters = new KeysRequestBuilder.KeysRequestBuilderGetQueryParameters
        {
            Ordering = GetOrderingQueryParameterType.Created_at
        };

        Assert.Equal(GetOrderingQueryParameterType.Created_at, queryParameters.Ordering);
    }

    /// <summary>
    /// QueryParameters.Pageプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_SetsCorrectly()
    {
        var queryParameters = new KeysRequestBuilder.KeysRequestBuilderGetQueryParameters
        {
            Page = 2
        };

        Assert.Equal(2, queryParameters.Page);
    }

    /// <summary>
    /// QueryParameters.PerPageプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_SetsCorrectly()
    {
        var queryParameters = new KeysRequestBuilder.KeysRequestBuilderGetQueryParameters
        {
            PerPage = 50
        };

        Assert.Equal(50, queryParameters.PerPage);
    }

    /// <summary>
    /// QueryParameters.Orderingが正しいQueryParameterAttributeを持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_HasCorrectQueryParameterAttribute()
    {
        var propertyInfo = typeof(KeysRequestBuilder.KeysRequestBuilderGetQueryParameters)
            .GetProperty(nameof(KeysRequestBuilder.KeysRequestBuilderGetQueryParameters.Ordering));
        var attribute = propertyInfo?.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;

        Assert.NotNull(attribute);
        Assert.Equal("ordering", attribute.TemplateName);
    }

    /// <summary>
    /// QueryParameters.Pageが正しいQueryParameterAttributeを持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_HasCorrectQueryParameterAttribute()
    {
        var propertyInfo = typeof(KeysRequestBuilder.KeysRequestBuilderGetQueryParameters)
            .GetProperty(nameof(KeysRequestBuilder.KeysRequestBuilderGetQueryParameters.Page));
        var attribute = propertyInfo?.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;

        Assert.NotNull(attribute);
        Assert.Equal("page", attribute.TemplateName);
    }

    /// <summary>
    /// QueryParameters.PerPageが正しいQueryParameterAttributeを持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_HasCorrectQueryParameterAttribute()
    {
        var propertyInfo = typeof(KeysRequestBuilder.KeysRequestBuilderGetQueryParameters)
            .GetProperty(nameof(KeysRequestBuilder.KeysRequestBuilderGetQueryParameters.PerPage));
        var attribute = propertyInfo?.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;

        Assert.NotNull(attribute);
        Assert.Equal("per_page", attribute.TemplateName);
    }

    /// <summary>
    /// ToGetRequestInformationがQueryParametersを受け取ることを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_AcceptsQueryParameters()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var config = (Action<RequestConfiguration<KeysRequestBuilder.KeysRequestBuilderGetQueryParameters>>)(c =>
        {
            c.QueryParameters.Ordering = GetOrderingQueryParameterType.Created_at;
            c.QueryParameters.Page = 2;
            c.QueryParameters.PerPage = 50;
        });

        var requestInfo = builder.ToGetRequestInformation(config);

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// GetAsyncがQueryParametersを持つConfigurationを受け取ることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_AcceptsConfigurationWithQueryParameters()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new KeysGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<KeysGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var config = (Action<RequestConfiguration<KeysRequestBuilder.KeysRequestBuilderGetQueryParameters>>)(c =>
        {
            c.QueryParameters.Ordering = GetOrderingQueryParameterType.Created_at;
            c.QueryParameters.Page = 1;
            c.QueryParameters.PerPage = 100;
        });

        var result = await builder.GetAsync(config);

        Assert.NotNull(result);
    }
}
