using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Plans;

namespace Sacloud.Sdk.APIGateway.Tests.Plans;

/// <summary>
/// PlansGetResponseのテスト
/// </summary>
public class PlansGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = PlansGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<PlansGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PlansGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new PlansGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("apigw"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new PlansGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Serializeがapigwプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesApigwProperty()
    {
        var apigwResponse = new PlansGetResponse_apigw();
        var response = new PlansGetResponse
        {
            Apigw = apigwResponse
        };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("apigw", apigwResponse);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがapigwがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullApigw_Works()
    {
        var response = new PlansGetResponse();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<PlansGetResponse_apigw>("apigw", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのApigwアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ApigwAction_WorksCorrectly()
    {
        var response = new PlansGetResponse();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigwValue = new PlansGetResponse_apigw();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<PlansGetResponse_apigw>>())
            .Returns(apigwValue);

        deserializers["apigw"](parseNode);

        Assert.Equal(apigwValue, response.Apigw);
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new PlansGetResponse();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataに値を追加できることを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new PlansGetResponse();
        var customKey = "customKey";
        var customValue = "customValue";

        response.AdditionalData[customKey] = customValue;

        Assert.True(response.AdditionalData.ContainsKey(customKey));
        Assert.Equal(customValue, response.AdditionalData[customKey]);
    }
}
