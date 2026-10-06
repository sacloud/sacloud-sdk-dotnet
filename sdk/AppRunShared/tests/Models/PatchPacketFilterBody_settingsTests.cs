using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// PatchPacketFilterBody_settingsモデルのテスト
/// </summary>
public class PatchPacketFilterBody_settingsTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PatchPacketFilterBody_settings_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PatchPacketFilterBody_settings();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("from_ip"));
        Assert.True(deserializers.ContainsKey("from_ip_prefix_length"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PatchPacketFilterBody_settings_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PatchPacketFilterBody_settings();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchPacketFilterBody_settings_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchPacketFilterBody_settings();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // String型フィールド: from_ip
        parseNode.GetStringValue().Returns("192.168.1.1");
        deserializers["from_ip"](parseNode);
        Assert.Equal("192.168.1.1", model.FromIp);

        // Int型フィールド: from_ip_prefix_length
        parseNode.GetIntValue().Returns(24);
        deserializers["from_ip_prefix_length"](parseNode);
        Assert.Equal(24, model.FromIpPrefixLength);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchPacketFilterBody_settings_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PatchPacketFilterBody_settings
        {
            FromIp = "192.168.1.1",
            FromIpPrefixLength = 24
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PatchPacketFilterBody_settings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.FromIp, deserializedModel.FromIp);
        Assert.Equal(originalModel.FromIpPrefixLength, deserializedModel.FromIpPrefixLength);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchPacketFilterBody_settings_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PatchPacketFilterBody_settings
        {
            FromIp = "192.168.1.1",
            FromIpPrefixLength = 24
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
        var deserializedModel = parseNode.GetObjectValue(PatchPacketFilterBody_settings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
