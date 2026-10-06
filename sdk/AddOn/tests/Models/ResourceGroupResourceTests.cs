using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// ResourceGroupResourceモデルのテスト
/// </summary>
public class ResourceGroupResourceTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ResourceGroupResource_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupResource();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("data"));
        Assert.True(deserializers.ContainsKey("hasData"));
        Assert.True(deserializers.ContainsKey("id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ResourceGroupResource_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupResource();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ResourceGroupResource_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupResource();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.ResourceGroupData>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.ResourceGroupData>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.ResourceGroupData>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);

        Assert.True(deserializers.ContainsKey("hasData"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["hasData"](parseNode);
        Assert.NotNull(model.HasData);

        Assert.True(deserializers.ContainsKey("id"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>());

        deserializers["id"](parseNode);
        Assert.NotNull(model.Id);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ResourceGroupResource_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupResource();
        originalModel.Data = new Sacloud.Sdk.AddOn.Models.ResourceGroupData();
        originalModel.Id = new Sacloud.Sdk.AddOn.Models.ResourceIdentifier();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.ResourceGroupResource>(global::Sacloud.Sdk.AddOn.Models.ResourceGroupResource.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.Data);
        Assert.NotNull(deserializedModel!.Id);
    }
}
