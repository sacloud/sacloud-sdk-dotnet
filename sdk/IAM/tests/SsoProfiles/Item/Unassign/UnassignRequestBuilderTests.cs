using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.SsoProfiles.Item.Unassign;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.SsoProfiles.Item.Unassign;

/// <summary>
/// UnassignRequestBuilderのテスト
/// </summary>
public class UnassignRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<UnassignRequestBuilder> _mockLogger;

    public UnassignRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<UnassignRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<UnassignRequestBuilder>().Returns(_mockLogger);
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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };

        Assert.Throws<ArgumentNullException>(() => new UnassignRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/sso-profiles/123/unassign";

        Assert.Throws<ArgumentNullException>(() => new UnassignRequestBuilder(rawUrl, requestAdapter, null));
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
            { "sso_profile_id", 123L }
        };

        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/sso-profiles/123/unassign";

        var builder = new UnassignRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("sso-profiles", requestInfo.UrlTemplate);
        Assert.Contains("unassign", requestInfo.UrlTemplate);
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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync();

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
                @"^指定したSSOプロファイルの割り当て解除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したSSOプロファイルの割り当て解除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync(config => { });

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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// PostAsyncがnull戻り値を処理できることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullResponse_ReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns((SSOProfile?)null);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync();

        Assert.Null(result);
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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            await builder.PostAsync();
        });
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したSSOプロファイル割り当て解除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception("Unexpected error");

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SSOProfile>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PostAsync();
        });
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したSSOプロファイルの割り当て解除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncに不正なパスパラメータが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("sso_profile_id", "")]
    [InlineData("sso_profile_id", " ")]
    [InlineData("sso_profile_id", "　")]
    public async Task PostAsync_WithInvalidPathParameters_ThrowsValidationException(string key, string value)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { key, value }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync());
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
            { "baseurl", "https://api.example.com" },
            { "sso_profile_id", 123L }
        };
        var builder = new UnassignRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/sso-profiles/123/unassign";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<UnassignRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
