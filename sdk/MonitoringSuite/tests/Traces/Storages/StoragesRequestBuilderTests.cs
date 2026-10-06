using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.MonitoringSuite.Traces.Storages;
using Sacloud.Sdk.MonitoringSuite.Traces.Storages.Item;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Traces.Storages;

/// <summary>
/// StoragesRequestBuilderのテスト
/// </summary>
public class StoragesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<StoragesRequestBuilder> _mockLogger;

    public StoragesRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<StoragesRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<StoragesRequestBuilder>().Returns(_mockLogger);
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
            { "baseurl", "https://api.example.com" }
        };

        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new StoragesRequestBuilder("https://api.example.com/traces/storages", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいResourceItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithLong_ReturnsResourceItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[1L];

        Assert.NotNull(itemBuilder);
        Assert.IsType<ResourceItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder[1L];
        var itemBuilder2 = builder[1L];

        Assert.NotSame(itemBuilder1, itemBuilder2);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// WithUrlが新しいStoragesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/traces/storages");

        Assert.NotNull(newBuilder);
        Assert.IsType<StoragesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCountプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Count_CanBeSet()
    {
        var queryParams = new StoragesRequestBuilder.StoragesRequestBuilderGetQueryParameters
        {
            Count = 50
        };

        Assert.Equal(50, queryParams.Count);
    }

    /// <summary>
    /// QueryParametersのFromプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_From_CanBeSet()
    {
        var queryParams = new StoragesRequestBuilder.StoragesRequestBuilderGetQueryParameters
        {
            From = 10
        };

        Assert.Equal(10, queryParams.From);
    }

    /// <summary>
    /// QueryParametersのAccountIdプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_AccountId_CanBeSet()
    {
        var queryParams = new StoragesRequestBuilder.StoragesRequestBuilderGetQueryParameters
        {
            AccountId = "account-123"
        };

        Assert.Equal("account-123", queryParams.AccountId);
    }

    /// <summary>
    /// QueryParametersのResourceIdプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ResourceId_CanBeSet()
    {
        var queryParams = new StoragesRequestBuilder.StoragesRequestBuilderGetQueryParameters
        {
            ResourceId = 100L
        };

        Assert.Equal(100L, queryParams.ResourceId);
    }

    /// <summary>
    /// QueryParametersのLogStorageBucketClassificationプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_LogStorageBucketClassification_CanBeSet()
    {
        var queryParams = new StoragesRequestBuilder.StoragesRequestBuilderGetQueryParameters
        {
            LogStorageBucketClassification = Sacloud.Sdk.MonitoringSuite.Traces.Storages.GetLog_storage__bucket__classificationQueryParameterType.Dedicated
        };

        Assert.Equal(Sacloud.Sdk.MonitoringSuite.Traces.Storages.GetLog_storage__bucket__classificationQueryParameterType.Dedicated, queryParams.LogStorageBucketClassification);
    }

    /// <summary>
    /// GetAsyncでアダプターがnullを返した場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync<PaginatedTraceStorageList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedTraceStorageList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<PaginatedTraceStorageList?>(null));
        var builder = new StoragesRequestBuilder(
            new Dictionary<string, object>
            {
                { "baseurl", "https://api.example.com" }
            },
            requestAdapter,
            _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.Null(result);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, adapter, _mockLoggerFactory);

        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.LogStorageBucketClassification = (GetLog_storage__bucket__classificationQueryParameterType)999; // 無効な値を設定
        }));

        Assert.Equal("LogStorageBucketClassificationはEnumに含まれる値を入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning("LogStorageBucketClassificationはEnumに含まれる値を入力してください。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<PaginatedTraceStorageList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedTraceStorageList>>(),
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
                @"^トレースストレージ一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<PaginatedTraceStorageList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedTraceStorageList>>(),
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
                @"^トレースストレージ一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new PaginatedTraceStorageList();

        requestAdapter.SendAsync<PaginatedTraceStorageList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedTraceStorageList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<PaginatedTraceStorageList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedTraceStorageList>>(),
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
                    @"^トレースストレージ一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^トレースストレージ一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
        var builder = new StoragesRequestBuilder(
            new Dictionary<string, object>
            {
                { "baseurl", "https://api.example.com" }
            },
            requestAdapter,
            _mockLoggerFactory);

        var result = await builder.PostAsync(new TraceStorageCreateRequest
        {
            Name = "test-trace-storage"
        });

        Assert.Null(result);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest
        {
            Name = null

        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Nameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの値が最大文字数を超過した場合に例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", 64)]
    [InlineData("Description", 512)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest
        {
            Name = invalidParameterName == "Name" ? new string('a', validMaxLength + 1) : "test-name",
            Description = invalidParameterName == "Description" ? new string('a', validMaxLength + 1) : "test-description"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのクエリパラメータの値が有効な数値範囲外だった場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("KmsKeyId", -1)]
    [InlineData("KmsKeyId", 1000000000000)]
    [InlineData("ServicePrincipalId", -1)]
    [InlineData("ServicePrincipalId", 1000000000000)]
    public async Task PostAsync_WithInvalidRangeRequestBodyParameter_ThrowsValidationException(string invalidParameterName, long invalidParameterValue)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, adapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest
        {
            Name = "test-name",
            KmsKeyId = invalidParameterName == "KmsKeyId" ? invalidParameterValue : 1234567890,
            ServicePrincipalId = invalidParameterName == "ServicePrincipalId" ? invalidParameterValue : 1234567890
        };
        var result = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Equal($"{invalidParameterName}の値は0以上999999999999以下で入力してください。", result.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の値は0以上999999999999以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidEnumRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest
        {
            Classification = (TraceStorageCreateRequest_classification)999,
            Name = "test-name",
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("ClassificationはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ClassificationはEnumに含まれる値を入力してください。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest
        {
            Name = "test-name"
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
                @"^トレースストレージ作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest
        {
            Name = "test-name"
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
                @"^トレースストレージの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new StoragesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new TraceStorageCreateRequest
        {
            Name = "test-name"
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
                    @"^トレースストレージの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^トレースストレージの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
