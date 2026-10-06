using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationTargets;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationTargets.Item;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects.Item.NotificationTargets;

/// <summary>
/// NotificationTargetsRequestBuilderのテスト
/// </summary>
public class NotificationTargetsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<NotificationTargetsRequestBuilder> _mockLogger;

    public NotificationTargetsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<NotificationTargetsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<NotificationTargetsRequestBuilder>().Returns(_mockLogger);
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

        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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

        var builder = new NotificationTargetsRequestBuilder("https://api.example.com/alerts/projects/1/notification-targets", requestAdapter, _mockLoggerFactory);

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
        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);
        var id = Guid.NewGuid();

        var itemBuilder1 = builder[id];
        var itemBuilder2 = builder[id];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new NotificationTargetRequest());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// WithUrlが新しいNotificationTargetsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new NotificationTargetsRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects/1/notification-targets");

        Assert.NotNull(newBuilder);
        Assert.IsType<NotificationTargetsRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのServiceTypeプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ServiceType_CanBeSet()
    {
        var queryParams = new NotificationTargetsRequestBuilder.NotificationTargetsRequestBuilderGetQueryParameters
        {
            ServiceType = GetService_typeQueryParameterType.SAKURA_SIMPLE_NOTICE
        };

        Assert.Equal(GetService_typeQueryParameterType.SAKURA_SIMPLE_NOTICE, queryParams.ServiceType);
    }

    /// <summary>
    /// QueryParametersのCountプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Count_CanBeSet()
    {
        var queryParams = new NotificationTargetsRequestBuilder.NotificationTargetsRequestBuilderGetQueryParameters { Count = 25 };

        Assert.Equal(25, queryParams.Count);
    }

    /// <summary>
    /// QueryParametersのFromプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_From_CanBeSet()
    {
        var queryParams = new NotificationTargetsRequestBuilder.NotificationTargetsRequestBuilderGetQueryParameters { From = 10 };

        Assert.Equal(10, queryParams.From);
    }

    /// <summary>
    /// GetAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<PaginatedNotificationTargetList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationTargetList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<PaginatedNotificationTargetList?>(null));
        var builder = new NotificationTargetsRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, adapter, _mockLoggerFactory);

        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("プロジェクトリソースIDを入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncで無効なクエリパラメーターが指定された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidQueryParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", 1L }
        };
        var builder = new NotificationTargetsRequestBuilder(pathParameters, adapter, _mockLoggerFactory);

        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.ServiceType = (GetService_typeQueryParameterType)999;
        }));

        Assert.Equal("ServiceTypeはEnumに含まれる値を入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning("ServiceTypeはEnumに含まれる値を入力してください。");
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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<PaginatedNotificationTargetList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationTargetList>>(),
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
                @"^通知対象一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<PaginatedNotificationTargetList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationTargetList>>(),
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
                @"^通知対象一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new PaginatedNotificationTargetList();

        requestAdapter.SendAsync<PaginatedNotificationTargetList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationTargetList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<PaginatedNotificationTargetList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedNotificationTargetList>>(),
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
                    @"^通知対象一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知対象一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        adapter.SendAsync<NotificationTarget>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationTarget>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<NotificationTarget?>(null));
        var builder = new NotificationTargetsRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PostAsync(new NotificationTargetRequest { ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE });

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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, adapter, _mockLoggerFactory);
        var body = new NotificationTargetRequest
        {
            ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE
        };

        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("プロジェクトリソースIDを入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
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
            { "projectResourceId", 1L }
        };
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationTargetRequest
        {
            ServiceType = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("ServiceTypeが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("ServiceTypeが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの値が最大文字数を超過した場合に例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Url", 1024)]
    [InlineData("Description", 100)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", 1L }
        };
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationTargetRequest
        {
            ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE,
            Url = invalidParameterName == "Url" ? new string('a', validMaxLength + 1) : "test-url",
            Description = invalidParameterName == "Description" ? new string('a', validMaxLength + 1) : "test-description"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidEnumRequestBodyparameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", 1L }
        };
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationTargetRequest
        {
            ServiceType = (NotificationTargetRequest_service_type)999
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("ServiceTypeはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ServiceTypeはEnumに含まれる値を入力してください。");
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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationTargetRequest
        {
            ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE
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

        requestAdapter.SendAsync<NotificationTarget>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationTarget>>(),
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
                @"^通知対象作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationTargetRequest
        {
            ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<NotificationTarget>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationTarget>>(),
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
                @"^通知対象の作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new NotificationTargetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new NotificationTargetRequest
        {
            ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE
        };
        var expectedResponse = new NotificationTarget();

        requestAdapter.SendAsync<NotificationTarget>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationTarget>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<NotificationTarget>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<NotificationTarget>>(),
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
                    @"^通知対象の作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知対象の作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
