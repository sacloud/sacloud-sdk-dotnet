using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// PatchApplicationBody_components_probe_http_get_headersモデルのテスト
/// </summary>
public class PatchApplicationBody_components_probe_http_get_headersTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_probe_http_get_headers_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PatchApplicationBody_components_probe_http_get_headers();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("value"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_probe_http_get_headers_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PatchApplicationBody_components_probe_http_get_headers();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_probe_http_get_headers_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchApplicationBody_components_probe_http_get_headers();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // String型フィールド: name
        parseNode.GetStringValue().Returns("Content-Type");
        deserializers["name"](parseNode);
        Assert.Equal("Content-Type", model.Name);

        // String型フィールド: value
        parseNode.GetStringValue().Returns("application/json");
        deserializers["value"](parseNode);
        Assert.Equal("application/json", model.Value);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchApplicationBody_components_probe_http_get_headers_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PatchApplicationBody_components_probe_http_get_headers
        {
            Name = "Content-Type",
            Value = "application/json"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PatchApplicationBody_components_probe_http_get_headers.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Value, deserializedModel.Value);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchApplicationBody_components_probe_http_get_headers_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PatchApplicationBody_components_probe_http_get_headers
        {
            Name = "Content-Type",
            Value = "application/json"
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
        var deserializedModel = parseNode.GetObjectValue(PatchApplicationBody_components_probe_http_get_headers.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
