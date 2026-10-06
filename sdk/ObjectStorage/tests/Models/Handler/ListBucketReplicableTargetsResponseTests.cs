using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Handler;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Handler;

/// <summary>
/// ListBucketReplicableTargetsResponseモデルのテスト
/// </summary>
public class ListBucketReplicableTargetsResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListBucketReplicableTargetsResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListBucketReplicableTargetsResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListBucketReplicableTargetsResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListBucketReplicableTargetsResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListBucketReplicableTargetsResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListBucketReplicableTargetsResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<BucketListResponseBody>>()).Returns([]);

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListBucketReplicableTargetsResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListBucketReplicableTargetsResponse
        {
            Data = []
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListBucketReplicableTargetsResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ListBucketReplicableTargetsResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ListBucketReplicableTargetsResponse
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
        var deserializedModel = parseNode.GetObjectValue(ListBucketReplicableTargetsResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
