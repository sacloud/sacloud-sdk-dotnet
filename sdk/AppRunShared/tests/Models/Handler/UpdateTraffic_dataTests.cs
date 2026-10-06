using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// UpdateTraffic_dataモデルのテスト
/// </summary>
public class UpdateTraffic_dataTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTraffic_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateTraffic_data();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("is_latest_version"));
        Assert.True(deserializers.ContainsKey("percent"));
        Assert.True(deserializers.ContainsKey("version_name"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateTraffic_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateTraffic_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateTraffic_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateTraffic_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_latest_version"](parseNode);
        Assert.True(model.IsLatestVersion);

        parseNode.GetIntValue().Returns(50);
        deserializers["percent"](parseNode);
        Assert.Equal(50, model.Percent);

        parseNode.GetStringValue().Returns("v1.0.0");
        deserializers["version_name"](parseNode);
        Assert.Equal("v1.0.0", model.VersionName);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateTraffic_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateTraffic_data
        {
            IsLatestVersion = true,
            Percent = 50,
            VersionName = "v1.0.0"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UpdateTraffic_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.IsLatestVersion, deserializedModel.IsLatestVersion);
        Assert.Equal(originalModel.Percent, deserializedModel.Percent);
        Assert.Equal(originalModel.VersionName, deserializedModel.VersionName);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateTraffic_data_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UpdateTraffic_data { VersionName = "v1.0.0" };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UpdateTraffic_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
    }
}
