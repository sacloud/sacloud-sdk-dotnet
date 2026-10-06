using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Services.Item.Routes.Item.Response;

namespace Sacloud.Sdk.APIGateway.Tests.Services.Item.Routes.Item.Response;

/// <summary>
/// ResponseGetResponseのテスト
/// </summary>
public class ResponseGetResponseTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseGetResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseGetResponse>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseGetResponse.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new ResponseGetResponse();
        var deserializers = response.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("apigw"));
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new ResponseGetResponse();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesApigwProperty()
    {
        var apigw = new ResponseGetResponse_apigw();
        var response = new ResponseGetResponse { Apigw = apigw };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("apigw", apigw);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void Serialize_WithNullApigw_Works()
    {
        var response = new ResponseGetResponse();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<ResponseGetResponse_apigw>("apigw", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void GetFieldDeserializers_ApigwAction_WorksCorrectly()
    {
        var response = new ResponseGetResponse();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigwValue = new ResponseGetResponse_apigw();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<ResponseGetResponse_apigw>>())
            .Returns(apigwValue);

        deserializers["apigw"](parseNode);

        Assert.Equal(apigwValue, response.Apigw);
    }

    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new ResponseGetResponse();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new ResponseGetResponse();
        response.AdditionalData["key"] = "value";
        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
