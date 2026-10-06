using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using ServicePrincipalJWTGrantRequestModel = Sacloud.Sdk.IAM.Models.ServicePrincipalJWTGrantRequest;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalJWTGrantRequestのテスト
/// </summary>
public class ServicePrincipalJWTGrantRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ServicePrincipalJWTGrantRequestModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicePrincipalJWTGrantRequestModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServicePrincipalJWTGrantRequestModel.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ServicePrincipalJWTGrantRequestModel();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("assertion"));
        Assert.True(deserializers.ContainsKey("grant_type"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServicePrincipalJWTGrantRequestModel();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ServicePrincipalJWTGrantRequestModel();
        var deserializers = model.GetFieldDeserializers();
        
        var parseNodeAssertion = Substitute.For<IParseNode>();
        parseNodeAssertion.GetStringValue().Returns("test-jwt-assertion");
        deserializers["assertion"](parseNodeAssertion);
        
        var parseNodeGrantType = Substitute.For<IParseNode>();
        parseNodeGrantType.GetEnumValue<IAM.Models.ServicePrincipalJWTGrantRequest_grant_type>()
            .Returns(Sacloud.Sdk.IAM.Models.ServicePrincipalJWTGrantRequest_grant_type.UrnIetfParamsOauthGrantTypeJwtBearer);
        deserializers["grant_type"](parseNodeGrantType);

        Assert.Equal("test-jwt-assertion", model.Assertion);
        Assert.Equal(Sacloud.Sdk.IAM.Models.ServicePrincipalJWTGrantRequest_grant_type.UrnIetfParamsOauthGrantTypeJwtBearer, model.GrantType);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipalJWTGrantRequest_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ServicePrincipalJWTGrantRequestModel
        {
            Assertion = "test-jwt-assertion",
            GrantType = Sacloud.Sdk.IAM.Models.ServicePrincipalJWTGrantRequest_grant_type.UrnIetfParamsOauthGrantTypeJwtBearer
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalJWTGrantRequestModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Assertion, deserializedModel.Assertion);
        Assert.Equal(originalModel.GrantType, deserializedModel.GrantType);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipalJWTGrantRequest_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ServicePrincipalJWTGrantRequestModel
        {
            Assertion = "test-jwt-assertion"
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
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalJWTGrantRequestModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Assertion, deserializedModel.Assertion);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
