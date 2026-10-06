using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Metrics.Routings.Item;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Metrics.Routings.Item;

/// <summary>
/// WithUItemRequestBuilderのテスト
/// </summary>
public class WithUItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithUItemRequestBuilder> _mockLogger;

    public WithUItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithUItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithUItemRequestBuilder>().Returns(_mockLogger);
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
            { "uid", Guid.NewGuid() }
        };

        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new WithUItemRequestBuilder("https://api.example.com/metrics/routings/some-uid", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドとBigIntヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsDeleteMethodAndBigIntHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, NullLoggerFactory.Instance);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPatchRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPatchRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(
            new JsonSerializationWriterFactory());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedMetricsRoutingRequest();

        var requestInfo = builder.ToPatchRequestInformation(body);

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
        requestAdapter.SerializationWriterFactory.Returns(
            new JsonSerializationWriterFactory());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new MetricsRoutingRequest();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// WithUrlが新しいWithUItemRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/metrics/routings/some-uid");

        Assert.NotNull(newBuilder);
        Assert.IsType<WithUItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", string.Empty }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal("UIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのuidパスパラメータにGUID形式でない値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidUidPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", "invalid-uid" }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            await builder.DeleteAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メトリクスルーティング削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.DeleteAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メトリクスルーティングの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncがRequestAdapter.SendNoContentAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティングの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティングの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでアダプターがnullを返した場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<WrappedMetricsRouting?>(null));
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// GetAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", string.Empty }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("UIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのuidパスパラメータにGUID形式でない値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidUidPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", "invalid-uid" }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
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
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
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
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メトリクスルーティング詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
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
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メトリクスルーティング詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new WrappedMetricsRouting();

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.Equal(expectedResponse, result);

        await requestAdapter.Received(1).SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティング詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティング詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncでアダプターがnullを返した場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<WrappedMetricsRouting?>(null));
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PatchAsync(new PatchedMetricsRoutingRequest());

        Assert.Null(result);
    }

    /// <summary>
    /// PatchAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", string.Empty }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new PatchedMetricsRoutingRequest();
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal("UIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDを入力してください。");
    }

    /// <summary>
    /// PatchAsyncのuidパスパラメータにGUID形式でない値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithInvalidUidPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", "invalid-uid" }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new PatchedMetricsRoutingRequest();
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディパラメータが有効な最小文字数を下回った場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("PublisherCode")]
    [InlineData("Variant")]
    public async Task PatchAsync_WithInvalidMinLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedMetricsRoutingRequest
        {
            PublisherCode = invalidParameterName == "PublisherCode" ? string.Empty : "test-publisher-code",
            Variant = invalidParameterName == "Variant" ? string.Empty : "test-variant"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"{invalidParameterName}は1文字以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は1文字以上で入力してください。");
    }

    /// <summary>
    /// PatchAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedMetricsRoutingRequest();
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

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PatchAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メトリクスルーティング部分更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedMetricsRoutingRequest();
        var expectedException = new Exception();

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PatchAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メトリクスルーティングの部分更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var body = new PatchedMetricsRoutingRequest();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new WrappedMetricsRouting();

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PatchAsync(body);

        Assert.Equal(expectedResponse, result);

        await requestAdapter.Received(1).SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティングの部分更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティングの部分更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでアダプターがnullを返した場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<WrappedMetricsRouting?>(null));
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PutAsync(new MetricsRoutingRequest
        {
            MetricsStorageId = 1L,
            PublisherCode = "publisher",
            Variant = "variant"
        });

        Assert.Null(result);
    }

    /// <summary>
    /// PutAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", string.Empty }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new MetricsRoutingRequest
        {
            PublisherCode = "test-publisher-code",
            Variant = "test-variant",
            MetricsStorageId = 1L
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("UIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのuidパスパラメータにGUID形式でない値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidUidPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", "invalid-uid" }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new MetricsRoutingRequest
        {
            PublisherCode = "test-publisher-code",
            Variant = "test-variant",
            MetricsStorageId = 1L
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディパラメータにnullを入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("PublisherCode")]
    [InlineData("Variant")]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new MetricsRoutingRequest
        {
            PublisherCode = invalidParameterName == "PublisherCode" ? null : "test-publisher-code",
            Variant = invalidParameterName == "Variant" ? null : "test-variant"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
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
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new MetricsRoutingRequest
        {
            PublisherCode = "test-publisher-code",
            Variant = "test-variant",
            MetricsStorageId = 1L
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

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
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
                @"^メトリクスルーティング更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new MetricsRoutingRequest
        {
            PublisherCode = "test-publisher-code",
            Variant = "test-variant",
            MetricsStorageId = 1L
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
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
                @"^メトリクスルーティングの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var body = new MetricsRoutingRequest
        {
            PublisherCode = "test-publisher-code",
            Variant = "test-variant",
            MetricsStorageId = 1L
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new WrappedMetricsRouting();

        requestAdapter.SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        Assert.Equal(expectedResponse, result);

        await requestAdapter.Received(1).SendAsync<WrappedMetricsRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedMetricsRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティングの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メトリクスルーティングの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
