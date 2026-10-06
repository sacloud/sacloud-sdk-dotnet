using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects;
using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Alerts.Projects;

/// <summary>
/// ProjectsRequestBuilderのテスト
/// </summary>
public class ProjectsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ProjectsRequestBuilder> _mockLogger;

    public ProjectsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<ProjectsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<ProjectsRequestBuilder>().Returns(_mockLogger);
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

        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new ProjectsRequestBuilder("https://api.example.com/alerts/projects", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいProjectResourceItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithLong_ReturnsProjectResourceItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[123L];

        Assert.NotNull(itemBuilder);
        Assert.IsType<ProjectResourceItemRequestBuilder>(itemBuilder);
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder[123L];
        var itemBuilder2 = builder[123L];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを正しく処理することを確認
    /// </summary>
    [Theory]
    [InlineData(10, 0)]
    [InlineData(20, 10)]
    [InlineData(null, null)]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly(int? count, int? from)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Count = count;
            config.QueryParameters.From = from;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(
            new JsonSerializationWriterFactory());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectCreateRequest();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectCreateRequest();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// WithUrlが新しいProjectsRequestBuilderインスタンスを返すことを確認
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/alerts/projects");

        Assert.NotNull(newBuilder);
        Assert.IsType<ProjectsRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCountプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Count_CanBeSet()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
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
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            From = 10
        };

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
        adapter.SendAsync<PaginatedAlertProjectList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedAlertProjectList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<PaginatedAlertProjectList?>(null));
        var builder = new ProjectsRequestBuilder(
            new Dictionary<string, object> { { "baseurl", "https://api.example.com" } },
            adapter,
            _mockLoggerFactory);

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "projectResourceId", 1L }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<PaginatedAlertProjectList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedAlertProjectList>>(),
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
                @"^アラートプロジェクト一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<PaginatedAlertProjectList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedAlertProjectList>>(),
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
                @"^アラートプロジェクト一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new PaginatedAlertProjectList();

        requestAdapter.SendAsync<PaginatedAlertProjectList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedAlertProjectList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<PaginatedAlertProjectList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedAlertProjectList>>(),
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
                    @"^アラートプロジェクト一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクト一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        adapter.SendAsync<AlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<AlertProject?>(null));
        var builder = new ProjectsRequestBuilder(
            new Dictionary<string, object> { { "baseurl", "https://api.example.com" } },
            adapter,
            _mockLoggerFactory);

        var result = await builder.PostAsync(new AlertProjectCreateRequest { Name = "test" });

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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectCreateRequest
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectCreateRequest
        {
            Name = invalidParameterName == "Name" ? new string('a', validMaxLength + 1) : "test-name",
            Description = invalidParameterName == "Description" ? new string('a', validMaxLength + 1) : "test-description"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。");
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectCreateRequest
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

        requestAdapter.SendAsync<AlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AlertProject>>(),
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
                @"^アラートプロジェクト作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectCreateRequest
        {
            Name = "test-name"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<AlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AlertProject>>(),
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
                @"^アラートプロジェクトの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new AlertProjectCreateRequest
        {
            Name = "test-name"
        };
        var expectedResponse = new AlertProject();

        requestAdapter.SendAsync<AlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AlertProject>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<AlertProject>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AlertProject>>(),
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
                    @"^アラートプロジェクトの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アラートプロジェクトの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
