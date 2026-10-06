using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// OidcDetailのテスト
/// </summary>
public class OidcDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OidcDetail.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OidcDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OidcDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが14個のキーを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new OidcDetail();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(14, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("authenticationMethods"));
        Assert.True(deserializers.ContainsKey("clientId"));
        Assert.True(deserializers.ContainsKey("clientSecret"));
        Assert.True(deserializers.ContainsKey("hideCredentials"));
        Assert.True(deserializers.ContainsKey("issuer"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("refreshTokenParamName"));
        Assert.True(deserializers.ContainsKey("scopes"));
        Assert.True(deserializers.ContainsKey("tokenAudiences"));
        Assert.True(deserializers.ContainsKey("useSession"));
        Assert.True(deserializers.ContainsKey("services"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new OidcDetail();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersの主要アクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new OidcDetail();
        var deserializers = model.GetFieldDeserializers();

        var service = new ServiceSummary();
        var parseNodeServices = Substitute.For<IParseNode>();
        parseNodeServices
            .GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ServiceSummary>>())
            .Returns([service]);
        deserializers["services"](parseNodeServices);

        var parseNodeClientId = Substitute.For<IParseNode>();
        parseNodeClientId.GetStringValue().Returns("client-id");
        deserializers["clientId"](parseNodeClientId);

        var parseNodeClientSecret = Substitute.For<IParseNode>();
        parseNodeClientSecret.GetStringValue().Returns("client-secret");
        deserializers["clientSecret"](parseNodeClientSecret);

        var parseNodeHideCredentials = Substitute.For<IParseNode>();
        parseNodeHideCredentials.GetBoolValue().Returns(true);
        deserializers["hideCredentials"](parseNodeHideCredentials);

        var parseNodeIssuer = Substitute.For<IParseNode>();
        parseNodeIssuer.GetStringValue().Returns("https://issuer.example.com");
        deserializers["issuer"](parseNodeIssuer);

        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("oidc-detail");
        deserializers["name"](parseNodeName);

        var parseNodeUseSession = Substitute.For<IParseNode>();
        parseNodeUseSession.GetBoolValue().Returns(false);
        deserializers["useSession"](parseNodeUseSession);

        Assert.NotNull(model.Services);
        Assert.Single(model.Services);
        Assert.Contains(service, model.Services);
        Assert.Equal("client-id", model.ClientId);
        Assert.Equal("client-secret", model.ClientSecret);
        Assert.True(model.HideCredentials);
        Assert.Equal("https://issuer.example.com", model.Issuer);
        Assert.Equal("oidc-detail", model.Name);
        Assert.False(model.UseSession);
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var services = new List<ServiceSummary> { new() };
        var model = new OidcDetail
        {
            ClientId = "client-id",
            Name = "oidc-detail",
            Services = services,
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("clientId", "client-id");
        writer.Received(1).WriteStringValue("name", "oidc-detail");
        writer.Received(1).WriteCollectionOfObjectValues("services", services);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task OidcDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new OidcDetail
        {
            ClientId = "client-id",
            Name = "oidc-detail",
            Scopes = ["openid"],
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(OidcDetail.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("client-id", deserializedModel.ClientId);
        Assert.Equal("oidc-detail", deserializedModel.Name);
        Assert.Contains("openid", deserializedModel.Scopes);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task OidcDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new OidcDetail
        {
            ClientId = "client-id",
            Name = "oidc-detail",
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
        var deserializedModel = parseNode.GetObjectValue(OidcDetail.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
