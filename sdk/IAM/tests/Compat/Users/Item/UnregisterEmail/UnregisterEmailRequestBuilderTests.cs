using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Compat.Users.Item.UnregisterEmail;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users.Item.UnregisterEmail;

/// <summary>
/// UnregisterEmailRequestBuilderの単体テスト
/// </summary>
public class UnregisterEmailRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<UnregisterEmailRequestBuilder> _mockLogger;

    public UnregisterEmailRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<UnregisterEmailRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<UnregisterEmailRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://example.com/compat/users/123/unregister-email";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new UnregisterEmailRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "user_id", "user123" } };

        Assert.Throws<ArgumentNullException>(() =>
            new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var rawUrl = "https://example.com/compat/users/123/unregister-email";

        Assert.Throws<ArgumentNullException>(() =>
            new UnregisterEmailRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("unregister-email", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// PostAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.PostAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザのメールアドレスの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザのメールアドレスの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがnullのrequestConfigurationで正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullRequestConfiguration_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.PostAsync(null);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザのメールアドレスの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザのメールアドレスの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            await builder.PostAsync();
        });
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザのメールアドレス削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception("Unexpected error");

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
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
                @"^ユーザのメールアドレスの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
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
        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync());
    }

    /// <summary>
    /// WithUrlが新しいUnregisterEmailRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new UnregisterEmailRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/compat/users/123/unregister-email";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<UnregisterEmailRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
