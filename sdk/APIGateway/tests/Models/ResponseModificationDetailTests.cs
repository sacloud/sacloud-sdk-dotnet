using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ResponseModificationDetailのテスト
/// </summary>
public class ResponseModificationDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseModificationDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseModificationDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseModificationDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"headers", "ifStatusCode", "json"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ResponseModificationDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("headers"));
        Assert.True(deserializers.ContainsKey("ifStatusCode"));
        Assert.True(deserializers.ContainsKey("json"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ResponseModificationDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ResponseModificationDetail();
        var deserializers = model.GetFieldDeserializers();
        var headers = new List<ResponseModificationDetail_headers> { new() { Key = "X-Header", Value = "1" } };
        var json = new List<ResponseModificationDetail_json> { new() { Key = "message", Value = "ok" } };

        var headersNode = Substitute.For<IParseNode>();
        headersNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ResponseModificationDetail_headers>>()).Returns(headers);
        deserializers["headers"](headersNode);

        var ifStatusCodeNode = Substitute.For<IParseNode>();
        ifStatusCodeNode.GetCollectionOfPrimitiveValues<int?>().Returns([200, 201]);
        deserializers["ifStatusCode"](ifStatusCodeNode);

        var jsonNode = Substitute.For<IParseNode>();
        jsonNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ResponseModificationDetail_json>>()).Returns(json);
        deserializers["json"](jsonNode);

        Assert.NotNull(model.Headers);
        Assert.Single(model.Headers);
        Assert.Equal("X-Header", model.Headers[0].Key);
        Assert.NotNull(model.IfStatusCode);
        Assert.Equal(2, model.IfStatusCode.Count);
        Assert.Equal(200, model.IfStatusCode[0]);
        Assert.Equal(201, model.IfStatusCode[1]);
        Assert.NotNull(model.Json);
        Assert.Single(model.Json);
        Assert.Equal("message", model.Json[0].Key);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseModificationDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ResponseModificationDetail
        {
            Headers = [new() { Key = "X-Header", Value = "1" }],
            IfStatusCode = [200],
            Json = [new() { Key = "message", Value = "ok" }],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ResponseModificationDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Headers);
        Assert.Single(deserializedModel.Headers);
        Assert.Equal("X-Header", deserializedModel.Headers[0].Key);
        Assert.NotNull(deserializedModel.IfStatusCode);
        Assert.Single(deserializedModel.IfStatusCode);
        Assert.Equal(200, deserializedModel.IfStatusCode[0]);
        Assert.NotNull(deserializedModel.Json);
        Assert.Single(deserializedModel.Json);
        Assert.Equal("message", deserializedModel.Json[0].Key);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseModificationDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ResponseModificationDetail
        {
            Headers = [new() { Key = "X-Header", Value = "1" }],
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
        var deserializedModel = parseNode.GetObjectValue(ResponseModificationDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Headers);
        Assert.Single(deserializedModel.Headers);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
