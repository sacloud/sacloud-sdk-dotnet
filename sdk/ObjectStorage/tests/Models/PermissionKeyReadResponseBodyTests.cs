using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PermissionKeyReadResponseBodyモデルのテスト
/// </summary>
public class PermissionKeyReadResponseBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PermissionKeyReadResponseBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PermissionKeyReadResponseBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PermissionKeyReadResponseBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PermissionKeyReadResponseBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PermissionKeyReadResponseBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PermissionKeyReadResponseBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("created_at"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["created_at"](parseNode);
        Assert.NotNull(model.CreatedAt);

        Assert.True(deserializers.ContainsKey("id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["id"](parseNode);
        Assert.NotNull(model.Id);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PermissionKeyReadResponseBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PermissionKeyReadResponseBody
        {
            CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
            Id = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PermissionKeyReadResponseBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PermissionKeyReadResponseBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PermissionKeyReadResponseBody
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
        var deserializedModel = parseNode.GetObjectValue(PermissionKeyReadResponseBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
