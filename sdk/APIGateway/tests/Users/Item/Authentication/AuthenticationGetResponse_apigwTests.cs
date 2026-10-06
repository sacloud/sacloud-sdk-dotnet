using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Users.Item.Authentication;

namespace Sacloud.Sdk.APIGateway.Tests.Users.Item.Authentication;

/// <summary>
/// AuthenticationGetResponse_apigwのテスト
/// </summary>
public class AuthenticationGetResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = AuthenticationGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<AuthenticationGetResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => AuthenticationGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがuserAuthenticationキーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsUserAuthenticationKey()
    {
        var response = new AuthenticationGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.Contains("userAuthentication", deserializers.Keys);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new AuthenticationGetResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new AuthenticationGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// UserAuthenticationプロパティが正しく設定されることを検証
    /// </summary>
    [Fact]
    public void UserAuthenticationProperty_CanBeSet()
    {
        var response = new AuthenticationGetResponse_apigw();
        var userAuthentication = new UserAuthentication();
        response.UserAuthentication = userAuthentication;
        Assert.Equal(userAuthentication, response.UserAuthentication);
    }

    /// <summary>
    /// SerializeがuserAuthenticationプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesUserAuthenticationProperty()
    {
        var userAuthentication = new UserAuthentication { BasicAuth = new BasicAuth() };
        var response = new AuthenticationGetResponse_apigw { UserAuthentication = userAuthentication };
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue("userAuthentication", userAuthentication);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// SerializeがuserAuthenticationがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullUserAuthentication_Works()
    {
        var response = new AuthenticationGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue<UserAuthentication>("userAuthentication", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのUserAuthenticationアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_UserAuthenticationAction_WorksCorrectly()
    {
        var response = new AuthenticationGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var userAuthentication = new UserAuthentication { BasicAuth = new BasicAuth() };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<UserAuthentication>>())
            .Returns(userAuthentication);
        deserializers["userAuthentication"](parseNode);
        Assert.Equal(userAuthentication, response.UserAuthentication);
    }
}
