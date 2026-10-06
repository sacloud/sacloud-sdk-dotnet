using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SimpleMQ.Queues.Item;
using Sacloud.Sdk.SimpleMQ.Queues.Item.Messages;

namespace Sacloud.Sdk.SimpleMQ.Tests.Queues.Item;

/// <summary>
/// WithQueueNameItemRequestBuilderのテスト
/// </summary>
public class WithQueueNameItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public WithQueueNameItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
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

        var builder = new WithQueueNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/queues/test-queue";

        var builder = new WithQueueNameItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Messagesプロパティが正しくMessagesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Messages_ReturnsMessagesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" }
        };
        var builder = new WithQueueNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var messagesBuilder = builder.Messages;

        Assert.NotNull(messagesBuilder);
        Assert.IsType<MessagesRequestBuilder>(messagesBuilder);
    }

    /// <summary>
    /// Messagesプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Messages_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "queueName", "test-queue" }
        };
        var builder = new WithQueueNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var messages1 = builder.Messages;
        var messages2 = builder.Messages;

        Assert.NotSame(messages1, messages2);
    }
}
