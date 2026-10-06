using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RequestRemoveDetailのテスト
/// </summary>
public class RequestRemoveDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RequestRemoveDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RequestRemoveDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RequestRemoveDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"body", "headerKeys", "queryParams"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RequestRemoveDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("body"));
        Assert.True(deserializers.ContainsKey("headerKeys"));
        Assert.True(deserializers.ContainsKey("queryParams"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RequestRemoveDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RequestRemoveDetail();
        var deserializers = model.GetFieldDeserializers();

        var bodyNode = Substitute.For<IParseNode>();
        bodyNode.GetCollectionOfPrimitiveValues<string>().Returns(["traceId"]);
        deserializers["body"](bodyNode);

        var headerKeysNode = Substitute.For<IParseNode>();
        headerKeysNode.GetCollectionOfPrimitiveValues<string>().Returns(["X-Trace-Id"]);
        deserializers["headerKeys"](headerKeysNode);

        var queryParamsNode = Substitute.For<IParseNode>();
        queryParamsNode.GetCollectionOfPrimitiveValues<string>().Returns(["page"]);
        deserializers["queryParams"](queryParamsNode);

        Assert.NotNull(model.Body);
        Assert.Single(model.Body);
        Assert.Equal("traceId", model.Body[0]);
        Assert.NotNull(model.HeaderKeys);
        Assert.Single(model.HeaderKeys);
        Assert.Equal("X-Trace-Id", model.HeaderKeys[0]);
        Assert.NotNull(model.QueryParams);
        Assert.Single(model.QueryParams);
        Assert.Equal("page", model.QueryParams[0]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestRemoveDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RequestRemoveDetail
        {
            Body = ["traceId"],
            HeaderKeys = ["X-Trace-Id"],
            QueryParams = ["page"],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RequestRemoveDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Body);
        Assert.Single(deserializedModel.Body);
        Assert.Equal("traceId", deserializedModel.Body[0]);
        Assert.NotNull(deserializedModel.HeaderKeys);
        Assert.Single(deserializedModel.HeaderKeys);
        Assert.Equal("X-Trace-Id", deserializedModel.HeaderKeys[0]);
        Assert.NotNull(deserializedModel.QueryParams);
        Assert.Single(deserializedModel.QueryParams);
        Assert.Equal("page", deserializedModel.QueryParams[0]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestRemoveDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RequestRemoveDetail
        {
            Body = ["traceId"],
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
        var deserializedModel = parseNode.GetObjectValue(RequestRemoveDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Body);
        Assert.Single(deserializedModel.Body);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
