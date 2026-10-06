using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Request;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// PatchApplicationBody_componentsモデルのテスト
/// </summary>
public class PatchApplicationBody_componentsTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PatchApplicationBody_components();
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
    public void PatchApplicationBody_components_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PatchApplicationBody_components();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchApplicationBody_components();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Object型フィールド: deploy_source
        var deploySource = new PatchApplicationBody_components_deploy_source();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PatchApplicationBody_components_deploy_source>>())
            .Returns(deploySource);
        deserializers["deploy_source"](parseNode);
        Assert.NotNull(model.DeploySource);

        // Collection型フィールド: env
        var envList = new List<EnvItem>
        {
            new EnvItem { Key = "ENV_NAME", Value = "env_value" }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<EnvItem>>())
            .Returns(envList);
        deserializers["env"](parseNode);
        Assert.NotNull(model.Env);
        Assert.Single(model.Env);

        // Enum型フィールド: max_cpu
        parseNode.GetEnumValue<PatchApplicationBody_components_max_cpu>()
            .Returns(PatchApplicationBody_components_max_cpu.One);
        deserializers["max_cpu"](parseNode);
        Assert.Equal(PatchApplicationBody_components_max_cpu.One, model.MaxCpu);

        // Enum型フィールド: max_memory
        parseNode.GetEnumValue<PatchApplicationBody_components_max_memory>()
            .Returns(PatchApplicationBody_components_max_memory.OneGi);
        deserializers["max_memory"](parseNode);
        Assert.Equal(PatchApplicationBody_components_max_memory.OneGi, model.MaxMemory);

        // String型フィールド: name
        parseNode.GetStringValue().Returns("component-name");
        deserializers["name"](parseNode);
        Assert.Equal("component-name", model.Name);

        // Object型フィールド: probe
        var probe = new PatchApplicationBody_components_probe();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PatchApplicationBody_components_probe>>())
            .Returns(probe);
        deserializers["probe"](parseNode);
        Assert.NotNull(model.Probe);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchApplicationBody_components_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PatchApplicationBody_components
        {
            DeploySource = new PatchApplicationBody_components_deploy_source
            {
                ContainerRegistry = new PatchApplicationBody_components_deploy_source_container_registry
                {
                    Image = "nginx:latest"
                }
            },
            Env = new List<EnvItem>
            {
                new EnvItem { Key = "ENV_NAME", Value = "env_value" }
            },
            MaxCpu = PatchApplicationBody_components_max_cpu.One,
            MaxMemory = PatchApplicationBody_components_max_memory.OneGi,
            Name = "component-name",
            Probe = new PatchApplicationBody_components_probe
            {
                HttpGet = new PatchApplicationBody_components_probe_http_get
                {
                    Path = "/health",
                    Port = 8080
                }
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
        var deserializedModel = parseNode.GetObjectValue(PatchApplicationBody_components.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.DeploySource);
        Assert.NotNull(deserializedModel.Env);
        Assert.Single(deserializedModel.Env);
        Assert.Equal(originalModel.Env[0].Key, deserializedModel.Env[0].Key);
        Assert.Equal(originalModel.Env[0].Value, deserializedModel.Env[0].Value);
        Assert.Equal(originalModel.MaxCpu, deserializedModel.MaxCpu);
        Assert.Equal(originalModel.MaxMemory, deserializedModel.MaxMemory);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Probe);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchApplicationBody_components_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PatchApplicationBody_components
        {
            Name = "component-name"
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
        var deserializedModel = parseNode.GetObjectValue(PatchApplicationBody_components.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
