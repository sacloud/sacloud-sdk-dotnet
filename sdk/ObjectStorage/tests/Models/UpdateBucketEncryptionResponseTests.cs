using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

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
        var model = new ObjectStorage.Models.UpdateBucketEncryptionResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateBucketEncryptionResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ObjectStorage.Models.UpdateBucketEncryptionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateBucketEncryptionResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ObjectStorage.Models.UpdateBucketEncryptionResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.UpdateBucketEncryptionResponse>>()).Returns(Substitute.For<ObjectStorage.Models.Handler.UpdateBucketEncryptionResponse>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateBucketEncryptionResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ObjectStorage.Models.UpdateBucketEncryptionResponse
        {
            Data = new ObjectStorage.Models.Handler.UpdateBucketEncryptionResponse
            {
                ConfiguredAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                KmsKeyId = "kms-key-id",
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.UpdateBucketEncryptionResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Equal(originalModel.Data.ConfiguredAt, deserializedModel.Data.ConfiguredAt);
        Assert.Equal(originalModel.Data.KmsKeyId, deserializedModel.Data.KmsKeyId);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task UpdateBucketEncryptionResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ObjectStorage.Models.UpdateBucketEncryptionResponse
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.UpdateBucketEncryptionResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
