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

namespace Sacloud.Sdk.SimpleMQ.Tests.Queues.Item.Messages;

/// <summary>
/// MessagesRequestBuilderのテスト
/// </summary>
public class MessagesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<MessagesRequestBuilder> _mockLogger;

    public MessagesRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<MessagesRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<MessagesRequestBuilder>().Returns(_mockLogger);
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
            { "queueName", "test-queue" }
        };

        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/queues/test-queue/messages";

        var builder = new MessagesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithMessageItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithMessageItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["message-id"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithMessageItemRequestBuilder>(itemBuilder);
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
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["message-id"];
        var itemBuilder2 = builder["message-id"];

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
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
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
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "test message" };

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
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "test message" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// GetAsyncのパスパラメータにqueueNameがないか値が空文字/空白だった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("queueName", "")]
    [InlineData("queueName", " ")]
    [InlineData("queueName", "　")]
    public async Task GetAsync_WithNullParameter_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new MessagesGetResponse();

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("キュー名を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名を入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータの文字数が最小文字数より少なかった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidMinLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "1234" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new MessagesGetResponse();

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("キュー名は5文字以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータの文字数が最大文字数より多かった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidMaxLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "12345678901234567890123456789012345678901234567890123456789012345" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new MessagesGetResponse();

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("キュー名は64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は64文字以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid_queue_name")]
    [InlineData("-invalid-queue-name")]
    [InlineData("無効なキュー名")]
    public async Task GetAsync_WithInvalidRegexPatternParameter_ThrowsValidationException(string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", pathParameterValue }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new MessagesGetResponse();

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("キュー名の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名の形式が正しくありません。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータに複数のバリデーションエラーがある場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithMultiplePathParameterValidationErrors_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "-123" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new MessagesGetResponse();

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        var expectedMessage = """
            キュー名は5文字以上で入力してください。
            キュー名の形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
        _mockLogger.Received(1).LogWarning("キュー名の形式が正しくありません。");
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
            { "queueName", "test-queue" },
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
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
                @"^キューに対するメッセージのreceive \(dequeue\)のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "queueName", "test-queue" },
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedException = new Exception();

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
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
                @"^キューに対するメッセージのreceive \(dequeue\)中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "queueName", "test-queue" },
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new MessagesGetResponse();

        requestAdapter.SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<MessagesGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesGetResponse>>(),
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
                    @"^キューに対するメッセージのreceive \(dequeue\)を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^キューに対するメッセージのreceive \(dequeue\)が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータにqueueNameがないか値が空文字/空白だった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("queueName", "")]
    [InlineData("queueName", " ")]
    [InlineData("queueName", "　")]
    public async Task PostAsync_WithNullParameter_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "testMessage" };

        var expectedResponse = new MessagesPostResponse();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("キュー名を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名を入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータの文字数が最小文字数より少なかった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidMinLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "1234" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "testMessage" };

        var expectedResponse = new MessagesPostResponse();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("キュー名は5文字以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータの文字数が最大文字数より多かった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidMaxLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "12345678901234567890123456789012345678901234567890123456789012345" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "testMessage" };

        var expectedResponse = new MessagesPostResponse();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("キュー名は64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は64文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid_queue_name")]
    [InlineData("-invalid-queue-name")]
    [InlineData("無効なキュー名")]
    public async Task PostAsync_WithInvalidRegexPatternParameter_ThrowsValidationException(string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", pathParameterValue }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "testMessage" };

        var expectedResponse = new MessagesPostResponse();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("キュー名の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータに複数のバリデーションエラーがある場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMultiplePathParameterValidationErrors_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "-123" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "testMessage" };

        var expectedResponse = new MessagesPostResponse();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            キュー名は5文字以上で入力してください。
            キュー名の形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("キュー名は5文字以上で入力してください。");
        _mockLogger.Received(1).LogWarning("キュー名の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullRequestBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new MessagesPostResponse();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータがnull、空文字または空白の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string? invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest
        {
            Content = invalidParameterValue
        };
        var expectedResponse = new MessagesPostResponse();
        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Contentが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Contentが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な最大文字数を超えた場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidLengthRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest
        {
            Content = new string('a', 256001)
        };
        var expectedResponse = new MessagesPostResponse();
        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Contentは256000文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Contentは256000文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な正規表現パターンに一致しない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-content")]
    [InlineData("test_content")]
    [InlineData("無効なContent")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest
        {
            Content = invalidParameterValue
        };
        var expectedResponse = new MessagesPostResponse();
        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Contentの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Contentの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータに複数のバリデーションエラーがある場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMultipleRequestBodyParameterValidationErrors_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue-name" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest
        {
            Content = new string('-', 256001)
        };
        var expectedResponse = new MessagesPostResponse();
        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            Contentは256000文字以下で入力してください。
            Contentの形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("Contentは256000文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("Contentの形式が正しくありません。");
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
            { "queueName", "test-queue" },
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var body = new SendRequest
        {
            Content = "testQueueMessage"
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

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
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
                @"^キューに対するメッセージのsend \(enqueue\)のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "queueName", "test-queue" },
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        
        var body = new SendRequest
        {
            Content = "testQueueMessage"
        };

        var expectedException = new Exception();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
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
                @"^キューに対するメッセージのsend \(enqueue\)中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "queueName", "test-queue" },
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendRequest { Content = "testQueueMessage" };
        
        var expectedResponse = new MessagesPostResponse();

        requestAdapter.SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<MessagesPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MessagesPostResponse>>(),
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
                    @"^キューに対するメッセージのsend \(enqueue\)を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^キューに対するメッセージのsend \(enqueue\)が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
            { "queueName", "test-queue" }
        };
        var builder = new MessagesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/queues/test-queue/messages";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<MessagesRequestBuilder>(newBuilder);
    }
}
