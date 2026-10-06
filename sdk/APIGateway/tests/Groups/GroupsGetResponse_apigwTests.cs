using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Groups;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Groups;

/// <summary>
/// GroupsGetResponse_apigwのテスト
/// </summary>
public class GroupsGetResponse_apigwTests
{
    /// <summary>
    /// GetFieldDeserializersが"groups"キーを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsGroupsKey()
    {
        var response = new GroupsGetResponse_apigw();

        var deserializers = response.GetFieldDeserializers();

        Assert.Contains("groups", deserializers.Keys);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがGroupsGetResponse_apigwインスタンスを作成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsGroupsGetResponseApigw()
    {
        var parseNode = Substitute.For<IParseNode>();

        var response = GroupsGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(response);
        Assert.IsType<GroupsGetResponse_apigw>(response);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullパラメータで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupsGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new GroupsGetResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// SerializeとCreateFromDiscriminatorValueを通じたラウンドトリップが成功することを確認
    /// </summary>
    [Fact]
    public async Task Serialize_Roundtrip_Success()
    {
        var response = new GroupsGetResponse_apigw
        {
            Groups =
            [
                new Group { Name = "test-group" }
            ],
            AdditionalData = new Dictionary<string, object>
            {
                { "test-key", "test-value" }
            }
        };

        var json = """
        {
            "groups": [
                {
                    "id": "123e4567-e89b-12d3-a456-426614174000",
                    "name": "test-group",
                    "createdAt": "2024-01-01T00:00:00Z",
                    "updatedAt": "2024-01-01T00:00:00Z"
                }
            ]
        }
        """;
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var factory = new JsonParseNodeFactory();
        var parseNode = await factory.GetRootParseNodeAsync("application/json", stream);
        var deserialized = GroupsGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new GroupsGetResponse_apigw();

        Assert.NotNull(response.AdditionalData);
        Assert.IsType<Dictionary<string, object>>(response.AdditionalData);
    }

    /// <summary>
    /// Groupsプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void GroupsProperty_CanBeSet()
    {
        var response = new GroupsGetResponse_apigw();
        var groups = new List<Group>
        {
            new() { Name = "group-1" },
            new() { Name = "group-2" }
        };

        response.Groups = groups;

        Assert.Equal(groups, response.Groups);
        Assert.Equal(2, response.Groups.Count);
    }

    /// <summary>
    /// Serializeがgroupsプロパティを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesGroupsProperty()
    {
        var groups = new List<Group> { new() { Name = "test" } };
        var response = new GroupsGetResponse_apigw { Groups = groups };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues("groups", groups);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがnullプロパティの場合でも動作することを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullGroups_Works()
    {
        var response = new GroupsGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues<Group>("groups", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのgroupsアクションが機能することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_GroupsAction_WorksCorrectly()
    {
        var response = new GroupsGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var group1 = new Group { Name = "group-1" };
        var group2 = new Group { Name = "group-2" };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Group>>())
            .Returns([group1, group2]);

        deserializers["groups"](parseNode);

        Assert.NotNull(response.Groups);
        Assert.Equal(2, response.Groups.Count);
    }
}
