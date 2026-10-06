using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ResponseTransformationのテスト
/// </summary>
public class ResponseTransformationTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ResponseTransformation.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ResponseTransformation>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ResponseTransformation.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"add", "allow", "append", "remove", "rename", "replace"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ResponseTransformation();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("add"));
        Assert.True(deserializers.ContainsKey("allow"));
        Assert.True(deserializers.ContainsKey("append"));
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
        var model = new ResponseTransformation();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ResponseTransformation();
        var deserializers = model.GetFieldDeserializers();
        var add = new ResponseModificationDetail();
        var allow = new ResponseAllowDetail();
        var append = new ResponseModificationDetail();
        var remove = new ResponseRemoveDetail();
        var rename = new ResponseRenameDetail();
        var replace = new ResponseReplaceDetail();

        var addNode = Substitute.For<IParseNode>();
        addNode.GetObjectValue(Arg.Any<ParsableFactory<ResponseModificationDetail>>()).Returns(add);
        deserializers["add"](addNode);

        var allowNode = Substitute.For<IParseNode>();
        allowNode.GetObjectValue(Arg.Any<ParsableFactory<ResponseAllowDetail>>()).Returns(allow);
        deserializers["allow"](allowNode);

        var appendNode = Substitute.For<IParseNode>();
        appendNode.GetObjectValue(Arg.Any<ParsableFactory<ResponseModificationDetail>>()).Returns(append);
        deserializers["append"](appendNode);

        var removeNode = Substitute.For<IParseNode>();
        removeNode.GetObjectValue(Arg.Any<ParsableFactory<ResponseRemoveDetail>>()).Returns(remove);
        deserializers["remove"](removeNode);

        var renameNode = Substitute.For<IParseNode>();
        renameNode.GetObjectValue(Arg.Any<ParsableFactory<ResponseRenameDetail>>()).Returns(rename);
        deserializers["rename"](renameNode);

        var replaceNode = Substitute.For<IParseNode>();
        replaceNode.GetObjectValue(Arg.Any<ParsableFactory<ResponseReplaceDetail>>()).Returns(replace);
        deserializers["replace"](replaceNode);

        Assert.Same(add, model.Add);
        Assert.Same(allow, model.Allow);
        Assert.Same(append, model.Append);
        Assert.Same(remove, model.Remove);
        Assert.Same(rename, model.Rename);
        Assert.Same(replace, model.Replace);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseTransformation_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ResponseTransformation
        {
            Add = new ResponseModificationDetail { Headers = [new() { Key = "X-Add", Value = "1" }] },
            Allow = new ResponseAllowDetail { JsonKeys = ["message"] },
            Append = new ResponseModificationDetail { Json = [new() { Key = "traceId", Value = "1" }] },
            Remove = new ResponseRemoveDetail { HeaderKeys = ["X-Remove"] },
            Rename = new ResponseRenameDetail { Json = [new() { From = "oldKey", To = "newKey" }] },
            Replace = new ResponseReplaceDetail { Body = "replacement" },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ResponseTransformation.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Add);
        Assert.Equal("X-Add", deserializedModel.Add.Headers[0].Key);
        Assert.NotNull(deserializedModel.Allow);
        Assert.Equal("message", deserializedModel.Allow.JsonKeys[0]);
        Assert.NotNull(deserializedModel.Append);
        Assert.Equal("traceId", deserializedModel.Append.Json[0].Key);
        Assert.NotNull(deserializedModel.Remove);
        Assert.Equal("X-Remove", deserializedModel.Remove.HeaderKeys[0]);
        Assert.NotNull(deserializedModel.Rename);
        Assert.Equal("oldKey", deserializedModel.Rename.Json[0].From);
        Assert.NotNull(deserializedModel.Replace);
        Assert.Equal("replacement", deserializedModel.Replace.Body);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ResponseTransformation_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ResponseTransformation
        {
            Replace = new ResponseReplaceDetail { Body = "replacement" },
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
        var deserializedModel = parseNode.GetObjectValue(ResponseTransformation.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Replace);
        Assert.Equal("replacement", deserializedModel.Replace.Body);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
