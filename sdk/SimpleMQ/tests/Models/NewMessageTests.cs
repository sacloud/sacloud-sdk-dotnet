using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleMQ.Models;

namespace Sacloud.Sdk.SimpleMQ.Tests.Models;

/// <summary>
/// NewMessageモデルのテスト
/// </summary>
public class NewMessageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void NewMessage_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = NewMessage.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<NewMessage>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NewMessage_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            NewMessage.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void NewMessage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new NewMessage();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("content"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("expires_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updated_at"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NewMessage_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new NewMessage();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NewMessage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NewMessage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test content");
        deserializers["content"](parseNode);
        Assert.Equal("test content", model.Content);

        parseNode.GetLongValue().Returns(1234567890L);
        deserializers["created_at"](parseNode);
        Assert.Equal(1234567890L, model.CreatedAt);

        parseNode.GetLongValue().Returns(1234567999L);
        deserializers["expires_at"](parseNode);
        Assert.Equal(1234567999L, model.ExpiresAt);

        parseNode.GetStringValue().Returns("msg-id-123");
        deserializers["id"](parseNode);
        Assert.Equal("msg-id-123", model.Id);

        parseNode.GetLongValue().Returns(1234567895L);
        deserializers["updated_at"](parseNode);
        Assert.Equal(1234567895L, model.UpdatedAt);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NewMessage_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new NewMessage
        {
            Content = "test content",
            CreatedAt = 1234567890L,
            ExpiresAt = 1234567999L,
            Id = "msg-id-123",
            UpdatedAt = 1234567895L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(NewMessage.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Content, deserializedModel.Content);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.ExpiresAt, deserializedModel.ExpiresAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NewMessage_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new NewMessage
        {
            Content = "test content",
            Id = "msg-id-123"
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
        var deserializedModel = parseNode.GetObjectValue(NewMessage.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
