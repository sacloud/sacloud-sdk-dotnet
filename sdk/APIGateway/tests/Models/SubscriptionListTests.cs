using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// SubscriptionListのテスト
/// </summary>
public class SubscriptionListTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new SubscriptionList();
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
        var result = SubscriptionList.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<SubscriptionList>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SubscriptionList.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"maxSubscription", "subscriptions"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new SubscriptionList();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("maxSubscription"));
        Assert.True(deserializers.ContainsKey("subscriptions"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SubscriptionList();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new SubscriptionList();
        var deserializers = model.GetFieldDeserializers();
        var subscriptions = new List<Subscription>
        {
            new() { Name = "sub-1", Service = new SubscriptionService { Name = "service-1" } },
        };

        var maxSubscriptionNode = Substitute.For<IParseNode>();
        maxSubscriptionNode.GetIntValue().Returns(10);
        deserializers["maxSubscription"](maxSubscriptionNode);

        var subscriptionsNode = Substitute.For<IParseNode>();
        subscriptionsNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Subscription>>()).Returns(subscriptions);
        deserializers["subscriptions"](subscriptionsNode);

        Assert.Equal(10, model.MaxSubscription);
        Assert.NotNull(model.Subscriptions);
        Assert.Single(model.Subscriptions);
        Assert.Equal("sub-1", model.Subscriptions[0].Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task SubscriptionList_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new SubscriptionList
        {
            MaxSubscription = 10,
            Subscriptions =
            [
                new() { Name = "sub-1", Service = new SubscriptionService { Name = "service-1" } },
            ],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(SubscriptionList.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MaxSubscription, deserializedModel.MaxSubscription);
        Assert.NotNull(deserializedModel.Subscriptions);
        Assert.Single(deserializedModel.Subscriptions);
        Assert.Equal("sub-1", deserializedModel.Subscriptions[0].Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task SubscriptionList_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new SubscriptionList
        {
            MaxSubscription = 10,
            Subscriptions =
            [
                new() { Name = "sub-1", Service = new SubscriptionService { Name = "service-1" } },
            ],
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
        var deserializedModel = parseNode.GetObjectValue(SubscriptionList.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MaxSubscription, deserializedModel.MaxSubscription);
        Assert.NotNull(deserializedModel.Subscriptions);
        Assert.Single(deserializedModel.Subscriptions);
        Assert.Equal("sub-1", deserializedModel.Subscriptions[0].Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
