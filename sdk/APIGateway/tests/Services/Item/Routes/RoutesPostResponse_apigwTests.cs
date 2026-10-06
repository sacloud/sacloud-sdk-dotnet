using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes;

/// <summary>
/// RoutesPostResponse_apigwのテスト
/// </summary>
public class RoutesPostResponse_apigwTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RoutesPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RoutesPostResponse_apigw>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RoutesPostResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new RoutesPostResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("route"));
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new RoutesPostResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesRouteProperty()
    {
        var route = new RouteDetail();
        var response = new RoutesPostResponse_apigw { Route = route };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("route", route);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void Serialize_WithNullRoute_Works()
    {
        var response = new RoutesPostResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<RouteDetail>("route", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void GetFieldDeserializers_RouteAction_WorksCorrectly()
    {
        var response = new RoutesPostResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var routeValue = new RouteDetail();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<RouteDetail>>())
            .Returns(routeValue);

        deserializers["route"](parseNode);

        Assert.Equal(routeValue, response.Route);
    }

    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new RoutesPostResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new RoutesPostResponse_apigw();
        response.AdditionalData["key"] = "value";
        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
