using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.LogMeasureRules;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.LogMeasureRules.Item;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects.Item.LogMeasureRules;

/// <summary>
/// LogMeasureRulesRequestBuilderのテスト
/// </summary>
public class LogMeasureRulesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<LogMeasureRulesRequestBuilder> _mockLogger;

    public LogMeasureRulesRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<LogMeasureRulesRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<LogMeasureRulesRequestBuilder>().Returns(_mockLogger);
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

        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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

        var builder = new LogMeasureRulesRequestBuilder("https://api.example.com/alerts/projects/1/log-measure-rules", requestAdapter, _mockLoggerFactory);

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
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);
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
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

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
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new LogMeasureRuleRequest());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// WithUrlが新しいLogMeasureRulesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects/1/log-measure-rules");

        Assert.NotNull(newBuilder);
        Assert.IsType<LogMeasureRulesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCountプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Count_CanBeSet()
    {
        var queryParams = new LogMeasureRulesRequestBuilder.LogMeasureRulesRequestBuilderGetQueryParameters { Count = 10 };

        Assert.Equal(10, queryParams.Count);
    }

    /// <summary>
    /// QueryParametersのFromプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_From_CanBeSet()
    {
        var queryParams = new LogMeasureRulesRequestBuilder.LogMeasureRulesRequestBuilderGetQueryParameters { From = 0 };

        Assert.Equal(0, queryParams.From);
    }

    /// <summary>
    /// QueryParametersのLogStorageIdプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_LogStorageId_CanBeSet()
    {
        var queryParams = new LogMeasureRulesRequestBuilder.LogMeasureRulesRequestBuilderGetQueryParameters { LogStorageId = 1L };

        Assert.Equal(1L, queryParams.LogStorageId);
    }

    /// <summary>
    /// GetAsyncがアダプターがnullを返す場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        adapter.SendAsync<PaginatedLogMeasureRuleList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedLogMeasureRuleList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<PaginatedLogMeasureRuleList?>(null));
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// GetAsyncのパスパラメーターが空文字の場合にValidationExceptionをスローすることを確認
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", string.Empty }
        };
        var builder = new LogMeasureRulesRequestBuilder(pathParams, adapter, _mockLoggerFactory);

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
        var builder = new LogMeasureRulesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<PaginatedLogMeasureRuleList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedLogMeasureRuleList>>(),
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
                @"^ログ計測ルール一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new LogMeasureRulesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<PaginatedLogMeasureRuleList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedLogMeasureRuleList>>(),
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
                @"^ログ計測ルール一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new LogMeasureRulesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new PaginatedLogMeasureRuleList();

        requestAdapter.SendAsync<PaginatedLogMeasureRuleList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedLogMeasureRuleList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<PaginatedLogMeasureRuleList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedLogMeasureRuleList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルール一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルール一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        adapter.SendAsync<LogMeasureRule>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<LogMeasureRule>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<LogMeasureRule?>(null));
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

        var result = await builder.PostAsync(new LogMeasureRuleRequest
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
    /// PostAsyncのパスパラメーターが空文字の場合にValidationExceptionをスローすることを確認
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostAsync_WithEmptyPathParameter_ThrowsValidationException()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var pathParams = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", string.Empty }
        };
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
        var builder = new LogMeasureRulesRequestBuilder(pathParams, adapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("プロジェクトリソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("プロジェクトリソースIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータにnull値を入力した場合に例外がスローされることを確認
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

    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータに無効な最大文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Description", 256)]
    [InlineData("Name", 256)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParamaterName, int validMaxLength)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParamaterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータに無効な列挙値を入力した場合に例外がスローされることを確認
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
    public async Task PostAsync_WithInvalidEnumRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータに無効な正規表現の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("InvalidName")]
    [InlineData("invalid name")]
    [InlineData("invalid+name")]
    [InlineData("無効なName")]
    public async Task PostAsync_WithInvalidRegexRequestBodyparamter_ThrowsValidationException(string invalidParameterValue)
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.BaseUrl = "https://api.example.com";
        var builder = new LogMeasureRulesRequestBuilder(PathParams(), adapter, _mockLoggerFactory);

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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Nameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = PathParams();
        var builder = new LogMeasureRulesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ログ計測ルール作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = PathParams();
        var builder = new LogMeasureRulesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ログ計測ルールの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var pathParameters = PathParams();
        var builder = new LogMeasureRulesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        await builder.PostAsync(body);

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
                    @"^ログ計測ルールの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ログ計測ルールの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
