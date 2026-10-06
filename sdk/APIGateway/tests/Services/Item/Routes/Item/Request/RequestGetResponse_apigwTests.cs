using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Request;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes.Item.Request;

/// <summary>
/// RequestGetResponse_apigwのテスト
/// </summary>
public class RequestGetResponse_apigwTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RequestGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RequestGetResponse_apigw>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RequestGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new RequestGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("requestTransformation"));
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new RequestGetResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesRequestTransformationProperty()
    {
        var rt = new RequestTransformation();
        var response = new RequestGetResponse_apigw { RequestTransformation = rt };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("requestTransformation", rt);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void Serialize_WithNullRequestTransformation_Works()
    {
        var response = new RequestGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<RequestTransformation>("requestTransformation", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void GetFieldDeserializers_RequestTransformationAction_WorksCorrectly()
    {
        var response = new RequestGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var value = new RequestTransformation();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<RequestTransformation>>())
            .Returns(value);

        deserializers["requestTransformation"](parseNode);

        Assert.Equal(value, response.RequestTransformation);
    }

    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new RequestGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new RequestGetResponse_apigw();
        response.AdditionalData["key"] = "value";
        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
