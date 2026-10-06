using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// DomainDTOモデルのテスト
/// </summary>
public class DomainDTOTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new DomainDTO();
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
        var result = DomainDTO.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<DomainDTO>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => DomainDTO.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"domains"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new DomainDTO();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("domains"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new DomainDTO();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new DomainDTO();
        var deserializers = model.GetFieldDeserializers();
        var domains = new List<Domain>
        {
            new() { DomainName = "domain1.com" },
            new() { DomainName = "domain2.com" },
        };
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Domain>>()).Returns(domains);
        deserializers["domains"](parseNode);
        Assert.NotNull(model.Domains);
        Assert.Equal(2, model.Domains.Count);
        Assert.Equal("domain1.com", model.Domains[0].DomainName);
        Assert.Equal("domain2.com", model.Domains[1].DomainName);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task DomainDTO_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new DomainDTO
        {
            Domains =
            [
                new() { DomainName = "domain1.com" },
                new() { DomainName = "domain2.com" },
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
        var deserializedModel = parseNode.GetObjectValue(DomainDTO.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Domains);
        Assert.Equal(2, deserializedModel.Domains.Count);
        Assert.Equal("domain1.com", deserializedModel.Domains[0].DomainName);
        Assert.Equal("domain2.com", deserializedModel.Domains[1].DomainName);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task DomainDTO_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new DomainDTO
        {
            Domains =
            [
                new() { DomainName = "domain1.com" },
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
        var deserializedModel = parseNode.GetObjectValue(DomainDTO.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Domains);
        Assert.Single(deserializedModel.Domains);
        Assert.Equal("domain1.com", deserializedModel.Domains[0].DomainName);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
