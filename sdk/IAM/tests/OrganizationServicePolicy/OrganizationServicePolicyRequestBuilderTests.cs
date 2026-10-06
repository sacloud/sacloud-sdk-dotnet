using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.OrganizationServicePolicy;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.OrganizationServicePolicy;

/// <summary>
/// OrganizationServicePolicyRequestBuilderのテスト
/// </summary>
public class OrganizationServicePolicyRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<OrganizationServicePolicyRequestBuilder> _mockLogger;

    public OrganizationServicePolicyRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<OrganizationServicePolicyRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<OrganizationServicePolicyRequestBuilder>().Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/organization-service-policy";

        Assert.Throws<ArgumentNullException>(() => new OrganizationServicePolicyRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/organization-service-policy";

        var builder = new OrganizationServicePolicyRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo.UrlTemplate);
        Assert.Equal("{+baseurl}/organization-service-policy{?code*,is_active*,is_dry_run*,name*,type*}", requestInfo.UrlTemplate);
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Code = "test-code";
            config.QueryParameters.IsActive = true;
            config.QueryParameters.IsDryRun = false;
            config.QueryParameters.Name = "test-name";
            config.QueryParameters.Type = GetTypeQueryParameterType.Bool;
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(null);

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationがContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsContentTypeHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
    }

    /// <summary>
    /// GetAsyncが正常にOrganizationServicePolicyGetResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsOrganizationServicePolicyGetResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationServicePolicyGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationServicePolicyGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.HttpMethod == Method.GET),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^組織のサービスポリシーを構成するルールの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^組織のサービスポリシーを構成するルールの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationServicePolicyGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationServicePolicyGetResponse();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync(cancellationToken: cancellationToken);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// GetAsyncがRequestConfigurationを受け取れることを確認
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationServicePolicyGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync(config =>
        {
            config.QueryParameters.Code = "test-code";
            config.QueryParameters.IsActive = true;
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PutAsyncが正常にOrganizationServicePolicyPutResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_ReturnsOrganizationServicePolicyPutResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = []
        };
        var expectedResponse = new OrganizationServicePolicyPutResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        Assert.Same(expectedResponse, result);
    }

    /// <summary>
    /// PutAsyncが正しいRequestInformationでRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterWithCorrectRequestInfo()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = []
        };
        var expectedResponse = new OrganizationServicePolicyPutResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(ri => ri.HttpMethod == Method.PUT),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^組織のサービスポリシーを構成するルールの設定値の更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^組織のサービスポリシーを構成するルールの設定値の更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがエラーマッピングを正しく設定することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_ConfiguresErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = []
        };
        var expectedResponse = new OrganizationServicePolicyPutResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(em => em.ContainsKey("XXX")),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PutAsyncがCancellationTokenを正しく伝達することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_PassesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = []
        };
        var expectedResponse = new OrganizationServicePolicyPutResponse();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PutAsync(body, cancellationToken: cancellationToken);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// PutAsyncがRequestConfigurationを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_AcceptsRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = []
        };
        var expectedResponse = new OrganizationServicePolicyPutResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PutAsync(body, config =>
        {
            config.Headers.Add("X-Test-Header", "test-value");
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PutAsyncがnullのbodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.Type = (GetTypeQueryParameterType)999;
        }));

        Assert.Equal("typeはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("typeはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PutAsyncにnullが含まれるリクエストボディが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Rules")]
    [InlineData("Code")]
    [InlineData("IsActive")]
    [InlineData("IsDryRun")]
    public async Task PutAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = nullParam == "Rules" ? null :
            [
                new Rule
                {
                    Code = nullParam == "Code" ? null : "test-code",
                    IsActive = nullParam == "IsActive" ? null : false,
                    IsDryRun = nullParam == "IsDryRun" ? null : false
                }
            ]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Equal($"{nullParam}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{nullParam}が入力されていません。");
    }

    /// <summary>
    /// WithUrlが新しいBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://api.example.com/new-url";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParameters_Codeプロパティをセットできることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Code_CanBeSet()
    {
        var queryParams = new OrganizationServicePolicyRequestBuilder.OrganizationServicePolicyRequestBuilderGetQueryParameters
        {
            Code = "test-code"
        };

        Assert.Equal("test-code", queryParams.Code);
    }

    /// <summary>
    /// QueryParameters_IsActiveプロパティをセットできることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_IsActive_CanBeSet()
    {
        var queryParams = new OrganizationServicePolicyRequestBuilder.OrganizationServicePolicyRequestBuilderGetQueryParameters
        {
            IsActive = true
        };

        Assert.True(queryParams.IsActive);
    }

    /// <summary>
    /// QueryParameters_IsDryRunプロパティをセットできることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_IsDryRun_CanBeSet()
    {
        var queryParams = new OrganizationServicePolicyRequestBuilder.OrganizationServicePolicyRequestBuilderGetQueryParameters
        {
            IsDryRun = true
        };

        Assert.True(queryParams.IsDryRun);
    }

    /// <summary>
    /// QueryParameters_Nameプロパティをセットできることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Name_CanBeSet()
    {
        var queryParams = new OrganizationServicePolicyRequestBuilder.OrganizationServicePolicyRequestBuilderGetQueryParameters
        {
            Name = "test-name"
        };

        Assert.Equal("test-name", queryParams.Name);
    }

    /// <summary>
    /// QueryParameters_Typeプロパティをセットできることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Type_CanBeSet()
    {
        var queryParams = new OrganizationServicePolicyRequestBuilder.OrganizationServicePolicyRequestBuilderGetQueryParameters
        {
            Type = GetTypeQueryParameterType.Bool
        };

        Assert.Equal(GetTypeQueryParameterType.Bool, queryParams.Type);
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
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
                @"^組織のサービスポリシーを構成するルール取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyGetResponse>>(),
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
                @"^組織のサービスポリシーを構成するルールの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = []
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
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PutAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^組織のサービスポリシーを構成するルール設定値更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationServicePolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationServicePolicyPutRequestBody
        {
            Rules = []
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationServicePolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PutAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^組織のサービスポリシーを構成するルールの設定値の更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// QueryParametersのすべてのプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Properties_HaveQueryParameterAttribute()
    {
        var type = typeof(OrganizationServicePolicyRequestBuilder.OrganizationServicePolicyRequestBuilderGetQueryParameters);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();
            Assert.NotNull(attribute);
        }
    }

    /// <summary>
    /// QueryParametersのすべてのプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_AllPropertiesWork()
    {
        var queryParams = new OrganizationServicePolicyRequestBuilder.OrganizationServicePolicyRequestBuilderGetQueryParameters
        {
            Code = "test-code",
            IsActive = true,
            IsDryRun = false,
            Name = "test-name",
            Type = GetTypeQueryParameterType.Bool
        };

        Assert.Equal("test-code", queryParams.Code);
        Assert.True(queryParams.IsActive);
        Assert.False(queryParams.IsDryRun);
        Assert.Equal("test-name", queryParams.Name);
        Assert.Equal(GetTypeQueryParameterType.Bool, queryParams.Type);
    }
}
