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
        var getCertificates = new GetCertificates
        {
            AutoIssueLimit = 10,
            Certificates = [new GetCertificates_certificates { Name = "test-cert" }]
        };
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<GetCertificates>>()).Returns(getCertificates);
        deserializers["apigw"](parseNode);
        Assert.Equal(getCertificates, model.Apigw);
    }

    /// <summary>
    /// すべてのプロパティとAdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CertificatesGetResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CertificatesGetResponse
        {
            Apigw = new GetCertificates
            {
                AutoIssueLimit = 10,
                Certificates =
                [
                    new GetCertificates_certificates
                    {
                        Name = "test-cert",
                        Sans = ["www.test-cert.example"],
                        Type = GetCertificates_certificates_type.Auto,
                        Status = GetCertificates_certificates_status.Valid,
                        StatusMessage = "valid"
                    }
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
        Assert.Equal(10, deserializedModel.Apigw.AutoIssueLimit);
        Assert.NotNull(deserializedModel.Apigw.Certificates);
        Assert.Single(deserializedModel.Apigw.Certificates);
        Assert.Equal("test-cert", deserializedModel.Apigw.Certificates[0].Name);
        Assert.Equal(["www.test-cert.example"], deserializedModel.Apigw.Certificates[0].Sans);
        Assert.Equal(GetCertificates_certificates_type.Auto, deserializedModel.Apigw.Certificates[0].Type);
        Assert.Equal(GetCertificates_certificates_status.Valid, deserializedModel.Apigw.Certificates[0].Status);
        Assert.Equal("valid", deserializedModel.Apigw.Certificates[0].StatusMessage);
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
