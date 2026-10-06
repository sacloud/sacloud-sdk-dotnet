using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListVersions_dataモデルのテスト
/// </summary>
public class ListVersions_dataTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListVersions_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListVersions_data();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListVersions_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListVersions_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListVersions_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListVersions_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // DateTimeOffset型フィールド: created_at
        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["created_at"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        // String型フィールド: id
        parseNode.GetStringValue().Returns("version-123");
        deserializers["id"](parseNode);
        Assert.Equal("version-123", model.Id);

        // String型フィールド: name
        parseNode.GetStringValue().Returns("version-name");
        deserializers["name"](parseNode);
        Assert.Equal("version-name", model.Name);

        // Enum型フィールド: status
        parseNode.GetEnumValue<ListVersions_data_status>().Returns(ListVersions_data_status.Healthy);
        deserializers["status"](parseNode);
        Assert.Equal(ListVersions_data_status.Healthy, model.Status);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListVersions_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListVersions_data
        {
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            Id = "version-123",
            Name = "version-name",
            Status = ListVersions_data_status.Healthy
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListVersions_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListVersions_data_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListVersions_data
        {
            Name = "version-name"
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListVersions_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
