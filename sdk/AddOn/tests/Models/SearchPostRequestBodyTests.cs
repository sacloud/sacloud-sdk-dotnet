using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// SearchPostRequestBodyモデルのテスト
/// </summary>
public class SearchPostRequestBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void SearchPostRequestBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.SearchPostRequestBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("location"));
        Assert.True(deserializers.ContainsKey("partitionCount"));
        Assert.True(deserializers.ContainsKey("replicaCount"));
        Assert.True(deserializers.ContainsKey("sku"));
        Assert.True(deserializers.ContainsKey("skuString"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SearchPostRequestBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.SearchPostRequestBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void SearchPostRequestBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.SearchPostRequestBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("location"));

        parseNode.GetEnumValue<Sacloud.Sdk.AddOn.Models.SearchPostRequestBody_location>().Returns((Sacloud.Sdk.AddOn.Models.SearchPostRequestBody_location)0);

        deserializers["location"](parseNode);
        Assert.NotNull(model.Location);

        Assert.True(deserializers.ContainsKey("partitionCount"));

        parseNode.GetIntValue().Returns(12);

        deserializers["partitionCount"](parseNode);
        Assert.NotNull(model.PartitionCount);

        Assert.True(deserializers.ContainsKey("replicaCount"));

        parseNode.GetIntValue().Returns(123);

        deserializers["replicaCount"](parseNode);
        Assert.NotNull(model.ReplicaCount);

        Assert.True(deserializers.ContainsKey("sku"));

        parseNode.GetIntValue().Returns(123);

        deserializers["sku"](parseNode);
        Assert.NotNull(model.Sku);

        Assert.True(deserializers.ContainsKey("skuString"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["skuString"](parseNode);
        Assert.NotNull(model.SkuString);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task SearchPostRequestBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.SearchPostRequestBody();
        originalModel.PartitionCount = 1;
        originalModel.ReplicaCount = 1;
        originalModel.Sku = 1;

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.SearchPostRequestBody>(global::Sacloud.Sdk.AddOn.Models.SearchPostRequestBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Location, deserializedModel!.Location);
        Assert.Equal(originalModel.PartitionCount, deserializedModel!.PartitionCount);
        Assert.Equal(originalModel.ReplicaCount, deserializedModel!.ReplicaCount);
        Assert.Equal(originalModel.Sku, deserializedModel!.Sku);
    }
}
