using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.LogMeasureRules.Item;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects.Item.LogMeasureRules.Item;

/// <summary>
/// LogMeasureRules.Item.WithUItemRequestBuilderのテスト
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

        var builder = new WithUItemRequestBuilder("https://api.example.com/alerts/projects/1/log-measure-rules/abc", requestAdapter, _mockLoggerFactory);

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

        var requestInfo = builder.ToPatchRequestInformation(new PatchedLogMeasureRuleRequest());

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

        var requestInfo = builder.ToPutRequestInformation(new LogMeasureRuleRequest());

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

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects/1/log-measure-rules/abc");

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
                @"^ログ計測ルール削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
                @"^ログ計測ルールの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
                    @"^ログ計測ルールの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルールの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
    /// GetAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<LogMeasureRule?>(null));
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

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
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
                @"^ログ計測ルール詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
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
                @"^ログ計測ルール詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var expectedResponse = new LogMeasureRule();

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルール詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルール詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
        var body = new PatchedLogMeasureRuleRequest();

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
        var body = new PatchedLogMeasureRuleRequest();

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PatchAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<LogMeasureRule?>(null));
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PatchAsync(new PatchedLogMeasureRuleRequest());

        Assert.Null(result);
    }

    /// <summary>
    /// PatchAsyncがnullボディで検証例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_ThrowsValidationException_WhenBodyIsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(null!));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのプロパティにnull値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Rule.Query")]
    [InlineData("Rule.Version")]
    [InlineData("Rule.Query.Matchers")]
    [InlineData("Rule.Query.Matchers.AndMatcher.Type")]
    [InlineData("Rule.Query.Matchers.AndMatcher.Matchers")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Type")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Field")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Value")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.ValueList")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.ValueList")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Value")]
    [InlineData("Rule.Query.Matchers.NumMatcher.ValueList")]
    [InlineData("Rule.Query.Matchers.OrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.OrMatcher.Matchers")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Value")]
    [InlineData("Rule.Query.Matchers.StrMatcher.ValueList")]

    public async Task PatchAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new PatchedLogMeasureRuleRequest
        {
            Rule = new LogMeasureRuleModelRequest
            {
                Query = invalidParameterName == "Rule.Query" ? null : new LogMeasureRuleV1
                {
                    Matchers = invalidParameterName == "Rule.Query.Matchers" ? null : new List<FieldMatcher>
                    {
                        invalidParameterName.Contains("AndMatcher") ? new FieldMatcher
                        {
                            AndMatcher = new AndMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.AndMatcher.Type" ? null : AndMatcher_type.And,
                                Matchers = invalidParameterName == "Rule.Query.Matchers.AndMatcher.Matchers" ? null : new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("BoolMatcher") ? new FieldMatcher
                        {
                            BoolMatcher = new BoolMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Type" ? null : BoolMatcher_type.Boolean,
                                Field = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Field" ? null : BooleanFieldName.Http_cache_lookup,
                                Operator = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Operator" ? null : Operator2.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Value" ? null : true
                            }
                        } : invalidParameterName.Contains("EnumMatcher") ? new FieldMatcher
                        {
                            EnumMatcher = new EnumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Type" ? null : EnumMatcher_type.Enum,
                                Field = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Field" ? null : FieldModel.Severity,
                                Value = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Value" ? null : new List<ValueEnum?>()
                            }
                        } : invalidParameterName.Contains("MapKeyExistsMatcher") ? new FieldMatcher
                        {
                            MapKeyExistsMatcher = new MapKeyExistsMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Type" ? null : MapKeyExistsMatcher_type.MapKeyExists,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Key" ? null : "valid-key"
                            }
                        } : invalidParameterName.Contains("MapKeyValueMatcher") ? new FieldMatcher
                        {
                            MapKeyValueMatcher = new MapKeyValueMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Type" ? null : MapKeyValueMatcher_type.MapKeyValueMatcher,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Key" ? null : "valid-key",
                                Value = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Value" ? null : "valid-value"
                            }
                        } : invalidParameterName.Contains("MapValueNumMatcher") ? new FieldMatcher
                        {
                            MapValueNumMatcher = new MapValueNumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Type" ? null : MapValueNumMatcher_type.MapValueNumber,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Key" ? null : "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Operator" ? null : Operator4.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Value" ? null : 100,
                                ValueList = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.ValueList" ? null : new List<double?>()
                            }
                        } : invalidParameterName.Contains("MapValueStrMatcher") ? new FieldMatcher
                        {
                            MapValueStrMatcher = new MapValueStrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Type" ? null : MapValueStrMatcher_type.MapValueString,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Key" ? null : "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Operator" ? null : Operator3.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Value" ? null : "valid-value",
                                ValueList = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.ValueList" ? null : new List<string>()
                            }
                        } : invalidParameterName.Contains("NumMatcher") ? new FieldMatcher
                        {
                            NumMatcher = new NumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Type" ? null : NumMatcher_type.Number,
                                Field = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Field" ? null : NumberFieldName.Http_request_size,
                                Operator = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Operator" ? null : Operator1.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Value" ? null : 100,
                                ValueList = invalidParameterName == "Rule.Query.Matchers.NumMatcher.ValueList" ? null : new List<double?>()
                            }
                        } : invalidParameterName.Contains("OrMatcher") ? new FieldMatcher
                        {
                            OrMatcher = new OrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.OrMatcher.Type" ? null : OrMatcher_type.Or,
                                Matchers = invalidParameterName == "Rule.Query.Matchers.OrMatcher.Matchers" ? null : new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("StrMatcher") ? new FieldMatcher
                        {
                            StrMatcher = new StrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Type" ? null : StrMatcher_type.String,
                                Field = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Field" ? null : StringFieldName.Log_name,
                                Operator = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Operator" ? null : Operator.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Value" ? null : "valid-value",
                                ValueList = invalidParameterName == "Rule.Query.Matchers.StrMatcher.ValueList" ? null : new List<string>()
                            }
                        } : new FieldMatcher()
                    }
                },
                Version = invalidParameterName == "Rule.Version" ? null : LogMeasureRuleVersionEnum.V1
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));
        
        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータに無効な最大文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    /// <param name="invalidParamaterName"></param>
    /// <param name="validMaxLength"></param>
    /// <returns></returns>
    [Theory]
    [InlineData("Description", 256)]
    [InlineData("Name", 256)]
    public async Task PatchAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParamaterName, int validMaxLength)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new PatchedLogMeasureRuleRequest
        {
            Description = invalidParamaterName == "Description" ? new string('a', validMaxLength + 1) : "Valid Description",
            Name = invalidParamaterName == "Name" ? new string('a', validMaxLength + 1) : "valid-name"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータに無効な列挙値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Rule.Version")]
    [InlineData("Rule.Query.Matchers.AndMatcher.Type")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Field")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Type")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.OrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Type")]
    public async Task PatchAsync_WithInvalidEnumRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new PatchedLogMeasureRuleRequest
        {
            Rule = new LogMeasureRuleModelRequest
            {
                Version = invalidParameterName == "Rule.Version" ? (LogMeasureRuleVersionEnum?)999 : LogMeasureRuleVersionEnum.V1,
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>
                    {
                        invalidParameterName.Contains("AndMatcher") ? new FieldMatcher
                        {
                            AndMatcher = new AndMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.AndMatcher.Type" ? (AndMatcher_type)999 : AndMatcher_type.And,
                                Matchers = new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("BoolMatcher") ? new FieldMatcher
                        {
                            BoolMatcher = new BoolMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Type" ? (BoolMatcher_type)999 : BoolMatcher_type.Boolean,
                                Field = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Field" ? (BooleanFieldName)999 : BooleanFieldName.Http_cache_lookup,
                                Operator = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Operator" ? (Operator2)999 : Operator2.Eq,
                                Value = false
                            }
                        } : invalidParameterName.Contains("EnumMatcher") ? new FieldMatcher
                        {
                            EnumMatcher = new EnumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Type" ? (EnumMatcher_type)999 : EnumMatcher_type.Enum,
                                Field = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Field" ? (FieldModel)999 : FieldModel.Severity,
                                Value = new List<ValueEnum?>()
                            }
                        } : invalidParameterName.Contains("MapKeyExistsMatcher") ? new FieldMatcher
                        {
                            MapKeyExistsMatcher = new MapKeyExistsMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Type" ? (MapKeyExistsMatcher_type)999 : MapKeyExistsMatcher_type.MapKeyExists,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key"
                            }
                        } : invalidParameterName.Contains("MapKeyValueMatcher") ? new FieldMatcher
                        {
                            MapKeyValueMatcher = new MapKeyValueMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Type" ? (MapKeyValueMatcher_type)999 : MapKeyValueMatcher_type.MapKeyValueMatcher,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key",
                                Value = "valid-value"
                            }
                        } : invalidParameterName.Contains("MapValueNumMatcher") ? new FieldMatcher
                        {
                            MapValueNumMatcher = new MapValueNumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Type" ? (MapValueNumMatcher_type)999 : MapValueNumMatcher_type.MapValueNumber,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Operator" ? (Operator4)999 : Operator4.Eq,
                                Value = 100,
                                ValueList = new List<double?>()
                            }
                        } : invalidParameterName.Contains("MapValueStrMatcher") ? new FieldMatcher
                        {
                            MapValueStrMatcher = new MapValueStrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Type" ? (MapValueStrMatcher_type)999 : MapValueStrMatcher_type.MapValueString,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Operator" ? (Operator3)999 : Operator3.Eq,
                                Value = "valid-value",
                                ValueList = new List<string>()
                            }
                        } : invalidParameterName.Contains("NumMatcher") ? new FieldMatcher
                        {
                            NumMatcher = new NumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Type" ? (NumMatcher_type)999 : NumMatcher_type.Number,
                                Field = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Field" ? (NumberFieldName)999 : NumberFieldName.Http_request_size,
                                Operator = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Operator" ? (Operator1)999 : Operator1.Eq,
                                Value = 100,
                                ValueList = new List<double?>()
                            }
                        } : invalidParameterName.Contains("OrMatcher") ? new FieldMatcher
                        {
                            OrMatcher = new OrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.OrMatcher.Type" ? (OrMatcher_type)999 : OrMatcher_type.Or,
                                Matchers = new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("StrMatcher") ? new FieldMatcher
                        {
                            StrMatcher = new StrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Type" ? (StrMatcher_type)999 : StrMatcher_type.String,
                                Field = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Field" ? (StringFieldName)999 : StringFieldName.Log_name,
                                Operator = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Operator" ? (Operator)999 : Operator.Eq,
                                Value = "valid-value",
                                ValueList = new List<string>()
                            }
                        } : new FieldMatcher()
                    }
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    [Theory]
    [InlineData("InvalidName")]
    [InlineData("invalid name")]
    [InlineData("invalid+name")]
    [InlineData("無効なName")]
    public async Task PatchAsync_WithInvalidRegexRequestBodyparamter_ThrowsValidationException(string invalidParameterValue)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new PatchedLogMeasureRuleRequest
        {
            Name = invalidParameterValue
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(body));

        Assert.Equal("Nameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
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
        var body = new PatchedLogMeasureRuleRequest();
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

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
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
                @"^ログ計測ルール部分更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var body = new PatchedLogMeasureRuleRequest();
        var expectedException = new Exception();

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
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
                @"^ログ計測ルールの部分更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchedLogMeasureRuleRequest();
        var expectedResponse = new LogMeasureRule();

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PatchAsync(body);

        await requestAdapter.Received(1).SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルールの部分更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルールの部分更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
        var body = new LogMeasureRuleRequest();

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
        var body = new LogMeasureRuleRequest();

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("UIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<LogMeasureRule?>(null));
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PutAsync(new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 2L,
            Rule = new LogMeasureRuleModelRequest
            {
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>()
                },
                Version = LogMeasureRuleVersionEnum.V1
            }
        });

        Assert.Null(result);
    }

    /// <summary>
    /// PutAsyncがnullボディで検証例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_ThrowsValidationException_WhenBodyIsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null!));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータにnull値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Rule.Query")]
    [InlineData("Rule.Version")]
    [InlineData("Rule.Query.Matchers")]
    [InlineData("Rule.Query.Matchers.AndMatcher.Type")]
    [InlineData("Rule.Query.Matchers.AndMatcher.Matchers")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Type")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Field")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Value")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.ValueList")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Key")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Value")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.ValueList")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Value")]
    [InlineData("Rule.Query.Matchers.NumMatcher.ValueList")]
    [InlineData("Rule.Query.Matchers.OrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.OrMatcher.Matchers")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Value")]
    [InlineData("Rule.Query.Matchers.StrMatcher.ValueList")]

    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 2L,
            Rule = new LogMeasureRuleModelRequest
            {
                Query = invalidParameterName == "Rule.Query" ? null : new LogMeasureRuleV1
                {
                    Matchers = invalidParameterName == "Rule.Query.Matchers" ? null : new List<FieldMatcher>
                    {
                        invalidParameterName.Contains("AndMatcher") ? new FieldMatcher
                        {
                            AndMatcher = new AndMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.AndMatcher.Type" ? null : AndMatcher_type.And,
                                Matchers = invalidParameterName == "Rule.Query.Matchers.AndMatcher.Matchers" ? null : new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("BoolMatcher") ? new FieldMatcher
                        {
                            BoolMatcher = new BoolMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Type" ? null : BoolMatcher_type.Boolean,
                                Field = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Field" ? null : BooleanFieldName.Http_cache_lookup,
                                Operator = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Operator" ? null : Operator2.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Value" ? null : true
                            }
                        } : invalidParameterName.Contains("EnumMatcher") ? new FieldMatcher
                        {
                            EnumMatcher = new EnumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Type" ? null : EnumMatcher_type.Enum,
                                Field = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Field" ? null : FieldModel.Severity,
                                Value = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Value" ? null : new List<ValueEnum?>()
                            }
                        } : invalidParameterName.Contains("MapKeyExistsMatcher") ? new FieldMatcher
                        {
                            MapKeyExistsMatcher = new MapKeyExistsMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Type" ? null : MapKeyExistsMatcher_type.MapKeyExists,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Key" ? null : "valid-key"
                            }
                        } : invalidParameterName.Contains("MapKeyValueMatcher") ? new FieldMatcher
                        {
                            MapKeyValueMatcher = new MapKeyValueMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Type" ? null : MapKeyValueMatcher_type.MapKeyValueMatcher,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Key" ? null : "valid-key",
                                Value = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Value" ? null : "valid-value"
                            }
                        } : invalidParameterName.Contains("MapValueNumMatcher") ? new FieldMatcher
                        {
                            MapValueNumMatcher = new MapValueNumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Type" ? null : MapValueNumMatcher_type.MapValueNumber,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Key" ? null : "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Operator" ? null : Operator4.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Value" ? null : 100,
                                ValueList = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.ValueList" ? null : new List<double?>()
                            }
                        } : invalidParameterName.Contains("MapValueStrMatcher") ? new FieldMatcher
                        {
                            MapValueStrMatcher = new MapValueStrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Type" ? null : MapValueStrMatcher_type.MapValueString,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Field" ? null : MapFieldName.Attributes,
                                Key = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Key" ? null : "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Operator" ? null : Operator3.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Value" ? null : "valid-value",
                                ValueList = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.ValueList" ? null : new List<string>()
                            }
                        } : invalidParameterName.Contains("NumMatcher") ? new FieldMatcher
                        {
                            NumMatcher = new NumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Type" ? null : NumMatcher_type.Number,
                                Field = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Field" ? null : NumberFieldName.Http_request_size,
                                Operator = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Operator" ? null : Operator1.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Value" ? null : 100,
                                ValueList = invalidParameterName == "Rule.Query.Matchers.NumMatcher.ValueList" ? null : new List<double?>()
                            }
                        } : invalidParameterName.Contains("OrMatcher") ? new FieldMatcher
                        {
                            OrMatcher = new OrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.OrMatcher.Type" ? null : OrMatcher_type.Or,
                                Matchers = invalidParameterName == "Rule.Query.Matchers.OrMatcher.Matchers" ? null : new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("StrMatcher") ? new FieldMatcher
                        {
                            StrMatcher = new StrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Type" ? null : StrMatcher_type.String,
                                Field = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Field" ? null : StringFieldName.Log_name,
                                Operator = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Operator" ? null : Operator.Eq,
                                Value = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Value" ? null : "valid-value",
                                ValueList = invalidParameterName == "Rule.Query.Matchers.StrMatcher.ValueList" ? null : new List<string>()
                            }
                        } : new FieldMatcher()
                    }
                },
                Version = invalidParameterName == "Rule.Version" ? null : LogMeasureRuleVersionEnum.V1
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータに無効な最大文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Description", 256)]
    [InlineData("Name", 256)]
    public async Task PutAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParamaterName, int validMaxLength)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new LogMeasureRuleRequest
        {
            Description = invalidParamaterName == "Description" ? new string('a', validMaxLength + 1) : "Valid Description",
            Name = invalidParamaterName == "Name" ? new string('a', validMaxLength + 1) : "valid-name",
            Rule = new LogMeasureRuleModelRequest()
            {
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>(),
                },
                Version = LogMeasureRuleVersionEnum.V1
            },
            LogStorageId = 1L,
            MetricsStorageId = 2L
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータに無効な列挙値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Rule.Version")]
    [InlineData("Rule.Query.Matchers.AndMatcher.Type")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Field")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.BoolMatcher.Type")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.EnumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyExistsMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapKeyValueMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueNumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.MapValueStrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Field")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.NumMatcher.Type")]
    [InlineData("Rule.Query.Matchers.OrMatcher.Type")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Field")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Operator")]
    [InlineData("Rule.Query.Matchers.StrMatcher.Type")]
    public async Task PutAsync_WithInvalidEnumRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new LogMeasureRuleRequest
        {
            Rule = new LogMeasureRuleModelRequest
            {
                Version = invalidParameterName == "Rule.Version" ? (LogMeasureRuleVersionEnum)999 : LogMeasureRuleVersionEnum.V1,
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>
                    {
                        invalidParameterName.Contains("AndMatcher") ? new FieldMatcher
                        {
                            AndMatcher = new AndMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.AndMatcher.Type" ? (AndMatcher_type)999 : AndMatcher_type.And,
                                Matchers = new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("BoolMatcher") ? new FieldMatcher
                        {
                            BoolMatcher = new BoolMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Type" ? (BoolMatcher_type)999 : BoolMatcher_type.Boolean,
                                Field = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Field" ? (BooleanFieldName)999 : BooleanFieldName.Http_cache_lookup,
                                Operator = invalidParameterName == "Rule.Query.Matchers.BoolMatcher.Operator" ? (Operator2)999 : Operator2.Eq,
                                Value = false
                            }
                        } : invalidParameterName.Contains("EnumMatcher") ? new FieldMatcher
                        {
                            EnumMatcher = new EnumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Type" ? (EnumMatcher_type)999 : EnumMatcher_type.Enum,
                                Field = invalidParameterName == "Rule.Query.Matchers.EnumMatcher.Field" ? (FieldModel)999 : FieldModel.Severity,
                                Value = new List<ValueEnum?>()
                            }
                        } : invalidParameterName.Contains("MapKeyExistsMatcher") ? new FieldMatcher
                        {
                            MapKeyExistsMatcher = new MapKeyExistsMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Type" ? (MapKeyExistsMatcher_type)999 : MapKeyExistsMatcher_type.MapKeyExists,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyExistsMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key"
                            }
                        } : invalidParameterName.Contains("MapKeyValueMatcher") ? new FieldMatcher
                        {
                            MapKeyValueMatcher = new MapKeyValueMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Type" ? (MapKeyValueMatcher_type)999 : MapKeyValueMatcher_type.MapKeyValueMatcher,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapKeyValueMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key",
                                Value = "valid-value"
                            }
                        } : invalidParameterName.Contains("MapValueNumMatcher") ? new FieldMatcher
                        {
                            MapValueNumMatcher = new MapValueNumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Type" ? (MapValueNumMatcher_type)999 : MapValueNumMatcher_type.MapValueNumber,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueNumMatcher.Operator" ? (Operator4)999 : Operator4.Eq,
                                Value = 100,
                                ValueList = new List<double?>()
                            }
                        } : invalidParameterName.Contains("MapValueStrMatcher") ? new FieldMatcher
                        {
                            MapValueStrMatcher = new MapValueStrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Type" ? (MapValueStrMatcher_type)999 : MapValueStrMatcher_type.MapValueString,
                                Field = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Field" ? (MapFieldName)999 : MapFieldName.Attributes,
                                Key = "valid-key",
                                Operator = invalidParameterName == "Rule.Query.Matchers.MapValueStrMatcher.Operator" ? (Operator3)999 : Operator3.Eq,
                                Value = "valid-value",
                                ValueList = new List<string>()
                            }
                        } : invalidParameterName.Contains("NumMatcher") ? new FieldMatcher
                        {
                            NumMatcher = new NumMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Type" ? (NumMatcher_type)999 : NumMatcher_type.Number,
                                Field = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Field" ? (NumberFieldName)999 : NumberFieldName.Http_request_size,
                                Operator = invalidParameterName == "Rule.Query.Matchers.NumMatcher.Operator" ? (Operator1)999 : Operator1.Eq,
                                Value = 100,
                                ValueList = new List<double?>()
                            }
                        } : invalidParameterName.Contains("OrMatcher") ? new FieldMatcher
                        {
                            OrMatcher = new OrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.OrMatcher.Type" ? (OrMatcher_type)999 : OrMatcher_type.Or,
                                Matchers = new List<FieldMatcher>()
                            }
                        } : invalidParameterName.Contains("StrMatcher") ? new FieldMatcher
                        {
                            StrMatcher = new StrMatcher
                            {
                                Type = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Type" ? (StrMatcher_type)999 : StrMatcher_type.String,
                                Field = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Field" ? (StringFieldName)999 : StringFieldName.Log_name,
                                Operator = invalidParameterName == "Rule.Query.Matchers.StrMatcher.Operator" ? (Operator)999 : Operator.Eq,
                                Value = "valid-value",
                                ValueList = new List<string>()
                            }
                        } : new FieldMatcher()
                    }
                }
            },
            LogStorageId = 1L,
            MetricsStorageId = 2L
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータに無効な正規表現の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("InvalidName")]
    [InlineData("invalid name")]
    [InlineData("invalid+name")]
    [InlineData("無効なName")]
    public async Task PutAsync_WithInvalidRegexRequestBodyparamter_ThrowsValidationException(string invalidParameterValue)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new WithUItemRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var body = new LogMeasureRuleRequest
        {
            Name = invalidParameterValue,
            Rule = new LogMeasureRuleModelRequest()
            {
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>(),
                },
                Version = LogMeasureRuleVersionEnum.V1
            },
            LogStorageId = 1L,
            MetricsStorageId = 2L
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("Nameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
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
        var body = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 2L,
            Rule = new LogMeasureRuleModelRequest
            {
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>()
                },
                Version = LogMeasureRuleVersionEnum.V1
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

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
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
                @"^ログ計測ルール更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var body = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 2L,
            Rule = new LogMeasureRuleModelRequest
            {
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>()
                },
                Version = LogMeasureRuleVersionEnum.V1
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
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
                @"^ログ計測ルールの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WithUItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 2L,
            Rule = new LogMeasureRuleModelRequest
            {
                Query = new LogMeasureRuleV1
                {
                    Matchers = new List<FieldMatcher>()
                },
                Version = LogMeasureRuleVersionEnum.V1
            }
        };
        var expectedResponse = new LogMeasureRule();

        requestAdapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルールの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルールの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PathParametersのprojectResourceIdが欠落している場合に検証例外が発生することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ThrowsValidationException_WhenProjectResourceIdIsMissing()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "uid", Guid.NewGuid() }
        };
        var builder = new WithUItemRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<global::System.ComponentModel.DataAnnotations.ValidationException>(() => builder.GetAsync());

        Assert.Contains("プロジェクトリソースIDを入力してください。", exception.Message);
    }
}
