using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RequestRenameDetail_queryParamsのテスト
/// </summary>
public class RequestRenameDetail_queryParamsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RequestRenameDetail_queryParams.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RequestRenameDetail_queryParams>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RequestRenameDetail_queryParams.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"from", "to"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RequestRenameDetail_queryParams();
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
        var model = new RequestRenameDetail_queryParams();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RequestRenameDetail_queryParams();
        var deserializers = model.GetFieldDeserializers();
        var fromNode = Substitute.For<IParseNode>();
        fromNode.GetStringValue().Returns("page");
        deserializers["from"](fromNode);
        var toNode = Substitute.For<IParseNode>();
        toNode.GetStringValue().Returns("pageNumber");
        deserializers["to"](toNode);
        Assert.Equal("page", model.From);
        Assert.Equal("pageNumber", model.To);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestRenameDetail_queryParams_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RequestRenameDetail_queryParams
        {
            From = "page",
            To = "pageNumber",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RequestRenameDetail_queryParams.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("page", deserializedModel.From);
        Assert.Equal("pageNumber", deserializedModel.To);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestRenameDetail_queryParams_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RequestRenameDetail_queryParams
        {
            From = "page",
            To = "pageNumber",
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
        var deserializedModel = parseNode.GetObjectValue(RequestRenameDetail_queryParams.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("page", deserializedModel.From);
        Assert.Equal("pageNumber", deserializedModel.To);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
