using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Compat.Users.Item.RegisterEmail;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users.Item.RegisterEmail;

/// <summary>
/// RegisterEmailRequestBuilderの単体テスト
/// </summary>
public class RegisterEmailRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<RegisterEmailRequestBuilder> _mockLogger;

    public RegisterEmailRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<RegisterEmailRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<RegisterEmailRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void RegisterEmailRequestBuilder_Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void RegisterEmailRequestBuilder_Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/compat/users/123/register-email";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new RegisterEmailRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void RegisterEmailRequestBuilder_Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>();

        Assert.Throws<ArgumentNullException>(() =>
            new RegisterEmailRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void RegisterEmailRequestBuilder_Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var rawUrl = "https://example.com/compat/users/123/register-email";

        Assert.Throws<ArgumentNullException>(() =>
            new RegisterEmailRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// PostAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task RegisterEmailRequestBuilder_PostAsync_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "user_id", "test-user" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new RegisterEmailPostRequestBody
        {
            Email = "test@example.com"
        };

        await builder.PostAsync(body);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザへのメールアドレス登録を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザへのメールアドレス登録が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがnullのbodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task RegisterEmailRequestBuilder_PostAsync_WithNullBody_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "user_id", "test-user" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(async () => await builder.PostAsync(null));
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task RegisterEmailRequestBuilder_PostAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "user_id", "test-user" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new RegisterEmailPostRequestBody
        {
            Email = "test@example.com"
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

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
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
                @"^ユーザへのメールアドレス登録のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task RegisterEmailRequestBuilder_PostAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "user_id", "test-user" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new RegisterEmailPostRequestBody
        {
            Email = "test@example.com"
        };
        var expectedException = new Exception("Unexpected error");

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
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
                @"^ユーザへのメールアドレス登録中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void RegisterEmailRequestBuilder_ToPostRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "user_id", "test-user" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new RegisterEmailPostRequestBody
        {
            Email = "test@example.com"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("register-email", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// PostAsyncに不正なパスパラメータが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("user_id", "")]
    [InlineData("user_id", " ")]
    [InlineData("user_id", "\u3000")]
    public async Task PostAsync_WithInvalidPathParameters_ThrowsValidationException(string key, string value)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { key, value }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new RegisterEmailPostRequestBody { Email = "test@example.com" };

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
    }

    /// <summary>
    /// PostAsyncに必須バディパラメータがない場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Email")]
    public async Task PostAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "user_id", "test-user" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new RegisterEmailPostRequestBody
        {
            Email = nullParam == "Email" ? null : "test@example.com"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Equal($"{nullParam}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{nullParam}が入力されていません。");
    }

    /// <summary>
    /// WithUrlが正しいRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void RegisterEmailRequestBuilder_WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RegisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://newexample.com/compat/users/456/register-email";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<RegisterEmailRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
