using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListTrafficモデルのテスト
/// </summary>
public class ListTrafficTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListTraffic_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListTraffic();
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
    public void ListTraffic_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListTraffic();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListTraffic_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListTraffic();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Collection型フィールド: data
        var dataList = new List<ListTraffic_data>
        {
            new ListTraffic_data { VersionName = "v1.0.0", Percent = 50 }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ListTraffic_data>>())
            .Returns(dataList);
        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
        Assert.Single(model.Data);

        // Object型フィールド: meta
        var meta = new ListTraffic_meta();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ListTraffic_meta>>())
            .Returns(meta);
        deserializers["meta"](parseNode);
        Assert.NotNull(model.Meta);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListTraffic_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListTraffic
        {
            Data = new List<ListTraffic_data>
            {
                new ListTraffic_data { VersionName = "v1.0.0", Percent = 50, IsLatestVersion = true }
            },
            Meta = new ListTraffic_meta()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListTraffic.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Single(deserializedModel.Data);
        Assert.Equal(originalModel.Data[0].VersionName, deserializedModel.Data[0].VersionName);
        Assert.Equal(originalModel.Data[0].Percent, deserializedModel.Data[0].Percent);
        Assert.NotNull(deserializedModel.Meta);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListTraffic_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListTraffic
        {
            Data = new List<ListTraffic_data>
            {
                new ListTraffic_data { VersionName = "v1.0.0" }
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
        var deserializedModel = parseNode.GetObjectValue(ListTraffic.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
