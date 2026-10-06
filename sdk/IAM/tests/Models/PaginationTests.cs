using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using PaginationModel = Sacloud.Sdk.IAM.Models.Pagination;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// Paginationのテスト
/// </summary>
public class PaginationTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = PaginationModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<PaginationModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.Pagination.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"count", "next", "previous"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var pagination = new PaginationModel();

        var deserializers = pagination.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("count"));
        Assert.True(deserializers.ContainsKey("next"));
        Assert.True(deserializers.ContainsKey("previous"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var pagination = new PaginationModel();

        Assert.Throws<ArgumentNullException>(() => pagination.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"count", "next", "previous"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var pagination = new PaginationModel();
        var deserializers = pagination.GetFieldDeserializers();
        
        var parseNodeCount = Substitute.For<IParseNode>();
        parseNodeCount.GetIntValue().Returns(100);
        deserializers["count"](parseNodeCount);
        
        var parseNodeNext = Substitute.For<IParseNode>();
        parseNodeNext.GetStringValue().Returns("https://example.com/next");
        deserializers["next"](parseNodeNext);

        var parseNodePrevious = Substitute.For<IParseNode>();
        parseNodePrevious.GetStringValue().Returns("https://example.com/previous");
        deserializers["previous"](parseNodePrevious);

        Assert.Equal(100, pagination.Count);
        Assert.Equal("https://example.com/next", pagination.Next);
        Assert.Equal("https://example.com/previous", pagination.Previous);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Pagination_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PaginationModel
        {
            Count = 100,
            Next = "https://example.com/next",
            Previous = "https://example.com/previous"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Pagination.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Count, deserializedModel.Count);
        Assert.Equal(originalModel.Next, deserializedModel.Next);
        Assert.Equal(originalModel.Previous, deserializedModel.Previous);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Pagination_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PaginationModel
        {
            Count = 100,
            Next = "https://example.com/next",
            Previous = "https://example.com/previous"
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Pagination.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
