using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ResponseReplaceDetail_jsonのテスト
/// </summary>
public class ResponseReplaceDetail_jsonTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseReplaceDetail_json.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseReplaceDetail_json>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseReplaceDetail_json.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"key", "value"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ResponseReplaceDetail_json();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("key"));
        Assert.True(deserializers.ContainsKey("value"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ResponseReplaceDetail_json();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ResponseReplaceDetail_json();
        var deserializers = model.GetFieldDeserializers();
        var keyNode = Substitute.For<IParseNode>();
        keyNode.GetStringValue().Returns("message");
        deserializers["key"](keyNode);
        var valueNode = Substitute.For<IParseNode>();
        valueNode.GetStringValue().Returns("updated");
        deserializers["value"](valueNode);
        Assert.Equal("message", model.Key);
        Assert.Equal("updated", model.Value);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseReplaceDetail_json_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ResponseReplaceDetail_json
        {
            Key = "message",
            Value = "updated",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ResponseReplaceDetail_json.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("message", deserializedModel.Key);
        Assert.Equal("updated", deserializedModel.Value);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseReplaceDetail_json_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ResponseReplaceDetail_json
        {
            Key = "message",
            Value = "updated",
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
        var deserializedModel = parseNode.GetObjectValue(ResponseReplaceDetail_json.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("message", deserializedModel.Key);
        Assert.Equal("updated", deserializedModel.Value);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
