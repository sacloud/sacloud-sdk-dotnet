using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// GetCertificates_certificatesモデルのテスト
/// </summary>
public class GetCertificates_certificatesTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = GetCertificates_certificates.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<GetCertificates_certificates>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GetCertificates_certificates.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new GetCertificates_certificates();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(10, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("ecdsa"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("rsa"));
        Assert.True(deserializers.ContainsKey("sans"));
        Assert.True(deserializers.ContainsKey("status"));
        Assert.True(deserializers.ContainsKey("statusMessage"));
        Assert.True(deserializers.ContainsKey("type"));
    }

    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new GetCertificates_certificates();
        var deserializers = model.GetFieldDeserializers();

        var sansNode = Substitute.For<IParseNode>();
        sansNode.GetCollectionOfPrimitiveValues<string>().Returns(["www.example.com", "api.example.com"]);
        deserializers["sans"](sansNode);

        var statusNode = Substitute.For<IParseNode>();
        statusNode.GetEnumValue<GetCertificates_certificates_status>().Returns(GetCertificates_certificates_status.Valid);
        deserializers["status"](statusNode);

        var statusMessageNode = Substitute.For<IParseNode>();
        statusMessageNode.GetStringValue().Returns("issued successfully");
        deserializers["statusMessage"](statusMessageNode);

        var typeNode = Substitute.For<IParseNode>();
        typeNode.GetEnumValue<GetCertificates_certificates_type>().Returns(GetCertificates_certificates_type.Auto);
        deserializers["type"](typeNode);

        Assert.NotNull(model.Sans);
        Assert.Equal(2, model.Sans.Count);
        Assert.Equal("www.example.com", model.Sans[0]);
        Assert.Equal("api.example.com", model.Sans[1]);
        Assert.Equal(GetCertificates_certificates_status.Valid, model.Status);
        Assert.Equal("issued successfully", model.StatusMessage);
        Assert.Equal(GetCertificates_certificates_type.Auto, model.Type);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new GetCertificates_certificates();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesValues()
    {
        var model = new GetCertificates_certificates
        {
            Name = "example.com",
            Sans = ["www.example.com", "api.example.com"],
            Status = GetCertificates_certificates_status.Valid,
            StatusMessage = "issued successfully",
            Type = GetCertificates_certificates_type.Auto,
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "example.com");
        writer.Received(1).WriteCollectionOfPrimitiveValues("sans", model.Sans);
        writer.Received(1).WriteEnumValue<GetCertificates_certificates_status>("status", GetCertificates_certificates_status.Valid);
        writer.Received(1).WriteStringValue("statusMessage", "issued successfully");
        writer.Received(1).WriteEnumValue<GetCertificates_certificates_type>("type", GetCertificates_certificates_type.Auto);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    [Fact]
    public async Task GetCertificates_certificates_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new GetCertificates_certificates
        {
            Name = "example.com",
            Sans = ["www.example.com", "api.example.com"],
            Status = GetCertificates_certificates_status.Valid,
            StatusMessage = "issued successfully",
            Type = GetCertificates_certificates_type.Auto,
        };

        var deserializedModel = await SerializeAndDeserializeAsync(originalModel, GetCertificates_certificates.CreateFromDiscriminatorValue);

        Assert.Equal("example.com", deserializedModel.Name);
        Assert.Equal(["www.example.com", "api.example.com"], deserializedModel.Sans);
        Assert.Equal(GetCertificates_certificates_status.Valid, deserializedModel.Status);
        Assert.Equal("issued successfully", deserializedModel.StatusMessage);
        Assert.Equal(GetCertificates_certificates_type.Auto, deserializedModel.Type);
    }

    [Fact]
    public async Task GetCertificates_certificates_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new GetCertificates_certificates
        {
            Name = "example.com",
            Sans = ["www.example.com", "api.example.com"],
            Status = GetCertificates_certificates_status.Valid,
            StatusMessage = "issued successfully",
            Type = GetCertificates_certificates_type.Auto,
        };
        originalModel.AdditionalData["customField"] = "customValue";

        var deserializedModel = await SerializeAndDeserializeAsync(originalModel, GetCertificates_certificates.CreateFromDiscriminatorValue);

        Assert.Equal("example.com", deserializedModel.Name);
        Assert.Equal(["www.example.com", "api.example.com"], deserializedModel.Sans);
        Assert.Equal(GetCertificates_certificates_status.Valid, deserializedModel.Status);
        Assert.Equal("issued successfully", deserializedModel.StatusMessage);
        Assert.Equal(GetCertificates_certificates_type.Auto, deserializedModel.Type);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    private static async Task<T> SerializeAndDeserializeAsync<T>(T model, ParsableFactory<T> factory)
        where T : IParsable
    {
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        writer.GetSerializedContent().CopyTo(serializeStream);
        serializeStream.Position = 0;

        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        return parseNode.GetObjectValue(factory)!;
    }
}