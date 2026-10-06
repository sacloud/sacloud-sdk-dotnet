using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketListResponseBodyモデルのテスト
/// </summary>
public class BucketListResponseBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketListResponseBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketListResponseBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("plan"));
        Assert.True(deserializers.ContainsKey("resource_id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketListResponseBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketListResponseBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketListResponseBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketListResponseBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var plan = new BucketListResponseBody_plan
        {
            ServiceClassPath = "service-class-path-1",
            Type = ObjectStorage.Models.Model.PlanType.Standard,
        };

        Assert.True(deserializers.ContainsKey("name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["name"](parseNode);
        Assert.NotNull(model.Name);

        Assert.True(deserializers.ContainsKey("plan"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketListResponseBody_plan>>()).Returns(plan);

        deserializers["plan"](parseNode);
        Assert.NotNull(model.Plan);

        Assert.True(deserializers.ContainsKey("resource_id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["resource_id"](parseNode);
        Assert.NotNull(model.ResourceId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketListResponseBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketListResponseBody
        {
            Name = "sample",
            Plan = new BucketListResponseBody_plan
            {
                ServiceClassPath = "service-class-path-1",
                Type = ObjectStorage.Models.Model.PlanType.Standard,
            },
            ResourceId = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketListResponseBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Plan);
        Assert.Equal(originalModel.Plan.ServiceClassPath, deserializedModel.Plan.ServiceClassPath);
        Assert.Equal(originalModel.Plan.Type, deserializedModel.Plan.Type);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketListResponseBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketListResponseBody
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketListResponseBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
