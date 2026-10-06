using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.NetworkingSuite.Tests.Models;

/// <summary>
/// ReadAddressモデルのテスト
/// </summary>
public class ReadAddressTests
{

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sdk.NetworkingSuite.Models.ReadAddress.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadAddress_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadAddress();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("AddressType"));
        Assert.True(deserializers.ContainsKey("IPAddress"));
        Assert.True(deserializers.ContainsKey("IPVersion"));
        Assert.True(deserializers.ContainsKey("SRN"));
        Assert.True(deserializers.ContainsKey("Subnet"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadAddress_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadAddress();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadAddress_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadAddress();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("AddressType"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["AddressType"](parseNode);
        Assert.NotNull(model.AddressType);

        Assert.True(deserializers.ContainsKey("IPAddress"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["IPAddress"](parseNode);
        Assert.NotNull(model.IPAddress);

        Assert.True(deserializers.ContainsKey("IPVersion"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["IPVersion"](parseNode);
        Assert.NotNull(model.IPVersion);

        Assert.True(deserializers.ContainsKey("SRN"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["SRN"](parseNode);
        Assert.NotNull(model.SRN);

        Assert.True(deserializers.ContainsKey("Subnet"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Sdk.NetworkingSuite.Models.SakuraResourceNameRef>>()).Returns(Substitute.For<Sdk.NetworkingSuite.Models.SakuraResourceNameRef>());

        deserializers["Subnet"](parseNode);
        Assert.NotNull(model.Subnet);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadAddress_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Sdk.NetworkingSuite.Models.ReadAddress
        {
            AddressType = "sample",
            IPAddress = "sample",
            IPVersion = "sample",
            SRN = "sample",
            Subnet = new Sdk.NetworkingSuite.Models.SakuraResourceNameRef()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.ReadAddress.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AddressType, deserializedModel.AddressType);
        Assert.Equal(originalModel.IPAddress, deserializedModel.IPAddress);
        Assert.Equal(originalModel.IPVersion, deserializedModel.IPVersion);
        Assert.Equal(originalModel.SRN, deserializedModel.SRN);
        Assert.NotNull(deserializedModel.Subnet);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ReadAddress_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadAddress
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
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.ReadAddress.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
