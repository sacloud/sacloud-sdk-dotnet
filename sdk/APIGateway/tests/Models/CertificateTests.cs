using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Certificateモデルのテスト
/// </summary>
public class CertificateTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = Certificate.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<Certificate>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Certificate.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"createdAt", "id", "updatedAt", "ecdsa", "name", "rsa"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new Certificate();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("ecdsa"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("rsa"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Certificate();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new Certificate();
        var deserializers = model.GetFieldDeserializers();
        var testId = new Guid("12345678-1234-1234-1234-123456789012");
        var ecdsa = new CertificateDetails();
        var rsa = new CertificateDetails();

        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        deserializers["createdAt"](parseNodeCreatedAt);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetGuidValue().Returns(testId);
        deserializers["id"](parseNodeId);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero));
        deserializers["updatedAt"](parseNodeUpdatedAt);

        var ecdsaNode = Substitute.For<IParseNode>();
        ecdsaNode.GetObjectValue(Arg.Any<ParsableFactory<CertificateDetails>>()).Returns(ecdsa);
        deserializers["ecdsa"](ecdsaNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("test-cert");
        deserializers["name"](nameNode);

        var rsaNode = Substitute.For<IParseNode>();
        rsaNode.GetObjectValue(Arg.Any<ParsableFactory<CertificateDetails>>()).Returns(rsa);
        deserializers["rsa"](rsaNode);

        Assert.Equal(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), model.CreatedAt);
        Assert.Equal(testId, model.Id);
        Assert.Equal(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero), model.UpdatedAt);
        Assert.Same(ecdsa, model.Ecdsa);
        Assert.Equal("test-cert", model.Name);
        Assert.Same(rsa, model.Rsa);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Certificate_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Certificate
        {
            Name = "test-cert",
            Ecdsa = new CertificateDetails { Cert = "ecdsa-cert", Key = "ecdsa-key" },
            Rsa = new CertificateDetails { Cert = "rsa-cert", Key = "rsa-key" },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Certificate.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("test-cert", deserializedModel.Name);
        Assert.NotNull(deserializedModel.Ecdsa);
        Assert.Equal("ecdsa-cert", deserializedModel.Ecdsa.Cert);
        Assert.Equal("ecdsa-key", deserializedModel.Ecdsa.Key);
        Assert.NotNull(deserializedModel.Rsa);
        Assert.Equal("rsa-cert", deserializedModel.Rsa.Cert);
        Assert.Equal("rsa-key", deserializedModel.Rsa.Key);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Certificate_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Certificate
        {
            Name = "test-cert",
            Ecdsa = new CertificateDetails(),
            Rsa = new CertificateDetails(),
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
        var deserializedModel = parseNode.GetObjectValue(Certificate.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Ecdsa);
        Assert.NotNull(deserializedModel.Rsa);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
