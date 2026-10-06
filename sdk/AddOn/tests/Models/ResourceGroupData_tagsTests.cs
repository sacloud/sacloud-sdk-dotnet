using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// ResourceGroupData_tagsモデルのテスト
/// </summary>
public class ResourceGroupData_tagsTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ResourceGroupData_tags_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ResourceGroupData_tags_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ResourceGroupData_tags_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ResourceGroupData_tags_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags>(global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ResourceGroupData_tags_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags>(global::Sacloud.Sdk.AddOn.Models.ResourceGroupData_tags.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
