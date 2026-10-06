using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.SsoProfiles;
using Sacloud.Sdk.IAM.SsoProfiles.Item;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.SsoProfiles;

/// <summary>
/// SsoProfilesRequestBuilderのテスト
/// </summary>
public class SsoProfilesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<SsoProfilesRequestBuilder> _mockLogger;

    public SsoProfilesRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<SsoProfilesRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<SsoProfilesRequestBuilder>().Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new SsoProfilesRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/sso-profiles";

        Assert.Throws<ArgumentNullException>(() => new SsoProfilesRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/sso-profiles";

        var builder = new SsoProfilesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithSso_profile_ItemRequestBuilderを返すことを確認
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[123L];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithSso_profile_ItemRequestBuilder>(itemBuilder);
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder[123L];
        var itemBuilder2 = builder[456L];

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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("sso-profiles", requestInfo.UrlTemplate);
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
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
        var expectedResponse = new SsoProfilesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^SSOプロファイル一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^SSOプロファイル一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithRequestConfiguration_UsesConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new SsoProfilesGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 1;
            config.QueryParameters.PerPage = 10;
        });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncがcancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithCancellationToken_UsesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new SsoProfilesGetResponse();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
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
                @"^SSOプロファイル一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SsoProfilesGetResponse>>(),
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
                @"^SSOプロファイル一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody { Name = "Test SSO Profile" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsContentTypeHeader()
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody { Name = "Test SSO Profile" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
    }

    /// <summary>
    /// ToPostRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody { Name = "Test SSO Profile" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// PostAsyncがRequestAdapterのSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new SSOProfile();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody
        {
            Description = "test-description",
            IdpCertificate = "test-certificate",
            IdpEntityId = "test-entity-id",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            Name = "Test SSO Profile"
        };

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^SSOプロファイルの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^SSOプロファイルの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(async () => await builder.PostAsync(null));
    }

    /// <summary>
    /// PostAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithRequestConfiguration_UsesConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new SSOProfile();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody
        {
            Description = "test-description",
            IdpCertificate = "test-certificate",
            IdpEntityId = "test-entity-id",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            Name = "Test SSO Profile"
        };

        var result = await builder.PostAsync(body, config => { });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PostAsyncがcancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithCancellationToken_UsesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new SSOProfile();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody
        {
            Description = "test-description",
            IdpCertificate = "test-certificate",
            IdpEntityId = "test-entity-id",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            Name = "Test SSO Profile"
        };

        var result = await builder.PostAsync(body, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody
        {
            Description = "test-description",
            IdpCertificate = "test-certificate",
            IdpEntityId = "test-entity-id",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            Name = "Test SSO Profile"
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
            Arg.Any<ParsableFactory<SSOProfile>>(),
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
                @"^SSOプロファイル作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody
        {
            Description = "test-description",
            IdpCertificate = "test-certificate",
            IdpEntityId = "test-entity-id",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            Name = "Test SSO Profile"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
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
                @"^SSOプロファイルの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncにnullが含まれるリクエストボディが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Description")]
    [InlineData("IdpCertificate")]
    [InlineData("IdpEntityId")]
    [InlineData("IdpLoginUrl")]
    [InlineData("IdpLogoutUrl")]
    [InlineData("Name")]
    public async Task PostAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SsoProfilesPostRequestBody
        {
            Description = nullParam == "Description" ? null : "test-description",
            IdpCertificate = nullParam == "IdpCertificate" ? null : "test-certificate",
            IdpEntityId = nullParam == "IdpEntityId" ? null : "test-entity-id",
            IdpLoginUrl = nullParam == "IdpLoginUrl" ? null : "https://idp.example.com/login",
            IdpLogoutUrl = nullParam == "IdpLogoutUrl" ? null : "https://idp.example.com/logout",
            Name = nullParam == "Name" ? null : "Test SSO Profile"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
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
        var builder = new SsoProfilesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/sso-profiles";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<SsoProfilesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのPageプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanBeSet()
    {
        var queryParams = new SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        Assert.Equal(1, queryParams.Page);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanBeSet()
    {
        var queryParams = new SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters
        {
            PerPage = 20
        };

        Assert.Equal(20, queryParams.PerPage);
    }

    /// <summary>
    /// QueryParametersのPageプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanSetNull()
    {
        var queryParams = new SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters
        {
            Page = null
        };

        Assert.Null(queryParams.Page);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanSetNull()
    {
        var queryParams = new SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters
        {
            PerPage = null
        };

        Assert.Null(queryParams.PerPage);
    }

    /// <summary>
    /// QueryParametersのPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_HasQueryParameterAttribute()
    {
        var property = typeof(SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters)
            .GetProperty(nameof(SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters.Page));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_HasQueryParameterAttribute()
    {
        var property = typeof(SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters)
            .GetProperty(nameof(SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters.PerPage));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのすべてのプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_AllProperties_CanBeSet()
    {
        var queryParams = new SsoProfilesRequestBuilder.SsoProfilesRequestBuilderGetQueryParameters
        {
            Page = 1,
            PerPage = 20
        };

        Assert.Equal(1, queryParams.Page);
        Assert.Equal(20, queryParams.PerPage);
    }
}
