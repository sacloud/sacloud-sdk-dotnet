using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// StreamingRequestBodyモデルのテスト
/// </summary>
public class StreamingRequestBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void StreamingRequestBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("location"));
        Assert.True(deserializers.ContainsKey("unitCount"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void StreamingRequestBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void StreamingRequestBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("location"));

        parseNode.GetEnumValue<Sacloud.Sdk.AddOn.Models.StreamingRequestBody_location>().Returns((Sacloud.Sdk.AddOn.Models.StreamingRequestBody_location)0);

        deserializers["location"](parseNode);
        Assert.NotNull(model.Location);

        Assert.True(deserializers.ContainsKey("unitCount"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["unitCount"](parseNode);
        Assert.NotNull(model.UnitCount);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task StreamingRequestBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody();
        originalModel.UnitCount = "sample";

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody>(global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Location, deserializedModel!.Location);
        Assert.Equal(originalModel.UnitCount, deserializedModel!.UnitCount);
    }
}
