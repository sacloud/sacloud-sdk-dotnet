using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketResponseBodyモデルのテスト
/// </summary>
public class BucketResponseBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketResponseBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketResponseBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("cluster_id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("plan"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketResponseBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketResponseBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketResponseBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketResponseBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("sample");
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Bucket_plan>>()).Returns(new Bucket_plan());

        foreach (var deserializer in deserializers.Values)
        {
            deserializer(parseNode);
        }

        Assert.Equal("sample", model.ClusterId);
        Assert.Equal("sample", model.Name);
        Assert.NotNull(model.Plan);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketResponseBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketResponseBody
        {
            ClusterId = "cluster-1",
            Name = "bucket-1",
            Plan = new Bucket_plan { ServiceClassPath = "standard", Type = PlanType.Standard },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketResponseBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ClusterId, deserializedModel.ClusterId);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Plan);
        Assert.Equal(originalModel.Plan.ServiceClassPath, deserializedModel.Plan.ServiceClassPath);
        Assert.Equal(originalModel.Plan.Type, deserializedModel.Plan.Type);
    }

    [Fact]
    public async Task BucketResponseBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketResponseBody
        {
            AdditionalData = new Dictionary<string, object> { ["extra_key"] = "extra-value" },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        writer.GetSerializedContent().CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketResponseBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Contains("extra_key", deserializedModel.AdditionalData.Keys);
    }
}
