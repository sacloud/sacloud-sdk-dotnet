using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketControlsItemモデルのテスト
/// </summary>
public class BucketControlsItemTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketControlsItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketControlsItem();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("bucket_name"));
        Assert.True(deserializers.ContainsKey("can_read"));
        Assert.True(deserializers.ContainsKey("can_write"));
        Assert.True(deserializers.ContainsKey("created_at"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketControlsItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketControlsItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketControlsItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketControlsItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("bucket_name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["bucket_name"](parseNode);
        Assert.NotNull(model.BucketName);

        Assert.True(deserializers.ContainsKey("can_read"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["can_read"](parseNode);
        Assert.NotNull(model.CanRead);

        Assert.True(deserializers.ContainsKey("can_write"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["can_write"](parseNode);
        Assert.NotNull(model.CanWrite);

        Assert.True(deserializers.ContainsKey("created_at"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["created_at"](parseNode);
        Assert.NotNull(model.CreatedAt);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketControlsItem_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketControlsItem
        {
            BucketName = "sample",
            CanRead = true,
            CanWrite = true,
            CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00")
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketControlsItem.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.BucketName, deserializedModel.BucketName);
        Assert.Equal(originalModel.CanRead, deserializedModel.CanRead);
        Assert.Equal(originalModel.CanWrite, deserializedModel.CanWrite);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketControlsItem_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketControlsItem
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
        var deserializedModel = parseNode.GetObjectValue(BucketControlsItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
