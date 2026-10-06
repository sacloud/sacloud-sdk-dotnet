using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketPenalty_dataモデルのテスト
/// </summary>
public class BucketPenalty_dataTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketPenalty_data();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("amount_gib_per_bucket"));
        Assert.True(deserializers.ContainsKey("num_objects_per_bucket"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketPenalty_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketPenalty_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var amount = new BucketPenalty_data_amount_gib_per_bucket
        {
            IsApplied = true,
            Quota = 1.5f,
            Val = 2.5f,
        };
        var objects = new BucketPenalty_data_num_objects_per_bucket
        {
            IsApplied = true,
            Quota = 10,
            Val = 20,
        };

        Assert.True(deserializers.ContainsKey("amount_gib_per_bucket"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketPenalty_data_amount_gib_per_bucket>>()).Returns(amount);

        deserializers["amount_gib_per_bucket"](parseNode);
        Assert.NotNull(model.AmountGibPerBucket);

        Assert.True(deserializers.ContainsKey("num_objects_per_bucket"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketPenalty_data_num_objects_per_bucket>>()).Returns(objects);

        deserializers["num_objects_per_bucket"](parseNode);
        Assert.NotNull(model.NumObjectsPerBucket);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketPenalty_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketPenalty_data
        {
            AmountGibPerBucket = new BucketPenalty_data_amount_gib_per_bucket
            {
                IsApplied = true,
                Quota = 1.5f,
                Val = 2.5f,
            },
            NumObjectsPerBucket = new BucketPenalty_data_num_objects_per_bucket
            {
                IsApplied = true,
                Quota = 10,
                Val = 20,
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketPenalty_data.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.AmountGibPerBucket);
        Assert.NotNull(deserializedModel.NumObjectsPerBucket);
        Assert.Equal(originalModel.AmountGibPerBucket.IsApplied, deserializedModel.AmountGibPerBucket.IsApplied);
        Assert.Equal(originalModel.AmountGibPerBucket.Quota, deserializedModel.AmountGibPerBucket.Quota);
        Assert.Equal(originalModel.AmountGibPerBucket.Val, deserializedModel.AmountGibPerBucket.Val);
        Assert.Equal(originalModel.NumObjectsPerBucket.IsApplied, deserializedModel.NumObjectsPerBucket.IsApplied);
        Assert.Equal(originalModel.NumObjectsPerBucket.Quota, deserializedModel.NumObjectsPerBucket.Quota);
        Assert.Equal(originalModel.NumObjectsPerBucket.Val, deserializedModel.NumObjectsPerBucket.Val);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketPenalty_data_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketPenalty_data
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
        var deserializedModel = parseNode.GetObjectValue(BucketPenalty_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
