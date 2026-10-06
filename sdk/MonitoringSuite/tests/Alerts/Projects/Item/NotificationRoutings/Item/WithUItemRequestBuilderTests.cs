using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationRoutings.Item;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects.Item.NotificationRoutings.Item;

/// <summary>
/// NotificationRoutings.Item.WithUItemRequestBuilderのテスト
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

    private static Dictionary<string, object> PathParams() => new()
    {
        { "baseurl", "https://api.example.com" },
        { "projectResourceId", 1L },
        { "uid", Guid.NewGuid() }
    };

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var builder = new WithUItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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

        var builder = new WithUItemRequestBuilder("https://api.example.com/alerts/projects/1/notification-routings/abc", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

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
        var builder = new WithUItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPatchRequestInformationがPATCHメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPatchRequestInformation_CreatesRequestInformationWithPatchMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(new JsonSerializationWriterFactory());
        var builder = new WithUItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPatchRequestInformation(new PatchedNotificationRoutingRequest());

        Assert.Equal(Method.PATCH, requestInfo.HttpMethod);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(new JsonSerializationWriterFactory());
        var builder = new WithUItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new NotificationRoutingRequest());

        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
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
        var builder = new WithUItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects/1/notification-routings/abc");

        Assert.NotNull(newBuilder);
        Assert.IsType<WithUItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("projectResourceId", "プロジェクトリソースID")]
    [InlineData("uid", "UID")]
    public async Task DeleteAsync_WithEmptyPathParameter_ThrowsValidationException(string invalidParameterName, string displayName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", invalidParameterName == "projectResourceId" ? string.Empty : 1L },
            { "uid", invalidParameterName == "uid" ? string.Empty : Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal($"{displayName}を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{displayName}を入力してください。");
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
            { "projectResourceId", 1L },
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
        var pathParameters = PathParams();
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
                @"^通知ルーティング削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = PathParams();
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
                @"^通知ルーティングの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var pathParameters = PathParams();
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
                    @"^通知ルーティングの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティングの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<NotificationRouting?>(null));
        adapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// GetAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("projectResourceId", "プロジェクトリソースID")]
    [InlineData("uid", "UID")]
    public async Task GetAsync_WithEmptyPathParameter_ThrowsValidationException(string invalidParameterName, string displayName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", invalidParameterName == "projectResourceId" ? string.Empty : 1L },
            { "uid", invalidParameterName == "uid" ? string.Empty : Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal($"{displayName}を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{displayName}を入力してください。");
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
            { "projectResourceId", 1L },
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
        var pathParameters = PathParams();
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

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
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
                @"^通知ルーティング詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
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
                @"^通知ルーティング詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new NotificationRouting();

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.Equal(expectedResponse, result);

        await requestAdapter.Received(1).SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティング詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティング詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<NotificationRouting?>(null));
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PatchAsync(new PatchedNotificationRoutingRequest());

        Assert.Null(result);
    }

    /// <summary>
    /// PatchAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("projectResourceId", "プロジェクトリソースID")]
    [InlineData("uid", "UID")]
    public async Task PatchAsync_WithEmptyPathParameter_ThrowsValidationException(string invalidParameterName, string displayName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", invalidParameterName == "projectResourceId" ? string.Empty : 1L },
            { "uid", invalidParameterName == "uid" ? string.Empty : Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new PatchedNotificationRoutingRequest();
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"{displayName}を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{displayName}を入力してください。");
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
            { "projectResourceId", 1L },
            { "uid", "invalid-uid" }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new PatchedNotificationRoutingRequest();
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディパラメータにnullを入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("MatchLabels.Name")]
    [InlineData("MatchLabels.Value")]
    public async Task PatchAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedNotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest
                {
                    Name = invalidParameterName == "MatchLabels.Name" ? null : "valid-name",
                    Value = invalidParameterName == "MatchLabels.Value" ? null : "valid-value"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディパラメータが有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("MatchLabels.Name")]
    [InlineData("MatchLabels.Value")]
    public async Task PatchAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedNotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest
                {
                    Name = invalidParameterName == "MatchLabels.Name" ? new string('a', 257) : "valid-name",
                    Value = invalidParameterName == "MatchLabels.Value" ? new string('a', 257) : "valid-value"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"{invalidParameterName}は256文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は256文字以下で入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディパラメータが有効な数値範囲を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(721)]
    public async Task PatchAsync_WithInvalidMaximumRequestBodyParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedNotificationRoutingRequest
        {
            ResendIntervalMinutes = invalidParameterValue
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"ResendIntervalMinutesの値は0以上720以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"ResendIntervalMinutesの値は0以上720以下で入力してください。");
    }

    /// <summary>
    /// PatchAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedNotificationRoutingRequest();
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

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
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
                @"^通知ルーティング部分更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedNotificationRoutingRequest();
        var expectedException = new Exception();

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
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
                @"^通知ルーティングの部分更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var pathParameters = PathParams();
        var body = new PatchedNotificationRoutingRequest();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new NotificationRouting();

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PatchAsync(body);

        Assert.Equal(expectedResponse, result);

        await requestAdapter.Received(1).SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティングの部分更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティングの部分更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<NotificationRouting?>(null));
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PutAsync(new NotificationRoutingRequest { MatchLabels = [], NotificationTargetUid = Guid.NewGuid() });

        Assert.Null(result);
    }

    /// <summary>
    /// PutAsyncのパスパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("projectResourceId", "プロジェクトリソースID")]
    [InlineData("uid", "UID")]
    public async Task PutAsync_WithEmptyPathParameter_ThrowsValidationException(string invalidParameterName, string displayName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", invalidParameterName == "projectResourceId" ? string.Empty : 1L },
            { "uid", invalidParameterName == "uid" ? string.Empty : Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = new Guid(),
            MatchLabels = new List<MatchLabelsItemRequest>()
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{displayName}を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{displayName}を入力してください。");
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
            { "projectResourceId", 1L },
            { "uid", "invalid-uid" }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = new Guid(),
            MatchLabels = new List<MatchLabelsItemRequest>()
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディパラメータにnullを入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("NotificationTargetUid")]
    [InlineData("MatchLabels")]
    [InlineData("MatchLabels.Name")]
    [InlineData("MatchLabels.Value")]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = invalidParameterName == "NotificationTargetUid" ? null : new Guid(),
            MatchLabels = invalidParameterName == "MatchLabels" ? null : new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest
                {
                    Name = invalidParameterName == "MatchLabels.Name" ? null : "valid-name",
                    Value = invalidParameterName == "MatchLabels.Value" ? null : "valid-value"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディパラメータが有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("MatchLabels.Name")]
    [InlineData("MatchLabels.Value")]
    public async Task PutAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = new Guid(),
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest
                {
                    Name = invalidParameterName == "MatchLabels.Name" ? new string('a', 257) : "valid-name",
                    Value = invalidParameterName == "MatchLabels.Value" ? new string('a', 257) : "valid-value"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}は256文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は256文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディパラメータが有効な数値範囲を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(721)]
    public async Task PutAsync_WithInvalidMaximumRequestBodyParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = new Guid(),
            MatchLabels = new List<MatchLabelsItemRequest>(),
            ResendIntervalMinutes = invalidParameterValue
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"ResendIntervalMinutesの値は0以上720以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"ResendIntervalMinutesの値は0以上720以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = new Guid(),
            MatchLabels = new List<MatchLabelsItemRequest>()
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

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
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
                @"^通知ルーティング更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = PathParams();
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = new Guid(),
            MatchLabels = new List<MatchLabelsItemRequest>()
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
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
                @"^通知ルーティングの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var pathParameters = PathParams();
        var body = new NotificationRoutingRequest
        {
            NotificationTargetUid = new Guid(),
            MatchLabels = new List<MatchLabelsItemRequest>()
        };
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new NotificationRouting();

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        Assert.Equal(expectedResponse, result);

        await requestAdapter.Received(1).SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティングの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティングの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
