using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using System.Text;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// UserGroupDetailUpdate（Composed type wrapper）のテスト
/// </summary>
public class UserGroupDetailUpdateTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがidノードでMember1ラッパーを返すことを検証
    /// </summary>
    [Fact]
    public async Task CreateFromDiscriminatorValue_WithIdNode_ReturnsMember1Wrapper()
    {
        var parseNode = await CreateParseNodeAsync("""
        {
            "id": "9c9f6e6c-0d5f-4a0f-b4dd-bd7f8f17ce91",
            "isAssigned": true
        }
        """);

        var model = UserGroupDetailUpdate.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(model.UserGroupDetailUpdateMember1);
        Assert.Null(model.UserGroupDetailUpdateMember2);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnameノードでMember2ラッパーを返すことを検証
    /// </summary>
    [Fact]
    public async Task CreateFromDiscriminatorValue_WithNameNode_ReturnsMember2Wrapper()
    {
        var parseNode = await CreateParseNodeAsync("""
        {
            "name": "group-name",
            "isAssigned": false
        }
        """);

        var model = UserGroupDetailUpdate.CreateFromDiscriminatorValue(parseNode);

        Assert.Null(model.UserGroupDetailUpdateMember1);
        Assert.NotNull(model.UserGroupDetailUpdateMember2);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UserGroupDetailUpdate.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがアクティブなMember1に委譲することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_DelegatesToMember1WhenActive()
    {
        var model = new UserGroupDetailUpdate
        {
            UserGroupDetailUpdateMember1 = new UserGroupDetailUpdateMember1(),
        };

        var deserializers = model.GetFieldDeserializers();

        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("isAssigned"));
    }

    /// <summary>
    /// GetFieldDeserializersがアクティブなMember2に委譲することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_DelegatesToMember2WhenActive()
    {
        var model = new UserGroupDetailUpdate
        {
            UserGroupDetailUpdateMember2 = new UserGroupDetailUpdateMember2(),
        };

        var deserializers = model.GetFieldDeserializers();

        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("isAssigned"));
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// アクティブなメンバーがない場合に空の辞書を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_WithoutActiveMember_ReturnsEmptyDictionary()
    {
        var model = new UserGroupDetailUpdate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UserGroupDetailUpdate();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// SerializeがアクティブなMember1を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesMember1WhenActive()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var member = new UserGroupDetailUpdateMember1
        {
            Id = Guid.NewGuid(),
            IsAssigned = true,
        };
        var model = new UserGroupDetailUpdate
        {
            UserGroupDetailUpdateMember1 = member,
        };

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue(null, member);
    }

    /// <summary>
    /// SerializeがアクティブなMember2を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesMember2WhenActive()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var member = new UserGroupDetailUpdateMember2
        {
            IsAssigned = false,
            Name = "group-name",
        };
        var model = new UserGroupDetailUpdate
        {
            UserGroupDetailUpdateMember2 = member,
        };

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue(null, member);
    }

    /// <summary>
    /// アクティブなメンバーがない場合にSerializeが何も書き込まないことを検証
    /// </summary>
    [Fact]
    public void Serialize_WithoutActiveMember_WritesNothing()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var model = new UserGroupDetailUpdate();

        model.Serialize(writer);

        writer.DidNotReceive().WriteObjectValue(Arg.Any<string>(), Arg.Any<UserGroupDetailUpdateMember1>());
        writer.DidNotReceive().WriteObjectValue(Arg.Any<string>(), Arg.Any<UserGroupDetailUpdateMember2>());
    }

    private static async Task<IParseNode> CreateParseNodeAsync(string json)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNodeFactory = new JsonParseNodeFactory();
        return await parseNodeFactory.GetRootParseNodeAsync("application/json", stream);
    }
}
