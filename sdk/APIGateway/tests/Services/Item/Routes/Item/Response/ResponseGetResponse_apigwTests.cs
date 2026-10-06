using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Response;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes.Item.Response;

/// <summary>
/// ResponseGetResponse_apigwのテスト
/// </summary>
public class ResponseGetResponse_apigwTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseGetResponse_apigw>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new ResponseGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("responseTransformation"));
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new ResponseGetResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesResponseTransformationProperty()
    {
        var rt = new ResponseTransformation();
        var response = new ResponseGetResponse_apigw { ResponseTransformation = rt };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("responseTransformation", rt);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void Serialize_WithNullResponseTransformation_Works()
    {
        var response = new ResponseGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<ResponseTransformation>("responseTransformation", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void GetFieldDeserializers_ResponseTransformationAction_WorksCorrectly()
    {
        var response = new ResponseGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var value = new ResponseTransformation();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<ResponseTransformation>>())
            .Returns(value);

        deserializers["responseTransformation"](parseNode);

        Assert.Equal(value, response.ResponseTransformation);
    }

    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new ResponseGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new ResponseGetResponse_apigw();
        response.AdditionalData["key"] = "value";
        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
