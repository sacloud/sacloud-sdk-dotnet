using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ResponseModificationDetail_headersのテスト
/// </summary>
public class ResponseModificationDetail_headersTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseModificationDetail_headers.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseModificationDetail_headers>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseModificationDetail_headers.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"key", "value"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ResponseModificationDetail_headers();
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
        var model = new ResponseModificationDetail_headers();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ResponseModificationDetail_headers();
        var deserializers = model.GetFieldDeserializers();
        var keyNode = Substitute.For<IParseNode>();
        keyNode.GetStringValue().Returns("X-Response");
        deserializers["key"](keyNode);
        var valueNode = Substitute.For<IParseNode>();
        valueNode.GetStringValue().Returns("yes");
        deserializers["value"](valueNode);
        Assert.Equal("X-Response", model.Key);
        Assert.Equal("yes", model.Value);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseModificationDetail_headers_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ResponseModificationDetail_headers
        {
            Key = "X-Response",
            Value = "yes",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ResponseModificationDetail_headers.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("X-Response", deserializedModel.Key);
        Assert.Equal("yes", deserializedModel.Value);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseModificationDetail_headers_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ResponseModificationDetail_headers
        {
            Key = "X-Response",
            Value = "yes",
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
        var deserializedModel = parseNode.GetObjectValue(ResponseModificationDetail_headers.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("X-Response", deserializedModel.Key);
        Assert.Equal("yes", deserializedModel.Value);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
