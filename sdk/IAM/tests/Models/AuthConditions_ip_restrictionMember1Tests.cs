using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// AuthConditions_ip_restrictionMember1のテスト
/// </summary>
public class AuthConditions_ip_restrictionMember1Tests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = AuthConditions_ip_restrictionMember1.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<AuthConditions_ip_restrictionMember1>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            AuthConditions_ip_restrictionMember1.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember1();

        var deserializers = ipRestriction.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("mode"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember1();

        Assert.Throws<ArgumentNullException>(() => ipRestriction.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember1();
        var deserializers = ipRestriction.GetFieldDeserializers();

        var parseNodeMode = Substitute.For<IParseNode>();
        parseNodeMode.GetEnumValue<AuthConditions_ip_restrictionMember1_mode>()
            .Returns(AuthConditions_ip_restrictionMember1_mode.Allow_all);
        deserializers["mode"](parseNodeMode);

        Assert.Equal(AuthConditions_ip_restrictionMember1_mode.Allow_all, ipRestriction.Mode);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを確認（JsonSerializationWriter/JsonParseNodeFactory使用）
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember1
        {
            Mode = AuthConditions_ip_restrictionMember1_mode.Allow_all
        };

        using var serializeStream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(null, ipRestriction);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserialized = parseNode.GetObjectValue(AuthConditions_ip_restrictionMember1.CreateFromDiscriminatorValue);

        Assert.Equal(AuthConditions_ip_restrictionMember1_mode.Allow_all, deserialized.Mode);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを確認（JsonSerializationWriter/JsonParseNodeFactory使用）
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var ipRestriction = new AuthConditions_ip_restrictionMember1();
        ipRestriction.AdditionalData["customField"] = "customValue";

        using var serializeStream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(null, ipRestriction);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserialized = parseNode.GetObjectValue(AuthConditions_ip_restrictionMember1.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
        Assert.Equal("customValue", deserialized.AdditionalData["customField"]?.ToString());
    }
}
