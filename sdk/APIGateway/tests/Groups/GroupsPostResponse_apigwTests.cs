using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Groups;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Groups;

/// <summary>
/// GroupsPostResponse_apigwのテスト
/// </summary>
public class GroupsPostResponse_apigwTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいデシリアライザーを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsDeserializers()
    {
        var response = new GroupsPostResponse_apigw();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがGroupsPostResponse_apigwインスタンスを作成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsGroupsPostResponseApigw()
    {
        var parseNode = Substitute.For<IParseNode>();

        var response = GroupsPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(response);
        Assert.IsType<GroupsPostResponse_apigw>(response);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullパラメータで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupsPostResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new GroupsPostResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// SerializeとCreateFromDiscriminatorValueを通じたラウンドトリップが成功することを確認
    /// </summary>
    [Fact]
    public async Task Serialize_Roundtrip_Success()
    {
        var response = new GroupsPostResponse_apigw
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "test-key", "test-value" }
            }
        };

        var json = """
        {
            "id": "123e4567-e89b-12d3-a456-426614174000",
            "name": "test-group",
            "createdAt": "2024-01-01T00:00:00Z",
            "updatedAt": "2024-01-01T00:00:00Z"
        }
        """;
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var factory = new JsonParseNodeFactory();
        var parseNode = await factory.GetRootParseNodeAsync("application/json", stream);
        var deserialized = GroupsPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new GroupsPostResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがgroupプロパティを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesGroupProperty()
    {
        var group = new Group { Name = "test-group" };
        var response = new GroupsPostResponse_apigw { Group = group };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("group", group);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのgroupアクションが機能することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_GroupAction_WorksCorrectly()
    {
        var response = new GroupsPostResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var group = new Group { Name = "test-group" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Group>>())
            .Returns(group);

        deserializers["group"](parseNode);

        Assert.Equal(group, response.Group);
    }

    /// <summary>
    /// Groupプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void GroupProperty_CanBeSet()
    {
        var response = new GroupsPostResponse_apigw();
        var group = new Group { Name = "test-group" };
        response.Group = group;
        Assert.Equal(group, response.Group);
        Assert.Equal("test-group", response.Group.Name);
    }

    /// <summary>
    /// Serializeがnullプロパティの場合でも動作することを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullGroup_Works()
    {
        var response = new GroupsPostResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue<Group>("group", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }
}
