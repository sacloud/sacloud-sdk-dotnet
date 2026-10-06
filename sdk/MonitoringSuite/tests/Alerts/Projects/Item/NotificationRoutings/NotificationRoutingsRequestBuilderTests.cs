using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationRoutings;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationRoutings.Item;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationRoutings.Reorder;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects.Item.NotificationRoutings;

/// <summary>
/// NotificationRoutingsRequestBuilderのテスト
/// </summary>
public class NotificationRoutingsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<NotificationRoutingsRequestBuilder> _mockLogger;

    public NotificationRoutingsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<NotificationRoutingsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<NotificationRoutingsRequestBuilder>().Returns(_mockLogger);
    }

    private static Dictionary<string, object> PathParams() => new()
    {
        { "baseurl", "https://api.example.com" },
        { "projectResourceId", 1L }
    };

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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

        var builder = new NotificationRoutingsRequestBuilder("https://api.example.com/alerts/projects/1/notification-routings", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithUItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithGuid_ReturnsWithUItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[Guid.NewGuid()];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithUItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);
        var id = Guid.NewGuid();

        var itemBuilder1 = builder[id];
        var itemBuilder2 = builder[id];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// ReorderプロパティがReorderRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Reorder_ReturnsReorderRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Reorder);
        Assert.IsType<ReorderRequestBuilder>(builder.Reorder);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
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
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPostRequestInformationにnullボディを渡すと例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        requestAdapter.SerializationWriterFactory.Returns(new JsonSerializationWriterFactory());
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new NotificationRoutingRequest());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// WithUrlが新しいNotificationRoutingsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects/1/notification-routings");

        Assert.NotNull(newBuilder);
        Assert.IsType<NotificationRoutingsRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCountプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Count_CanBeSet()
    {
        var queryParams = new NotificationRoutingsRequestBuilder.NotificationRoutingsRequestBuilderGetQueryParameters { Count = 5 };

        Assert.Equal(5, queryParams.Count);
    }

    /// <summary>
    /// QueryParametersのFromプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_From_CanBeSet()
    {
        var queryParams = new NotificationRoutingsRequestBuilder.NotificationRoutingsRequestBuilderGetQueryParameters { From = 0 };

        Assert.Equal(0, queryParams.From);
    }

    /// <summary>
    /// QueryParametersのTargetプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Target_CanBeSet()
    {
        var queryParams = new NotificationRoutingsRequestBuilder.NotificationRoutingsRequestBuilderGetQueryParameters { Target = 1 };

        Assert.Equal(1, queryParams.Target);
    }

    /// <summary>
    /// GetAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<PaginatedNotificationRoutingList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationRoutingList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<PaginatedNotificationRoutingList?>(null));
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// GetAsyncのパスパラメータの値に空文字が入力された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", string.Empty }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, adapter, _mockLoggerFactory);

        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("プロジェクトリソースIDを入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
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
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<PaginatedNotificationRoutingList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationRoutingList>>(),
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
                @"^通知ルーティング一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<PaginatedNotificationRoutingList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationRoutingList>>(),
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
                @"^通知ルーティング一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new PaginatedNotificationRoutingList();

        requestAdapter.SendAsync<PaginatedNotificationRoutingList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationRoutingList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<PaginatedNotificationRoutingList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationRoutingList>>(),
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
                    @"^通知ルーティング一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティング一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<NotificationRouting?>(null));
        var builder = new NotificationRoutingsRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PostAsync(new NotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest { Name = "key", Value = "value" }
            },
            NotificationTargetUid = Guid.NewGuid()
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
            { "projectResourceId", string.Empty }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, adapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest { Name = "test-name", Value = "test-value" }
            },
            NotificationTargetUid = Guid.NewGuid()
        };

        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("プロジェクトリソースIDを入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータにnullを入力した場合に例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("NotificationTargetUid")]
    [InlineData("MatchLabels")]
    [InlineData("MatchLabels.Name")]
    [InlineData("MatchLabels.Value")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            MatchLabels = invalidParameterName == "MatchLabels" ? null : new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest
                { 
                    Name = invalidParameterName == "MatchLabels.Name" ? null : "test-name",
                    Value = invalidParameterName == "MatchLabels.Value" ? null : "test-value"
                }
            },
            NotificationTargetUid = invalidParameterName == "NotificationTargetUid" ? null : new Guid()
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの値が最大文字数を超過した場合に例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("MatchLabels.Name")]
    [InlineData("MatchLabels.Value")]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest
                {
                    Name = invalidParameterName == "MatchLabels.Name" ? new string('a', 257) : "test-name",
                    Value = invalidParameterName == "MatchLabels.Value" ? new string('a', 257) : "test-value"
                }
            },
            NotificationTargetUid = new Guid()
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}は256文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は256文字以下で入力してください。");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(721)]
    public async Task PostAsync_WithInvalidRangeRequestBodyparameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest
                {
                    Name = "test-name",
                    Value = "test-value"
                }
            },
            NotificationTargetUid = new Guid(),
            ResendIntervalMinutes = invalidParameterValue
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("ResendIntervalMinutesの値は0以上720以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ResendIntervalMinutesの値は0以上720以下で入力してください。");
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
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest { Name = "test-name", Value = "test-value" }
            },
            NotificationTargetUid = Guid.NewGuid()
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
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^通知ルーティング作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest { Name = "test-name", Value = "test-value" }
            },
            NotificationTargetUid = Guid.NewGuid()
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
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^通知ルーティングの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "projectResourceId", 1L }
        };
        var builder = new NotificationRoutingsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationRoutingRequest
        {
            MatchLabels = new List<MatchLabelsItemRequest>
            {
                new MatchLabelsItemRequest { Name = "test-name", Value = "test-value" }
            },
            NotificationTargetUid = Guid.NewGuid()
        };
        var expectedResponse = new NotificationRouting();

        requestAdapter.SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<NotificationRouting>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationRouting>>(),
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
                    @"^通知ルーティングの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティングの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
