using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// CertificateDTOモデルのテスト
/// </summary>
public class CertificateDTOTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new CertificateDTO();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CertificateDTO.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CertificateDTO>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CertificateDTO.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"certificates"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new CertificateDTO();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("certificates"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CertificateDTO();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new CertificateDTO();
        var deserializers = model.GetFieldDeserializers();
        var certificates = new List<Certificate>
        {
            new() { Name = "cert1" },
            new() { Name = "cert2" },
        };
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Certificate>>()).Returns(certificates);
        deserializers["certificates"](parseNode);
        Assert.NotNull(model.Certificates);
        Assert.Equal(2, model.Certificates.Count);
        Assert.Equal("cert1", model.Certificates[0].Name);
        Assert.Equal("cert2", model.Certificates[1].Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CertificateDTO_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CertificateDTO
        {
            Certificates =
            [
                new() { Name = "cert1" },
                new() { Name = "cert2" },
            ],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CertificateDTO.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Certificates);
        Assert.Equal(2, deserializedModel.Certificates.Count);
        Assert.Equal("cert1", deserializedModel.Certificates[0].Name);
        Assert.Equal("cert2", deserializedModel.Certificates[1].Name);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CertificateDTO_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CertificateDTO
        {
            Certificates =
            [
                new() { Name = "cert1" },
            ],
        };
        originalModel.AdditionalData["customField"] = "customValue";
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CertificateDTO.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Certificates);
        Assert.Single(deserializedModel.Certificates);
        Assert.Equal("cert1", deserializedModel.Certificates[0].Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
