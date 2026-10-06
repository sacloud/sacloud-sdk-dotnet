using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ServiceSubscriptionResponseのテスト
/// </summary>
public class ServiceSubscriptionResponseTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new ServiceSubscriptionResponse();
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
        var result = ServiceSubscriptionResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ServiceSubscriptionResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServiceSubscriptionResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"id", "name"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ServiceSubscriptionResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServiceSubscriptionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ServiceSubscriptionResponse();
        var deserializers = model.GetFieldDeserializers();
        var id = Guid.NewGuid();

        var idNode = Substitute.For<IParseNode>();
        idNode.GetGuidValue().Returns(id);
        deserializers["id"](idNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("subscription-response");
        deserializers["name"](nameNode);

        Assert.Equal(id, model.Id);
        Assert.Equal("subscription-response", model.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ServiceSubscriptionResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ServiceSubscriptionResponse
        {
            Id = Guid.NewGuid(),
            Name = "subscription-response",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServiceSubscriptionResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ServiceSubscriptionResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ServiceSubscriptionResponse
        {
            Id = Guid.NewGuid(),
            Name = "subscription-response",
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
        var deserializedModel = parseNode.GetObjectValue(ServiceSubscriptionResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
