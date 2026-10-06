using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// UpdateTrafficBodyMember2モデルのテスト
/// </summary>
public class UpdateTrafficBodyMember2Tests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBodyMember2_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateTrafficBodyMember2();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("percent"));
        Assert.True(deserializers.ContainsKey("version_name"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBodyMember2_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateTrafficBodyMember2();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBodyMember2_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateTrafficBodyMember2();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

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
    public async Task UpdateTrafficBodyMember2_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateTrafficBodyMember2
        {
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
        var deserializedModel = parseNode.GetObjectValue(UpdateTrafficBodyMember2.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Percent, deserializedModel.Percent);
        Assert.Equal(originalModel.VersionName, deserializedModel.VersionName);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateTrafficBodyMember2_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UpdateTrafficBodyMember2
        {
            Percent = 50,
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
        var deserializedModel = parseNode.GetObjectValue(UpdateTrafficBodyMember2.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
