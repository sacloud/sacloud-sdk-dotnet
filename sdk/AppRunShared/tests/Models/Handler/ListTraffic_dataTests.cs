using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListTraffic_dataモデルのテスト
/// </summary>
public class ListTraffic_dataTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListTraffic_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListTraffic_data();
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
    public void ListTraffic_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListTraffic_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListTraffic_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListTraffic_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Bool型フィールド: is_latest_version
        parseNode.GetBoolValue().Returns(true);
        deserializers["is_latest_version"](parseNode);
        Assert.True(model.IsLatestVersion);

        // Int型フィールド: percent
        parseNode.GetIntValue().Returns(50);
        deserializers["percent"](parseNode);
        Assert.Equal(50, model.Percent);

        // String型フィールド: version_name
        parseNode.GetStringValue().Returns("v1.0.0");
        deserializers["version_name"](parseNode);
        Assert.Equal("v1.0.0", model.VersionName);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListTraffic_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListTraffic_data
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
        var deserializedModel = parseNode.GetObjectValue(ListTraffic_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.IsLatestVersion, deserializedModel.IsLatestVersion);
        Assert.Equal(originalModel.Percent, deserializedModel.Percent);
        Assert.Equal(originalModel.VersionName, deserializedModel.VersionName);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListTraffic_data_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListTraffic_data
        {
            VersionName = "v1.0.0"
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
        var deserializedModel = parseNode.GetObjectValue(ListTraffic_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
