using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.AppRunDedicated.Service_classes.Worker;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Service_classes.Worker;

/// <summary>
/// WorkerRequestBuilderのテスト
/// </summary>
public class WorkerRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WorkerRequestBuilder> _mockLogger;

    public WorkerRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WorkerRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WorkerRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters）が正常に動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl）が正常に動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/service_classes/worker";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WorkerRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationがnullのrequestConfigurationで正常に動作することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithNullConfiguration_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(null);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListWorkerServiceClassesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerServiceClassesResponse>>(),
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
                @"^ワーカノードサービスクラス一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListWorkerServiceClassesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerServiceClassesResponse>>(),
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
                @"^ワーカノードサービスクラス一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが正しくRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapter()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListWorkerServiceClassesResponse();

        requestAdapter.SendAsync<ListWorkerServiceClassesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerServiceClassesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerServiceClassesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワーカノードサービスクラス一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワーカノードサービスクラス一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがCancellationTokenを正しく渡すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_PassesCancellationToken()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var cancellationToken = new CancellationToken();

        await builder.GetAsync(cancellationToken: cancellationToken);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerServiceClassesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken);
    }

    /// <summary>
    /// GetAsyncがnullのrequestConfigurationで正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithNullConfiguration_Succeeds()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.GetAsync(null, CancellationToken.None);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerServiceClassesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// WithUrlが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WorkerRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/service_classes/worker";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<WorkerRequestBuilder>(newBuilder);
    }
}
