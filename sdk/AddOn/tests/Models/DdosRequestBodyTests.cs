using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// DdosRequestBodyモデルのテスト
/// </summary>
public class DdosRequestBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void DdosRequestBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.DdosRequestBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("endpoint"));
        Assert.True(deserializers.ContainsKey("location"));
        Assert.True(deserializers.ContainsKey("profile"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DdosRequestBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.DdosRequestBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void DdosRequestBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.DdosRequestBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("endpoint"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.FrontDoorEndpoint>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.FrontDoorEndpoint>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.FrontDoorEndpoint>());

        deserializers["endpoint"](parseNode);
        Assert.NotNull(model.Endpoint);

        Assert.True(deserializers.ContainsKey("location"));

        parseNode.GetEnumValue<Sacloud.Sdk.AddOn.Models.DdosRequestBody_location>().Returns((Sacloud.Sdk.AddOn.Models.DdosRequestBody_location)0);

        deserializers["location"](parseNode);
        Assert.NotNull(model.Location);

        Assert.True(deserializers.ContainsKey("profile"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.FrontDoorProfile>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.FrontDoorProfile>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.FrontDoorProfile>());

        deserializers["profile"](parseNode);
        Assert.NotNull(model.Profile);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task DdosRequestBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.DdosRequestBody();
        originalModel.Endpoint = new Sacloud.Sdk.AddOn.Models.FrontDoorEndpoint();
        originalModel.Profile = new Sacloud.Sdk.AddOn.Models.FrontDoorProfile();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.DdosRequestBody>(global::Sacloud.Sdk.AddOn.Models.DdosRequestBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.Endpoint);
        Assert.Equal(originalModel.Location, deserializedModel!.Location);
        Assert.NotNull(deserializedModel!.Profile);
    }
}
