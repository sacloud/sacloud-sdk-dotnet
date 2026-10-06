using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketPenaltyモデルのテスト
/// </summary>
public class BucketPenaltyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketPenalty();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketPenalty();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketPenalty_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketPenalty();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var data = new BucketPenalty_data
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
            },
        };

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketPenalty_data>>()).Returns(data);

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketPenalty_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketPenalty
        {
            Data = new BucketPenalty_data
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
                },
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
        var deserializedModel = parseNode.GetObjectValue(BucketPenalty.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.NotNull(deserializedModel.Data.AmountGibPerBucket);
        Assert.NotNull(deserializedModel.Data.NumObjectsPerBucket);
        Assert.Equal(originalModel.Data.AmountGibPerBucket.IsApplied, deserializedModel.Data.AmountGibPerBucket.IsApplied);
        Assert.Equal(originalModel.Data.AmountGibPerBucket.Quota, deserializedModel.Data.AmountGibPerBucket.Quota);
        Assert.Equal(originalModel.Data.AmountGibPerBucket.Val, deserializedModel.Data.AmountGibPerBucket.Val);
        Assert.Equal(originalModel.Data.NumObjectsPerBucket.IsApplied, deserializedModel.Data.NumObjectsPerBucket.IsApplied);
        Assert.Equal(originalModel.Data.NumObjectsPerBucket.Quota, deserializedModel.Data.NumObjectsPerBucket.Quota);
        Assert.Equal(originalModel.Data.NumObjectsPerBucket.Val, deserializedModel.Data.NumObjectsPerBucket.Val);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketPenalty_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketPenalty
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
        var deserializedModel = parseNode.GetObjectValue(BucketPenalty.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
