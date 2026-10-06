using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Certificates;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Certificates;

/// <summary>
/// CertificatesPostResponse_apigwモデルのテスト
/// </summary>
public class CertificatesPostResponse_apigwTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_apigw_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CertificatesPostResponse_apigw();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("certificate"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_apigw_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CertificatesPostResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_apigw_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CertificatesPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CertificatesPostResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_apigw_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() =>
            CertificatesPostResponse_apigw.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_apigw_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CertificatesPostResponse_apigw();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var certificate = new Certificate { Name = "test-cert" };
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<Certificate>>()).Returns(certificate);
        deserializers["certificate"](parseNode);
        Assert.Equal(certificate, model.Certificate);
    }

    /// <summary>
    /// すべてのプロパティとAdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CertificatesPostResponse_apigw_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CertificatesPostResponse_apigw
        {
            Certificate = new Certificate
            {
                Name = "test-cert"
            },
            AdditionalData = new Dictionary<string, object>
            {
                { "extraKey", "extraValue" }
            }
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CertificatesPostResponse_apigw.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Certificate);
        Assert.Equal(originalModel.Certificate.Name, deserializedModel.Certificate.Name);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extraKey"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_apigw_AdditionalData_IsInitialized()
    {
        var model = new CertificatesPostResponse_apigw();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }
}
