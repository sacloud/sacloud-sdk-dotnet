using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Permissions_dataモデルのテスト
/// </summary>
public class Permissions_dataTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Permissions_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Permissions_data();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("bucket_controls"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("display_name"));
        Assert.True(deserializers.ContainsKey("id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Permissions_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Permissions_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Permissions_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Permissions_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("bucket_controls"));

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<BucketControlsItem>>()).Returns([new BucketControlsItem()]);

        deserializers["bucket_controls"](parseNode);
        Assert.NotNull(model.BucketControls);

        Assert.True(deserializers.ContainsKey("created_at"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["created_at"](parseNode);
        Assert.NotNull(model.CreatedAt);

        Assert.True(deserializers.ContainsKey("display_name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["display_name"](parseNode);
        Assert.NotNull(model.DisplayName);

        Assert.True(deserializers.ContainsKey("id"));

        parseNode.GetLongValue().Returns(123L);

        deserializers["id"](parseNode);
        Assert.NotNull(model.Id);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Permissions_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Permissions_data
        {
            BucketControls =
            [
                new BucketControlsItem
                {
                    BucketName = "bucket-1",
                    CanRead = true,
                    CanWrite = false,
                    CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                },
            ],
            CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
            DisplayName = "sample",
            Id = 1L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Permissions_data.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.BucketControls);
        Assert.Single(deserializedModel.BucketControls);
        Assert.Equal(originalModel.BucketControls[0].BucketName, deserializedModel.BucketControls[0].BucketName);
        Assert.Equal(originalModel.BucketControls[0].CanRead, deserializedModel.BucketControls[0].CanRead);
        Assert.Equal(originalModel.BucketControls[0].CanWrite, deserializedModel.BucketControls[0].CanWrite);
        Assert.Equal(originalModel.BucketControls[0].CreatedAt, deserializedModel.BucketControls[0].CreatedAt);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.DisplayName, deserializedModel.DisplayName);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Permissions_data_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Permissions_data
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
        var deserializedModel = parseNode.GetObjectValue(Permissions_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
