using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// QueryPostRequestBodyモデルのテスト
/// </summary>
public class QueryPostRequestBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void QueryPostRequestBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.QueryPostRequestBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("location"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void QueryPostRequestBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.QueryPostRequestBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryPostRequestBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.QueryPostRequestBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("location"));

        parseNode.GetEnumValue<Sacloud.Sdk.AddOn.Models.QueryPostRequestBody_location>().Returns((Sacloud.Sdk.AddOn.Models.QueryPostRequestBody_location)0);

        deserializers["location"](parseNode);
        Assert.NotNull(model.Location);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task QueryPostRequestBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.QueryPostRequestBody();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.QueryPostRequestBody>(global::Sacloud.Sdk.AddOn.Models.QueryPostRequestBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Location, deserializedModel!.Location);
    }
}
