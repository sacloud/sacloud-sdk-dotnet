using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// ResourceIdentifierモデルのテスト
/// </summary>
public class ResourceIdentifierTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ResourceIdentifier_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceIdentifier();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("location"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("parent"));
        Assert.True(deserializers.ContainsKey("provider"));
        Assert.True(deserializers.ContainsKey("resourceGroupName"));
        Assert.True(deserializers.ContainsKey("resourceType"));
        Assert.True(deserializers.ContainsKey("subscriptionId"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ResourceIdentifier_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceIdentifier();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ResourceIdentifier_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceIdentifier();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("location"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.AzureLocation>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.AzureLocation>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.AzureLocation>());

        deserializers["location"](parseNode);
        Assert.NotNull(model.Location);

        Assert.True(deserializers.ContainsKey("name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["name"](parseNode);
        Assert.NotNull(model.Name);

        Assert.True(deserializers.ContainsKey("parent"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.ResourceIdentifier>());

        deserializers["parent"](parseNode);
        Assert.NotNull(model.Parent);

        Assert.True(deserializers.ContainsKey("provider"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["provider"](parseNode);
        Assert.NotNull(model.Provider);

        Assert.True(deserializers.ContainsKey("resourceGroupName"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["resourceGroupName"](parseNode);
        Assert.NotNull(model.ResourceGroupName);

        Assert.True(deserializers.ContainsKey("resourceType"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.ResourceType>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.ResourceType>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.ResourceType>());

        deserializers["resourceType"](parseNode);
        Assert.NotNull(model.ResourceType);

        Assert.True(deserializers.ContainsKey("subscriptionId"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["subscriptionId"](parseNode);
        Assert.NotNull(model.SubscriptionId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ResourceIdentifier_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.ResourceIdentifier();
        originalModel.Location = new Sacloud.Sdk.AddOn.Models.AzureLocation();
        originalModel.Parent = new Sacloud.Sdk.AddOn.Models.ResourceIdentifier();
        originalModel.ResourceType = new Sacloud.Sdk.AddOn.Models.ResourceType();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.ResourceIdentifier>(global::Sacloud.Sdk.AddOn.Models.ResourceIdentifier.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.Location);
        Assert.NotNull(deserializedModel!.Parent);
        Assert.NotNull(deserializedModel!.ResourceType);
    }
}
