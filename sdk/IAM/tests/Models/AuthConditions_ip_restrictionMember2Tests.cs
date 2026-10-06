using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// AuthConditions_ip_restrictionMember2のテスト
/// </summary>
public class AuthConditions_ip_restrictionMember2Tests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = AuthConditions_ip_restrictionMember2.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<AuthConditions_ip_restrictionMember2>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            AuthConditions_ip_restrictionMember2.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember2();

        var deserializers = ipRestriction.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("mode"));
        Assert.True(deserializers.ContainsKey("source_network"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember2();

        Assert.Throws<ArgumentNullException>(() => ipRestriction.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember2();
        var deserializers = ipRestriction.GetFieldDeserializers();

        var parseNodeMode = Substitute.For<IParseNode>();
        parseNodeMode.GetEnumValue<AuthConditions_ip_restrictionMember2_mode>()
            .Returns(AuthConditions_ip_restrictionMember2_mode.Allow_list);
        deserializers["mode"](parseNodeMode);

        var sourceNetworkList = new List<string> { "192.168.1.0/24", "10.0.0.0/8" };
        var parseNodeSourceNetwork = Substitute.For<IParseNode>();
        parseNodeSourceNetwork.GetCollectionOfPrimitiveValues<string>()
            .Returns(sourceNetworkList);
        deserializers["source_network"](parseNodeSourceNetwork);

        Assert.Equal(AuthConditions_ip_restrictionMember2_mode.Allow_list, ipRestriction.Mode);
        Assert.Equal(sourceNetworkList, ipRestriction.SourceNetwork);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_ip_restrictionMember2_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new AuthConditions_ip_restrictionMember2
        {
            Mode = AuthConditions_ip_restrictionMember2_mode.Allow_list,
            SourceNetwork = ["192.168.1.0/24", "10.0.0.0/8"]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions_ip_restrictionMember2.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(AuthConditions_ip_restrictionMember2_mode.Allow_list, deserializedModel.Mode);
        Assert.NotNull(deserializedModel.SourceNetwork);
        Assert.Equal(2, deserializedModel.SourceNetwork.Count);
        Assert.Contains("192.168.1.0/24", deserializedModel.SourceNetwork);
        Assert.Contains("10.0.0.0/8", deserializedModel.SourceNetwork);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_ip_restrictionMember2_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new AuthConditions_ip_restrictionMember2();
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions_ip_restrictionMember2.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
