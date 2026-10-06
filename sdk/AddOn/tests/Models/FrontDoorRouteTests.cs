using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// FrontDoorRouteモデルのテスト
/// </summary>
public class FrontDoorRouteTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void FrontDoorRoute_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.FrontDoorRoute();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("originGroup"));
        Assert.True(deserializers.ContainsKey("patterns"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void FrontDoorRoute_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.FrontDoorRoute();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void FrontDoorRoute_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.FrontDoorRoute();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("originGroup"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.FrontDoorOriginGroup>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.FrontDoorOriginGroup>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.FrontDoorOriginGroup>());

        deserializers["originGroup"](parseNode);
        Assert.NotNull(model.OriginGroup);

        Assert.True(deserializers.ContainsKey("patterns"));

        Assert.NotNull(parseNode);

        deserializers["patterns"](parseNode);
        Assert.NotNull(model.Patterns);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task FrontDoorRoute_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.FrontDoorRoute();
        originalModel.OriginGroup = new Sacloud.Sdk.AddOn.Models.FrontDoorOriginGroup();
        originalModel.Patterns = [];

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.FrontDoorRoute>(global::Sacloud.Sdk.AddOn.Models.FrontDoorRoute.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.OriginGroup);
        Assert.NotNull(deserializedModel!.Patterns);
    }
}
