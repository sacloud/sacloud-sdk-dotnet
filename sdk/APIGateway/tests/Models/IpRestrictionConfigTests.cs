using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// IpRestrictionConfigのテスト
/// </summary>
public class IpRestrictionConfigTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new IpRestrictionConfig();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = IpRestrictionConfig.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<IpRestrictionConfig>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IpRestrictionConfig.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"ips", "protocols", "restrictedBy"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new IpRestrictionConfig();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("ips"));
        Assert.True(deserializers.ContainsKey("protocols"));
        Assert.True(deserializers.ContainsKey("restrictedBy"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new IpRestrictionConfig();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new IpRestrictionConfig();
        var deserializers = model.GetFieldDeserializers();

        var ipsNode = Substitute.For<IParseNode>();
        ipsNode.GetCollectionOfPrimitiveValues<string>().Returns(["0.0.0.0", "0.0.0.1"]);
        deserializers["ips"](ipsNode);

        var protocolsNode = Substitute.For<IParseNode>();
        protocolsNode.GetEnumValue<IpRestrictionConfig_protocols>().Returns(IpRestrictionConfig_protocols.Https);
        deserializers["protocols"](protocolsNode);

        var restrictedByNode = Substitute.For<IParseNode>();
        restrictedByNode.GetEnumValue<IpRestrictionConfig_restrictedBy>().Returns(IpRestrictionConfig_restrictedBy.AllowIps);
        deserializers["restrictedBy"](restrictedByNode);

        Assert.Equal(["0.0.0.0", "0.0.0.1"], model.Ips);
        Assert.Equal(IpRestrictionConfig_protocols.Https, model.Protocols);
        Assert.Equal(IpRestrictionConfig_restrictedBy.AllowIps, model.RestrictedBy);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task IpRestrictionConfig_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IpRestrictionConfig
        {
            Ips = ["0.0.0.0", "0.0.0.1"],
            Protocols = IpRestrictionConfig_protocols.Https,
            RestrictedBy = IpRestrictionConfig_restrictedBy.AllowIps,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IpRestrictionConfig.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Ips, deserializedModel.Ips);
        Assert.Equal(originalModel.Protocols, deserializedModel.Protocols);
        Assert.Equal(originalModel.RestrictedBy, deserializedModel.RestrictedBy);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task IpRestrictionConfig_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IpRestrictionConfig
        {
            Ips = ["0.0.0.0", "0.0.0.1"],
            Protocols = IpRestrictionConfig_protocols.Https,
            RestrictedBy = IpRestrictionConfig_restrictedBy.AllowIps,
        };
        originalModel.AdditionalData["customField"] = "customValue";
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IpRestrictionConfig.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Ips, deserializedModel.Ips);
        Assert.Equal(originalModel.Protocols, deserializedModel.Protocols);
        Assert.Equal(originalModel.RestrictedBy, deserializedModel.RestrictedBy);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
