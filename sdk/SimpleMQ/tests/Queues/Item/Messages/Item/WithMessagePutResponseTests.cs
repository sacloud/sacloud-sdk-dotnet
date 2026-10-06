using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleMQ.Models;
using Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.Item;

namespace Sacloud.Sdk.SimpleMQ.Tests.Queues.Item.Messages.Item;

/// <summary>
/// WithMessagePutResponseモデルのテスト
/// </summary>
public class WithMessagePutResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void WithMessagePutResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WithMessagePutResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WithMessagePutResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WithMessagePutResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            WithMessagePutResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WithMessagePutResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WithMessagePutResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("message"));
        Assert.True(deserializers.ContainsKey("result"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WithMessagePutResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WithMessagePutResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WithMessagePutResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WithMessagePutResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("success");
        deserializers["result"](parseNode);
        Assert.Equal("success", model.Result);

        var message = new Message { Id = "msg-1", Content = "test" };
        parseNode.GetObjectValue<Message>(Message.CreateFromDiscriminatorValue).Returns(message);
        deserializers["message"](parseNode);
        Assert.NotNull(model.Message);
        Assert.Equal("msg-1", model.Message.Id);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WithMessagePutResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WithMessagePutResponse
        {
            Result = "success",
            Message = new Message
            {
                Id = "msg-1",
                Content = "test content",
                AcquiredAt = 1234567890L,
                VisibilityTimeoutAt = 1234567999L
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
        var deserializedModel = parseNode.GetObjectValue(WithMessagePutResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Result, deserializedModel.Result);
        Assert.NotNull(deserializedModel.Message);
        Assert.Equal("msg-1", deserializedModel.Message.Id);
        Assert.Equal("test content", deserializedModel.Message.Content);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WithMessagePutResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WithMessagePutResponse
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
        var deserializedModel = parseNode.GetObjectValue(WithMessagePutResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// デフォルトコンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void WithMessagePutResponse_DefaultConstructor_InitializesAdditionalData()
    {
        var model = new WithMessagePutResponse();

        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }
}
