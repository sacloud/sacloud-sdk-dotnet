using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// CreateBucketReplicationモデルのテスト
/// </summary>
public class CreateBucketReplicationTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateBucketReplication_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateBucketReplication();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("dest_bucket"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateBucketReplication_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateBucketReplication();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateBucketReplication_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateBucketReplication();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("dest_bucket"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["dest_bucket"](parseNode);
        Assert.NotNull(model.DestBucket);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateBucketReplication_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateBucketReplication
        {
            DestBucket = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateBucketReplication.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DestBucket, deserializedModel.DestBucket);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CreateBucketReplication_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new CreateBucketReplication
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
        var deserializedModel = parseNode.GetObjectValue(CreateBucketReplication.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
