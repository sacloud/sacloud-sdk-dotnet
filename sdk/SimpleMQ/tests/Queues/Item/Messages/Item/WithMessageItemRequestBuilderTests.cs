using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.SimpleMQ.Models;
using Sacloud.Sdk.SimpleMQ.Queues.Item.Messages;
using Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.Item;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleMQ.Tests.Queues.Item.Messages.Item;

/// <summary>
/// WithMessageItemRequestBuilderのテスト
/// </summary>
public class WithMessageItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithMessageItemRequestBuilder> _mockLogger;

    public WithMessageItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithMessageItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithMessageItemRequestBuilder>().Returns(_mockLogger);
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
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };

        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/queues/test-queue/messages/message-id";

        var builder = new WithMessageItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにqueueNameがないか値が空文字/空白だった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("queueName", "")]
    [InlineData("queueName", " ")]
    [InlineData("queueName", "　")]
    public async Task DeleteAsync_WithEmptyQueueName_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("キュー名を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名を入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにmessageIdがないか値が空文字/空白だった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("messageId", "")]
    [InlineData("messageId", " ")]
    [InlineData("messageId", "　")]
    public async Task DeleteAsync_WithEmptyMessageId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("メッセージIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("メッセージIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータの文字数が最小文字数より少なかった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidMinLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "1234" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("キュー名は5文字以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータの文字数が最大文字数より多かった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidMaxLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "12345678901234567890123456789012345678901234567890123456789012345" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("キュー名は64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は64文字以下で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータの文字数が指定の文字数でない場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" },
            { "messageId", "invalid-message-id" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        // 正規表現パターンチェックでも実質的に文字数チェックを行うため、
        // 期待するエラーメッセージには文字数チェックと正規表現パターンチェックのエラーメッセージが含まれることを想定
        var expectedMessage = """
            メッセージIDは36文字で入力してください。
            メッセージIDの形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("メッセージIDは36文字で入力してください。");
        _mockLogger.Received(1).LogWarning("メッセージIDの形式が正しくありません。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("queueName", "invalid_queue_name", "キュー名")]
    [InlineData("queueName", "-invalid-queue-name", "キュー名")]
    [InlineData("queueName", "無効なキュー名", "キュー名")]
    [InlineData("messageId", "1234567-890ab-cdef-ghij-klmnopqrstuv", "メッセージID")]
    [InlineData("messageId", "1234567890abcdefghijklmnopqrstuvwxyz", "メッセージID")]
    [InlineData("messageId", "無効なメッセージID12345678901234567890123456", "メッセージID")]
    public async Task DeleteAsync_WithInvalidRegexPatternParameter_ThrowsValidationException(string pathParameterKey, string pathParameterValues, string pathParameterDisplayName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", pathParameterKey == "queueName" ? pathParameterValues : "test-queue" },
            { "messageId", pathParameterKey == "messageId" ? pathParameterValues : "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal($"{pathParameterDisplayName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{pathParameterDisplayName}の形式が正しくありません。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータに複数のバリデーションエラーがある場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithMultiplePathParameterValidationErrors_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "-123" },
            { "messageId", "invalid-message-id" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        var expectedMessage = """
            キュー名は5文字以上で入力してください。
            キュー名の形式が正しくありません。
            メッセージIDは36文字で入力してください。
            メッセージIDの形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
        _mockLogger.Received(1).LogWarning("キュー名の形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("メッセージIDは36文字で入力してください。");
        _mockLogger.Received(1).LogWarning("メッセージIDの形式が正しくありません。");
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
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
                @"^読み取り済みのメッセージ削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedException = new Exception();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
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
                @"^読み取り済みのメッセージの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new WithMessageDeleteResponse();

        requestAdapter.SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.DeleteAsync();

        await requestAdapter.Received(1).SendAsync<WithMessageDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessageDeleteResponse>>(),
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
                    @"^読み取り済みのメッセージの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^読み取り済みのメッセージの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncのパスパラメータにqueueNameがないか値が空文字/空白だった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("queueName", "")]
    [InlineData("queueName", " ")]
    [InlineData("queueName", "　")]
    public async Task PutAsync_WithEmptyQueueName_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync();
        });

        Assert.Equal("キュー名を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名を入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータにmessageIdがないか値が空文字/空白だった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("messageId", "")]
    [InlineData("messageId", " ")]
    [InlineData("messageId", "　")]
    public async Task PutAsync_WithEmptyMessageId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync();
        });

        Assert.Equal("メッセージIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("メッセージIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が最小文字数より少なかった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidMinLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "1234" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync();
        });

        Assert.Equal("キュー名は5文字以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が最大文字数より多かった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidMaxLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "12345678901234567890123456789012345678901234567890123456789012345" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync();
        });

        Assert.Equal("キュー名は64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は64文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が指定の文字数でない場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" },
            { "messageId", "invalid-message-id" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync();
        });

        // 正規表現パターンチェックでも実質的に文字数チェックを行うため、
        // 期待するエラーメッセージには文字数チェックと正規表現パターンチェックのエラーメッセージが含まれることを想定
        var expectedMessage = """
            メッセージIDは36文字で入力してください。
            メッセージIDの形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("メッセージIDは36文字で入力してください。");
        _mockLogger.Received(1).LogWarning("メッセージIDの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("queueName", "invalid_queue_name", "キュー名")]
    [InlineData("queueName", "-invalid-queue-name", "キュー名")]
    [InlineData("queueName", "無効なキュー名", "キュー名")]
    [InlineData("messageId", "1234567-890ab-cdef-ghij-klmnopqrstuv", "メッセージID")]
    [InlineData("messageId", "1234567890abcdefghijklmnopqrstuvwxyz", "メッセージID")]
    [InlineData("messageId", "無効なメッセージID12345678901234567890123456", "メッセージID")]
    public async Task PutAsync_WithInvalidRegexPatternParameter_ThrowsValidationException(string pathParameterKey, string pathParameterValues, string pathParameterDisplayName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", pathParameterKey == "queueName" ? pathParameterValues : "test-queue" },
            { "messageId", pathParameterKey == "messageId" ? pathParameterValues : "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync();
        });

        Assert.Equal($"{pathParameterDisplayName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{pathParameterDisplayName}の形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータに複数のバリデーションエラーがある場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithMultiplePathParameterValidationErrors_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "-123" },
            { "messageId", "invalid-message-id" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync();
        });

        var expectedMessage = """
            キュー名は5文字以上で入力してください。
            キュー名の形式が正しくありません。
            メッセージIDは36文字で入力してください。
            メッセージIDの形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
        _mockLogger.Received(1).LogWarning("キュー名の形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("メッセージIDは36文字で入力してください。");
        _mockLogger.Received(1).LogWarning("メッセージIDの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PutAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メッセージのタイムアウト延長のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedException = new Exception();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PutAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^メッセージのタイムアウトの延長中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new WithMessagePutResponse();

        requestAdapter.SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PutAsync();

        await requestAdapter.Received(1).SendAsync<WithMessagePutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WithMessagePutResponse>>(),
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
                    @"^メッセージのタイムアウトの延長を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^メッセージのタイムアウトの延長が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" },
            { "messageId", "0193b878-1b25-7775-87f5-9c698206a7e7" }
        };
        var builder = new WithMessageItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/queues/test-queue/messages/new-message-id";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WithMessageItemRequestBuilder>(newBuilder);
    }
}
