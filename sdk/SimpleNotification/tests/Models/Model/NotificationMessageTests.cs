using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Model;

/// <summary>
/// NotificationMessageモデルのテスト
/// </summary>
public class NotificationMessageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = NotificationMessage.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<NotificationMessage>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => NotificationMessage.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new NotificationMessage();
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("body"));
        Assert.True(deserializers.ContainsKey("color"));
        Assert.True(deserializers.ContainsKey("color_code"));
        Assert.True(deserializers.ContainsKey("icon_url"));
        Assert.True(deserializers.ContainsKey("image_url"));
        Assert.True(deserializers.ContainsKey("title"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new NotificationMessage();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NotificationMessage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-body");
        deserializers["body"](parseNode);
        Assert.Equal("test-body", model.Body);

        parseNode.GetStringValue().Returns("test-title");
        deserializers["title"](parseNode);
        Assert.Equal("test-title", model.Title);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new NotificationMessage
        {
            Body = "test body",
            Title = "test title",
            Color = "red",
            ColorCode = "#FF0000",
            IconUrl = "https://example.com/icon.png",
            ImageUrl = "https://example.com/image.png",
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(NotificationMessage.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(originalModel.Body, deserialized.Body);
        Assert.Equal(originalModel.Title, deserialized.Title);
        Assert.Equal(originalModel.Color, deserialized.Color);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new NotificationMessage();
        originalModel.AdditionalData.Add("custom", "value");

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(NotificationMessage.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.True(deserialized.AdditionalData.ContainsKey("custom"));
    }
}
