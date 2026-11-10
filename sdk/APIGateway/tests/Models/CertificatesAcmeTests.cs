using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ACME証明書モデルのテスト
/// </summary>
public class CertificatesAcmeTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new CertificatesAcme();

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

        var result = CertificatesAcme.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CertificatesAcme>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CertificatesAcme.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"name", "sans"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new CertificatesAcme();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("sans"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CertificatesAcme();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// Sansプロパティに要素の一意性を検証する属性が付与されていることを検証
    /// </summary>
    [Fact]
    public void Sans_HasLocalizedUniqueArrayAttribute()
    {
        var property = typeof(CertificatesAcme).GetProperty(nameof(CertificatesAcme.Sans));

        Assert.NotNull(property?.GetCustomAttribute<LocalizedUniqueArrayAttribute>());
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new CertificatesAcme();
        var deserializers = model.GetFieldDeserializers();

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("example.com");
        deserializers["name"](nameNode);

        var sansNode = Substitute.For<IParseNode>();
        sansNode.GetCollectionOfPrimitiveValues<string>().Returns(["www.example.com", "api.example.com"]);
        deserializers["sans"](sansNode);

        Assert.Equal("example.com", model.Name);
        Assert.NotNull(model.Sans);
        Assert.Equal(2, model.Sans.Count);
        Assert.Equal("www.example.com", model.Sans[0]);
        Assert.Equal("api.example.com", model.Sans[1]);
    }

    /// <summary>
    /// Serializeが各プロパティを正しく書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var model = new CertificatesAcme
        {
            Name = "example.com",
            Sans = ["www.example.com", "api.example.com"]
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "example.com");
        writer.Received(1).WriteCollectionOfPrimitiveValues("sans", model.Sans);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CertificatesAcme_CanSerializeAndDeserializeAllProperties()
    {
        var originalModel = new CertificatesAcme
        {
            Name = "example.com",
            Sans = ["www.example.com", "api.example.com"]
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CertificatesAcme.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("example.com", deserializedModel.Name);
        Assert.Equal(["www.example.com", "api.example.com"], deserializedModel.Sans);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CertificatesAcme_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CertificatesAcme
        {
            Name = "example.com",
            Sans = ["www.example.com", "api.example.com"]
        };
        originalModel.AdditionalData["customField"] = "customValue";

        var deserializedModel = await SerializeAndDeserializeAsync(originalModel, CertificatesAcme.CreateFromDiscriminatorValue);

        Assert.Equal("example.com", deserializedModel.Name);
        Assert.Equal(["www.example.com", "api.example.com"], deserializedModel.Sans);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    private static async Task<T> SerializeAndDeserializeAsync<T>(T model, Microsoft.Kiota.Abstractions.Serialization.ParsableFactory<T> factory)
        where T : Microsoft.Kiota.Abstractions.Serialization.IParsable
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
