using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ResponseRenameDetail_jsonのテスト
/// </summary>
public class ResponseRenameDetail_jsonTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseRenameDetail_json.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseRenameDetail_json>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseRenameDetail_json.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"from", "to"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ResponseRenameDetail_json();
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
        var model = new ResponseRenameDetail_json();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ResponseRenameDetail_json();
        var deserializers = model.GetFieldDeserializers();
        var fromNode = Substitute.For<IParseNode>();
        fromNode.GetStringValue().Returns("oldKey");
        deserializers["from"](fromNode);
        var toNode = Substitute.For<IParseNode>();
        toNode.GetStringValue().Returns("newKey");
        deserializers["to"](toNode);
        Assert.Equal("oldKey", model.From);
        Assert.Equal("newKey", model.To);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseRenameDetail_json_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ResponseRenameDetail_json
        {
            From = "oldKey",
            To = "newKey",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ResponseRenameDetail_json.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("oldKey", deserializedModel.From);
        Assert.Equal("newKey", deserializedModel.To);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseRenameDetail_json_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ResponseRenameDetail_json
        {
            From = "oldKey",
            To = "newKey",
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
        var deserializedModel = parseNode.GetObjectValue(ResponseRenameDetail_json.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("oldKey", deserializedModel.From);
        Assert.Equal("newKey", deserializedModel.To);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
