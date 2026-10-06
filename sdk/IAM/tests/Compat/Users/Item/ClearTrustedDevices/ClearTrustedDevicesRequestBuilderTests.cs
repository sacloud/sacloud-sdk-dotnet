using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Compat.Users.Item.ClearTrustedDevices;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users.Item.ClearTrustedDevices;

/// <summary>
/// ClearTrustedDevicesRequestBuilderの単体テスト
/// </summary>
public class ClearTrustedDevicesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ClearTrustedDevicesRequestBuilder> _mockLogger;

    public ClearTrustedDevicesRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ClearTrustedDevicesRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ClearTrustedDevicesRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ClearTrustedDevicesRequestBuilder_Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ClearTrustedDevicesRequestBuilder_Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/compat/users/123/clear-trusted-devices";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new ClearTrustedDevicesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ClearTrustedDevicesRequestBuilder_Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>();

        Assert.Throws<ArgumentNullException>(() =>
            new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ClearTrustedDevicesRequestBuilder_Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var rawUrl = "https://example.com/compat/users/123/clear-trusted-devices";

        Assert.Throws<ArgumentNullException>(() =>
            new ClearTrustedDevicesRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ClearTrustedDevicesRequestBuilder_ToPostRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("clear-trusted-devices", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// PostAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task ClearTrustedDevicesRequestBuilder_PostAsync_CompletesSuccessfully()
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
        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
                @"^ユーザの全信頼済みデバイスの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザの全信頼済みデバイスの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがnullのrequestConfigurationで正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task ClearTrustedDevicesRequestBuilder_PostAsync_WithNullRequestConfiguration_CompletesSuccessfully()
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
        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
                @"^ユーザの全信頼済みデバイスの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザの全信頼済みデバイスの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task ClearTrustedDevicesRequestBuilder_PostAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^ユーザの全信頼済みデバイス削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task ClearTrustedDevicesRequestBuilder_PostAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "user_id", "user123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^ユーザの全信頼済みデバイスの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータにuser_idがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("user_id", "")]
    [InlineData("user_id", " ")]
    [InlineData("user_id", "　")]
    public async Task PostAsync_WithInvalidPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync();
        });

        Assert.Equal("ユーザIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザIDを入力してください。");
    }

    /// <summary>
    /// WithUrlが新しいClearTrustedDevicesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void ClearTrustedDevicesRequestBuilder_WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClearTrustedDevicesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/compat/users/123/clear-trusted-devices";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<ClearTrustedDevicesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
