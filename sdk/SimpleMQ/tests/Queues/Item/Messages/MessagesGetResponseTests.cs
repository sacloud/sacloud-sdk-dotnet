using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleMQ.Models;
using Sacloud.Sdk.SimpleMQ.Queues.Item.Messages;

namespace Sacloud.Sdk.SimpleMQ.Tests.Queues.Item.Messages;

/// <summary>
/// MessagesGetResponseモデルのテスト
/// </summary>
public class MessagesGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void MessagesGetResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = MessagesGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<MessagesGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void MessagesGetResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            MessagesGetResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void MessagesGetResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new MessagesGetResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("messages"));
        Assert.True(deserializers.ContainsKey("result"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void MessagesGetResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new MessagesGetResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MessagesGetResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MessagesGetResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("success");
        deserializers["result"](parseNode);
        Assert.Equal("success", model.Result);

        var messageList = new List<Message> { new Message { Id = "msg-1" } };
        parseNode.GetCollectionOfObjectValues<Message>(Message.CreateFromDiscriminatorValue).Returns(messageList);
        deserializers["messages"](parseNode);
        Assert.NotNull(model.Messages);
        Assert.Single(model.Messages);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MessagesGetResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new MessagesGetResponse
        {
            Result = "success",
            Messages = new List<Message>
            {
                new Message
                {
                    Id = "msg-1",
                    Content = "test content 1",
                    AcquiredAt = 1234567890L,
                    VisibilityTimeoutAt = 1234567999L
                }
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(MessagesGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Result, deserializedModel.Result);
        Assert.NotNull(deserializedModel.Messages);
        Assert.Single(deserializedModel.Messages);
        Assert.Equal("msg-1", deserializedModel.Messages[0].Id);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MessagesGetResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new MessagesGetResponse
        {
            Result = "success"
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(MessagesGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// デフォルトコンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void MessagesGetResponse_DefaultConstructor_InitializesAdditionalData()
    {
        var model = new MessagesGetResponse();

        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }
}
