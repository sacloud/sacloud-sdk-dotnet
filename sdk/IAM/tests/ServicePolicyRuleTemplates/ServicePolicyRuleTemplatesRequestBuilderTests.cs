using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.ServicePolicyRuleTemplates;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.ServicePolicyRuleTemplates;

/// <summary>
/// ServicePolicyRuleTemplatesRequestBuilderのテスト
/// </summary>
public class ServicePolicyRuleTemplatesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ServicePolicyRuleTemplatesRequestBuilder> _mockLogger;

    public ServicePolicyRuleTemplatesRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ServicePolicyRuleTemplatesRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ServicePolicyRuleTemplatesRequestBuilder>().Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/service-policy-rule-templates";

        Assert.Throws<ArgumentNullException>(() => new ServicePolicyRuleTemplatesRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/service-policy-rule-templates";

        var builder = new ServicePolicyRuleTemplatesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいURLテンプレートを使用することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_UsesCorrectUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo.UrlTemplate);
        Assert.Equal("{+baseurl}/service-policy-rule-templates{?code*,name*,page*,per_page*,type*}", requestInfo.UrlTemplate);
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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Code = "test-code";
            config.QueryParameters.Name = "test-name";
            config.QueryParameters.Page = 1;
            config.QueryParameters.PerPage = 10;
            config.QueryParameters.Type = GetTypeQueryParameterType.Boolean;
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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(null);

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// GetAsyncが正常にServicePolicyRuleTemplatesGetResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsServicePolicyRuleTemplatesGetResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePolicyRuleTemplatesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.Same(expectedResponse, result);
    }

    /// <summary>
    /// GetAsyncが正しいRequestInformationでRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterWithCorrectRequestInfo()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePolicyRuleTemplatesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.HttpMethod == Method.GET),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ルールテンプレートの一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ルールテンプレートの一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがエラーマッピングを正しく設定することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ConfiguresErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePolicyRuleTemplatesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(em => em.ContainsKey("XXX")),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncがCancellationTokenを正しく伝達することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_PassesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePolicyRuleTemplatesGetResponse();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync(cancellationToken: cancellationToken);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// GetAsyncがRequestConfigurationを受け入れることを確認
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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ServicePolicyRuleTemplatesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 1;
            config.QueryParameters.PerPage = 10;
        });

        Assert.NotNull(result);
        Assert.Same(expectedResponse, result);
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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
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
                @"^ルールテンプレート一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ServicePolicyRuleTemplatesGetResponse>>(),
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
                @"^ルールテンプレートの一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのTypeに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidType_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.Type = (GetTypeQueryParameterType)999;
        }));

        Assert.Equal("typeはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("typeはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// WithUrlが正しいServicePolicyRuleTemplatesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://api.example.com/service-policy-rule-templates";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<ServicePolicyRuleTemplatesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
        var builder = new ServicePolicyRuleTemplatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://api.example.com/service-policy-rule-templates";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCodeプロパティが正しく設定・取得されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Code_SetsAndGetsCorrectly()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Code = "test-code"
        };

        Assert.Equal("test-code", queryParameters.Code);
    }

    /// <summary>
    /// QueryParametersのNameプロパティが正しく設定・取得されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Name_SetsAndGetsCorrectly()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Name = "test-name"
        };

        Assert.Equal("test-name", queryParameters.Name);
    }

    /// <summary>
    /// QueryParametersのPageプロパティが正しく設定・取得されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_SetsAndGetsCorrectly()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        Assert.Equal(1, queryParameters.Page);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティが正しく設定・取得されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_SetsAndGetsCorrectly()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            PerPage = 20
        };

        Assert.Equal(20, queryParameters.PerPage);
    }

    /// <summary>
    /// QueryParametersのTypeプロパティが正しく設定・取得されることを確認（Boolean）
    /// </summary>
    [Fact]
    public void QueryParameters_Type_SetsAndGetsCorrectly_WithBoolean()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Type = GetTypeQueryParameterType.Boolean
        };

        Assert.Equal(GetTypeQueryParameterType.Boolean, queryParameters.Type);
    }

    /// <summary>
    /// QueryParametersのTypeプロパティが正しく設定・取得されることを確認（List）
    /// </summary>
    [Fact]
    public void QueryParameters_Type_SetsAndGetsCorrectly_WithList()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Type = GetTypeQueryParameterType.List
        };

        Assert.Equal(GetTypeQueryParameterType.List, queryParameters.Type);
    }

    /// <summary>
    /// QueryParametersのCodeプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Code_CanSetNull()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Code = "test-code"
        };

        queryParameters.Code = null;

        Assert.Null(queryParameters.Code);
    }

    /// <summary>
    /// QueryParametersのNameプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Name_CanSetNull()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Name = "test-name"
        };

        queryParameters.Name = null;

        Assert.Null(queryParameters.Name);
    }

    /// <summary>
    /// QueryParametersのPageプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanSetNull()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        queryParameters.Page = null;

        Assert.Null(queryParameters.Page);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanSetNull()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            PerPage = 20
        };

        queryParameters.PerPage = null;

        Assert.Null(queryParameters.PerPage);
    }

    /// <summary>
    /// QueryParametersのTypeプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Type_CanSetNull()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Type = GetTypeQueryParameterType.Boolean
        };

        queryParameters.Type = null;

        Assert.Null(queryParameters.Type);
    }

    /// <summary>
    /// QueryParametersのCodeプロパティにQueryParameter属性が設定されていることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Code_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters)
            .GetProperty("Code");

        Assert.NotNull(property);
        var attribute = property.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;
        Assert.NotNull(attribute);
        Assert.Equal("code", attribute.TemplateName);
    }

    /// <summary>
    /// QueryParametersのNameプロパティにQueryParameter属性が設定されていることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Name_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters)
            .GetProperty("Name");

        Assert.NotNull(property);
        var attribute = property.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;
        Assert.NotNull(attribute);
        Assert.Equal("name", attribute.TemplateName);
    }

    /// <summary>
    /// QueryParametersのPageプロパティにQueryParameter属性が設定されていることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters)
            .GetProperty("Page");

        Assert.NotNull(property);
        var attribute = property.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;
        Assert.NotNull(attribute);
        Assert.Equal("page", attribute.TemplateName);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティにQueryParameter属性が設定されていることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters)
            .GetProperty("PerPage");

        Assert.NotNull(property);
        var attribute = property.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;
        Assert.NotNull(attribute);
        Assert.Equal("per_page", attribute.TemplateName);
    }

    /// <summary>
    /// QueryParametersのTypeプロパティにQueryParameter属性が設定されていることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Type_HasQueryParameterAttribute()
    {
        var property = typeof(ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters)
            .GetProperty("Type");

        Assert.NotNull(property);
        var attribute = property.GetCustomAttributes(typeof(QueryParameterAttribute), false)
            .FirstOrDefault() as QueryParameterAttribute;
        Assert.NotNull(attribute);
        Assert.Equal("type", attribute.TemplateName);
    }

    /// <summary>
    /// QueryParametersのすべてのプロパティが同時に設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_AllProperties_CanBeSet()
    {
        var queryParameters = new ServicePolicyRuleTemplatesRequestBuilder.ServicePolicyRuleTemplatesRequestBuilderGetQueryParameters
        {
            Code = "test-code",
            Name = "test-name",
            Page = 1,
            PerPage = 20,
            Type = GetTypeQueryParameterType.Boolean
        };

        Assert.Equal("test-code", queryParameters.Code);
        Assert.Equal("test-name", queryParameters.Name);
        Assert.Equal(1, queryParameters.Page);
        Assert.Equal(20, queryParameters.PerPage);
        Assert.Equal(GetTypeQueryParameterType.Boolean, queryParameters.Type);
    }
}
