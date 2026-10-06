using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// CertificateDetailsのテスト
/// </summary>
public class CertificateDetailsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CertificateDetails.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CertificateDetails>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CertificateDetails.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"cert", "expiredAt", "key"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new CertificateDetails();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("cert"));
        Assert.True(deserializers.ContainsKey("expiredAt"));
        Assert.True(deserializers.ContainsKey("key"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CertificateDetails();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new CertificateDetails();
        var deserializers = model.GetFieldDeserializers();

        var parseNodeCert = Substitute.For<IParseNode>();
        parseNodeCert.GetStringValue().Returns("certificate-body");
        deserializers["cert"](parseNodeCert);

        var parseNodeExpiredAt = Substitute.For<IParseNode>();
        parseNodeExpiredAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero));
        deserializers["expiredAt"](parseNodeExpiredAt);

        var parseNodeKey = Substitute.For<IParseNode>();
        parseNodeKey.GetStringValue().Returns("private-key");
        deserializers["key"](parseNodeKey);

        Assert.Equal("certificate-body", model.Cert);
        Assert.Equal(new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero), model.ExpiredAt);
        Assert.Equal("private-key", model.Key);
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var model = new CertificateDetails
        {
            Cert = "certificate-body",
            Key = "private-key",
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("cert", "certificate-body");
        writer.Received(1).WriteStringValue("key", "private-key");
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CertificateDetails_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CertificateDetails
        {
            Cert = "certificate-body",
            Key = "private-key",
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CertificateDetails.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("certificate-body", deserializedModel.Cert);
        Assert.Equal("private-key", deserializedModel.Key);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CertificateDetails_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CertificateDetails
        {
            Cert = "certificate-body",
            Key = "private-key",
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CertificateDetails.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
