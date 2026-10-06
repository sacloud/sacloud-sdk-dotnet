using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketQuota_dataモデルのテスト
/// </summary>
public class BucketQuota_dataTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketQuota_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketQuota_data();
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
    public void BucketQuota_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketQuota_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketQuota_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketQuota_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("amount_gib_per_bucket"));

        parseNode.GetFloatValue().Returns(1.5f);

        deserializers["amount_gib_per_bucket"](parseNode);
        Assert.NotNull(model.AmountGibPerBucket);

        Assert.True(deserializers.ContainsKey("num_objects_per_bucket"));

        parseNode.GetIntValue().Returns(123);

        deserializers["num_objects_per_bucket"](parseNode);
        Assert.NotNull(model.NumObjectsPerBucket);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketQuota_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketQuota_data
        {
            AmountGibPerBucket = 1.5f,
            NumObjectsPerBucket = 1
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketQuota_data.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AmountGibPerBucket, deserializedModel.AmountGibPerBucket);
        Assert.Equal(originalModel.NumObjectsPerBucket, deserializedModel.NumObjectsPerBucket);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketQuota_data_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketQuota_data
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
        var deserializedModel = parseNode.GetObjectValue(BucketQuota_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
