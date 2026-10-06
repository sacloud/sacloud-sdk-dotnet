using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Users;

namespace Sacloud.Sdk.APIGateway.Tests.Users;

/// <summary>
/// UsersGetResponse_apigwのテスト
/// </summary>
public class UsersGetResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = UsersGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UsersGetResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UsersGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがusersキーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsUsersKey()
    {
        var response = new UsersGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.Contains("users", deserializers.Keys);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new UsersGetResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new UsersGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Usersプロパティが正しく設定されることを検証
    /// </summary>
    [Fact]
    public void UsersProperty_CanBeSet()
    {
        var response = new UsersGetResponse_apigw();
        response.Users =
        [
            new(),
            new()
        ];
        Assert.NotNull(response.Users);
        Assert.Equal(2, response.Users.Count);
    }

    /// <summary>
    /// Serializeがusersプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesUsersProperty()
    {
        var users = new List<User>
        {
            new(),
            new()
        };
        var response = new UsersGetResponse_apigw { Users = users };
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteCollectionOfObjectValues("users", users);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがusersがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullUsers_Works()
    {
        var response = new UsersGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteCollectionOfObjectValues<User>("users", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのUsersアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_UsersAction_WorksCorrectly()
    {
        var response = new UsersGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var user1 = new User { Name = "user1" };
        var user2 = new User { Name = "user2" };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<User>>())
            .Returns([user1, user2]);
        deserializers["users"](parseNode);
        Assert.NotNull(response.Users);
        Assert.Equal(2, response.Users.Count);
    }
}
