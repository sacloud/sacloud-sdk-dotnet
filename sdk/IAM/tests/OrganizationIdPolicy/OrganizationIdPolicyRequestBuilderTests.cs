using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.OrganizationIdPolicy;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.OrganizationIdPolicy;

/// <summary>
/// OrganizationIdPolicyRequestBuilderのテスト
/// </summary>
public class OrganizationIdPolicyRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<OrganizationIdPolicyRequestBuilder> _mockLogger;

    public OrganizationIdPolicyRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<OrganizationIdPolicyRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<OrganizationIdPolicyRequestBuilder>().Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/organization-id-policy";

        Assert.Throws<ArgumentNullException>(() => new OrganizationIdPolicyRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/organization-id-policy";

        var builder = new OrganizationIdPolicyRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("organization-id-policy", requestInfo.UrlTemplate);
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
    }

    /// <summary>
    /// PutAsyncにnullが含まれるリクエストボディが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Bindings")]
    public async Task PutAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody
        {
            Bindings = nullParam == "Bindings" ? null : []
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Equal($"{nullParam}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{nullParam}が入力されていません。");
    }

    /// <summary>
    /// WithUrlが正しい新しいRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/organization-id-policy";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<OrganizationIdPolicyRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// GetAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsOrganizationIdPolicyGetResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationIdPolicyGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシーの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシーの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationIdPolicyGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyGetResponse>>(),
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new OrganizationIdPolicyGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync(config => { });

        Assert.NotNull(result);
    }

    /// <summary>
    /// PutAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_ReturnsOrganizationIdPolicyPutResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody
        {
            Bindings = []
        };
        var expectedResponse = new OrganizationIdPolicyPutResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシーの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシーの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがCancellationTokenを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_AcceptsCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody
        {
            Bindings = []
        };
        var expectedResponse = new OrganizationIdPolicyPutResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var cancellationToken = new CancellationToken();

        var result = await builder.PutAsync(body, cancellationToken: cancellationToken);

        Assert.NotNull(result);
    }

    /// <summary>
    /// PutAsyncがrequestConfigurationを受け取れることを確認
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody
        {
            Bindings = []
        };
        var expectedResponse = new OrganizationIdPolicyPutResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PutAsync(body, config => { });

        Assert.NotNull(result);
    }

    /// <summary>
    /// PutAsyncがnull bodyで例外をスローすることを確認
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(async () => await builder.PutAsync(null));
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<OrganizationIdPolicyGetResponse>>(),
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
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシー取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyGetResponse>>(),
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
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシーの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody
        {
            Bindings = []
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
            Arg.Any<ParsableFactory<OrganizationIdPolicyPutResponse>>(),
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
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシー更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new OrganizationIdPolicyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new OrganizationIdPolicyPutRequestBody
        {
            Bindings = []
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<OrganizationIdPolicyPutResponse>>(),
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
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString(),
                    @"^組織IDポリシーの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
