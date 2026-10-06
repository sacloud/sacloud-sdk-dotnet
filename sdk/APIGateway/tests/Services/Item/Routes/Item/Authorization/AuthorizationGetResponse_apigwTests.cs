using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Authorization;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes.Item.Authorization;

/// <summary>
/// AuthorizationGetResponse_apigwのテスト
/// </summary>
public class AuthorizationGetResponse_apigwTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = AuthorizationGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<AuthorizationGetResponse_apigw>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => AuthorizationGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new AuthorizationGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("routeAuthorization"));
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new AuthorizationGetResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesRouteAuthorizationProperty()
    {
        var routeAuth = new RouteAuthorizationDetailResponse();
        var response = new AuthorizationGetResponse_apigw { RouteAuthorization = routeAuth };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("routeAuthorization", routeAuth);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void Serialize_WithNullRouteAuthorization_Works()
    {
        var response = new AuthorizationGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<RouteAuthorizationDetailResponse>("routeAuthorization", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void GetFieldDeserializers_RouteAuthorizationAction_WorksCorrectly()
    {
        var response = new AuthorizationGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var value = new RouteAuthorizationDetailResponse();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<RouteAuthorizationDetailResponse>>())
            .Returns(value);

        deserializers["routeAuthorization"](parseNode);

        Assert.Equal(value, response.RouteAuthorization);
    }

    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new AuthorizationGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new AuthorizationGetResponse_apigw();
        response.AdditionalData["key"] = "value";
        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
