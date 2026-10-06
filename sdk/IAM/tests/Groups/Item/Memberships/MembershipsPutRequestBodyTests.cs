using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Groups.Item.Memberships;

namespace Sacloud.Sdk.IAM.Tests.Groups.Item.Memberships;

/// <summary>
/// MembershipsPutRequestBodyのテスト
/// </summary>
public class MembershipsPutRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = MembershipsPutRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<MembershipsPutRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => MembershipsPutRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new MembershipsPutRequestBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("compat_users"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new MembershipsPutRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var body = new MembershipsPutRequestBody();
        var deserializers = body.GetFieldDeserializers();

        var parseNodeCompatUsers = Substitute.For<IParseNode>();
        var compatUsers = new List<MembershipsPutRequestBody_compat_users>
        {
            new MembershipsPutRequestBody_compat_users { Id = 1 }
        };
        parseNodeCompatUsers.GetCollectionOfObjectValues(
            Arg.Any<ParsableFactory<MembershipsPutRequestBody_compat_users>>()
        ).Returns(compatUsers);

        deserializers["compat_users"](parseNodeCompatUsers);

        Assert.NotNull(body.CompatUsers);
        Assert.Single(body.CompatUsers);
        Assert.Equal(1, body.CompatUsers[0].Id);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanSetAndGet()
    {
        var body = new MembershipsPutRequestBody();
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
    /// CompatUsersプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void CompatUsers_CanSetAndGet()
    {
        var body = new MembershipsPutRequestBody();
        var compatUsers = new List<MembershipsPutRequestBody_compat_users>
        {
            new MembershipsPutRequestBody_compat_users { Id = 1 },
            new MembershipsPutRequestBody_compat_users { Id = 2 }
        };

        body.CompatUsers = compatUsers;

        Assert.NotNull(body.CompatUsers);
        Assert.Equal(2, body.CompatUsers.Count);
        Assert.Equal(1, body.CompatUsers[0].Id);
        Assert.Equal(2, body.CompatUsers[1].Id);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new MembershipsPutRequestBody
        {
            CompatUsers =
            [
                new MembershipsPutRequestBody_compat_users { Id = 1 },
                new MembershipsPutRequestBody_compat_users { Id = 2 }
            ]
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(MembershipsPutRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.CompatUsers);
        Assert.Equal(2, deserialized.CompatUsers.Count);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new MembershipsPutRequestBody
        {
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
        var deserialized = parseNode.GetObjectValue(MembershipsPutRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
    }
}
