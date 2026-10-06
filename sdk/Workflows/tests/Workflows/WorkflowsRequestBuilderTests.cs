using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Workflows.Models;
using Sacloud.Sdk.Workflows.Workflows;
using Sacloud.Sdk.Workflows.Workflows.Item;
using Sacloud.Sdk.Workflows.Workflows.Suggest;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Workflows.Tests.Workflows;

/// <summary>
/// WorkflowsRequestBuilderのテスト
/// </summary>
public class WorkflowsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WorkflowsRequestBuilder> _mockLogger;

    public WorkflowsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WorkflowsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
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

        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/workflows";

        var builder = new WorkflowsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Suggestプロパティが正しいSuggestRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Suggest_ReturnsValidSuggestRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var suggestBuilder = builder.Suggest;

        Assert.NotNull(suggestBuilder);
        Assert.IsType<SuggestRequestBuilder>(suggestBuilder);
    }

    /// <summary>
    /// インデクサーが正しいWorkflowsItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWorkflowsItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["workflow-123"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WorkflowsItemRequestBuilder>(itemBuilder);
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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["workflow-123"];
        var itemBuilder2 = builder["workflow-123"];

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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.TryGetValue("Accept", out var acceptHeader));
        Assert.Contains("application/json", acceptHeader);
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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow();

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.TryGetValue("Accept", out var acceptHeader));
        Assert.Contains("application/json", acceptHeader);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/workflows";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<WorkflowsRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    /// GetAsyncのリクエストパラメータのNameに有効な最大文字数より多い文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidMaxLengthName_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = "12345678901234567890123456789012345678901234567890123456789012345";
            });
        });

        Assert.Equal("Nameは64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameは64文字以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのNameに無効な正規表現パターンの値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("&invalidName")]
    public async Task GetAsync_WithInvalidRegexPatternName_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = invalidParameterValue;
            });
        });

        Assert.Equal("Nameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのNameMatchTypeに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidNameMatchType_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.NameMatchType = (NameMatchType)999;
            });
        });

        Assert.Equal("NameMatchTypeはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("NameMatchTypeはEnumに含まれる値を入力してください。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Page = 0;
                config.QueryParameters.PageLimit = 4;
                config.QueryParameters.SortBy = (SortBy)999;
                config.QueryParameters.Order = (Order)999;
                config.QueryParameters.Name = "";
                config.QueryParameters.NameMatchType = (NameMatchType)999;
            });
        });

        var expectedMessage = """
            Nameの形式が正しくありません。
            NameMatchTypeはEnumに含まれる値を入力してください。
            OrderはEnumに含まれる値を入力してください。
            Pageの値は1以上で入力してください。
            PageLimitの値は5以上500以下で入力してください。
            SortByはEnumに含まれる値を入力してください。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("NameMatchTypeはEnumに含まれる値を入力してください。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

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
                @"^ワークフローの一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

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
                @"^ワークフローの一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListWorkflowResponse();

        requestAdapter.SendAsync<ListWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ListWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    [Theory]
    [InlineData("Logging")]
    [InlineData("Name")]
    [InlineData("Publish")]
    [InlineData("Runbook")]
    [InlineData("Tags.Name")]
    public async Task PostAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        { 
            Name = invalidParameterName == "Name" ? null : "test-workflow",
            Runbook = invalidParameterName == "Runbook" ? null : "test-runbook",
            Publish = invalidParameterName == "Publish" ? null : true,
            Logging = invalidParameterName == "Logging" ? null : true,
            Tags = new List<Tag>
            {
                new Tag
                {
                    Name = invalidParameterName == "Tags.Name" ? null : "test-tag"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", 65, 64)]
    [InlineData("Description", 1025, 1024)]
    [InlineData("Tags.Name", 65, 64)]
    [InlineData("RevisionAlias", 65, 64)]
    public async Task PostAsync_WithInvalidLengthParameter_ThrowsValidationException(string invalidParameterName, int invalidParameterValue, int? validMaxLengthValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        {
            Name = invalidParameterName == "Name" ? new string('a', invalidParameterValue) : "test-name",
            Description = invalidParameterName == "Description" ? new string('a', invalidParameterValue) : "test-description",
            Runbook = invalidParameterName == "Runbook" ? new string('a', invalidParameterValue) : "test-runbook",
            Publish = true,
            Logging = true,
            Tags = new List<Tag>
            {
                new Tag
                {
                    Name = invalidParameterName == "Tags.Name" ? new string('a', invalidParameterValue) : "test-tag"
                }
            },
            RevisionAlias = invalidParameterName == "RevisionAlias" ? new string('a', invalidParameterValue) : "test-revision-alias"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}は{validMaxLengthValue}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLengthValue}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", "invalid+name")]
    [InlineData("Name", "invalid&name")]
    [InlineData("RevisionAlias", "invalid+revision-alias")]
    [InlineData("RevisionAlias", "invalid&revision-alias")]
    [InlineData("ServicePrincipalId", "abcdefghijkl")]
    [InlineData("ServicePrincipalId", "12345678901")]
    [InlineData("ServicePrincipalId", "1234567890123")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterName, string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        {
            Name = invalidParameterName == "Name" ? invalidParameterValue : "test-name",
            Runbook = "test-runbook",
            Publish = true,
            Logging = true,
            RevisionAlias = invalidParameterName == "RevisionAlias" ? invalidParameterValue : "test-revision-alias",
            ServicePrincipalId = invalidParameterName == "ServicePrincipalId" ? invalidParameterValue : "123456789012",
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の形式が正しくありません。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        {
            Name = "12345678901234567890123456789012345678901234567890123456789012345",
            Description = new string('a', 1025),
            Runbook = null,
            Publish = null,
            Logging = null,
            Tags = new List<Tag>
            {
                new Tag
                {
                    Name = "12345678901234567890123456789012345678901234567890123456789012345",
                }
            },
            RevisionAlias = "invalid+revision-alias",
            ServicePrincipalId = "12345678901"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            Descriptionは1024文字以下で入力してください。
            Loggingが入力されていません。
            Nameは64文字以下で入力してください。
            Publishが入力されていません。
            RevisionAliasの形式が正しくありません。
            Runbookが入力されていません。
            ServicePrincipalIdの形式が正しくありません。
            Tags.Nameは64文字以下で入力してください。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("Descriptionは1024文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("Loggingが入力されていません。");
        _mockLogger.Received(1).LogWarning("Nameは64文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("Publishが入力されていません。");
        _mockLogger.Received(1).LogWarning("RevisionAliasの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("Runbookが入力されていません。");
        _mockLogger.Received(1).LogWarning("ServicePrincipalIdの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("Tags.Nameは64文字以下で入力してください。");
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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        {
            Name = "test-workflow",
            Runbook = "test-runbook",
            Publish = true,
            Logging = false
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

        requestAdapter.SendAsync<CreateWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowResponse>>(),
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
                @"^ワークフロー作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        {
            Name = "test-workflow",
            Runbook = "test-runbook",
            Publish = true,
            Logging = false
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowResponse>>(),
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
                @"^ワークフローの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        {
            Name = "test-workflow",
            Runbook = "test-runbook",
            Publish = true,
            Logging = false
        };
        var expectedResponse = new CreateWorkflowResponse();

        requestAdapter.SendAsync<CreateWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<CreateWorkflowResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateWorkflowResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WorkflowsRequestBuilderGetQueryParameters.Nameプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Name_CanBeSet()
    {
        var queryParams = new WorkflowsRequestBuilder.WorkflowsRequestBuilderGetQueryParameters
        {
            Name = "test-workflow"
        };

        Assert.Equal("test-workflow", queryParams.Name);
    }

    /// <summary>
    /// WorkflowsRequestBuilderGetQueryParameters.NameMatchTypeプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_NameMatchType_CanBeSet()
    {
        var queryParams = new WorkflowsRequestBuilder.WorkflowsRequestBuilderGetQueryParameters
        {
            NameMatchType = NameMatchType.Partial
        };

        Assert.Equal(NameMatchType.Partial, queryParams.NameMatchType);
    }

    /// <summary>
    /// WorkflowsRequestBuilderGetQueryParameters.Orderプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Order_CanBeSet()
    {
        var queryParams = new WorkflowsRequestBuilder.WorkflowsRequestBuilderGetQueryParameters
        {
            Order = Order.Asc
        };

        Assert.Equal(Order.Asc, queryParams.Order);
    }

    /// <summary>
    /// WorkflowsRequestBuilderGetQueryParameters.Pageプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanBeSet()
    {
        var queryParams = new WorkflowsRequestBuilder.WorkflowsRequestBuilderGetQueryParameters
        {
            Page = 2
        };

        Assert.Equal(2, queryParams.Page);
    }

    /// <summary>
    /// WorkflowsRequestBuilderGetQueryParameters.PageLimitプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PageLimit_CanBeSet()
    {
        var queryParams = new WorkflowsRequestBuilder.WorkflowsRequestBuilderGetQueryParameters
        {
            PageLimit = 50
        };

        Assert.Equal(50, queryParams.PageLimit);
    }

    /// <summary>
    /// WorkflowsRequestBuilderGetQueryParameters.Publishedプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Published_CanBeSet()
    {
        var queryParams = new WorkflowsRequestBuilder.WorkflowsRequestBuilderGetQueryParameters
        {
            Published = true
        };

        Assert.True(queryParams.Published);
    }

    /// <summary>
    /// WorkflowsRequestBuilderGetQueryParameters.SortByプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_SortBy_CanBeSet()
    {
        var queryParams = new WorkflowsRequestBuilder.WorkflowsRequestBuilderGetQueryParameters
        {
            SortBy = SortBy.CreatedAt
        };

        Assert.Equal(SortBy.CreatedAt, queryParams.SortBy);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのConcurrencyModeがEnumの範囲外の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidConcurrencyMode_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WorkflowsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateWorkflow
        {
            Name = "test-workflow",
            Runbook = "test-runbook",
            Publish = true,
            Logging = true,
            ConcurrencyMode = (CreateWorkflow_ConcurrencyMode)999
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("ConcurrencyModeはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ConcurrencyModeはEnumに含まれる値を入力してください。");
    }
}
