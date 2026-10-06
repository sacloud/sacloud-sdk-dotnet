using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

/// <summary>
/// UpdateCommonServiceItemRequest_CommonServiceItemモデルのテスト
/// </summary>
public class UpdateCommonServiceItemRequest_CommonServiceItemTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void UpdateCommonServiceItemRequest_CommonServiceItem_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UpdateCommonServiceItemRequest_CommonServiceItem.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UpdateCommonServiceItemRequest_CommonServiceItem>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateCommonServiceItemRequest_CommonServiceItem_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            UpdateCommonServiceItemRequest_CommonServiceItem.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateCommonServiceItemRequest_CommonServiceItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateCommonServiceItemRequest_CommonServiceItem();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("Icon"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Provider"));
        Assert.True(deserializers.ContainsKey("Settings"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateCommonServiceItemRequest_CommonServiceItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateCommonServiceItemRequest_CommonServiceItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateCommonServiceItemRequest_CommonServiceItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateCommonServiceItemRequest_CommonServiceItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("Updated Name");
        deserializers["Name"](parseNode);
        Assert.Equal("Updated Name", model.Name);

        parseNode.GetStringValue().Returns("Updated Description");
        deserializers["Description"](parseNode);
        Assert.Equal("Updated Description", model.Description);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateCommonServiceItemRequest_CommonServiceItem_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateCommonServiceItemRequest_CommonServiceItem
        {
            Name = "Updated Name",
            Description = "Updated Description",
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
        var deserializedModel = parseNode.GetObjectValue(UpdateCommonServiceItemRequest_CommonServiceItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateCommonServiceItemRequest_CommonServiceItem_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UpdateCommonServiceItemRequest_CommonServiceItem { Name = "Test" };
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
        var deserializedModel = parseNode.GetObjectValue(UpdateCommonServiceItemRequest_CommonServiceItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
