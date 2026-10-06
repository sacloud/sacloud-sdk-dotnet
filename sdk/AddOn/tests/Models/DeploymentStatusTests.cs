using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// DeploymentStatusモデルのテスト
/// </summary>
public class DeploymentStatusTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void DeploymentStatus_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.DeploymentStatus();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("properties"));
        Assert.True(deserializers.ContainsKey("type"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DeploymentStatus_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.DeploymentStatus();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void DeploymentStatus_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.DeploymentStatus();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["id"](parseNode);
        Assert.NotNull(model.Id);

        Assert.True(deserializers.ContainsKey("name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["name"](parseNode);
        Assert.NotNull(model.Name);

        Assert.True(deserializers.ContainsKey("properties"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.DeploymentStatusProperties>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.DeploymentStatusProperties>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.DeploymentStatusProperties>());

        deserializers["properties"](parseNode);
        Assert.NotNull(model.Properties);

        Assert.True(deserializers.ContainsKey("type"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["type"](parseNode);
        Assert.NotNull(model.Type);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task DeploymentStatus_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.DeploymentStatus();
        originalModel.Id = "sample";
        originalModel.Name = "sample";
        originalModel.Properties = new Sacloud.Sdk.AddOn.Models.DeploymentStatusProperties();
        originalModel.Type = "sample";

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.DeploymentStatus>(global::Sacloud.Sdk.AddOn.Models.DeploymentStatus.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel!.Id);
        Assert.Equal(originalModel.Name, deserializedModel!.Name);
        Assert.NotNull(deserializedModel!.Properties);
        Assert.Equal(originalModel.Type, deserializedModel!.Type);
    }
}
