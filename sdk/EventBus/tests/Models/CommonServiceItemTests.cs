using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

/// <summary>
/// CommonServiceItemモデルのテスト
/// </summary>
public class CommonServiceItemTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CommonServiceItem_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CommonServiceItem.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CommonServiceItem>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CommonServiceItem_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            CommonServiceItem.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CommonServiceItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CommonServiceItem();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(13, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Availability"));
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("Icon"));
        Assert.True(deserializers.ContainsKey("ModifiedAt"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Provider"));
        Assert.True(deserializers.ContainsKey("ServiceClass"));
        Assert.True(deserializers.ContainsKey("Settings"));
        Assert.True(deserializers.ContainsKey("SettingsHash"));
        Assert.True(deserializers.ContainsKey("Status"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CommonServiceItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CommonServiceItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CommonServiceItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CommonServiceItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("available");
        deserializers["Availability"](parseNode);
        Assert.Equal("available", model.Availability);

        var createdAt = DateTimeOffset.UtcNow;
        parseNode.GetDateTimeOffsetValue().Returns(createdAt);
        deserializers["CreatedAt"](parseNode);
        Assert.Equal(createdAt, model.CreatedAt);

        parseNode.GetStringValue().Returns("Test Description");
        deserializers["Description"](parseNode);
        Assert.Equal("Test Description", model.Description);

        parseNode.GetStringValue().Returns("123456");
        deserializers["ID"](parseNode);
        Assert.Equal("123456", model.ID);

        parseNode.GetStringValue().Returns("Test Name");
        deserializers["Name"](parseNode);
        Assert.Equal("Test Name", model.Name);

        parseNode.GetStringValue().Returns("cloud/eventbus");
        deserializers["ServiceClass"](parseNode);
        Assert.Equal("cloud/eventbus", model.ServiceClass);

        parseNode.GetStringValue().Returns("hash123");
        deserializers["SettingsHash"](parseNode);
        Assert.Equal("hash123", model.SettingsHash);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CommonServiceItem_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CommonServiceItem
        {
            Availability = "available",
            CreatedAt = DateTimeOffset.UtcNow,
            Description = "Test Description",
            ID = "123456",
            Name = "Test Name",
            ServiceClass = "cloud/eventbus",
            SettingsHash = "hash123",
            Tags = new List<string> { "tag1", "tag2" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CommonServiceItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Availability, deserializedModel.Availability);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.ID, deserializedModel.ID);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.ServiceClass, deserializedModel.ServiceClass);
        Assert.Equal(originalModel.SettingsHash, deserializedModel.SettingsHash);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CommonServiceItem_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CommonServiceItem
        {
            ID = "123456",
            Name = "Test"
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CommonServiceItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
