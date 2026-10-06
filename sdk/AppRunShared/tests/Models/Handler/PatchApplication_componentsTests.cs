using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;
using Sacloud.Sdk.AppRunShared.Models.Response;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// PatchApplication_componentsモデルのテスト
/// </summary>
public class PatchApplication_componentsTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PatchApplication_components_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PatchApplication_components();
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
    public void PatchApplication_components_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PatchApplication_components();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchApplication_components_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchApplication_components();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Object型フィールド: deploy_source
        var deploySource = new PatchApplication_components_deploy_source();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PatchApplication_components_deploy_source>>())
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

        // String型フィールド: max_cpu
        parseNode.GetStringValue().Returns("1");
        deserializers["max_cpu"](parseNode);
        Assert.Equal("1", model.MaxCpu);

        // String型フィールド: max_memory
        parseNode.GetStringValue().Returns("1Gi");
        deserializers["max_memory"](parseNode);
        Assert.Equal("1Gi", model.MaxMemory);

        // String型フィールド: name
        parseNode.GetStringValue().Returns("component-name");
        deserializers["name"](parseNode);
        Assert.Equal("component-name", model.Name);

        // Object型フィールド: probe
        var probe = new PatchApplication_components_probe();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PatchApplication_components_probe>>())
            .Returns(probe);
        deserializers["probe"](parseNode);
        Assert.NotNull(model.Probe);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchApplication_components_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PatchApplication_components
        {
            DeploySource = new PatchApplication_components_deploy_source
            {
                ContainerRegistry = new PatchApplication_components_deploy_source_container_registry
                {
                    Image = "nginx:latest"
                }
            },
            Env = new List<EnvItem>
            {
                new EnvItem { Key = "ENV_NAME", Value = "env_value" }
            },
            MaxCpu = "1",
            MaxMemory = "1Gi",
            Name = "component-name",
            Probe = new PatchApplication_components_probe
            {
                HttpGet = new PatchApplication_components_probe_http_get
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
        var deserializedModel = parseNode.GetObjectValue(PatchApplication_components.CreateFromDiscriminatorValue);

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
    public async Task PatchApplication_components_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PatchApplication_components
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
        var deserializedModel = parseNode.GetObjectValue(PatchApplication_components.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
