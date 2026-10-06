using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RequestTransformationのテスト
/// </summary>
public class RequestTransformationTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RequestTransformation.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RequestTransformation>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RequestTransformation.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"add", "allow", "append", "httpMethod", "remove", "rename", "replace"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RequestTransformation();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("add"));
        Assert.True(deserializers.ContainsKey("allow"));
        Assert.True(deserializers.ContainsKey("append"));
        Assert.True(deserializers.ContainsKey("httpMethod"));
        Assert.True(deserializers.ContainsKey("remove"));
        Assert.True(deserializers.ContainsKey("rename"));
        Assert.True(deserializers.ContainsKey("replace"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RequestTransformation();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RequestTransformation();
        var deserializers = model.GetFieldDeserializers();
        var add = new RequestModificationDetail();
        var allow = new RequestAllowDetail();
        var append = new RequestModificationDetail();
        var remove = new RequestRemoveDetail();
        var rename = new RequestRenameDetail();
        var replace = new RequestModificationDetail();

        var addNode = Substitute.For<IParseNode>();
        addNode.GetObjectValue(Arg.Any<ParsableFactory<RequestModificationDetail>>()).Returns(add);
        deserializers["add"](addNode);

        var allowNode = Substitute.For<IParseNode>();
        allowNode.GetObjectValue(Arg.Any<ParsableFactory<RequestAllowDetail>>()).Returns(allow);
        deserializers["allow"](allowNode);

        var appendNode = Substitute.For<IParseNode>();
        appendNode.GetObjectValue(Arg.Any<ParsableFactory<RequestModificationDetail>>()).Returns(append);
        deserializers["append"](appendNode);

        var httpMethodNode = Substitute.For<IParseNode>();
        httpMethodNode.GetEnumValue<HTTPMethod>().Returns(HTTPMethod.POST);
        deserializers["httpMethod"](httpMethodNode);

        var removeNode = Substitute.For<IParseNode>();
        removeNode.GetObjectValue(Arg.Any<ParsableFactory<RequestRemoveDetail>>()).Returns(remove);
        deserializers["remove"](removeNode);

        var renameNode = Substitute.For<IParseNode>();
        renameNode.GetObjectValue(Arg.Any<ParsableFactory<RequestRenameDetail>>()).Returns(rename);
        deserializers["rename"](renameNode);

        var replaceNode = Substitute.For<IParseNode>();
        replaceNode.GetObjectValue(Arg.Any<ParsableFactory<RequestModificationDetail>>()).Returns(replace);
        deserializers["replace"](replaceNode);

        Assert.Same(add, model.Add);
        Assert.Same(allow, model.Allow);
        Assert.Same(append, model.Append);
        Assert.Equal(HTTPMethod.POST, model.HttpMethod);
        Assert.Same(remove, model.Remove);
        Assert.Same(rename, model.Rename);
        Assert.Same(replace, model.Replace);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestTransformation_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RequestTransformation
        {
            Add = new RequestModificationDetail { Headers = [new() { Key = "X-Add", Value = "1" }] },
            Allow = new RequestAllowDetail { Body = ["traceId"] },
            Append = new RequestModificationDetail { QueryParams = [new() { Key = "page", Value = "2" }] },
            HttpMethod = HTTPMethod.POST,
            Remove = new RequestRemoveDetail { HeaderKeys = ["X-Remove"] },
            Rename = new RequestRenameDetail { Headers = [new() { From = "X-Old", To = "X-New" }] },
            Replace = new RequestModificationDetail { Body = [new() { Key = "traceId", Value = "updated" }] },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RequestTransformation.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Add);
        Assert.Equal("X-Add", deserializedModel.Add.Headers[0].Key);
        Assert.NotNull(deserializedModel.Allow);
        Assert.Equal("traceId", deserializedModel.Allow.Body[0]);
        Assert.NotNull(deserializedModel.Append);
        Assert.Equal("page", deserializedModel.Append.QueryParams[0].Key);
        Assert.Equal(HTTPMethod.POST, deserializedModel.HttpMethod);
        Assert.NotNull(deserializedModel.Remove);
        Assert.Equal("X-Remove", deserializedModel.Remove.HeaderKeys[0]);
        Assert.NotNull(deserializedModel.Rename);
        Assert.Equal("X-Old", deserializedModel.Rename.Headers[0].From);
        Assert.NotNull(deserializedModel.Replace);
        Assert.Equal("traceId", deserializedModel.Replace.Body[0].Key);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RequestTransformation_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RequestTransformation
        {
            HttpMethod = HTTPMethod.POST,
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
        var deserializedModel = parseNode.GetObjectValue(RequestTransformation.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(HTTPMethod.POST, deserializedModel.HttpMethod);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
