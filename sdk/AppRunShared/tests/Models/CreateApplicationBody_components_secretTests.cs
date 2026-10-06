using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// CreateApplicationBody_components_secretモデルのテスト
/// </summary>
public class CreateApplicationBody_components_secretTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateApplicationBody_components_secret_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateApplicationBody_components_secret();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("key"));
        Assert.True(deserializers.ContainsKey("value"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateApplicationBody_components_secret_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateApplicationBody_components_secret();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateApplicationBody_components_secret_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateApplicationBody_components_secret();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // String型フィールド: key
        parseNode.GetStringValue().Returns("MY_SECRET");
        deserializers["key"](parseNode);
        Assert.Equal("MY_SECRET", model.Key);

        // String型フィールド: value
        parseNode.GetStringValue().Returns("secret-value");
        deserializers["value"](parseNode);
        Assert.Equal("secret-value", model.Value);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplicationBody_components_secret_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateApplicationBody_components_secret
        {
            Key = "MY_SECRET",
            Value = "secret-value"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplicationBody_components_secret.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Key, deserializedModel.Key);
        Assert.Equal(originalModel.Value, deserializedModel.Value);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplicationBody_components_secret_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateApplicationBody_components_secret
        {
            Key = "MY_SECRET",
            Value = "secret-value"
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplicationBody_components_secret.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}