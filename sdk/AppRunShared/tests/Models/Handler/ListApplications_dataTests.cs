using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListApplications_dataモデルのテスト
/// </summary>
public class ListApplications_dataTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListApplications_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListApplications_data();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("public_url"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListApplications_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListApplications_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListApplications_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListApplications_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // DateTimeOffset型フィールド: created_at
        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["created_at"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        // String型フィールド: id
        parseNode.GetStringValue().Returns("app-123");
        deserializers["id"](parseNode);
        Assert.Equal("app-123", model.Id);

        // String型フィールド: name
        parseNode.GetStringValue().Returns("app-name");
        deserializers["name"](parseNode);
        Assert.Equal("app-name", model.Name);

        // String型フィールド: public_url
        parseNode.GetStringValue().Returns("https://example.com");
        deserializers["public_url"](parseNode);
        Assert.Equal("https://example.com", model.PublicUrl);

        // Enum型フィールド: status
        parseNode.GetEnumValue<ListApplications_data_status>().Returns(ListApplications_data_status.Healthy);
        deserializers["status"](parseNode);
        Assert.Equal(ListApplications_data_status.Healthy, model.Status);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListApplications_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListApplications_data
        {
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            Id = "app-123",
            Name = "app-name",
            PublicUrl = "https://example.com",
            Status = ListApplications_data_status.Healthy
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListApplications_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.PublicUrl, deserializedModel.PublicUrl);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListApplications_data_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListApplications_data
        {
            Name = "app-name"
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
        var deserializedModel = parseNode.GetObjectValue(ListApplications_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
