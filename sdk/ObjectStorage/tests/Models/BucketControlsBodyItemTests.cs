using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketControlsBodyItemモデルのテスト
/// </summary>
public class BucketControlsBodyItemTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketControlsBodyItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketControlsBodyItem();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("bucket_name"));
        Assert.True(deserializers.ContainsKey("can_read"));
        Assert.True(deserializers.ContainsKey("can_write"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketControlsBodyItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketControlsBodyItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketControlsBodyItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketControlsBodyItem();
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
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketControlsBodyItem_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketControlsBodyItem
        {
            BucketName = "sample",
            CanRead = true,
            CanWrite = true
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketControlsBodyItem.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.BucketName, deserializedModel.BucketName);
        Assert.Equal(originalModel.CanRead, deserializedModel.CanRead);
        Assert.Equal(originalModel.CanWrite, deserializedModel.CanWrite);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketControlsBodyItem_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketControlsBodyItem
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
        var deserializedModel = parseNode.GetObjectValue(BucketControlsBodyItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
