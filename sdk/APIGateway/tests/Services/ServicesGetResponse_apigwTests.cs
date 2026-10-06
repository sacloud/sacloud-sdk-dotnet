using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Services;

namespace Sacloud.Sdk.APIGateway.Tests.Services;

/// <summary>
/// ServicesGetResponse_apigwのテスト
/// </summary>
public class ServicesGetResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ServicesGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicesGetResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServicesGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new ServicesGetResponse_apigw();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("services"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new ServicesGetResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Serializeがservicesプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesServicesProperty()
    {
        var services = new List<ServiceDetailResponse> { new() };
        var response = new ServicesGetResponse_apigw { Services = services };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues("services", services);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがservicesがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullServices_Works()
    {
        var response = new ServicesGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues<ServiceDetailResponse>("services", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのServicesアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ServicesAction_WorksCorrectly()
    {
        var response = new ServicesGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var servicesValue = new List<ServiceDetailResponse> { new() };
        parseNode.GetCollectionOfObjectValues(
            Arg.Any<ParsableFactory<ServiceDetailResponse>>())
            .Returns(servicesValue);

        deserializers["services"](parseNode);

        Assert.NotNull(response.Services);
        Assert.Equal(servicesValue.Count, response.Services.Count);
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new ServicesGetResponse_apigw();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataに値を追加できることを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new ServicesGetResponse_apigw();
        response.AdditionalData["key"] = "value";

        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
