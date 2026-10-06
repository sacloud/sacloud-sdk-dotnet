using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketPenalty_data_num_objects_per_bucketモデルのテスト
/// </summary>
public class BucketPenalty_data_num_objects_per_bucketTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_data_num_objects_per_bucket_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketPenalty_data_num_objects_per_bucket();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("is_applied"));
        Assert.True(deserializers.ContainsKey("quota"));
        Assert.True(deserializers.ContainsKey("val"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_data_num_objects_per_bucket_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketPenalty_data_num_objects_per_bucket();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_data_num_objects_per_bucket_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketPenalty_data_num_objects_per_bucket();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("is_applied"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["is_applied"](parseNode);
        Assert.NotNull(model.IsApplied);

        Assert.True(deserializers.ContainsKey("quota"));

        parseNode.GetIntValue().Returns(123);

        deserializers["quota"](parseNode);
        Assert.NotNull(model.Quota);

        Assert.True(deserializers.ContainsKey("val"));

        parseNode.GetIntValue().Returns(123);

        deserializers["val"](parseNode);
        Assert.NotNull(model.Val);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketPenalty_data_num_objects_per_bucket_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketPenalty_data_num_objects_per_bucket
        {
            IsApplied = true,
            Quota = 1,
            Val = 1
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketPenalty_data_num_objects_per_bucket.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.IsApplied, deserializedModel.IsApplied);
        Assert.Equal(originalModel.Quota, deserializedModel.Quota);
        Assert.Equal(originalModel.Val, deserializedModel.Val);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketPenalty_data_num_objects_per_bucket_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketPenalty_data_num_objects_per_bucket
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
        var deserializedModel = parseNode.GetObjectValue(BucketPenalty_data_num_objects_per_bucket.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
