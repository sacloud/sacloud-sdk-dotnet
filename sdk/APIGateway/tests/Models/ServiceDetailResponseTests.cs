using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ServiceDetailResponseのテスト
/// </summary>
public class ServiceDetailResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ServiceDetailResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ServiceDetailResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServiceDetailResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親ServiceDetail18+独自1=19キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ServiceDetailResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(19, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("authentication"));
        Assert.True(deserializers.ContainsKey("connectTimeout"));
        Assert.True(deserializers.ContainsKey("host"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("oidc"));
        Assert.True(deserializers.ContainsKey("path"));
        Assert.True(deserializers.ContainsKey("port"));
        Assert.True(deserializers.ContainsKey("protocol"));
        Assert.True(deserializers.ContainsKey("readTimeout"));
        Assert.True(deserializers.ContainsKey("retries"));
        Assert.True(deserializers.ContainsKey("routeHost"));
        Assert.True(deserializers.ContainsKey("tags"));
        Assert.True(deserializers.ContainsKey("writeTimeout"));
        Assert.True(deserializers.ContainsKey("corsConfig"));
        Assert.True(deserializers.ContainsKey("objectStorageConfig"));
        Assert.True(deserializers.ContainsKey("subscription"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServiceDetailResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証（独自プロパティのみ）
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ServiceDetailResponse();
        var deserializers = model.GetFieldDeserializers();
        var subscription = new ServiceSubscriptionResponse { Id = Guid.NewGuid(), Name = "subscription-1" };

        var subscriptionNode = Substitute.For<IParseNode>();
        subscriptionNode.GetObjectValue(Arg.Any<ParsableFactory<ServiceSubscriptionResponse>>()).Returns(subscription);
        deserializers["subscription"](subscriptionNode);

        Assert.Same(subscription, model.Subscription);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ServiceDetailResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ServiceDetailResponse
        {
            Host = "backend.example.com",
            Name = "service-response",
            Subscription = new ServiceSubscriptionResponse { Id = Guid.NewGuid(), Name = "subscription-1" },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServiceDetailResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Host, deserializedModel.Host);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Subscription);
        Assert.Equal(originalModel.Subscription.Id, deserializedModel.Subscription.Id);
        Assert.Equal(originalModel.Subscription.Name, deserializedModel.Subscription.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ServiceDetailResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ServiceDetailResponse
        {
            Host = "backend.example.com",
            Name = "service-response",
            Subscription = new ServiceSubscriptionResponse { Id = Guid.NewGuid(), Name = "subscription-1" },
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
        var deserializedModel = parseNode.GetObjectValue(ServiceDetailResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Subscription);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
