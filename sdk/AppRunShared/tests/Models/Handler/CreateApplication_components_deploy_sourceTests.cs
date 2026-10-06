using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// CreateApplication_components_deploy_sourceモデルのテスト
/// </summary>
public class CreateApplication_components_deploy_sourceTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_deploy_source_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateApplication_components_deploy_source();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("container_registry"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_deploy_source_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateApplication_components_deploy_source();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_deploy_source_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateApplication_components_deploy_source();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var containerRegistry = new CreateApplication_components_deploy_source_container_registry();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateApplication_components_deploy_source_container_registry>>()).Returns(containerRegistry);
        deserializers["container_registry"](parseNode);
        Assert.NotNull(model.ContainerRegistry);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_components_deploy_source_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateApplication_components_deploy_source
        {
            ContainerRegistry = new CreateApplication_components_deploy_source_container_registry { Image = "nginx:latest" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplication_components_deploy_source.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.ContainerRegistry);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_components_deploy_source_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateApplication_components_deploy_source();
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplication_components_deploy_source.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
    }
}
