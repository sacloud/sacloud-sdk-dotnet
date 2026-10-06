using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.NetworkingSuite.Tests.Models;

/// <summary>
/// AddressResponseモデルのテスト
/// </summary>
public class AddressResponseTests
{

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sdk.NetworkingSuite.Models.AddressResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void AddressResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Sdk.NetworkingSuite.Models.AddressResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Address"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void AddressResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Sdk.NetworkingSuite.Models.AddressResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void AddressResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Sdk.NetworkingSuite.Models.AddressResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("Address"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Sdk.NetworkingSuite.Models.ReadAddress>>()).Returns(Substitute.For<Sdk.NetworkingSuite.Models.ReadAddress>());

        deserializers["Address"](parseNode);
        Assert.NotNull(model.Address);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task AddressResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Sdk.NetworkingSuite.Models.AddressResponse
        {
            Address = new Sdk.NetworkingSuite.Models.ReadAddress()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.AddressResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Address);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task AddressResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Sdk.NetworkingSuite.Models.AddressResponse
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
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.AddressResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
