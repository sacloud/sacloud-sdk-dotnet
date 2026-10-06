using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.NetworkingSuite.Tests.Models;

/// <summary>
/// ReadInterfaceConnectionモデルのテスト
/// </summary>
public class ReadInterfaceConnectionTests
{

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sdk.NetworkingSuite.Models.ReadInterfaceConnection.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadInterfaceConnection_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadInterfaceConnection();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("EphemeralIPv4Address"));
        Assert.True(deserializers.ContainsKey("SRN"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadInterfaceConnection_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadInterfaceConnection();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadInterfaceConnection_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadInterfaceConnection();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("EphemeralIPv4Address"));

        parseNode.GetStringValue().Returns("10.0.0.10");

        deserializers["EphemeralIPv4Address"](parseNode);
        Assert.NotNull(model.EphemeralIPv4Address);

        Assert.True(deserializers.ContainsKey("SRN"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["SRN"](parseNode);
        Assert.NotNull(model.SRN);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadInterfaceConnection_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Sdk.NetworkingSuite.Models.ReadInterfaceConnection
        {
            EphemeralIPv4Address = "10.0.0.10",
            SRN = "srn:demo:interface-connection"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.ReadInterfaceConnection.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.EphemeralIPv4Address, deserializedModel.EphemeralIPv4Address);
        Assert.Equal(originalModel.SRN, deserializedModel.SRN);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ReadInterfaceConnection_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Sdk.NetworkingSuite.Models.ReadInterfaceConnection
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
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.ReadInterfaceConnection.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
