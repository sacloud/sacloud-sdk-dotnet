using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Workflows.Models;
using Sacloud.Sdk.Workflows.Workflows.Item.Executions.Item.Exec_history;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Workflows.Tests.Workflows.Item.Executions.Item.Exec_history;

/// <summary>
/// Exec_historyRequestBuilderのテスト
/// </summary>
public class Exec_historyRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<Exec_historyRequestBuilder> _mockLogger;

    public Exec_historyRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<Exec_historyRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<Exec_historyRequestBuilder>().Returns(_mockLogger);
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };

        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/workflows/workflow-123/executions/execution-456/exec_history";

        var builder = new Exec_historyRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

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
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/workflows/workflow-123/executions/execution-456/exec_history";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<Exec_historyRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// Exec_historyRequestBuilderGetQueryParameters.Pageプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanBeSet()
    {
        var queryParams = new Exec_historyRequestBuilder.Exec_historyRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        Assert.Equal(1, queryParams.Page);
    }

    /// <summary>
    /// Exec_historyRequestBuilderGetQueryParameters.PageLimitプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PageLimit_CanBeSet()
    {
        var queryParams = new Exec_historyRequestBuilder.Exec_historyRequestBuilderGetQueryParameters
        {
            PageLimit = 50
        };

        Assert.Equal(50, queryParams.PageLimit);
    }

    /// <summary>
    /// Exec_historyRequestBuilderGetQueryParameters.SortOrderプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_SortOrder_CanBeSet()
    {
        var queryParams = new Exec_historyRequestBuilder.Exec_historyRequestBuilderGetQueryParameters
        {
            SortOrder = SortOrder.Asc
        };

        Assert.Equal(SortOrder.Asc, queryParams.SortOrder);
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
            { pathParameterKey, pathParameterValue },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListExecutionHistoryResponse();

        requestAdapter.SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
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
    /// GetAsyncのパスパラメータに無効な文字数のidを入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidLengthId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "1234567890123456789012345678901234567" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListExecutionHistoryResponse();

        requestAdapter.SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
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
    /// GetAsyncのパスパラメータにexecutionIdがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("executionId", "")]
    [InlineData("executionId", " ")]
    [InlineData("executionId", "　")]
    public async Task GetAsync_WithEmptyExecutionId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListExecutionHistoryResponse();

        requestAdapter.SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("実行IDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("実行IDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータに無効な文字数のexecutionIdを入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidLengthExecutionId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "executionId", "1234567890123456789012345678901234567" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListExecutionHistoryResponse();

        requestAdapter.SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("実行IDは36文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("実行IDは36文字以下で入力してください。");
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.SortOrder = (SortOrder)999;
            });
        });

        Assert.Equal("SortOrderはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("SortOrderはEnumに含まれる値を入力してください。");
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Page = 0;
                config.QueryParameters.PageLimit = 4;
                config.QueryParameters.SortOrder = (SortOrder)999;
            });
        });

        var expectedMessage = """
            Pageの値は1以上で入力してください。
            PageLimitの値は5以上500以下で入力してください。
            SortOrderはEnumに含まれる値を入力してください。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message);
        _mockLogger.Received(1).LogWarning("Pageの値は1以上で入力してください。");
        _mockLogger.Received(1).LogWarning("PageLimitの値は5以上500以下で入力してください。");
        _mockLogger.Received(1).LogWarning("SortOrderはEnumに含まれる値を入力してください。");
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
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
                @"^ワークフローの実行履歴取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
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
                @"^ワークフローの実行履歴の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new Exec_historyRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListExecutionHistoryResponse();

        requestAdapter.SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ListExecutionHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListExecutionHistoryResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの実行履歴の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの実行履歴の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
