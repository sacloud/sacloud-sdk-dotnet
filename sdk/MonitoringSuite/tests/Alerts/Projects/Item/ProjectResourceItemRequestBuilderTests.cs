using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.Histories;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.LogMeasureRules;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationRoutings;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationTargets;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.Rules;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects.Item;

/// <summary>
/// ProjectResourceItemRequestBuilderのテスト
/// </summary>
public class ProjectResourceItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ProjectResourceItemRequestBuilder> _mockLogger;

    public ProjectResourceItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<ProjectResourceItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<ProjectResourceItemRequestBuilder>().Returns(_mockLogger);
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

        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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

        var builder = new ProjectResourceItemRequestBuilder("https://api.example.com/alerts/projects/1", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// HistoriesプロパティがHistoriesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Histories_ReturnsHistoriesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Histories);
        Assert.IsType<HistoriesRequestBuilder>(builder.Histories);
    }

    /// <summary>
    /// LogMeasureRulesプロパティがLogMeasureRulesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRules_ReturnsLogMeasureRulesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.LogMeasureRules);
        Assert.IsType<LogMeasureRulesRequestBuilder>(builder.LogMeasureRules);
    }

    /// <summary>
    /// NotificationRoutingsプロパティがNotificationRoutingsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void NotificationRoutings_ReturnsNotificationRoutingsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.NotificationRoutings);
        Assert.IsType<NotificationRoutingsRequestBuilder>(builder.NotificationRoutings);
    }

    /// <summary>
    /// NotificationTargetsプロパティがNotificationTargetsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void NotificationTargets_ReturnsNotificationTargetsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.NotificationTargets);
        Assert.IsType<NotificationTargetsRequestBuilder>(builder.NotificationTargets);
    }

    /// <summary>
    /// RulesプロパティがRulesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Rules_ReturnsRulesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Rules);
        Assert.IsType<RulesRequestBuilder>(builder.Rules);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドのRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPatchRequestInformationにnullボディを渡すと例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPatchRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPatchRequestInformation(null!));
    }

    /// <summary>
    /// ToPutRequestInformationにnullボディを渡すと例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
    }

    /// <summary>
    /// ToPatchRequestInformationが正しいAcceptヘッダーとBigIntヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPatchRequestInformation_SetsAcceptAndBigIntHeaders()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(new JsonSerializationWriterFactory());
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPatchRequestInformation(new PatchedAlertProjectRequest());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーとBigIntヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptAndBigIntHeaders()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(new JsonSerializationWriterFactory());
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new AlertProjectRequest());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// WithUrlが新しいProjectResourceItemRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects/1");

        Assert.NotNull(newBuilder);
        Assert.IsType<ProjectResourceItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// GetAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<WrappedAlertProject?>(null));
        adapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// GetAsyncでパスパラメーターが空文字の場合にValidationExceptionをスローすることを確認
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
        Assert.Equal("プロジェクトリソースIDを入力してください。", exception.Message);
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
        var pathParameters = PathParams();
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
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
                @"^アラートプロジェクト詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
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
                @"^アラートプロジェクト詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new WrappedAlertProject();

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
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
                    @"^アラートプロジェクト詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクト詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
            { "projectResourceId", string.Empty }
        };
        var builder = new ProjectResourceItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal("プロジェクトリソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^アラートプロジェクト削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^アラートプロジェクトの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
                    @"^アラートプロジェクトの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクトの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
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
        adapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<WrappedAlertProject?>(null));
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PatchAsync(new PatchedAlertProjectRequest());

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
            { "projectResourceId", string.Empty }
        };
        var builder = new ProjectResourceItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new PatchedAlertProjectRequest();

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal("プロジェクトリソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータに無効な最大文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", 64)]
    [InlineData("Description", 512)]
    public async Task PatchAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParamaterName, int validMaxLength)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new PatchedAlertProjectRequest
        {
            Name = invalidParamaterName == "Name" ? new string('a', validMaxLength + 1) : "valid-name",
            Description = invalidParamaterName == "Description" ? new string('a', validMaxLength + 1) : "Valid Description",
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。");
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedAlertProjectRequest();
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

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
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
                @"^アラートプロジェクト部分更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedAlertProjectRequest();
        var expectedException = new Exception();

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
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
                @"^アラートプロジェクトの部分更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedAlertProjectRequest();
        var expectedResponse = new WrappedAlertProject();

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PatchAsync(body);

        await requestAdapter.Received(1).SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクトの部分更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクトの部分更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        adapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<WrappedAlertProject?>(null));
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PutAsync(new AlertProjectRequest());

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
            { "projectResourceId", string.Empty }
        };
        var builder = new ProjectResourceItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);
        var body = new AlertProjectRequest();

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("プロジェクトリソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータに無効な最大文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", 64)]
    [InlineData("Description", 512)]
    public async Task PutAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParamaterName, int validMaxLength)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new AlertProjectRequest
        {
            Name = invalidParamaterName == "Name" ? new string('a', validMaxLength + 1) : "valid-name",
            Description = invalidParamaterName == "Description" ? new string('a', validMaxLength + 1) : "Valid Description",
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。");
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectRequest();
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

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
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
                @"^アラートプロジェクト更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectRequest();
        var expectedException = new Exception();

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
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
                @"^アラートプロジェクトの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ProjectResourceItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectRequest();
        var expectedResponse = new WrappedAlertProject();

        requestAdapter.SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync<WrappedAlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedAlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクトの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクトの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// projectResourceId が欠落している場合に検証例外が発生することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ThrowsValidationException_WhenProjectResourceIdIsMissing()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectResourceItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Contains("プロジェクトリソースIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// GetAsyncのパスパラメータが有効な数値範囲外の場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(1000000000000)]
    public async Task GetAsync_WithOutOfRangePathParameter_ThrowsValidationException(long projectResourceId)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "projectResourceId", projectResourceId } }, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
        var expectedMessage = projectResourceId < 0 ? "プロジェクトリソースIDは0以上で入力してください。" : "プロジェクトリソースIDは999999999999以下で入力してください。";
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータが有効な数値範囲外の場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(1000000000000)]
    public async Task DeleteAsync_WithOutOfRangePathParameter_ThrowsValidationException(long projectResourceId)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "projectResourceId", projectResourceId } }, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());
        var expectedMessage = projectResourceId < 0 ? "プロジェクトリソースIDは0以上で入力してください。" : "プロジェクトリソースIDは999999999999以下で入力してください。";
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PatchAsyncのパスパラメータが有効な数値範囲外の場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(1000000000000)]
    public async Task PatchAsync_WithOutOfRangePathParameter_ThrowsValidationException(long projectResourceId)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "projectResourceId", projectResourceId } }, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(new PatchedAlertProjectRequest()));
        var expectedMessage = projectResourceId < 0 ? "プロジェクトリソースIDは0以上で入力してください。" : "プロジェクトリソースIDは999999999999以下で入力してください。";
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncのパスパラメータが有効な数値範囲外の場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(1000000000000)]
    public async Task PutAsync_WithOutOfRangePathParameter_ThrowsValidationException(long projectResourceId)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new ProjectResourceItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "projectResourceId", projectResourceId } }, adapter, _mockLoggerFactory);
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new AlertProjectRequest()));
        var expectedMessage = projectResourceId < 0 ? "プロジェクトリソースIDは0以上で入力してください。" : "プロジェクトリソースIDは999999999999以下で入力してください。";
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }
}
