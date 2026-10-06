using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RequestModificationDetail_bodyのテスト
/// </summary>
public class RequestModificationDetail_bodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RequestModificationDetail_body.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RequestModificationDetail_body>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RequestModificationDetail_body.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"key", "value"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RequestModificationDetail_body();
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
        var model = new RequestModificationDetail_body();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RequestModificationDetail_body();
        var deserializers = model.GetFieldDeserializers();
        var keyNode = Substitute.For<IParseNode>();
        keyNode.GetStringValue().Returns("traceId");
        deserializers["key"](keyNode);
        var valueNode = Substitute.For<IParseNode>();
        valueNode.GetStringValue().Returns("12345");
        deserializers["value"](valueNode);
        Assert.Equal("traceId", model.Key);
        Assert.Equal("12345", model.Value);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestModificationDetail_body_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RequestModificationDetail_body
        {
            Key = "traceId",
            Value = "12345",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RequestModificationDetail_body.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("traceId", deserializedModel.Key);
        Assert.Equal("12345", deserializedModel.Value);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestModificationDetail_body_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RequestModificationDetail_body
        {
            Key = "traceId",
            Value = "12345",
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
        var deserializedModel = parseNode.GetObjectValue(RequestModificationDetail_body.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("traceId", deserializedModel.Key);
        Assert.Equal("12345", deserializedModel.Value);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
