using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// CreateApplicationBody_components_deploy_sourceモデルのテスト
/// </summary>
public class CreateApplicationBody_components_deploy_sourceTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateApplicationBody_components_deploy_source_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateApplicationBody_components_deploy_source();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("container_registry"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateApplicationBody_components_deploy_source_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateApplicationBody_components_deploy_source();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateApplicationBody_components_deploy_source_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateApplicationBody_components_deploy_source();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Object型フィールド: container_registry
        var containerRegistry = new CreateApplicationBody_components_deploy_source_container_registry();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateApplicationBody_components_deploy_source_container_registry>>())
            .Returns(containerRegistry);
        deserializers["container_registry"](parseNode);
        Assert.NotNull(model.ContainerRegistry);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplicationBody_components_deploy_source_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateApplicationBody_components_deploy_source
        {
            ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry
            {
                Image = "nginx:latest",
                Server = "registry.example.com"
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplicationBody_components_deploy_source.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.ContainerRegistry);
        Assert.Equal(originalModel.ContainerRegistry.Image, deserializedModel.ContainerRegistry.Image);
        Assert.Equal(originalModel.ContainerRegistry.Server, deserializedModel.ContainerRegistry.Server);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplicationBody_components_deploy_source_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateApplicationBody_components_deploy_source
        {
            ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry
            {
                Image = "nginx:latest"
            }
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
        var deserializedModel = parseNode.GetObjectValue(CreateApplicationBody_components_deploy_source.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
