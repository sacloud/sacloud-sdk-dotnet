using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadVersion_components_deploy_source_container_registryモデルのテスト
/// </summary>
public class ReadVersion_components_deploy_source_container_registryTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadVersion_components_deploy_source_container_registry_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ReadVersion_components_deploy_source_container_registry();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("image"));
        Assert.True(deserializers.ContainsKey("server"));
        Assert.True(deserializers.ContainsKey("username"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadVersion_components_deploy_source_container_registry_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ReadVersion_components_deploy_source_container_registry();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadVersion_components_deploy_source_container_registry_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ReadVersion_components_deploy_source_container_registry();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("nginx:latest");
        deserializers["image"](parseNode);
        Assert.Equal("nginx:latest", model.Image);
        parseNode.GetStringValue().Returns("registry.example.com");
        deserializers["server"](parseNode);
        Assert.Equal("registry.example.com", model.Server);
        parseNode.GetStringValue().Returns("user123");
        deserializers["username"](parseNode);
        Assert.Equal("user123", model.Username);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadVersion_components_deploy_source_container_registry_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ReadVersion_components_deploy_source_container_registry { Image = "nginx:latest", Server = "registry.example.com", Username = "user123" };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ReadVersion_components_deploy_source_container_registry.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Image, deserializedModel.Image);
        Assert.Equal(originalModel.Server, deserializedModel.Server);
        Assert.Equal(originalModel.Username, deserializedModel.Username);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadVersion_components_deploy_source_container_registry_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ReadVersion_components_deploy_source_container_registry { Image = "nginx:latest", Server = "registry.example.com" };
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
        var deserializedModel = parseNode.GetObjectValue(ReadVersion_components_deploy_source_container_registry.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
