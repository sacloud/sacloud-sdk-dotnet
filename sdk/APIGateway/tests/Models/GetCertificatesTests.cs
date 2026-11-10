using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// GetCertificatesモデルのテスト
/// </summary>
public class GetCertificatesTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = GetCertificates.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<GetCertificates>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GetCertificates.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new GetCertificates();

        var deserializers = model.GetFieldDeserializers();

        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("autoIssueLimit"));
        Assert.True(deserializers.ContainsKey("certificates"));
    }

    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new GetCertificates();
        var deserializers = model.GetFieldDeserializers();

        var autoIssueLimitNode = Substitute.For<IParseNode>();
        autoIssueLimitNode.GetIntValue().Returns(10);
        deserializers["autoIssueLimit"](autoIssueLimitNode);

        var certificates = new List<GetCertificates_certificates>
        {
            new() { Name = "example.com" }
        };
        var certificatesNode = Substitute.For<IParseNode>();
        certificatesNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<GetCertificates_certificates>>()).Returns(certificates);
        deserializers["certificates"](certificatesNode);

        Assert.Equal(10, model.AutoIssueLimit);
        Assert.Equal(certificates, model.Certificates);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new GetCertificates();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesValues()
    {
        var certificates = new List<GetCertificates_certificates>
        {
            new() { Name = "example.com" }
        };
        var model = new GetCertificates
        {
            AutoIssueLimit = 10,
            Certificates = certificates
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteIntValue("autoIssueLimit", 10);
        writer.Received(1).WriteCollectionOfObjectValues("certificates", certificates);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    [Fact]
    public async Task GetCertificates_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new GetCertificates
        {
            AutoIssueLimit = 10,
            Certificates =
            [
                new GetCertificates_certificates { Name = "example.com" }
            ]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;

        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(GetCertificates.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(10, deserializedModel.AutoIssueLimit);
        Assert.NotNull(deserializedModel.Certificates);
        Assert.Single(deserializedModel.Certificates);
        Assert.Equal("example.com", deserializedModel.Certificates[0].Name);
    }

    [Fact]
    public async Task GetCertificates_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new GetCertificates
        {
            AutoIssueLimit = 10,
            Certificates =
            [
                new GetCertificates_certificates { Name = "example.com" }
            ]
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
        var deserializedModel = parseNode.GetObjectValue(GetCertificates.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
