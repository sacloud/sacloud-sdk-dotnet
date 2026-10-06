using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models.Handler;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Handler;

/// <summary>
/// CreateBucketRequestBodyモデルのテスト
/// </summary>
public class CreateBucketRequestBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateBucketRequestBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateBucketRequestBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("cluster_id"));
        Assert.True(deserializers.ContainsKey("plan"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateBucketRequestBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateBucketRequestBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateBucketRequestBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateBucketRequestBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("cluster_id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["cluster_id"](parseNode);
        Assert.NotNull(model.ClusterId);

        Assert.True(deserializers.ContainsKey("plan"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateBucketRequestBody_plan>>()).Returns(Substitute.For<CreateBucketRequestBody_plan>());

        deserializers["plan"](parseNode);
        Assert.NotNull(model.Plan);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateBucketRequestBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateBucketRequestBody
        {
            ClusterId = "sample",
            Plan = new CreateBucketRequestBody_plan()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateBucketRequestBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ClusterId, deserializedModel.ClusterId);
        Assert.NotNull(deserializedModel.Plan);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CreateBucketRequestBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new CreateBucketRequestBody
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
        var deserializedModel = parseNode.GetObjectValue(CreateBucketRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
