using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// UpdateTrafficモデルのテスト
/// </summary>
public class UpdateTrafficTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTraffic_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateTraffic();
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
    public void UpdateTraffic_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateTraffic();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateTraffic_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateTraffic();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var dataList = new List<UpdateTraffic_data>
        {
            new UpdateTraffic_data { VersionName = "v1.0.0", Percent = 50 }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<UpdateTraffic_data>>()).Returns(dataList);
        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
        Assert.Single(model.Data);

        var meta = new UpdateTraffic_meta();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<UpdateTraffic_meta>>()).Returns(meta);
        deserializers["meta"](parseNode);
        Assert.NotNull(model.Meta);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateTraffic_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateTraffic
        {
            Data = new List<UpdateTraffic_data>
            {
                new UpdateTraffic_data { VersionName = "v1.0.0", Percent = 50, IsLatestVersion = true }
            },
            Meta = new UpdateTraffic_meta()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UpdateTraffic.CreateFromDiscriminatorValue);

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
    public async Task UpdateTraffic_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UpdateTraffic();
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UpdateTraffic.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
    }
}
