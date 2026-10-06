using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Model;

/// <summary>
/// DestinationSettingsモデルのテスト
/// </summary>
public class DestinationSettingsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = DestinationSettings.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<DestinationSettings>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => DestinationSettings.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new DestinationSettings();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Disabled"));
        Assert.True(deserializers.ContainsKey("Type"));
        Assert.True(deserializers.ContainsKey("Value"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new DestinationSettings();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new DestinationSettings();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["Disabled"](parseNode);
        Assert.True(model.Disabled);

        parseNode.GetEnumValue<DestinationSettings_Type>().Returns(DestinationSettings_Type.Email);
        deserializers["Type"](parseNode);
        Assert.Equal(DestinationSettings_Type.Email, model.Type);

        parseNode.GetStringValue().Returns("test@example.com");
        deserializers["Value"](parseNode);
        Assert.Equal("test@example.com", model.Value);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認（Email）
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties_Email()
    {
        var originalModel = new DestinationSettings
        {
            Type = DestinationSettings_Type.Email,
            Value = "user@example.com",
            Disabled = false,
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(DestinationSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Type, deserializedModel.Type);
        Assert.Equal(originalModel.Value, deserializedModel.Value);
        Assert.Equal(originalModel.Disabled, deserializedModel.Disabled);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認（Webhook）
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties_Webhook()
    {
        var originalModel = new DestinationSettings
        {
            Type = DestinationSettings_Type.Webhook,
            Value = "https://example.com/webhook",
            Disabled = true,
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(DestinationSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Type, deserializedModel.Type);
        Assert.Equal(originalModel.Value, deserializedModel.Value);
        Assert.Equal(originalModel.Disabled, deserializedModel.Disabled);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new DestinationSettings();
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(DestinationSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
