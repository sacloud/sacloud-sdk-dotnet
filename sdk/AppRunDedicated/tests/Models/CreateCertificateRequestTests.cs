using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// CreateCertificateRequestモデルのテスト
/// </summary>
public class CreateCertificateRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateCertificate_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateCertificateRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateCertificateRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateCertificate_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            CreateCertificateRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateCertificate_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateCertificateRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("certificatePem"));
        Assert.True(deserializers.ContainsKey("intermediateCertificatePem"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("privatekeyPem"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateCertificate_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateCertificateRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateCertificate_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateCertificateRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("cert-pem");
        deserializers["certificatePem"](parseNode);
        Assert.Equal("cert-pem", model.CertificatePem);

        parseNode.GetStringValue().Returns("intermediate-pem");
        deserializers["intermediateCertificatePem"](parseNode);
        Assert.Equal("intermediate-pem", model.IntermediateCertificatePem);

        parseNode.GetStringValue().Returns("test-cert");
        deserializers["name"](parseNode);
        Assert.Equal("test-cert", model.Name);

        parseNode.GetStringValue().Returns("private-key-pem");
        deserializers["privatekeyPem"](parseNode);
        Assert.Equal("private-key-pem", model.PrivatekeyPem);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateCertificate_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateCertificateRequest
        {
            CertificatePem = "cert-pem",
            IntermediateCertificatePem = "intermediate-pem",
            Name = "test-cert",
            PrivatekeyPem = "private-key-pem"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateCertificateRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CertificatePem, deserializedModel.CertificatePem);
        Assert.Equal(originalModel.IntermediateCertificatePem, deserializedModel.IntermediateCertificatePem);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.PrivatekeyPem, deserializedModel.PrivatekeyPem);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateCertificate_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateCertificateRequest
        {
            Name = "test-cert"
        };
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateCertificateRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
