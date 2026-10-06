using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Permissionモデルのテスト
/// </summary>
public class PermissionTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Permission_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Permission();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Permission_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Permission();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Permission_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Permission();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Permission_data>>()).Returns(new Permission_data());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Permission_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Permission
        {
            Data = new Permission_data
            {
                BucketControls =
                [
                    new BucketControlsItem
                    {
                        BucketName = "bucket-1",
                        CanRead = true,
                        CanWrite = true,
                        CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                    },
                ],
                CreatedAt = DateTimeOffset.Parse("2026-01-02T00:00:00+00:00"),
                DisplayName = "permission",
                Id = 1L,
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
        var deserializedModel = parseNode.GetObjectValue(Permission.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Equal(originalModel.Data.DisplayName, deserializedModel.Data.DisplayName);
        Assert.Equal(originalModel.Data.Id, deserializedModel.Data.Id);
        Assert.NotNull(deserializedModel.Data.BucketControls);
        Assert.Single(deserializedModel.Data.BucketControls);
        Assert.Equal(originalModel.Data.BucketControls[0].BucketName, deserializedModel.Data.BucketControls[0].BucketName);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Permission_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Permission
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
        var deserializedModel = parseNode.GetObjectValue(Permission.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
