using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadPacketFilterモデルのテスト
/// </summary>
public class ReadPacketFilterTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadPacketFilter_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ReadPacketFilter();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("is_enabled"));
        Assert.True(deserializers.ContainsKey("settings"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadPacketFilter_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ReadPacketFilter();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadPacketFilter_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ReadPacketFilter();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Bool型フィールド: is_enabled
        parseNode.GetBoolValue().Returns(true);
        deserializers["is_enabled"](parseNode);
        Assert.True(model.IsEnabled);

        // Collection型フィールド: settings
        var settingsList = new List<ReadPacketFilter_settings>
        {
            new ReadPacketFilter_settings { FromIp = "192.168.1.0", FromIpPrefixLength = 24 }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ReadPacketFilter_settings>>())
            .Returns(settingsList);
        deserializers["settings"](parseNode);
        Assert.NotNull(model.Settings);
        Assert.Single(model.Settings);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadPacketFilter_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ReadPacketFilter
        {
            IsEnabled = true,
            Settings = new List<ReadPacketFilter_settings>
            {
                new ReadPacketFilter_settings { FromIp = "192.168.1.0", FromIpPrefixLength = 24 }
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
        var deserializedModel = parseNode.GetObjectValue(ReadPacketFilter.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.IsEnabled, deserializedModel.IsEnabled);
        Assert.NotNull(deserializedModel.Settings);
        Assert.Single(deserializedModel.Settings);
        Assert.Equal(originalModel.Settings[0].FromIp, deserializedModel.Settings[0].FromIp);
        Assert.Equal(originalModel.Settings[0].FromIpPrefixLength, deserializedModel.Settings[0].FromIpPrefixLength);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadPacketFilter_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ReadPacketFilter
        {
            IsEnabled = true
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
        var deserializedModel = parseNode.GetObjectValue(ReadPacketFilter.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
