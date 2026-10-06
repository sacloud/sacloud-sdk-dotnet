using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item.Eventbus.Processconfiguration.SetSecret;
using Sacloud.Sdk.EventBus.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.EventBus.Tests.Commonserviceitem.Item.Eventbus.Processconfiguration.SetSecret;

/// <summary>
/// SetSecretRequestBuilderのテスト
/// </summary>
public class SetSecretRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<SetSecretRequestBuilder> _mockLogger;

    public SetSecretRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<SetSecretRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<SetSecretRequestBuilder>().Returns(_mockLogger);
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
            { "id", "test-id" }
        };

        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/commonserviceitem/test-id/eventbus/processconfiguration/set-secret";

        var builder = new SetSecretRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsValidRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetSecretRequest();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationがnullのbodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
    }

    /// <summary>
    /// PutAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task PutAsync_WithInvalidPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetSecretRequest();
        var expectedResponse = new SetSecretPutResponse();

        requestAdapter.SendAsync<SetSecretPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SetSecretPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("リソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("リソースIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Secret")]
    [InlineData("Secret.AccessToken")]
    [InlineData("Secret.AccessTokenSecret")]
    [InlineData("Secret.APIKey")]
    public async Task PutAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetSecretRequest
        {
            Secret = invalidParameterName == "Secret" ? null : new SetSecretRequest.SetSecretRequest_Secret
            {
                SacloudAPISecret = new SacloudAPISecret
                {
                    AccessToken = invalidParameterName == "Secret.AccessToken" ? null : "test-access-token",
                    AccessTokenSecret = invalidParameterName == "Secret.AccessTokenSecret" ? null : "test-access-token-secret"
                },
                SimpleMQSecret = new SimpleMQSecret
                {
                    APIKey = invalidParameterName == "Secret.APIKey" ? null : "test-api-key"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithMultipleInvalidParams_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetSecretRequest
        {
            Secret = new SetSecretRequest.SetSecretRequest_Secret
            {
                SacloudAPISecret = new SacloudAPISecret
                {
                    AccessToken = "",
                    AccessTokenSecret = string.Empty
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        string expectedMessage = """
            Secret.AccessTokenが入力されていません。
            Secret.AccessTokenSecretが入力されていません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("Secret.AccessTokenが入力されていません。");
        _mockLogger.Received(1).LogWarning("Secret.AccessTokenSecretが入力されていません。");
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
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetSecretRequest
        {
            Secret = new SetSecretRequest.SetSecretRequest_Secret
            {
                SimpleMQSecret = new SimpleMQSecret
                {
                    APIKey = "test-api-key"
                }
            }
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

        requestAdapter.SendAsync<SetSecretPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SetSecretPutResponse>>(),
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
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^実行設定のSecret設定のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetSecretRequest
        {
            Secret = new SetSecretRequest.SetSecretRequest_Secret
            {
                SimpleMQSecret = new SimpleMQSecret
                {
                    APIKey = "test-api-key"
                }
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<SetSecretPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SetSecretPutResponse>>(),
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
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^実行設定のSecret設定中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが正しくRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetSecretRequest
        {
            Secret = new SetSecretRequest.SetSecretRequest_Secret
            {
                SimpleMQSecret = new SimpleMQSecret
                {
                    APIKey = "test-api-key"
                }
            }
        };
        var expectedResponse = new SetSecretPutResponse();
        requestAdapter.SendAsync<SetSecretPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SetSecretPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.PutAsync(body);

        Assert.NotNull(response);
        await requestAdapter.Received(1).SendAsync<SetSecretPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SetSecretPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, response);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定のSecret設定を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定のSecret設定が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいSetSecretRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewSetSecretRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new SetSecretRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://api.example.com/commonserviceitem/new-id/eventbus/processconfiguration/set-secret";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<SetSecretRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
