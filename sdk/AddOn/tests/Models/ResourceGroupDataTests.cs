using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// ResourceGroupDataモデルのテスト
/// </summary>
public class ResourceGroupDataTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ResourceGroupData_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("location"));
        Assert.True(deserializers.ContainsKey("managedBy"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("resourceGroupProvisioningState"));
        Assert.True(deserializers.ContainsKey("resourceType"));
        Assert.True(deserializers.ContainsKey("systemData"));
        Assert.True(deserializers.ContainsKey("tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ResourceGroupData_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ResourceGroupData_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("id"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>());

        deserializers["id"](parseNode);
        Assert.NotNull(model.Id);

        Assert.True(deserializers.ContainsKey("location"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.AzureLocation>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.AzureLocation>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.AzureLocation>());

        deserializers["location"](parseNode);
        Assert.NotNull(model.Location);

        Assert.True(deserializers.ContainsKey("managedBy"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["managedBy"](parseNode);
        Assert.NotNull(model.ManagedBy);

        Assert.True(deserializers.ContainsKey("name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["name"](parseNode);
        Assert.NotNull(model.Name);

        Assert.True(deserializers.ContainsKey("resourceGroupProvisioningState"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["resourceGroupProvisioningState"](parseNode);
        Assert.NotNull(model.ResourceGroupProvisioningState);

        Assert.True(deserializers.ContainsKey("resourceType"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.ResourceType>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.ResourceType>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.ResourceType>());

        deserializers["resourceType"](parseNode);
        Assert.NotNull(model.ResourceType);

        Assert.True(deserializers.ContainsKey("systemData"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.SystemData>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.SystemData>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.SystemData>());

        deserializers["systemData"](parseNode);
        Assert.NotNull(model.SystemData);

        Assert.True(deserializers.ContainsKey("tags"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags>());

        deserializers["tags"](parseNode);
        Assert.NotNull(model.Tags);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ResourceGroupData_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData();
        originalModel.Id = new Sacloud.Sdk.AddOn.Models.ResourceIdentifier();
        originalModel.Location = new Sacloud.Sdk.AddOn.Models.AzureLocation();
        originalModel.ManagedBy = "sample";
        originalModel.ResourceType = new Sacloud.Sdk.AddOn.Models.ResourceType();
        originalModel.SystemData = new Sacloud.Sdk.AddOn.Models.SystemData();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.ResourceGroupData>(global::Sacloud.Sdk.AddOn.Models.ResourceGroupData.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.Id);
        Assert.NotNull(deserializedModel!.Location);
        Assert.Equal(originalModel.ManagedBy, deserializedModel!.ManagedBy);
        Assert.NotNull(deserializedModel!.ResourceType);
        Assert.NotNull(deserializedModel!.SystemData);
    }
}
