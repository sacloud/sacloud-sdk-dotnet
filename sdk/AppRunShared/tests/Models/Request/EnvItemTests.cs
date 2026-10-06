using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Request;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Request;

/// <summary>
/// EnvItemモデルのテスト
/// </summary>
public class EnvItemTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void EnvItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new EnvItem();
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
    public void EnvItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new EnvItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void EnvItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new EnvItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // String型フィールド: key
        parseNode.GetStringValue().Returns("MY_ENV_KEY");
        deserializers["key"](parseNode);
        Assert.Equal("MY_ENV_KEY", model.Key);

        // String型フィールド: value
        parseNode.GetStringValue().Returns("my-env-value");
        deserializers["value"](parseNode);
        Assert.Equal("my-env-value", model.Value);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task EnvItem_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new EnvItem
        {
            Key = "MY_ENV_KEY",
            Value = "my-env-value"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(EnvItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Key, deserializedModel.Key);
        Assert.Equal(originalModel.Value, deserializedModel.Value);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task EnvItem_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new EnvItem
        {
            Key = "MY_ENV_KEY",
            Value = "my-env-value"
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
        var deserializedModel = parseNode.GetObjectValue(EnvItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}