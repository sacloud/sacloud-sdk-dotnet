using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RouteAuthorizationDetailResponseのテスト
/// </summary>
public class RouteAuthorizationDetailResponseTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new RouteAuthorizationDetailResponse();
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
        var result = RouteAuthorizationDetailResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RouteAuthorizationDetailResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RouteAuthorizationDetailResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"groups", "isACLEnabled"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RouteAuthorizationDetailResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("groups"));
        Assert.True(deserializers.ContainsKey("isACLEnabled"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RouteAuthorizationDetailResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RouteAuthorizationDetailResponse();
        var deserializers = model.GetFieldDeserializers();
        var groups = new List<RouteAuthorization> { new() { Name = "group-a", Enabled = true } };

        var groupsNode = Substitute.For<IParseNode>();
        groupsNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RouteAuthorization>>()).Returns(groups);
        deserializers["groups"](groupsNode);

        var isACLEnabledNode = Substitute.For<IParseNode>();
        isACLEnabledNode.GetBoolValue().Returns(true);
        deserializers["isACLEnabled"](isACLEnabledNode);

        Assert.NotNull(model.Groups);
        Assert.Single(model.Groups);
        Assert.Equal("group-a", model.Groups[0].Name);
        Assert.True(model.IsACLEnabled);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RouteAuthorizationDetailResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RouteAuthorizationDetailResponse
        {
            Groups = [new() { Name = "group-a", Enabled = true }],
            IsACLEnabled = true,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RouteAuthorizationDetailResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Groups);
        Assert.Single(deserializedModel.Groups);
        Assert.Equal("group-a", deserializedModel.Groups[0].Name);
        Assert.True(deserializedModel.IsACLEnabled);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RouteAuthorizationDetailResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RouteAuthorizationDetailResponse
        {
            Groups = [new() { Name = "group-a" }],
            IsACLEnabled = true,
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
        var deserializedModel = parseNode.GetObjectValue(RouteAuthorizationDetailResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Groups);
        Assert.Single(deserializedModel.Groups);
        Assert.True(deserializedModel.IsACLEnabled);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
