using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Workflows.Models;
using Sacloud.Sdk.Workflows.Workflows.Item.Revisions;
using Sacloud.Sdk.Workflows.Workflows.Item.Revisions.Item;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Workflows.Tests.Workflows.Item.Revisions;

/// <summary>
/// RevisionsRequestBuilderのテスト
/// </summary>
public class RevisionsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<RevisionsRequestBuilder> _mockLogger;

    public RevisionsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<RevisionsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<RevisionsRequestBuilder>().Returns(_mockLogger);
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
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };

        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/workflows/workflow-123/revisions";

        var builder = new RevisionsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithRevisionItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithRevisionItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[1];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithRevisionItemRequestBuilder>(itemBuilder);
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
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
    }

    /// <summary>
    /// WithUrlが正しい新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/workflows/workflow-123/revisions";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<RevisionsRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// RevisionsRequestBuilderGetQueryParameters.Orderプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Order_CanBeSet()
    {
        var queryParams = new RevisionsRequestBuilder.RevisionsRequestBuilderGetQueryParameters
        {
            Order = Order.Desc
        };

        Assert.Equal(Order.Desc, queryParams.Order);
    }

    /// <summary>
    /// RevisionsRequestBuilderGetQueryParameters.Pageプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanBeSet()
    {
        var queryParams = new RevisionsRequestBuilder.RevisionsRequestBuilderGetQueryParameters
        {
            Page = 2
        };

        Assert.Equal(2, queryParams.Page);
    }

    /// <summary>
    /// RevisionsRequestBuilderGetQueryParameters.PageLimitプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PageLimit_CanBeSet()
    {
        var queryParams = new RevisionsRequestBuilder.RevisionsRequestBuilderGetQueryParameters
        {
            PageLimit = 100
        };

        Assert.Equal(100, queryParams.PageLimit);
    }

    /// <summary>
    /// RevisionsRequestBuilderGetQueryParameters.Publishedプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Published_CanBeSet()
    {
        var queryParams = new RevisionsRequestBuilder.RevisionsRequestBuilderGetQueryParameters
        {
            Published = true
        };

        Assert.True(queryParams.Published);
    }

    /// <summary>
    /// RevisionsRequestBuilderGetQueryParameters.SortByプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_SortBy_CanBeSet()
    {
        var queryParams = new RevisionsRequestBuilder.RevisionsRequestBuilderGetQueryParameters
        {
            SortBy = SortBy.UpdatedAt
        };

        Assert.Equal(SortBy.UpdatedAt, queryParams.SortBy);
    }

    /// <summary>
    /// GetAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task GetAsync_WithEmptyId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListWorkflowRevisionsResponse();

        requestAdapter.SendAsync<ListWorkflowRevisionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowRevisionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("ワークフローIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータに無効な文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidLengthId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "1234567890123456789012345678901234567" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListWorkflowRevisionsResponse();

        requestAdapter.SendAsync<ListWorkflowRevisionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowRevisionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("ワークフローIDは36文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDは36文字以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのPageに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetAsync_WithInvalidPage_ThrowsValidationException(int? invalidPage)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Page = invalidPage;
            });
        });

        Assert.Equal($"Pageの値は1以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Pageの値は1以上で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのPageLimitに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    [InlineData(501)]
    public async Task GetAsync_WithInvalidPageLimit_ThrowsValidationException(int? invalidPageLimit)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.PageLimit = invalidPageLimit;
            });
        });

        Assert.Equal("PageLimitの値は5以上500以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("PageLimitの値は5以上500以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのSortByに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidSortBy_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.SortBy = (SortBy)999;
            });
        });

        Assert.Equal("SortByはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("SortByはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのOrderに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidOrder_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Order = (Order)999;
            });
        });

        Assert.Equal("OrderはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("OrderはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// GetAsyncの複数のリクエストパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithMultipleInvalidParams_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Page = 0;
                config.QueryParameters.PageLimit = 4;
                config.QueryParameters.SortBy = (SortBy)999;
                config.QueryParameters.Order = (Order)999;
            });
        });

        var expectedMessage = """
            OrderはEnumに含まれる値を入力してください。
            Pageの値は1以上で入力してください。
            PageLimitの値は5以上500以下で入力してください。
            SortByはEnumに含まれる値を入力してください。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message);
        _mockLogger.Received(1).LogWarning("OrderはEnumに含まれる値を入力してください。");
        _mockLogger.Received(1).LogWarning("Pageの値は1以上で入力してください。");
        _mockLogger.Received(1).LogWarning("PageLimitの値は5以上500以下で入力してください。");
        _mockLogger.Received(1).LogWarning("SortByはEnumに含まれる値を入力してください。");
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
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListWorkflowRevisionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowRevisionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョンの一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListWorkflowRevisionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowRevisionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョンの一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListWorkflowRevisionsResponse();

        requestAdapter.SendAsync<ListWorkflowRevisionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowRevisionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ListWorkflowRevisionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowRevisionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンの一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンの一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task PostAsync_WithEmptyId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = "test-runbook"
        };
        var expectedResponse = new CreateWorkflowRevisionResponse();

        requestAdapter.SendAsync<CreateWorkflowRevisionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowRevisionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("ワークフローIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータに無効な文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidLengthId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "1234567890123456789012345678901234567" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = "test-runbook"
        };
        var expectedResponse = new CreateWorkflowRevisionResponse();

        requestAdapter.SendAsync<CreateWorkflowRevisionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowRevisionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("ワークフローIDは36文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDは36文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("Runbookが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Runbookが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = "test-runbook",
            RevisionAlias = new string('a', 65)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("RevisionAliasは64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("RevisionAliasは64文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid+revision-alias")]
    [InlineData("invalid&revision-alias")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = "test-runbook",
            RevisionAlias = invalidParameterValue
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("RevisionAliasの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("RevisionAliasの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMultipleInvalidRequestBodyParameters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = null,
            RevisionAlias = "invalid+revision-alias"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            RevisionAliasの形式が正しくありません。
            Runbookが入力されていません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("RevisionAliasの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("Runbookが入力されていません。");
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
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = "test-runbook"
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

        requestAdapter.SendAsync<CreateWorkflowRevisionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowRevisionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョン追加のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = "test-runbook"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateWorkflowRevisionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowRevisionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョンの追加中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "id", "workflow-123" }
        };
        var builder = new RevisionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflowRevision
        {
            Runbook = "test-runbook"
        };
        var expectedResponse = new CreateWorkflowRevisionResponse();

        requestAdapter.SendAsync<CreateWorkflowRevisionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowRevisionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<CreateWorkflowRevisionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowRevisionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンの追加を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンの追加が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
