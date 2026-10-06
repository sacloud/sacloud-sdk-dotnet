using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Users.Item.Groups;

namespace Sacloud.Sdk.APIGateway.Tests.Users.Item.Groups;

/// <summary>
/// GroupsGetResponse_apigwのテスト
/// </summary>
public class GroupsGetResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = GroupsGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<GroupsGetResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupsGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがgroupsキーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsGroupsKey()
    {
        var response = new GroupsGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.Contains("groups", deserializers.Keys);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new GroupsGetResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new GroupsGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Groupsプロパティが正しく設定されることを検証
    /// </summary>
    [Fact]
    public void GroupsProperty_CanBeSet()
    {
        var response = new GroupsGetResponse_apigw();
        response.Groups =
        [
            new(),
            new()
        ];
        Assert.NotNull(response.Groups);
        Assert.Equal(2, response.Groups.Count);
    }

    /// <summary>
    /// Serializeがgroupsプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesGroupsProperty()
    {
        var groups = new List<UserGroupDetail> { new(), new() };
        var response = new GroupsGetResponse_apigw { Groups = groups };
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteCollectionOfObjectValues("groups", groups);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがgroupsがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullGroups_Works()
    {
        var response = new GroupsGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteCollectionOfObjectValues<UserGroupDetail>("groups", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのGroupsアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_GroupsAction_WorksCorrectly()
    {
        var response = new GroupsGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var groups = new List<UserGroupDetail> { new(), new() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<UserGroupDetail>>())
            .Returns(groups);
        deserializers["groups"](parseNode);
        Assert.NotNull(response.Groups);
        Assert.Equal(2, response.Groups.Count);
    }
}
