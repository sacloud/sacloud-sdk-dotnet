using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Certificates;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Certificates;

/// <summary>
/// CertificatesGetResponseモデルのテスト
/// </summary>
public class CertificatesGetResponseTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CertificatesGetResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CertificatesGetResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("apigw"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CertificatesGetResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CertificatesGetResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CertificatesGetResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CertificatesGetResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CertificatesGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CertificatesGetResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() =>
            CertificatesGetResponse.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CertificatesGetResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CertificatesGetResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var certificateDto = new CertificateDTO
        {
            Certificates = [new Certificate { Name = "test-cert" }]
        };
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<CertificateDTO>>()).Returns(certificateDto);
        deserializers["apigw"](parseNode);
        Assert.Equal(certificateDto, model.Apigw);
    }

    /// <summary>
    /// すべてのプロパティとAdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CertificatesGetResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CertificatesGetResponse
        {
            Apigw = new CertificateDTO
            {
                Certificates =
                [
                    new Certificate { Name = "test-cert" }
                ]
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
        var deserializedModel = parseNode.GetObjectValue(CertificatesGetResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Apigw);
        Assert.NotNull(deserializedModel.Apigw.Certificates);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extraKey"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void CertificatesGetResponse_AdditionalData_IsInitialized()
    {
        var model = new CertificatesGetResponse();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }
}
