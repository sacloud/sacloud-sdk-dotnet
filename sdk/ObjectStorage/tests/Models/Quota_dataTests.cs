using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Quota_dataモデルのテスト
/// </summary>
public class Quota_dataTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Quota_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Quota_data();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("amount_gib_per_bucket"));
        Assert.True(deserializers.ContainsKey("num_buckets"));
        Assert.True(deserializers.ContainsKey("num_buckets_per_permission"));
        Assert.True(deserializers.ContainsKey("num_keys_per_permission"));
        Assert.True(deserializers.ContainsKey("num_objects_per_bucket"));
        Assert.True(deserializers.ContainsKey("num_permissions"));
        Assert.True(deserializers.ContainsKey("num_root_keys"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Quota_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Quota_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Quota_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Quota_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("amount_gib_per_bucket"));

        parseNode.GetFloatValue().Returns(1.5f);

        deserializers["amount_gib_per_bucket"](parseNode);
        Assert.NotNull(model.AmountGibPerBucket);

        Assert.True(deserializers.ContainsKey("num_buckets"));

        parseNode.GetIntValue().Returns(123);

        deserializers["num_buckets"](parseNode);
        Assert.NotNull(model.NumBuckets);

        Assert.True(deserializers.ContainsKey("num_buckets_per_permission"));

        parseNode.GetIntValue().Returns(123);

        deserializers["num_buckets_per_permission"](parseNode);
        Assert.NotNull(model.NumBucketsPerPermission);

        Assert.True(deserializers.ContainsKey("num_keys_per_permission"));

        parseNode.GetIntValue().Returns(123);

        deserializers["num_keys_per_permission"](parseNode);
        Assert.NotNull(model.NumKeysPerPermission);

        Assert.True(deserializers.ContainsKey("num_objects_per_bucket"));

        parseNode.GetIntValue().Returns(123);

        deserializers["num_objects_per_bucket"](parseNode);
        Assert.NotNull(model.NumObjectsPerBucket);

        Assert.True(deserializers.ContainsKey("num_permissions"));

        parseNode.GetIntValue().Returns(123);

        deserializers["num_permissions"](parseNode);
        Assert.NotNull(model.NumPermissions);

        Assert.True(deserializers.ContainsKey("num_root_keys"));

        parseNode.GetIntValue().Returns(123);

        deserializers["num_root_keys"](parseNode);
        Assert.NotNull(model.NumRootKeys);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Quota_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Quota_data
        {
            AmountGibPerBucket = 1.5f,
            NumBuckets = 1,
            NumBucketsPerPermission = 1,
            NumKeysPerPermission = 1,
            NumObjectsPerBucket = 1,
            NumPermissions = 1,
            NumRootKeys = 1
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Quota_data.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AmountGibPerBucket, deserializedModel.AmountGibPerBucket);
        Assert.Equal(originalModel.NumBuckets, deserializedModel.NumBuckets);
        Assert.Equal(originalModel.NumBucketsPerPermission, deserializedModel.NumBucketsPerPermission);
        Assert.Equal(originalModel.NumKeysPerPermission, deserializedModel.NumKeysPerPermission);
        Assert.Equal(originalModel.NumObjectsPerBucket, deserializedModel.NumObjectsPerBucket);
        Assert.Equal(originalModel.NumPermissions, deserializedModel.NumPermissions);
        Assert.Equal(originalModel.NumRootKeys, deserializedModel.NumRootKeys);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Quota_data_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Quota_data
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
        var deserializedModel = parseNode.GetObjectValue(Quota_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
