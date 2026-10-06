using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using ServicePrincipalKeyModel = Sacloud.Sdk.IAM.Models.ServicePrincipalKey;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalKeyのテスト
/// </summary>
public class ServicePrincipalKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ServicePrincipalKeyModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicePrincipalKeyModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServicePrincipalKeyModel.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ServicePrincipalKeyModel();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("key_expires_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("key_origin"));
        Assert.True(deserializers.ContainsKey("kid"));
        Assert.True(deserializers.ContainsKey("public_key"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServicePrincipalKeyModel();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ServicePrincipalKeyModel();
        var deserializers = model.GetFieldDeserializers();
        
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetStringValue().Returns("2024-01-01T00:00:00Z");
        deserializers["created_at"](parseNodeCreatedAt);
        
        var parseNodeKeyExpiresAt = Substitute.For<IParseNode>();
        parseNodeKeyExpiresAt.GetStringValue().Returns("2024-12-31T23:59:59Z");
        deserializers["key_expires_at"](parseNodeKeyExpiresAt);
        
        var testGuid = Guid.NewGuid();
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetGuidValue().Returns(testGuid);
        deserializers["id"](parseNodeId);
        
        var parseNodeKeyOrigin = Substitute.For<IParseNode>();
        parseNodeKeyOrigin.GetEnumValue<IAM.Models.ServicePrincipalKey_key_origin>()
            .Returns(Sacloud.Sdk.IAM.Models.ServicePrincipalKey_key_origin.User);
        deserializers["key_origin"](parseNodeKeyOrigin);
        
        var parseNodeKid = Substitute.For<IParseNode>();
        parseNodeKid.GetStringValue().Returns("test-kid-123");
        deserializers["kid"](parseNodeKid);
        
        var parseNodePublicKey = Substitute.For<IParseNode>();
        parseNodePublicKey.GetStringValue().Returns("-----BEGIN PUBLIC KEY-----\ntest\n-----END PUBLIC KEY-----");
        deserializers["public_key"](parseNodePublicKey);
        
        var parseNodeStatus = Substitute.For<IParseNode>();
        parseNodeStatus.GetEnumValue<IAM.Models.ServicePrincipalKey_status>()
            .Returns(Sacloud.Sdk.IAM.Models.ServicePrincipalKey_status.Enabled);
        deserializers["status"](parseNodeStatus);

        Assert.Equal("2024-01-01T00:00:00Z", model.CreatedAt);
        Assert.Equal("2024-12-31T23:59:59Z", model.KeyExpiresAt);
        Assert.Equal(testGuid, model.Id);
        Assert.Equal(Sacloud.Sdk.IAM.Models.ServicePrincipalKey_key_origin.User, model.KeyOrigin);
        Assert.Equal("test-kid-123", model.Kid);
        Assert.Equal("-----BEGIN PUBLIC KEY-----\ntest\n-----END PUBLIC KEY-----", model.PublicKey);
        Assert.Equal(Sacloud.Sdk.IAM.Models.ServicePrincipalKey_status.Enabled, model.Status);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipalKey_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ServicePrincipalKeyModel
        {
            CreatedAt = "2024-01-01T00:00:00Z",
            KeyExpiresAt = "2024-12-31T23:59:59Z",
            PublicKey = "-----BEGIN PUBLIC KEY-----\ntest\n-----END PUBLIC KEY-----",
            Status = Sacloud.Sdk.IAM.Models.ServicePrincipalKey_status.Enabled
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalKeyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.KeyExpiresAt, deserializedModel.KeyExpiresAt);
        Assert.Equal(originalModel.PublicKey, deserializedModel.PublicKey);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipalKey_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ServicePrincipalKeyModel
        {
            PublicKey = "-----BEGIN PUBLIC KEY-----\ntest\n-----END PUBLIC KEY-----"
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalKeyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.PublicKey, deserializedModel.PublicKey);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}