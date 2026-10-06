using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using ServicePrincipalOAuth2AccessTokenModel = Sacloud.Sdk.IAM.Models.ServicePrincipalOAuth2AccessToken;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalOAuth2AccessTokenのテスト
/// </summary>
public class ServicePrincipalOAuth2AccessTokenTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ServicePrincipalOAuth2AccessTokenModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicePrincipalOAuth2AccessTokenModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServicePrincipalOAuth2AccessTokenModel.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ServicePrincipalOAuth2AccessTokenModel();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("access_token"));
        Assert.True(deserializers.ContainsKey("expires_in"));
        Assert.True(deserializers.ContainsKey("token_expired_at"));
        Assert.True(deserializers.ContainsKey("token_type"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServicePrincipalOAuth2AccessTokenModel();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ServicePrincipalOAuth2AccessTokenModel();
        var deserializers = model.GetFieldDeserializers();
        
        var parseNodeAccessToken = Substitute.For<IParseNode>();
        parseNodeAccessToken.GetStringValue().Returns("test-access-token-123");
        deserializers["access_token"](parseNodeAccessToken);
        
        var parseNodeExpiresIn = Substitute.For<IParseNode>();
        parseNodeExpiresIn.GetIntValue().Returns(3600);
        deserializers["expires_in"](parseNodeExpiresIn);
        
        var testExpiredAt = DateTimeOffset.Parse("2024-01-01T12:00:00Z");
        var parseNodeTokenExpiredAt = Substitute.For<IParseNode>();
        parseNodeTokenExpiredAt.GetDateTimeOffsetValue().Returns(testExpiredAt);
        deserializers["token_expired_at"](parseNodeTokenExpiredAt);
        
        var parseNodeTokenType = Substitute.For<IParseNode>();
        parseNodeTokenType.GetStringValue().Returns("Bearer");
        deserializers["token_type"](parseNodeTokenType);

        Assert.Equal("test-access-token-123", model.AccessToken);
        Assert.Equal(3600, model.ExpiresIn);
        Assert.Equal(testExpiredAt, model.TokenExpiredAt);
        Assert.Equal("Bearer", model.TokenType);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipalOAuth2AccessToken_CanSerializeAndDeserializeWithAllProperties()
    {
        var testExpiredAt = DateTimeOffset.Parse("2024-01-01T12:00:00Z");
        var originalModel = new ServicePrincipalOAuth2AccessTokenModel
        {
            AccessToken = "test-access-token-123",
            ExpiresIn = 3600,
            TokenExpiredAt = testExpiredAt,
            TokenType = "Bearer"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalOAuth2AccessTokenModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccessToken, deserializedModel.AccessToken);
        Assert.Equal(originalModel.ExpiresIn, deserializedModel.ExpiresIn);
        Assert.Equal(originalModel.TokenExpiredAt, deserializedModel.TokenExpiredAt);
        Assert.Equal(originalModel.TokenType, deserializedModel.TokenType);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipalOAuth2AccessToken_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ServicePrincipalOAuth2AccessTokenModel
        {
            AccessToken = "test-access-token-123"
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
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalOAuth2AccessTokenModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccessToken, deserializedModel.AccessToken);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
