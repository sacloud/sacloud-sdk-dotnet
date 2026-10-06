using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Compat.Users;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users;

/// <summary>
/// UsersGetResponseの単体テスト
/// </summary>
public class UsersGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正常にインスタンスを作成することを確認
    /// </summary>
    [Fact]
    public void UsersGetResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UsersGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UsersGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull引数で例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UsersGetResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UsersGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが基底クラスのキーを含むすべてのキーを返すことを確認
    /// </summary>
    [Fact]
    public void UsersGetResponse_GetFieldDeserializers_ReturnsAllKeys()
    {
        var response = new UsersGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.Contains("count", deserializers.Keys);
        Assert.Contains("next", deserializers.Keys);
        Assert.Contains("previous", deserializers.Keys);
        Assert.Contains("items", deserializers.Keys);
    }

    /// <summary>
    /// Serializeが正常に動作することを確認
    /// </summary>
    [Fact]
    public void UsersGetResponse_Serialize_WritesAllProperties()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var user1 = new User { Name = "Test User 1", Description = "User 1" };
        var user2 = new User { Name = "Test User 2", Description = "User 2" };
        var response = new UsersGetResponse
        {
            Count = 10,
            Next = "https://api.example.com/users?page=2",
            Previous = "https://api.example.com/users?page=0",
            Items = [user1, user2]
        };

        response.Serialize(writer);

        writer.Received(1).WriteIntValue("count", 10);
        writer.Received(1).WriteStringValue("next", "https://api.example.com/users?page=2");
        writer.Received(1).WriteStringValue("previous", "https://api.example.com/users?page=0");
        writer.Received(1).WriteCollectionOfObjectValues("items", response.Items);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UsersGetResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new UsersGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// SerializeDeserializeラウンドトリップが正常に動作することを確認
    /// </summary>
    [Fact]
    public void UsersGetResponse_SerializeDeserialize_RoundTrip()
    {
        var user = new User { Name = "Alice", Description = "Test user" };
        var originalResponse = new UsersGetResponse
        {
            Count = 5,
            Next = "https://api.example.com/users?page=3",
            Previous = "https://api.example.com/users?page=1",
            Items = [user]
        };
        var writer = Substitute.For<ISerializationWriter>();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetIntValue().Returns(5);
        parseNode.GetStringValue().Returns("https://api.example.com/users?page=3", "https://api.example.com/users?page=1");
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<User>>())
            .Returns(new List<User> { user });

        originalResponse.Serialize(writer);
        var newResponse = new UsersGetResponse();
        var deserializers = newResponse.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
    }

    /// <summary>
    /// AdditionalDataが正常に動作することを確認
    /// </summary>
    [Fact]
    public void UsersGetResponse_AdditionalData_WorksCorrectly()
    {
        var response = new UsersGetResponse();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);

        response.AdditionalData["customKey"] = "customValue";
        Assert.Single(response.AdditionalData);
        Assert.Equal("customValue", response.AdditionalData["customKey"]);
    }
}
