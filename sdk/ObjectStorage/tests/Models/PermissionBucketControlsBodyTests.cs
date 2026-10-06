using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PermissionBucketControlsBodyモデルのテスト
/// </summary>
public class PermissionBucketControlsBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PermissionBucketControlsBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PermissionBucketControlsBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("bucket_controls"));
        Assert.True(deserializers.ContainsKey("display_name"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PermissionBucketControlsBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PermissionBucketControlsBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PermissionBucketControlsBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PermissionBucketControlsBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("bucket_controls"));

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<BucketControlsBodyItem>>()).Returns([new BucketControlsBodyItem()]);

        deserializers["bucket_controls"](parseNode);
        Assert.NotNull(model.BucketControls);

        Assert.True(deserializers.ContainsKey("display_name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["display_name"](parseNode);
        Assert.NotNull(model.DisplayName);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PermissionBucketControlsBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PermissionBucketControlsBody
        {
            BucketControls =
            [
                new BucketControlsBodyItem
                {
                    BucketName = "bucket-1",
                    CanRead = true,
                    CanWrite = false,
                },
            ],
            DisplayName = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PermissionBucketControlsBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.BucketControls);
        Assert.Single(deserializedModel.BucketControls);
        Assert.Equal(originalModel.BucketControls[0].BucketName, deserializedModel.BucketControls[0].BucketName);
        Assert.Equal(originalModel.BucketControls[0].CanRead, deserializedModel.BucketControls[0].CanRead);
        Assert.Equal(originalModel.BucketControls[0].CanWrite, deserializedModel.BucketControls[0].CanWrite);
        Assert.Equal(originalModel.DisplayName, deserializedModel.DisplayName);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PermissionBucketControlsBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PermissionBucketControlsBody
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
        var deserializedModel = parseNode.GetObjectValue(PermissionBucketControlsBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
