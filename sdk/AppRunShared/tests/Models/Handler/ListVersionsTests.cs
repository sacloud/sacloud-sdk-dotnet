using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListVersionsモデルのテスト
/// </summary>
public class ListVersionsTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListVersions_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListVersions();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("data"));
        Assert.True(deserializers.ContainsKey("meta"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListVersions_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListVersions();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListVersions_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListVersions();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Collection型フィールド: data
        var dataList = new List<ListVersions_data>
        {
            new ListVersions_data { Name = "version-1", Id = "v1" }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ListVersions_data>>())
            .Returns(dataList);
        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
        Assert.Single(model.Data);

        // Object型フィールド: meta
        var meta = new ListVersions_meta();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ListVersions_meta>>())
            .Returns(meta);
        deserializers["meta"](parseNode);
        Assert.NotNull(model.Meta);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListVersions_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListVersions
        {
            Data = new List<ListVersions_data>
            {
                new ListVersions_data 
                { 
                    Name = "version-1", 
                    Id = "v1", 
                    Status = ListVersions_data_status.Healthy,
                    CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z")
                }
            },
            Meta = new ListVersions_meta
            {
                PageNum = 1,
                PageSize = 20
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
        var deserializedModel = parseNode.GetObjectValue(ListVersions.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Single(deserializedModel.Data);
        Assert.Equal(originalModel.Data[0].Name, deserializedModel.Data[0].Name);
        Assert.Equal(originalModel.Data[0].Id, deserializedModel.Data[0].Id);
        Assert.NotNull(deserializedModel.Meta);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListVersions_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListVersions
        {
            Data = new List<ListVersions_data>
            {
                new ListVersions_data { Name = "version-1" }
            }
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
        var deserializedModel = parseNode.GetObjectValue(ListVersions.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
