using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Item.Simplenotification.Message;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Item.Simplenotification.Message;

/// <summary>
/// MessageRequestBuilderのテスト
/// </summary>
public class MessageRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<MessageRequestBuilder> _mockLogger;

    public MessageRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<MessageRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<MessageRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };

        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new MessageRequestBuilder("https://api.example.com/commonserviceitem/123456789012/simplenotification/message", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ReturnsCorrectRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);
        var body = new SendNotificationMessageRequest
        {
            Message = "test message"
        };

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
    }

    /// <summary>
    /// PostAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);
        var body = new SendNotificationMessageRequest
        {
            Message = "test message"
        };
        var expectedResponse = new SendNotificationMessageResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SendNotificationMessageResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SendNotificationMessageResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// PostAsyncのリクエストボディの必須パラメータがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Message")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendNotificationMessageRequest();

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncがnullボディでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null!));
    }

    /// <summary>
    /// PostAsyncが不正なid形式でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);
        var body = new SendNotificationMessageRequest
        {
            Message = "test message"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Contains("通知先グループID", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが最大文字数を超過した場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Message", 2048)]
    public async Task PostAsync_WithMaxLengthExceededRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int maxLength)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new SendNotificationMessageRequest
        {
            Message = new string('a', maxLength + 1)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}は{maxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{maxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncでApiErrorがスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);
        var body = new SendNotificationMessageRequest
        {
            Message = "test message"
        };
        var expectedException = new ApiError();

        requestAdapter.SendAsync<SendNotificationMessageResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SendNotificationMessageResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SendNotificationMessageResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));
    }

    /// <summary>
    /// PostAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);
        var body = new SendNotificationMessageRequest
        {
            Message = "test message"
        };
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<SendNotificationMessageResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SendNotificationMessageResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SendNotificationMessageResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync(body));
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }

    private static IRequestAdapter CreateRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        return requestAdapter;
    }

    /// <summary>
    /// WithUrlが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new MessageRequestBuilder(pathParameters, requestAdapter);

        var result = builder.WithUrl("https://api.example.com/commonserviceitem/123456789012/simplenotification/message");

        Assert.NotNull(result);
        Assert.IsType<MessageRequestBuilder>(result);
    }
}
