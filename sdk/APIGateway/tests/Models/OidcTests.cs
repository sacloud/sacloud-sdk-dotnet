using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Oidcのテスト
/// </summary>
public class OidcTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = APIGateway.Models.Oidc.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<APIGateway.Models.Oidc>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => APIGateway.Models.Oidc.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが13個のキーを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new APIGateway.Models.Oidc();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(13, deserializers.Count);
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
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new APIGateway.Models.Oidc();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersの主要アクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new APIGateway.Models.Oidc();
        var deserializers = model.GetFieldDeserializers();

        var authenticationMethod = APIGateway.Models.Oidc_authenticationMethods.AccessToken;
        var parseNodeAuthenticationMethods = Substitute.For<IParseNode>();
        parseNodeAuthenticationMethods
            .GetCollectionOfEnumValues<APIGateway.Models.Oidc_authenticationMethods>()
            .Returns([authenticationMethod]);
        deserializers["authenticationMethods"](parseNodeAuthenticationMethods);

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
        parseNodeName.GetStringValue().Returns("oidc-auth");
        deserializers["name"](parseNodeName);

        var parseNodeRefreshTokenParamName = Substitute.For<IParseNode>();
        parseNodeRefreshTokenParamName.GetStringValue().Returns("refresh_token");
        deserializers["refreshTokenParamName"](parseNodeRefreshTokenParamName);

        var tokenAudiences = new[] { "api://audience-1", "api://audience-2" };
        var parseNodeTokenAudiences = Substitute.For<IParseNode>();
        parseNodeTokenAudiences.GetCollectionOfPrimitiveValues<string>().Returns(tokenAudiences);
        deserializers["tokenAudiences"](parseNodeTokenAudiences);

        var parseNodeUseSession = Substitute.For<IParseNode>();
        parseNodeUseSession.GetBoolValue().Returns(true);
        deserializers["useSession"](parseNodeUseSession);

        Assert.NotNull(model.AuthenticationMethods);
        Assert.Single(model.AuthenticationMethods);
        Assert.Contains(authenticationMethod, model.AuthenticationMethods);
        Assert.Equal("client-id", model.ClientId);
        Assert.Equal("client-secret", model.ClientSecret);
        Assert.True(model.HideCredentials);
        Assert.Equal("https://issuer.example.com", model.Issuer);
        Assert.Equal("oidc-auth", model.Name);

        // RefreshTokenParamNameはAPI側で非推奨となっているが、利用されているためSDK側では保持し検証を行う。
        // そのため意図的に警告を抑制している。
#pragma warning disable CS0612
        Assert.Equal("refresh_token", model.RefreshTokenParamName);
#pragma warning restore CS0612

        Assert.NotNull(model.TokenAudiences);
        Assert.Equal(tokenAudiences.Length, model.TokenAudiences.Count);
        Assert.Contains("api://audience-1", model.TokenAudiences);
        Assert.Contains("api://audience-2", model.TokenAudiences);
        Assert.True(model.UseSession);
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var authenticationMethods = new List<APIGateway.Models.Oidc_authenticationMethods?>
        {
            APIGateway.Models.Oidc_authenticationMethods.AccessToken
        };
        var scopes = new List<string> { "openid", "profile" };
        var tokenAudiences = new List<string> { "api://audience" };
        var model = new APIGateway.Models.Oidc
        {
            AuthenticationMethods = authenticationMethods,
            ClientId = "client-id",
            ClientSecret = "client-secret",
            HideCredentials = true,
            Issuer = "https://issuer.example.com",
            Name = "oidc-auth",
            Scopes = scopes,
            TokenAudiences = tokenAudiences,
            UseSession = true,
        };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteCollectionOfEnumValues("authenticationMethods", authenticationMethods);
        writer.Received(1).WriteStringValue("clientId", "client-id");
        writer.Received(1).WriteStringValue("clientSecret", "client-secret");
        writer.Received(1).WriteBoolValue("hideCredentials", true);
        writer.Received(1).WriteStringValue("issuer", "https://issuer.example.com");
        writer.Received(1).WriteStringValue("name", "oidc-auth");
        writer.Received(1).WriteCollectionOfPrimitiveValues("scopes", scopes);
        writer.Received(1).WriteCollectionOfPrimitiveValues("tokenAudiences", tokenAudiences);
        writer.Received(1).WriteBoolValue("useSession", true);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Oidc_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new APIGateway.Models.Oidc
        {
            ClientId = "client-id",
            Name = "oidc-auth",
            Scopes = ["openid", "profile"],
            HideCredentials = true,
            UseSession = true,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(APIGateway.Models.Oidc.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("client-id", deserializedModel.ClientId);
        Assert.Equal("oidc-auth", deserializedModel.Name);
        Assert.Contains("openid", deserializedModel.Scopes);
        Assert.True(deserializedModel.HideCredentials);
        Assert.True(deserializedModel.UseSession);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Oidc_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new APIGateway.Models.Oidc
        {
            ClientId = "client-id",
            Name = "oidc-auth",
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
        var deserializedModel = parseNode.GetObjectValue(APIGateway.Models.Oidc.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
