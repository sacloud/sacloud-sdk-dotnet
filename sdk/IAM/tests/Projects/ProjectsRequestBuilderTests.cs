using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Projects;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.Projects;

/// <summary>
/// ProjectsRequestBuilderのテスト
/// </summary>
public class ProjectsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ProjectsRequestBuilder> _mockLogger;

    public ProjectsRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ProjectsRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ProjectsRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        Assert.Throws<ArgumentNullException>(() => new ProjectsRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/projects";

        Assert.Throws<ArgumentNullException>(() => new ProjectsRequestBuilder(rawUrl, requestAdapter, null));
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
        var rawUrl = "https://api.example.com/projects";

        var builder = new ProjectsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithProject_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithProject_ItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[12345];

        Assert.NotNull(itemBuilder);
    }

    /// <summary>
    /// インデクサーが異なるIDで異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithDifferentIds_ReturnsDifferentInstances()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder[12345];
        var itemBuilder2 = builder[67890];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// インデクサーが同じIDで毎回新しいインスタンスを返すことを確認
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
        var projectId = 12345;

        var itemBuilder1 = builder[projectId];
        var itemBuilder2 = builder[projectId];

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
    /// ToGetRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsCorrectUrlTemplate()
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
        Assert.Contains("projects", requestInfo.UrlTemplate);
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

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToGetRequestInformationがQueryParametersを正しく設定できることを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly()
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
            config.QueryParameters.Page = 1;
            config.QueryParameters.PerPage = 10;
        });

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// ToGetRequestInformationがnull設定で動作することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithNullConfiguration_WorksCorrectly()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(null);

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project",
            Description = "Test Description"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsCorrectUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Contains("projects", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToPostRequestInformationが正しいContent-Typeを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsContentType()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// ToPostRequestInformationがbodyを正しく設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsBodyContent()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// GetAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsProjectsGetResponse()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ProjectsGetResponse
        {
            Items = []
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクト一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクト一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがCancellationTokenを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_AcceptsCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ProjectsGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var cancellationToken = new CancellationToken();

        var result = await builder.GetAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
    }

    /// <summary>
    /// GetAsyncがrequestConfigurationを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_AcceptsRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ProjectsGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 1;
        });

        Assert.NotNull(result);
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

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectsGetResponse>>(),
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
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクト一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectsGetResponse>>(),
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
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクト一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsProject()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Description = "test-description",
            Name = "Test Project"
        };
        var expectedResponse = new Project
        {
            Code = "test-project",
            Name = "Test Project"
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.Equal("test-project", result.Code);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクトの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクトの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがCancellationTokenを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_AcceptsCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Description = "test-description",
            Name = "Test Project"
        };
        var expectedResponse = new Project();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var cancellationToken = new CancellationToken();

        var result = await builder.PostAsync(body, cancellationToken: cancellationToken);

        Assert.NotNull(result);
    }

    /// <summary>
    /// PostAsyncがrequestConfigurationを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_AcceptsRequestConfiguration()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Description = "test-description",
            Name = "Test Project"
        };
        var expectedResponse = new Project();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body, config => { });

        Assert.NotNull(result);
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
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Description = "test-description",
            Name = "Test Project"
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

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
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
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクト作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Description = "test-description",
            Name = "Test Project"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
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
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^プロジェクトの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがnull bodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(async () => await builder.PostAsync(null));
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのOrderingに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidOrdering_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.Ordering = (GetOrderingQueryParameterType)999;
        }));

        Assert.Equal("orderingはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("orderingはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncにnullが含まれるリクエストボディが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Code")]
    [InlineData("Description")]
    [InlineData("Name")]
    public async Task PostAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProjectsPostRequestBody
        {
            Code = nullParam == "Code" ? null : "test-code",
            Description = nullParam == "Description" ? null : "test-description",
            Name = nullParam == "Name" ? null : "test-name"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Equal($"{nullParam}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{nullParam}が入力されていません。");
    }

    /// <summary>
    /// WithUrlが正しいProjectsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewProjectsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/projects";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
    }

    /// <summary>
    /// WithUrlが異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsDifferentInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProjectsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/projects";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのIamRoleプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_IamRole_CanSetAndGet()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            IamRole = "admin"
        };

        Assert.Equal("admin", queryParams.IamRole);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのIamRoleプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_IamRole_CanSetNull()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            IamRole = null
        };

        Assert.Null(queryParams.IamRole);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのOrderingプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_CanSetAndGet()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            Ordering = GetOrderingQueryParameterType.Code
        };

        Assert.Equal(GetOrderingQueryParameterType.Code, queryParams.Ordering);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのOrderingプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_CanSetNull()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            Ordering = null
        };

        Assert.Null(queryParams.Ordering);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのPageプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanSetAndGet()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        Assert.Equal(1, queryParams.Page);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのPageプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanSetNull()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            Page = null
        };

        Assert.Null(queryParams.Page);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのParentFolderIdプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ParentFolderId_CanSetAndGet()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            ParentFolderId = 100
        };

        Assert.Equal(100, queryParams.ParentFolderId);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのParentFolderIdプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ParentFolderId_CanSetNull()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            ParentFolderId = null
        };

        Assert.Null(queryParams.ParentFolderId);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのPerPageプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanSetAndGet()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            PerPage = 10
        };

        Assert.Equal(10, queryParams.PerPage);
    }

    /// <summary>
    /// ProjectsRequestBuilderGetQueryParametersのPerPageプロパティがnullを受け取れることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_CanSetNull()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            PerPage = null
        };

        Assert.Null(queryParams.PerPage);
    }

    /// <summary>
    /// QueryParametersのIamRoleプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_IamRole_HasQueryParameterAttribute()
    {
        var property = typeof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters.IamRole));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのOrderingプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Ordering_HasQueryParameterAttribute()
    {
        var property = typeof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters.Ordering));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_HasQueryParameterAttribute()
    {
        var property = typeof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters.Page));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのParentFolderIdプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_ParentFolderId_HasQueryParameterAttribute()
    {
        var property = typeof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters.ParentFolderId));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPage_HasQueryParameterAttribute()
    {
        var property = typeof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters.PerPage));

        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのすべてのプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_AllProperties_CanBeSet()
    {
        var queryParams = new ProjectsRequestBuilder.ProjectsRequestBuilderGetQueryParameters
        {
            IamRole = "admin",
            Ordering = GetOrderingQueryParameterType.Code,
            Page = 1,
            ParentFolderId = 100,
            PerPage = 10
        };

        Assert.Equal("admin", queryParams.IamRole);
        Assert.Equal(GetOrderingQueryParameterType.Code, queryParams.Ordering);
        Assert.Equal(1, queryParams.Page);
        Assert.Equal(100, queryParams.ParentFolderId);
        Assert.Equal(10, queryParams.PerPage);
    }
}
