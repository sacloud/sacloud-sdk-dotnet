using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// UserAuthenticationのテスト
/// </summary>
public class UserAuthenticationTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new UserAuthentication();
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
        var result = UserAuthentication.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UserAuthentication>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UserAuthentication.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"basicAuth", "hmacAuth", "jwt"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new UserAuthentication();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("basicAuth"));
        Assert.True(deserializers.ContainsKey("hmacAuth"));
        Assert.True(deserializers.ContainsKey("jwt"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UserAuthentication();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new UserAuthentication();
        var deserializers = model.GetFieldDeserializers();
        var basicAuth = new BasicAuth { UserName = "basic-user", Password = "basic-pass" };
        var hmacAuth = new HmacAuth { UserName = "hmac-user", Secret = "hmac-secret" };
        var jwt = new Jwt { Algorithm = Jwt_algorithm.HS256, Key = "jwt-key", Secret = "jwt-secret" };

        var basicAuthNode = Substitute.For<IParseNode>();
        basicAuthNode.GetObjectValue(Arg.Any<ParsableFactory<BasicAuth>>()).Returns(basicAuth);
        deserializers["basicAuth"](basicAuthNode);

        var hmacAuthNode = Substitute.For<IParseNode>();
        hmacAuthNode.GetObjectValue(Arg.Any<ParsableFactory<HmacAuth>>()).Returns(hmacAuth);
        deserializers["hmacAuth"](hmacAuthNode);

        var jwtNode = Substitute.For<IParseNode>();
        jwtNode.GetObjectValue(Arg.Any<ParsableFactory<Jwt>>()).Returns(jwt);
        deserializers["jwt"](jwtNode);

        Assert.Same(basicAuth, model.BasicAuth);
        Assert.Same(hmacAuth, model.HmacAuth);
        Assert.Same(jwt, model.Jwt);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserAuthentication_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UserAuthentication
        {
            BasicAuth = new BasicAuth { UserName = "basic-user", Password = "basic-pass" },
            HmacAuth = new HmacAuth { UserName = "hmac-user", Secret = "hmac-secret" },
            Jwt = new Jwt { Algorithm = Jwt_algorithm.HS256, Key = "jwt-key", Secret = "jwt-secret" },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserAuthentication.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.BasicAuth);
        Assert.Equal(originalModel.BasicAuth.UserName, deserializedModel.BasicAuth.UserName);
        Assert.Equal(originalModel.BasicAuth.Password, deserializedModel.BasicAuth.Password);
        Assert.NotNull(deserializedModel.HmacAuth);
        Assert.Equal(originalModel.HmacAuth.UserName, deserializedModel.HmacAuth.UserName);
        Assert.Equal(originalModel.HmacAuth.Secret, deserializedModel.HmacAuth.Secret);
        Assert.NotNull(deserializedModel.Jwt);
        Assert.Equal(originalModel.Jwt.Algorithm, deserializedModel.Jwt.Algorithm);
        Assert.Equal(originalModel.Jwt.Key, deserializedModel.Jwt.Key);
        Assert.Equal(originalModel.Jwt.Secret, deserializedModel.Jwt.Secret);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserAuthentication_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UserAuthentication
        {
            BasicAuth = new BasicAuth { UserName = "basic-user", Password = "basic-pass" },
            HmacAuth = new HmacAuth { UserName = "hmac-user", Secret = "hmac-secret" },
            Jwt = new Jwt { Algorithm = Jwt_algorithm.HS256, Key = "jwt-key", Secret = "jwt-secret" },
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
        var deserializedModel = parseNode.GetObjectValue(UserAuthentication.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.BasicAuth);
        Assert.NotNull(deserializedModel.HmacAuth);
        Assert.NotNull(deserializedModel.Jwt);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
