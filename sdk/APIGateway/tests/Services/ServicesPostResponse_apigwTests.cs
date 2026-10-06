using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services;

namespace Sacloud.Sdk.APIGateway.Tests.Services;

/// <summary>
/// ServicesPostResponse_apigwのテスト
/// </summary>
public class ServicesPostResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ServicesPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicesPostResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServicesPostResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new ServicesPostResponse_apigw();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("service"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new ServicesPostResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Serializeがserviceプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesServiceProperty()
    {
        var service = new ServiceDetailRequest();
        var response = new ServicesPostResponse_apigw { Service = service };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("service", service);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがserviceがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullService_Works()
    {
        var response = new ServicesPostResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<ServiceDetailRequest>("service", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのServiceアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ServiceAction_WorksCorrectly()
    {
        var response = new ServicesPostResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var serviceValue = new ServiceDetailRequest();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<ServiceDetailRequest>>())
            .Returns(serviceValue);

        deserializers["service"](parseNode);

        Assert.Equal(serviceValue, response.Service);
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new ServicesPostResponse_apigw();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataに値を追加できることを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new ServicesPostResponse_apigw();
        response.AdditionalData["key"] = "value";

        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
