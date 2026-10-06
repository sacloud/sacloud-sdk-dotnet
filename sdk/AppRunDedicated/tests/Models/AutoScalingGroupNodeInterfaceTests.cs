using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// AutoScalingGroupNodeInterfaceモデルのテスト
/// </summary>
public class AutoScalingGroupNodeInterfaceTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void AutoScalingGroupNodeInterface_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = AutoScalingGroupNodeInterface.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<AutoScalingGroupNodeInterface>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void AutoScalingGroupNodeInterface_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            AutoScalingGroupNodeInterface.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void AutoScalingGroupNodeInterface_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new AutoScalingGroupNodeInterface();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("connectsToLB"));
        Assert.True(deserializers.ContainsKey("defaultGateway"));
        Assert.True(deserializers.ContainsKey("interfaceIndex"));
        Assert.True(deserializers.ContainsKey("ipPool"));
        Assert.True(deserializers.ContainsKey("netmaskLen"));
        Assert.True(deserializers.ContainsKey("packetFilterID"));
        Assert.True(deserializers.ContainsKey("upstream"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void AutoScalingGroupNodeInterface_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new AutoScalingGroupNodeInterface();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void AutoScalingGroupNodeInterface_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new AutoScalingGroupNodeInterface();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["connectsToLB"](parseNode);
        Assert.True(model.ConnectsToLB);

        parseNode.GetStringValue().Returns("192.168.1.1");
        deserializers["defaultGateway"](parseNode);
        Assert.Equal("192.168.1.1", model.DefaultGateway);

        parseNode.GetIntValue().Returns(0);
        deserializers["interfaceIndex"](parseNode);
        Assert.Equal(0, model.InterfaceIndex);

        var testIpPool = new List<IpRange> { new IpRange() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<IpRange>>())
            .Returns(testIpPool);
        deserializers["ipPool"](parseNode);
        Assert.NotNull(model.IpPool);
        Assert.Single(model.IpPool);

        parseNode.GetIntValue().Returns(24);
        deserializers["netmaskLen"](parseNode);
        Assert.Equal(24, model.NetmaskLen);

        parseNode.GetStringValue().Returns("pf-123456");
        deserializers["packetFilterID"](parseNode);
        Assert.Equal("pf-123456", model.PacketFilterID);

        parseNode.GetStringValue().Returns("shared");
        deserializers["upstream"](parseNode);
        Assert.Equal("shared", model.Upstream);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task AutoScalingGroupNodeInterface_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new AutoScalingGroupNodeInterface
        {
            ConnectsToLB = true,
            DefaultGateway = "192.168.1.1",
            InterfaceIndex = 0,
            IpPool = new List<IpRange>
            {
                new IpRange { Start = "192.168.1.10", End = "192.168.1.20" }
            },
            NetmaskLen = 24,
            PacketFilterID = "pf-123456",
            Upstream = "shared"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AutoScalingGroupNodeInterface.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ConnectsToLB, deserializedModel.ConnectsToLB);
        Assert.Equal(originalModel.DefaultGateway, deserializedModel.DefaultGateway);
        Assert.Equal(originalModel.InterfaceIndex, deserializedModel.InterfaceIndex);
        Assert.NotNull(deserializedModel.IpPool);
        Assert.Single(deserializedModel.IpPool);
        Assert.Equal(originalModel.NetmaskLen, deserializedModel.NetmaskLen);
        Assert.Equal(originalModel.PacketFilterID, deserializedModel.PacketFilterID);
        Assert.Equal(originalModel.Upstream, deserializedModel.Upstream);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task AutoScalingGroupNodeInterface_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new AutoScalingGroupNodeInterface
        {
            InterfaceIndex = 0
        };
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AutoScalingGroupNodeInterface.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
