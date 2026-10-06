using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Users;

namespace Sacloud.Sdk.APIGateway.Tests.Users;

/// <summary>
/// UsersPostResponse_apigwのテスト
/// </summary>
public class UsersPostResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = UsersPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UsersPostResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UsersPostResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがuserキーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsUserKey()
    {
        var response = new UsersPostResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.Contains("user", deserializers.Keys);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new UsersPostResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new UsersPostResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Userプロパティが正しく設定されることを検証
    /// </summary>
    [Fact]
    public void UserProperty_CanBeSet()
    {
        var response = new UsersPostResponse_apigw();
        var user = new UserDetail();
        response.User = user;
        Assert.Equal(user, response.User);
    }

    /// <summary>
    /// Serializeがuserプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesUserProperty()
    {
        var user = new UserDetail { Name = "test" };
        var response = new UsersPostResponse_apigw { User = user };
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue("user", user);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがuserがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullUser_Works()
    {
        var response = new UsersPostResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue<UserDetail>("user", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのUserアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_UserAction_WorksCorrectly()
    {
        var response = new UsersPostResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var user = new UserDetail { Name = "test" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<UserDetail>>())
            .Returns(user);
        deserializers["user"](parseNode);
        Assert.Equal(user, response.User);
    }
}
