using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationRoutings.Reorder;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects.Item.NotificationRoutings.Reorder;

/// <summary>
/// ReorderRequestBuilderのテスト
/// </summary>
public class ReorderRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ReorderRequestBuilder> _mockLogger;

    public ReorderRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<ReorderRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<ReorderRequestBuilder>().Returns(_mockLogger);
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

        var builder = new ReorderRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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

        var builder = new ReorderRequestBuilder("https://api.example.com/alerts/projects/1/notification-routings/reorder", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
        var builder = new ReorderRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new List<NotificationRoutingOrderRequest>());

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPutRequestInformationにnullボディを渡すと例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ReorderRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
    }

    /// <summary>
    /// WithUrlが新しいReorderRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ReorderRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects/1/notification-routings/reorder");

        Assert.NotNull(newBuilder);
        Assert.IsType<ReorderRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
            { "projectResourceId", string.Empty }
        };
        var builder = new ReorderRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new List<NotificationRoutingOrderRequest>
        {
            new NotificationRoutingOrderRequest
            {
                NotificationRoutingUid = new Guid(),
                Order = 1
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("プロジェクトリソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディパラメータにnullを入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("NotificationRoutingUid")]
    [InlineData("Order")]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<NotificationRoutingOrderRequest>
        {
            new NotificationRoutingOrderRequest
            {
                NotificationRoutingUid = invalidParameterName == "NotificationRoutingUid" ? null : new Guid(),
                Order = invalidParameterName == "Order" ? null : 1
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }
    
    /// <summary>
    /// PutAsyncのリクエストボディパラメータが有効な最小数値を下回った場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidMaximumRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<NotificationRoutingOrderRequest>
        {
            new NotificationRoutingOrderRequest
            {
                NotificationRoutingUid = new Guid(),
                Order = 0
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"Orderの値は1以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"Orderの値は1以上で入力してください。");
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
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<NotificationRoutingOrderRequest>
        {
            new NotificationRoutingOrderRequest
            {
                NotificationRoutingUid = new Guid(),
                Order = 1
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

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
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
                @"^通知ルーティング順序更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<NotificationRoutingOrderRequest>
        {
            new NotificationRoutingOrderRequest
            {
                NotificationRoutingUid = new Guid(),
                Order = 1
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
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
                @"^通知ルーティング順序の更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var body = new List<NotificationRoutingOrderRequest>
        {
            new NotificationRoutingOrderRequest
            {
                NotificationRoutingUid = new Guid(),
                Order = 1
            }
        };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);

        await builder.PutAsync(body);

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
                    @"^通知ルーティング順序の更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^通知ルーティング順序の更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
