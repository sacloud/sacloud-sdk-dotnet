using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RequestModificationDetailのテスト
/// </summary>
public class RequestModificationDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RequestModificationDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RequestModificationDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RequestModificationDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"body", "headers", "queryParams"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RequestModificationDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("body"));
        Assert.True(deserializers.ContainsKey("headers"));
        Assert.True(deserializers.ContainsKey("queryParams"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RequestModificationDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RequestModificationDetail();
        var deserializers = model.GetFieldDeserializers();
        var body = new List<RequestModificationDetail_body> { new() { Key = "traceId", Value = "1" } };
        var headers = new List<RequestModificationDetail_headers> { new() { Key = "X-Trace-Id", Value = "1" } };
        var queryParams = new List<RequestModificationDetail_queryParams> { new() { Key = "page", Value = "2" } };

        var bodyNode = Substitute.For<IParseNode>();
        bodyNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RequestModificationDetail_body>>()).Returns(body);
        deserializers["body"](bodyNode);

        var headersNode = Substitute.For<IParseNode>();
        headersNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RequestModificationDetail_headers>>()).Returns(headers);
        deserializers["headers"](headersNode);

        var queryParamsNode = Substitute.For<IParseNode>();
        queryParamsNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RequestModificationDetail_queryParams>>()).Returns(queryParams);
        deserializers["queryParams"](queryParamsNode);

        Assert.NotNull(model.Body);
        Assert.Single(model.Body);
        Assert.Equal("traceId", model.Body[0].Key);
        Assert.NotNull(model.Headers);
        Assert.Single(model.Headers);
        Assert.Equal("X-Trace-Id", model.Headers[0].Key);
        Assert.NotNull(model.QueryParams);
        Assert.Single(model.QueryParams);
        Assert.Equal("page", model.QueryParams[0].Key);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestModificationDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RequestModificationDetail
        {
            Body = [new() { Key = "traceId", Value = "1" }],
            Headers = [new() { Key = "X-Trace-Id", Value = "1" }],
            QueryParams = [new() { Key = "page", Value = "2" }],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RequestModificationDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Body);
        Assert.Single(deserializedModel.Body);
        Assert.Equal("traceId", deserializedModel.Body[0].Key);
        Assert.NotNull(deserializedModel.Headers);
        Assert.Single(deserializedModel.Headers);
        Assert.Equal("X-Trace-Id", deserializedModel.Headers[0].Key);
        Assert.NotNull(deserializedModel.QueryParams);
        Assert.Single(deserializedModel.QueryParams);
        Assert.Equal("page", deserializedModel.QueryParams[0].Key);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestModificationDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RequestModificationDetail
        {
            Body = [new() { Key = "traceId", Value = "1" }],
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
        var deserializedModel = parseNode.GetObjectValue(RequestModificationDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Body);
        Assert.Single(deserializedModel.Body);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
