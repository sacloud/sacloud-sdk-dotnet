using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RequestRenameDetailのテスト
/// </summary>
public class RequestRenameDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RequestRenameDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RequestRenameDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RequestRenameDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"body", "headers", "queryParams"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RequestRenameDetail();
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
        var model = new RequestRenameDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RequestRenameDetail();
        var deserializers = model.GetFieldDeserializers();
        var body = new List<RequestRenameDetail_body> { new() { From = "before", To = "after" } };
        var headers = new List<RequestRenameDetail_headers> { new() { From = "X-Old", To = "X-New" } };
        var queryParams = new List<RequestRenameDetail_queryParams> { new() { From = "page", To = "pageNumber" } };

        var bodyNode = Substitute.For<IParseNode>();
        bodyNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RequestRenameDetail_body>>()).Returns(body);
        deserializers["body"](bodyNode);

        var headersNode = Substitute.For<IParseNode>();
        headersNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RequestRenameDetail_headers>>()).Returns(headers);
        deserializers["headers"](headersNode);

        var queryParamsNode = Substitute.For<IParseNode>();
        queryParamsNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RequestRenameDetail_queryParams>>()).Returns(queryParams);
        deserializers["queryParams"](queryParamsNode);

        Assert.NotNull(model.Body);
        Assert.Single(model.Body);
        Assert.Equal("before", model.Body[0].From);
        Assert.NotNull(model.Headers);
        Assert.Single(model.Headers);
        Assert.Equal("X-Old", model.Headers[0].From);
        Assert.NotNull(model.QueryParams);
        Assert.Single(model.QueryParams);
        Assert.Equal("page", model.QueryParams[0].From);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestRenameDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RequestRenameDetail
        {
            Body = [new() { From = "before", To = "after" }],
            Headers = [new() { From = "X-Old", To = "X-New" }],
            QueryParams = [new() { From = "page", To = "pageNumber" }],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RequestRenameDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Body);
        Assert.Single(deserializedModel.Body);
        Assert.Equal("before", deserializedModel.Body[0].From);
        Assert.Equal("after", deserializedModel.Body[0].To);
        Assert.NotNull(deserializedModel.Headers);
        Assert.Single(deserializedModel.Headers);
        Assert.Equal("X-Old", deserializedModel.Headers[0].From);
        Assert.NotNull(deserializedModel.QueryParams);
        Assert.Single(deserializedModel.QueryParams);
        Assert.Equal("page", deserializedModel.QueryParams[0].From);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestRenameDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RequestRenameDetail
        {
            Body = [new() { From = "before", To = "after" }],
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
        var deserializedModel = parseNode.GetObjectValue(RequestRenameDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Body);
        Assert.Single(deserializedModel.Body);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
