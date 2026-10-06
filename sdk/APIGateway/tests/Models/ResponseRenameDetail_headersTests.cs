using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ResponseRenameDetail_headersのテスト
/// </summary>
public class ResponseRenameDetail_headersTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseRenameDetail_headers.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseRenameDetail_headers>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseRenameDetail_headers.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"from", "to"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ResponseRenameDetail_headers();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("from"));
        Assert.True(deserializers.ContainsKey("to"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ResponseRenameDetail_headers();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ResponseRenameDetail_headers();
        var deserializers = model.GetFieldDeserializers();
        var fromNode = Substitute.For<IParseNode>();
        fromNode.GetStringValue().Returns("X-Old");
        deserializers["from"](fromNode);
        var toNode = Substitute.For<IParseNode>();
        toNode.GetStringValue().Returns("X-New");
        deserializers["to"](toNode);
        Assert.Equal("X-Old", model.From);
        Assert.Equal("X-New", model.To);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseRenameDetail_headers_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ResponseRenameDetail_headers
        {
            From = "X-Old",
            To = "X-New",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ResponseRenameDetail_headers.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("X-Old", deserializedModel.From);
        Assert.Equal("X-New", deserializedModel.To);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseRenameDetail_headers_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ResponseRenameDetail_headers
        {
            From = "X-Old",
            To = "X-New",
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
        var deserializedModel = parseNode.GetObjectValue(ResponseRenameDetail_headers.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("X-Old", deserializedModel.From);
        Assert.Equal("X-New", deserializedModel.To);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
