using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using SSOProfileModel = Sacloud.Sdk.IAM.Models.SSOProfile;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// SSOProfileのテスト
/// </summary>
public class SSOProfileTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = SSOProfileModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<SSOProfileModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SSOProfileModel.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new SSOProfileModel();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(12, deserializers.Count);
        Assert.True(deserializers.ContainsKey("assigned"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("idp_certificate"));
        Assert.True(deserializers.ContainsKey("idp_entity_id"));
        Assert.True(deserializers.ContainsKey("idp_login_url"));
        Assert.True(deserializers.ContainsKey("idp_logout_url"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("sp_acs_url"));
        Assert.True(deserializers.ContainsKey("sp_entity_id"));
        Assert.True(deserializers.ContainsKey("updated_at"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SSOProfileModel();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new SSOProfileModel();
        var deserializers = model.GetFieldDeserializers();
        
        var parseNodeAssigned = Substitute.For<IParseNode>();
        parseNodeAssigned.GetBoolValue().Returns(true);
        deserializers["assigned"](parseNodeAssigned);
        
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetStringValue().Returns("2024-01-01T00:00:00Z");
        deserializers["created_at"](parseNodeCreatedAt);
        
        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123456L);
        deserializers["id"](parseNodeId);
        
        var parseNodeIdpCertificate = Substitute.For<IParseNode>();
        parseNodeIdpCertificate.GetStringValue().Returns("-----BEGIN CERTIFICATE-----\ntest\n-----END CERTIFICATE-----");
        deserializers["idp_certificate"](parseNodeIdpCertificate);
        
        var parseNodeIdpEntityId = Substitute.For<IParseNode>();
        parseNodeIdpEntityId.GetStringValue().Returns("https://idp.example.com");
        deserializers["idp_entity_id"](parseNodeIdpEntityId);
        
        var parseNodeIdpLoginUrl = Substitute.For<IParseNode>();
        parseNodeIdpLoginUrl.GetStringValue().Returns("https://idp.example.com/login");
        deserializers["idp_login_url"](parseNodeIdpLoginUrl);
        
        var parseNodeIdpLogoutUrl = Substitute.For<IParseNode>();
        parseNodeIdpLogoutUrl.GetStringValue().Returns("https://idp.example.com/logout");
        deserializers["idp_logout_url"](parseNodeIdpLogoutUrl);
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test SSO Profile");
        deserializers["name"](parseNodeName);
        
        var parseNodeSpAcsUrl = Substitute.For<IParseNode>();
        parseNodeSpAcsUrl.GetStringValue().Returns("https://sp.example.com/acs");
        deserializers["sp_acs_url"](parseNodeSpAcsUrl);
        
        var parseNodeSpEntityId = Substitute.For<IParseNode>();
        parseNodeSpEntityId.GetStringValue().Returns("https://sp.example.com");
        deserializers["sp_entity_id"](parseNodeSpEntityId);
        
        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetStringValue().Returns("2024-01-02T00:00:00Z");
        deserializers["updated_at"](parseNodeUpdatedAt);

        Assert.True(model.Assigned);
        Assert.Equal("2024-01-01T00:00:00Z", model.CreatedAt);
        Assert.Equal("Test Description", model.Description);
        Assert.Equal(123456L, model.Id);
        Assert.Equal("-----BEGIN CERTIFICATE-----\ntest\n-----END CERTIFICATE-----", model.IdpCertificate);
        Assert.Equal("https://idp.example.com", model.IdpEntityId);
        Assert.Equal("https://idp.example.com/login", model.IdpLoginUrl);
        Assert.Equal("https://idp.example.com/logout", model.IdpLogoutUrl);
        Assert.Equal("Test SSO Profile", model.Name);
        Assert.Equal("https://sp.example.com/acs", model.SpAcsUrl);
        Assert.Equal("https://sp.example.com", model.SpEntityId);
        Assert.Equal("2024-01-02T00:00:00Z", model.UpdatedAt);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task SSOProfile_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new SSOProfileModel
        {
            Description = "Test Description",
            IdpCertificate = "-----BEGIN CERTIFICATE-----\ntest\n-----END CERTIFICATE-----",
            IdpEntityId = "https://idp.example.com",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            Name = "Test SSO Profile"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(SSOProfileModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.IdpCertificate, deserializedModel.IdpCertificate);
        Assert.Equal(originalModel.IdpEntityId, deserializedModel.IdpEntityId);
        Assert.Equal(originalModel.IdpLoginUrl, deserializedModel.IdpLoginUrl);
        Assert.Equal(originalModel.IdpLogoutUrl, deserializedModel.IdpLogoutUrl);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task SSOProfile_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new SSOProfileModel
        {
            Name = "Test SSO Profile"
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
        var deserializedModel = parseNode.GetObjectValue(SSOProfileModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
