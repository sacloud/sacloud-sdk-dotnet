using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// UpdateTrafficBodyMember1モデルのテスト
/// </summary>
public class UpdateTrafficBodyMember1Tests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBodyMember1_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateTrafficBodyMember1();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("is_latest_version"));
        Assert.True(deserializers.ContainsKey("percent"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBodyMember1_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateTrafficBodyMember1();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBodyMember1_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateTrafficBodyMember1();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Bool型フィールド: is_latest_version
        parseNode.GetBoolValue().Returns(true);
        deserializers["is_latest_version"](parseNode);
        Assert.True(model.IsLatestVersion);

        // Int型フィールド: percent
        parseNode.GetIntValue().Returns(100);
        deserializers["percent"](parseNode);
        Assert.Equal(100, model.Percent);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateTrafficBodyMember1_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateTrafficBodyMember1
        {
            IsLatestVersion = true,
            Percent = 100
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UpdateTrafficBodyMember1.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.IsLatestVersion, deserializedModel.IsLatestVersion);
        Assert.Equal(originalModel.Percent, deserializedModel.Percent);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateTrafficBodyMember1_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UpdateTrafficBodyMember1
        {
            IsLatestVersion = true,
            Percent = 100
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
        var deserializedModel = parseNode.GetObjectValue(UpdateTrafficBodyMember1.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
