using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RouteAuthorizationDetail（Composed type wrapper）のテスト
/// </summary>
public class RouteAuthorizationDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがfalseフラグでMember1ラッパーを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithFalseFlag_ReturnsMember1Wrapper()
    {
        var flagNode = Substitute.For<IParseNode>();
        flagNode.GetBoolValue().Returns(false);
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetChildNode("isACLEnabled").Returns(flagNode);
        var model = RouteAuthorizationDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(model.RouteAuthorizationDetailMember1);
        Assert.Null(model.RouteAuthorizationDetailMember2);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがtrueフラグでMember2ラッパーを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithTrueFlag_ReturnsMember2Wrapper()
    {
        var flagNode = Substitute.For<IParseNode>();
        flagNode.GetBoolValue().Returns(true);
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetChildNode("isACLEnabled").Returns(flagNode);
        var model = RouteAuthorizationDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.Null(model.RouteAuthorizationDetailMember1);
        Assert.NotNull(model.RouteAuthorizationDetailMember2);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RouteAuthorizationDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersがアクティブなMember1に委譲することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_DelegatesToMember1WhenActive()
    {
        var model = new RouteAuthorizationDetail
        {
            RouteAuthorizationDetailMember1 = new RouteAuthorizationDetailMember1(),
        };
        var deserializers = model.GetFieldDeserializers();
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("isACLEnabled"));
    }

    /// <summary>
    /// GetFieldDeserializersがアクティブなMember2に委譲することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_DelegatesToMember2WhenActive()
    {
        var model = new RouteAuthorizationDetail
        {
            RouteAuthorizationDetailMember2 = new RouteAuthorizationDetailMember2(),
        };
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("groups"));
        Assert.True(deserializers.ContainsKey("isACLEnabled"));
    }

    /// <summary>
    /// アクティブなメンバーがない場合に空の辞書を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_WithoutActiveMember_ReturnsEmptyDictionary()
    {
        var model = new RouteAuthorizationDetail();
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
        var model = new RouteAuthorizationDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// SerializeがアクティブなMember1を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesMember1WhenActive()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var member = new RouteAuthorizationDetailMember1 { IsACLEnabled = false };
        var model = new RouteAuthorizationDetail
        {
            RouteAuthorizationDetailMember1 = member,
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
        var member = new RouteAuthorizationDetailMember2 { IsACLEnabled = true };
        var model = new RouteAuthorizationDetail
        {
            RouteAuthorizationDetailMember2 = member,
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
        var model = new RouteAuthorizationDetail();
        model.Serialize(writer);
        writer.DidNotReceive().WriteObjectValue(Arg.Any<string>(), Arg.Any<RouteAuthorizationDetailMember1>());
        writer.DidNotReceive().WriteObjectValue(Arg.Any<string>(), Arg.Any<RouteAuthorizationDetailMember2>());
    }
}
