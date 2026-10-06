using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ResponseRemoveDetailのテスト
/// </summary>
public class ResponseRemoveDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseRemoveDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseRemoveDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseRemoveDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"headerKeys", "ifStatusCode", "jsonKeys"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ResponseRemoveDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("headerKeys"));
        Assert.True(deserializers.ContainsKey("ifStatusCode"));
        Assert.True(deserializers.ContainsKey("jsonKeys"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ResponseRemoveDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ResponseRemoveDetail();
        var deserializers = model.GetFieldDeserializers();

        var headerKeysNode = Substitute.For<IParseNode>();
        headerKeysNode.GetCollectionOfPrimitiveValues<string>().Returns(["X-Remove"]);
        deserializers["headerKeys"](headerKeysNode);

        var ifStatusCodeNode = Substitute.For<IParseNode>();
        ifStatusCodeNode.GetCollectionOfPrimitiveValues<int?>().Returns([400, 500]);
        deserializers["ifStatusCode"](ifStatusCodeNode);

        var jsonKeysNode = Substitute.For<IParseNode>();
        jsonKeysNode.GetCollectionOfPrimitiveValues<string>().Returns(["traceId"]);
        deserializers["jsonKeys"](jsonKeysNode);

        Assert.NotNull(model.HeaderKeys);
        Assert.Single(model.HeaderKeys);
        Assert.Equal("X-Remove", model.HeaderKeys[0]);
        Assert.NotNull(model.IfStatusCode);
        Assert.Equal(2, model.IfStatusCode.Count);
        Assert.Equal(400, model.IfStatusCode[0]);
        Assert.Equal(500, model.IfStatusCode[1]);
        Assert.NotNull(model.JsonKeys);
        Assert.Single(model.JsonKeys);
        Assert.Equal("traceId", model.JsonKeys[0]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseRemoveDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ResponseRemoveDetail
        {
            HeaderKeys = ["X-Remove"],
            IfStatusCode = [400, 500],
            JsonKeys = ["traceId"],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ResponseRemoveDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.HeaderKeys);
        Assert.Single(deserializedModel.HeaderKeys);
        Assert.Equal("X-Remove", deserializedModel.HeaderKeys[0]);
        Assert.NotNull(deserializedModel.IfStatusCode);
        Assert.Equal(2, deserializedModel.IfStatusCode.Count);
        Assert.NotNull(deserializedModel.JsonKeys);
        Assert.Single(deserializedModel.JsonKeys);
        Assert.Equal("traceId", deserializedModel.JsonKeys[0]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseRemoveDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ResponseRemoveDetail
        {
            HeaderKeys = ["X-Remove"],
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
        var deserializedModel = parseNode.GetObjectValue(ResponseRemoveDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.HeaderKeys);
        Assert.Single(deserializedModel.HeaderKeys);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
