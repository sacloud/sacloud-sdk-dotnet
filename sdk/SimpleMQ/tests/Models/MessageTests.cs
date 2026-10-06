using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleMQ.Models;

namespace Sacloud.Sdk.SimpleMQ.Tests.Models;

/// <summary>
/// Messageモデルのテスト
/// </summary>
public class MessageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Message_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Message.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Message>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Message_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            Message.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Message_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Message();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("content"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("expires_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updated_at"));
        Assert.True(deserializers.ContainsKey("acquired_at"));
        Assert.True(deserializers.ContainsKey("visibility_timeout_at"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Message_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Message();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Message_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Message();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(1234567890L);
        deserializers["acquired_at"](parseNode);
        Assert.Equal(1234567890L, model.AcquiredAt);

        parseNode.GetLongValue().Returns(1234567999L);
        deserializers["visibility_timeout_at"](parseNode);
        Assert.Equal(1234567999L, model.VisibilityTimeoutAt);

        parseNode.GetStringValue().Returns("test content");
        deserializers["content"](parseNode);
        Assert.Equal("test content", model.Content);

        parseNode.GetStringValue().Returns("msg-id-123");
        deserializers["id"](parseNode);
        Assert.Equal("msg-id-123", model.Id);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Message_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Message
        {
            Content = "test content",
            CreatedAt = 1234567890L,
            ExpiresAt = 1234567999L,
            Id = "msg-id-123",
            UpdatedAt = 1234567895L,
            AcquiredAt = 1234567892L,
            VisibilityTimeoutAt = 1234567998L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Message.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Content, deserializedModel.Content);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.ExpiresAt, deserializedModel.ExpiresAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
        Assert.Equal(originalModel.AcquiredAt, deserializedModel.AcquiredAt);
        Assert.Equal(originalModel.VisibilityTimeoutAt, deserializedModel.VisibilityTimeoutAt);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Message_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Message
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
        var deserializedModel = parseNode.GetObjectValue(Message.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// MessageがNewMessageを継承していることを確認
    /// </summary>
    [Fact]
    public void Message_InheritsFromNewMessage()
    {
        var model = new Message();

        Assert.IsAssignableFrom<NewMessage>(model);
    }
}
