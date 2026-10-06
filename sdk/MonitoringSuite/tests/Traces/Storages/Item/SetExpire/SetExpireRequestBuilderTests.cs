using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.MonitoringSuite.Traces.Storages.Item.SetExpire;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Traces.Storages.Item.SetExpire;

/// <summary>
/// SetExpireRequestBuilderのテスト
/// </summary>
public class SetExpireRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<SetExpireRequestBuilder> _mockLogger;

    public SetExpireRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<SetExpireRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<SetExpireRequestBuilder>().Returns(_mockLogger);
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
            { "resourceId", 1L }
        };

        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new SetExpireRequestBuilder("https://api.example.com/traces/storages/1/set-expire", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPostRequestInformationにnullボディを渡すと例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(
            new JsonSerializationWriterFactory());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetTraceStorageExpireDayRequest();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// WithUrlが新しいSetExpireRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/traces/storages/1/set-expire");

        Assert.NotNull(newBuilder);
        Assert.IsType<SetExpireRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// PostAsyncでアダプターがnullを返した場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync<TraceStorage>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<TraceStorage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<TraceStorage?>(null));
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync(new SetTraceStorageExpireDayRequest
        {
            Days = 30
        });

        Assert.Null(result);
    }

    /// <summary>
    /// PostAsyncのパスパラメータの値に空文字が入力された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", string.Empty }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, adapter, _mockLoggerFactory);
        var body = new SetTraceStorageExpireDayRequest
        {
            Days = 7
        };

        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("リソースIDを入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning("リソースIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータにnullを入力した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetTraceStorageExpireDayRequest
        {
            Days = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Daysが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Daysが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの値が有効な数値範囲外だった場合に例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(731)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetTraceStorageExpireDayRequest
        {
            Days = invalidParameterValue
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Daysの値は1以上730以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Daysの値は1以上730以下で入力してください。");
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
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetTraceStorageExpireDayRequest
        {
            Days = 7
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

        requestAdapter.SendAsync<TraceStorage>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<TraceStorage>>(),
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
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^トレースストレージ保存期間設定のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetTraceStorageExpireDayRequest
        {
            Days = 7
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<TraceStorage>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<TraceStorage>>(),
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
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^トレースストレージ保存期間の設定中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceId", 1L }
        };
        var builder = new SetExpireRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SetTraceStorageExpireDayRequest
        {
            Days = 7
        };
        var expectedResponse = new TraceStorage();

        requestAdapter.SendAsync<TraceStorage>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<TraceStorage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<TraceStorage>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<TraceStorage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^トレースストレージ保存期間の設定を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^トレースストレージ保存期間の設定が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータが有効な数値範囲外の場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(1000000000000)]
    public async Task PostAsync_WithOutOfRangePathParameter_ThrowsValidationException(long resourceId)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new SetExpireRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "resourceId", resourceId } }, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(new SetTraceStorageExpireDayRequest()));
        var expectedMessage = resourceId < 0 ? "リソースIDは0以上で入力してください。" : "リソースIDは999999999999以下で入力してください。";
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }
}
