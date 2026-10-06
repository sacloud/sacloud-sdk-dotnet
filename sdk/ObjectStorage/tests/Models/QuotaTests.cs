using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Quotaモデルのテスト
/// </summary>
public class QuotaTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Quota_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ObjectStorage.Models.Quota();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Quota_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ObjectStorage.Models.Quota();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Quota_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ObjectStorage.Models.Quota();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Quota_data>>()).Returns(Substitute.For<Quota_data>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Quota_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ObjectStorage.Models.Quota
        {
            Data = new Quota_data
            {
                AmountGibPerBucket = 10.5f,
                NumBuckets = 11,
                NumBucketsPerPermission = 12,
                NumKeysPerPermission = 13,
                NumObjectsPerBucket = 14,
                NumPermissions = 15,
                NumRootKeys = 16,
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.Quota.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Equal(originalModel.Data.AmountGibPerBucket, deserializedModel.Data.AmountGibPerBucket);
        Assert.Equal(originalModel.Data.NumBuckets, deserializedModel.Data.NumBuckets);
        Assert.Equal(originalModel.Data.NumBucketsPerPermission, deserializedModel.Data.NumBucketsPerPermission);
        Assert.Equal(originalModel.Data.NumKeysPerPermission, deserializedModel.Data.NumKeysPerPermission);
        Assert.Equal(originalModel.Data.NumObjectsPerBucket, deserializedModel.Data.NumObjectsPerBucket);
        Assert.Equal(originalModel.Data.NumPermissions, deserializedModel.Data.NumPermissions);
        Assert.Equal(originalModel.Data.NumRootKeys, deserializedModel.Data.NumRootKeys);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Quota_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ObjectStorage.Models.Quota
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.Quota.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
