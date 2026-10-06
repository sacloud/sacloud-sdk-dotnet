using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Workflows.Models;
using Sacloud.Sdk.Workflows.Workflows.Item.Executions.Item.Cancel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Sacloud.Sdk.Workflows.Tests.Workflows.Item.Executions.Item.Cancel;

/// <summary>
/// CancelRequestBuilderのテスト
/// </summary>
public class CancelRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<CancelRequestBuilder> _mockLogger;

    public CancelRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<CancelRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<CancelRequestBuilder>().Returns(_mockLogger);
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

        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/workflows/workflow-123/executions/execution-456/cancel";

        var builder = new CancelRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
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
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

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
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/workflows/workflow-123/executions/execution-456/cancel";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<CancelRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
            { pathParameterKey, pathParameterValue },
            { "executionId", "execution-456" }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CancelExecutionResponse();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync();
        });

        Assert.Equal("ワークフローIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータに無効な文字数のidを入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidLengthId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "1234567890123456789012345678901234567" },
            { "executionId", "execution-456" }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CancelExecutionResponse();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync();
        });

        Assert.Equal("ワークフローIDは36文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDは36文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータにexecutionIdがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("executionId", "")]
    [InlineData("executionId", " ")]
    [InlineData("executionId", "　")]
    public async Task PostAsync_WithEmptyExecutionId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CancelExecutionResponse();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync();
        });

        Assert.Equal("実行IDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("実行IDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータに無効な文字数のexecutionIdを入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidLengthExecutionId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "executionId", "1234567890123456789012345678901234567" }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CancelExecutionResponse();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync();
        });

        Assert.Equal("実行IDは36文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("実行IDは36文字以下で入力してください。");
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PostAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローの実行キャンセルのAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "id", "workflow-123" },
            { "executionId", "execution-456" }
        };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローの実行のキャンセル中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがステータスコード200で適切なログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithStatusCode200_LogsCancellationCompletion()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
    {
        { "baseurl", "https://api.example.com" },
        { "id", "workflow-123" },
        { "executionId", "execution-456" }
    };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CancelExecutionResponse();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var requestInfo = callInfo.ArgAt<RequestInformation>(0);
                var responseHandlerOption = requestInfo.GetRequestOption<ResponseHandlerOption>();
                
                if (responseHandlerOption?.ResponseHandler is NativeResponseHandler handler)
                {
                    handler.Value = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                }

                return expectedResponse;
            });

        var result = await builder.PostAsync();

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの実行のキャンセルを開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの実行のキャンセルが完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがステータスコード202で適切なログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithStatusCode202_LogsQueueRegistrationCompletion()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
    {
        { "baseurl", "https://api.example.com" },
        { "id", "workflow-123" },
        { "executionId", "execution-456" }
    };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CancelExecutionResponse();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var requestInfo = callInfo.ArgAt<RequestInformation>(0);
                var responseHandlerOption = requestInfo.GetRequestOption<ResponseHandlerOption>();

                if (responseHandlerOption?.ResponseHandler is NativeResponseHandler handler)
                {
                    handler.Value = new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                }

                return expectedResponse;
            });

        var result = await builder.PostAsync();

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの実行のキャンセルを開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローの実行のキャンセルのキュー登録が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 202\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが正常時に200または202以外のステータスコードを受け取ったときに警告ログを出力することを確認
    /// </summary>
    [Theory]
    [InlineData(HttpStatusCode.Created)]
    [InlineData(HttpStatusCode.NotModified)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task PostAsync_WithUnexpectedStatusCode_LogsWarning(HttpStatusCode statusCode)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
    {
        { "baseurl", "https://api.example.com" },
        { "id", "workflow-123" },
        { "executionId", "execution-456" }
    };
        var builder = new CancelRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CancelExecutionResponse();

        requestAdapter.SendAsync<CancelExecutionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CancelExecutionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var requestInfo = callInfo.ArgAt<RequestInformation>(0);
                var responseHandlerOption = requestInfo.GetRequestOption<ResponseHandlerOption>();

                if (responseHandlerOption?.ResponseHandler is NativeResponseHandler handler)
                {
                    handler.Value = new HttpResponseMessage(statusCode);
                }

                return expectedResponse;
            });

        var result = await builder.PostAsync();

        Assert.Equal(expectedResponse, result);

        // 警告ログの検証（予期しないステータスコード）
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    $@"^ワークフローの実行のキャンセルで予期しないステータスコードを受信しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z \[StatusCode, {(int)statusCode}\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
