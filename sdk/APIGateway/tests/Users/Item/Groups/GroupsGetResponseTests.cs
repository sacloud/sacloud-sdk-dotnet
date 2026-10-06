using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Users.Item.Groups;

namespace Sacloud.Sdk.APIGateway.Tests.Users.Item.Groups;

/// <summary>
/// GroupsGetResponseのテスト
/// </summary>
public class GroupsGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = GroupsGetResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<GroupsGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupsGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがapigwキーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsApigwKey()
    {
        var response = new GroupsGetResponse();
        var deserializers = response.GetFieldDeserializers();
        Assert.Contains("apigw", deserializers.Keys);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new GroupsGetResponse();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new GroupsGetResponse();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Apigwプロパティが正しく設定されることを検証
    /// </summary>
    [Fact]
    public void ApigwProperty_CanBeSet()
    {
        var response = new GroupsGetResponse();
        var apigw = new GroupsGetResponse_apigw { Groups = [] };
        response.Apigw = apigw;
        Assert.Equal(apigw, response.Apigw);
    }

    /// <summary>
    /// Serializeがapigwプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesApigwProperty()
    {
        var apigw = new GroupsGetResponse_apigw { Groups = [] };
        var response = new GroupsGetResponse { Apigw = apigw };
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue("apigw", apigw);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがapigwがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullApigw_Works()
    {
        var response = new GroupsGetResponse();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue<GroupsGetResponse_apigw>("apigw", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのApigwアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ApigwAction_WorksCorrectly()
    {
        var response = new GroupsGetResponse();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigw = new GroupsGetResponse_apigw { Groups = [] };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<GroupsGetResponse_apigw>>())
            .Returns(apigw);
        deserializers["apigw"](parseNode);
        Assert.Equal(apigw, response.Apigw);
    }
}
