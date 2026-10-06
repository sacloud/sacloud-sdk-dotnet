using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Groups.Item.Memberships;

namespace Sacloud.Sdk.IAM.Tests.Groups.Item.Memberships;

/// <summary>
/// MembershipsPutRequestBody_compat_usersのテスト
/// </summary>
public class MembershipsPutRequestBody_compat_usersTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = MembershipsPutRequestBody_compat_users.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<MembershipsPutRequestBody_compat_users>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => MembershipsPutRequestBody_compat_users.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new MembershipsPutRequestBody_compat_users();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("id"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new MembershipsPutRequestBody_compat_users();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var body = new MembershipsPutRequestBody_compat_users();
        var deserializers = body.GetFieldDeserializers();

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(12345L);
        deserializers["id"](parseNodeId);

        Assert.Equal(12345L, body.Id);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanSetAndGet()
    {
        var body = new MembershipsPutRequestBody_compat_users();
        var additionalData = new Dictionary<string, object>
        {
            { "custom_field", "custom_value" }
        };

        body.AdditionalData = additionalData;

        Assert.NotNull(body.AdditionalData);
        Assert.True(body.AdditionalData.ContainsKey("custom_field"));
        Assert.Equal("custom_value", body.AdditionalData["custom_field"]);
    }

    /// <summary>
    /// Idプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Id_CanSetAndGet()
    {
        var body = new MembershipsPutRequestBody_compat_users
        {
            Id = 12345L
        };

        Assert.Equal(12345L, body.Id);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new MembershipsPutRequestBody_compat_users
        {
            Id = 12345L
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(MembershipsPutRequestBody_compat_users.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(12345L, deserialized.Id);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new MembershipsPutRequestBody_compat_users
        {
            Id = 12345L,
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(MembershipsPutRequestBody_compat_users.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(12345L, deserialized.Id);
        Assert.NotNull(deserialized.AdditionalData);
    }
}
