using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Users;

namespace Sacloud.Sdk.APIGateway.Tests.Users;

/// <summary>
/// UsersPostResponseのテスト
/// </summary>
public class UsersPostResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = UsersPostResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UsersPostResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UsersPostResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがapigwキーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsApigwKey()
    {
        var response = new UsersPostResponse();
        var deserializers = response.GetFieldDeserializers();
        Assert.Contains("apigw", deserializers.Keys);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new UsersPostResponse();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new UsersPostResponse();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Apigwプロパティが正しく設定されることを検証
    /// </summary>
    [Fact]
    public void ApigwProperty_CanBeSet()
    {
        var response = new UsersPostResponse();
        var apigw = new UsersPostResponse_apigw { User = new UserDetail() };
        response.Apigw = apigw;
        Assert.Equal(apigw, response.Apigw);
    }

    /// <summary>
    /// Serializeがapigwプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesApigwProperty()
    {
        var apigw = new UsersPostResponse_apigw { User = new UserDetail() };
        var response = new UsersPostResponse { Apigw = apigw };
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
        var response = new UsersPostResponse();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue<UsersPostResponse_apigw>("apigw", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのApigwアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ApigwAction_WorksCorrectly()
    {
        var response = new UsersPostResponse();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigw = new UsersPostResponse_apigw { User = new UserDetail() };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<UsersPostResponse_apigw>>())
            .Returns(apigw);
        deserializers["apigw"](parseNode);
        Assert.Equal(apigw, response.Apigw);
    }
}
