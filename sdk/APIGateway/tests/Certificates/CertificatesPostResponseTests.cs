using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Certificates;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Certificates;

/// <summary>
/// CertificatesPostResponseモデルのテスト
/// </summary>
public class CertificatesPostResponseTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CertificatesPostResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("apigw"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CertificatesPostResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CertificatesPostResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CertificatesPostResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() =>
            CertificatesPostResponse.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CertificatesPostResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigw = new CertificatesPostResponse_apigw
        {
            Certificate = new Certificate { Name = "test-cert" }
        };
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<CertificatesPostResponse_apigw>>()).Returns(apigw);
        deserializers["apigw"](parseNode);
        Assert.Equal(apigw, model.Apigw);
    }

    /// <summary>
    /// すべてのプロパティとAdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CertificatesPostResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CertificatesPostResponse
        {
            Apigw = new CertificatesPostResponse_apigw
            {
                Certificate = new Certificate
                {
                    Name = "test-cert"
                }
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
        var deserializedModel = parseNode.GetObjectValue(CertificatesPostResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Apigw);
        Assert.NotNull(deserializedModel.Apigw.Certificate);
        Assert.Equal(originalModel.Apigw.Certificate.Name, deserializedModel.Apigw.Certificate.Name);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extraKey"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void CertificatesPostResponse_AdditionalData_IsInitialized()
    {
        var model = new CertificatesPostResponse();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }
}
