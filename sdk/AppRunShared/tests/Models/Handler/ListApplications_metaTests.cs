using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListApplications_metaモデルのテスト
/// </summary>
public class ListApplications_metaTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListApplications_meta_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListApplications_meta();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("object_total"));
        Assert.True(deserializers.ContainsKey("page_num"));
        Assert.True(deserializers.ContainsKey("page_size"));
        Assert.True(deserializers.ContainsKey("sort_field"));
        Assert.True(deserializers.ContainsKey("sort_order"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListApplications_meta_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListApplications_meta();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListApplications_meta_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListApplications_meta();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Int型フィールド: object_total
        parseNode.GetIntValue().Returns(100);
        deserializers["object_total"](parseNode);
        Assert.Equal(100, model.ObjectTotal);

        // Int型フィールド: page_num
        parseNode.GetIntValue().Returns(1);
        deserializers["page_num"](parseNode);
        Assert.Equal(1, model.PageNum);

        // Int型フィールド: page_size
        parseNode.GetIntValue().Returns(20);
        deserializers["page_size"](parseNode);
        Assert.Equal(20, model.PageSize);

        // String型フィールド: sort_field
        parseNode.GetStringValue().Returns("name");
        deserializers["sort_field"](parseNode);
        Assert.Equal("name", model.SortField);

        // Enum型フィールド: sort_order
        parseNode.GetEnumValue<ListApplications_meta_sort_order>().Returns(ListApplications_meta_sort_order.Asc);
        deserializers["sort_order"](parseNode);
        Assert.Equal(ListApplications_meta_sort_order.Asc, model.SortOrder);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListApplications_meta_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListApplications_meta
        {
            ObjectTotal = 100,
            PageNum = 1,
            PageSize = 20,
            SortField = "name",
            SortOrder = ListApplications_meta_sort_order.Asc
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListApplications_meta.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ObjectTotal, deserializedModel.ObjectTotal);
        Assert.Equal(originalModel.PageNum, deserializedModel.PageNum);
        Assert.Equal(originalModel.PageSize, deserializedModel.PageSize);
        Assert.Equal(originalModel.SortField, deserializedModel.SortField);
        Assert.Equal(originalModel.SortOrder, deserializedModel.SortOrder);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListApplications_meta_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListApplications_meta
        {
            PageNum = 1
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
        var deserializedModel = parseNode.GetObjectValue(ListApplications_meta.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
