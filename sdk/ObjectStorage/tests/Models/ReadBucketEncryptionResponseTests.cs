using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// ReadBucketEncryptionResponseモデルのテスト
/// </summary>
public class ReadBucketEncryptionResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadBucketEncryptionResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ObjectStorage.Models.ReadBucketEncryptionResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadBucketEncryptionResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ObjectStorage.Models.ReadBucketEncryptionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadBucketEncryptionResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ObjectStorage.Models.ReadBucketEncryptionResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.ReadBucketEncryptionResponse>>()).Returns(Substitute.For<ObjectStorage.Models.Handler.ReadBucketEncryptionResponse>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadBucketEncryptionResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ObjectStorage.Models.ReadBucketEncryptionResponse
        {
            Data = new ObjectStorage.Models.Handler.ReadBucketEncryptionResponse
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.ReadBucketEncryptionResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Equal(originalModel.Data.ConfiguredAt, deserializedModel.Data.ConfiguredAt);
        Assert.Equal(originalModel.Data.KmsKeyId, deserializedModel.Data.KmsKeyId);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ReadBucketEncryptionResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ObjectStorage.Models.ReadBucketEncryptionResponse
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.ReadBucketEncryptionResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
