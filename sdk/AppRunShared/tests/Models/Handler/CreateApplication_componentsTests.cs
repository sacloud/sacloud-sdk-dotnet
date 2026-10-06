using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;
using Sacloud.Sdk.AppRunShared.Models.Response;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// CreateApplication_componentsモデルのテスト
/// </summary>
public class CreateApplication_componentsTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateApplication_components();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("deploy_source"));
        Assert.True(deserializers.ContainsKey("env"));
        Assert.True(deserializers.ContainsKey("max_cpu"));
        Assert.True(deserializers.ContainsKey("max_memory"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("probe"));
        Assert.True(deserializers.ContainsKey("secret"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateApplication_components();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateApplication_components();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var deploySource = new CreateApplication_components_deploy_source();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateApplication_components_deploy_source>>()).Returns(deploySource);
        deserializers["deploy_source"](parseNode);
        Assert.NotNull(model.DeploySource);

        var envList = new List<EnvItem> { new EnvItem() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<EnvItem>>()).Returns(envList);
        deserializers["env"](parseNode);
        Assert.NotNull(model.Env);

        parseNode.GetStringValue().Returns("1");
        deserializers["max_cpu"](parseNode);
        Assert.Equal("1", model.MaxCpu);

        parseNode.GetStringValue().Returns("1Gi");
        deserializers["max_memory"](parseNode);
        Assert.Equal("1Gi", model.MaxMemory);

        parseNode.GetStringValue().Returns("component-name");
        deserializers["name"](parseNode);
        Assert.Equal("component-name", model.Name);

        var probe = new CreateApplication_components_probe();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateApplication_components_probe>>()).Returns(probe);
        deserializers["probe"](parseNode);
        Assert.NotNull(model.Probe);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_components_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateApplication_components
        {
            DeploySource = new CreateApplication_components_deploy_source(),
            Env = new List<EnvItem>(),
            MaxCpu = "1",
            MaxMemory = "1Gi",
            Name = "component-name",
            Probe = new CreateApplication_components_probe()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplication_components.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_components_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateApplication_components { Name = "component-name" };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplication_components.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
    }
}
