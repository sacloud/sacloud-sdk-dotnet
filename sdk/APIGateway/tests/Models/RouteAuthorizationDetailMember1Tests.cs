using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RouteAuthorizationDetailMember1のテスト
/// </summary>
public class RouteAuthorizationDetailMember1Tests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new RouteAuthorizationDetailMember1();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RouteAuthorizationDetailMember1.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RouteAuthorizationDetailMember1>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RouteAuthorizationDetailMember1.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"isACLEnabled"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RouteAuthorizationDetailMember1();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("isACLEnabled"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RouteAuthorizationDetailMember1();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RouteAuthorizationDetailMember1();
        var deserializers = model.GetFieldDeserializers();
        var node = Substitute.For<IParseNode>();
        node.GetBoolValue().Returns(false);
        deserializers["isACLEnabled"](node);
        Assert.False(model.IsACLEnabled);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RouteAuthorizationDetailMember1_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RouteAuthorizationDetailMember1
        {
            IsACLEnabled = false,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RouteAuthorizationDetailMember1.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.False(deserializedModel.IsACLEnabled);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RouteAuthorizationDetailMember1_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RouteAuthorizationDetailMember1
        {
            IsACLEnabled = false,
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
        var deserializedModel = parseNode.GetObjectValue(RouteAuthorizationDetailMember1.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.False(deserializedModel.IsACLEnabled);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
