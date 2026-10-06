using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models.Handler;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Handler;

/// <summary>
/// UpdateBucketEncryptionResponseモデルのテスト
/// </summary>
public class UpdateBucketEncryptionResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateBucketEncryptionResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateBucketEncryptionResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("configured_at"));
        Assert.True(deserializers.ContainsKey("kms_key_id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateBucketEncryptionResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateBucketEncryptionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateBucketEncryptionResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateBucketEncryptionResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("configured_at"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["configured_at"](parseNode);
        Assert.NotNull(model.ConfiguredAt);

        Assert.True(deserializers.ContainsKey("kms_key_id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["kms_key_id"](parseNode);
        Assert.NotNull(model.KmsKeyId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateBucketEncryptionResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateBucketEncryptionResponse
        {
            ConfiguredAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
            KmsKeyId = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UpdateBucketEncryptionResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ConfiguredAt, deserializedModel.ConfiguredAt);
        Assert.Equal(originalModel.KmsKeyId, deserializedModel.KmsKeyId);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task UpdateBucketEncryptionResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new UpdateBucketEncryptionResponse
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
        var deserializedModel = parseNode.GetObjectValue(UpdateBucketEncryptionResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
