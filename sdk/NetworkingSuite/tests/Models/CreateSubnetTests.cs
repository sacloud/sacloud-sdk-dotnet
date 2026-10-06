using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.NetworkingSuite.Tests.Models;

/// <summary>
/// CreateSubnetモデルのテスト
/// </summary>
public class CreateSubnetTests
{

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sdk.NetworkingSuite.Models.CreateSubnet.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateSubnet_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Sdk.NetworkingSuite.Models.CreateSubnet();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("IPv4AddressRangeCIDR"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("SubnetGroup"));
        Assert.True(deserializers.ContainsKey("Zone"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateSubnet_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Sdk.NetworkingSuite.Models.CreateSubnet();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateSubnet_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Sdk.NetworkingSuite.Models.CreateSubnet();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("Description"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["Description"](parseNode);
        Assert.NotNull(model.Description);

        Assert.True(deserializers.ContainsKey("IPv4AddressRangeCIDR"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["IPv4AddressRangeCIDR"](parseNode);
        Assert.NotNull(model.IPv4AddressRangeCIDR);

        Assert.True(deserializers.ContainsKey("Name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["Name"](parseNode);
        Assert.NotNull(model.Name);

        Assert.True(deserializers.ContainsKey("SubnetGroup"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Sdk.NetworkingSuite.Models.SakuraResourceNameRef>>()).Returns(Substitute.For<Sdk.NetworkingSuite.Models.SakuraResourceNameRef>());

        deserializers["SubnetGroup"](parseNode);
        Assert.NotNull(model.SubnetGroup);

        Assert.True(deserializers.ContainsKey("Zone"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Sdk.NetworkingSuite.Models.Zone>>()).Returns(Substitute.For<Sdk.NetworkingSuite.Models.Zone>());

        deserializers["Zone"](parseNode);
        Assert.NotNull(model.Zone);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateSubnet_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Sdk.NetworkingSuite.Models.CreateSubnet
        {
            Description = "sample",
            IPv4AddressRangeCIDR = "sample",
            Name = "sample",
            SubnetGroup = new Sdk.NetworkingSuite.Models.SakuraResourceNameRef(),
            Zone = new Sdk.NetworkingSuite.Models.Zone()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.CreateSubnet.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.IPv4AddressRangeCIDR, deserializedModel.IPv4AddressRangeCIDR);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.SubnetGroup);
        Assert.NotNull(deserializedModel.Zone);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CreateSubnet_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Sdk.NetworkingSuite.Models.CreateSubnet
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
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.CreateSubnet.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
