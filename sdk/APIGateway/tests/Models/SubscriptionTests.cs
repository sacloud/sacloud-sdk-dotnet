using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Subscriptionモデルのテスト
/// </summary>
public class SubscriptionTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = Subscription.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<Subscription>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Subscription.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親3+独自5=8キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new Subscription();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("monthlyRequest"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("planId"));
        Assert.True(deserializers.ContainsKey("resourceId"));
        Assert.True(deserializers.ContainsKey("service"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Subscription();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new Subscription();
        var deserializers = model.GetFieldDeserializers();
        var testId = new Guid("12345678-1234-1234-1234-123456789012");
        var planId = Guid.NewGuid();
        var service = new SubscriptionService { Id = Guid.NewGuid(), Name = "service-1" };

        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        deserializers["createdAt"](parseNodeCreatedAt);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetGuidValue().Returns(testId);
        deserializers["id"](parseNodeId);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero));
        deserializers["updatedAt"](parseNodeUpdatedAt);

        var monthlyRequestNode = Substitute.For<IParseNode>();
        monthlyRequestNode.GetIntValue().Returns(100);
        deserializers["monthlyRequest"](monthlyRequestNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("subscription-1");
        deserializers["name"](nameNode);

        var planIdNode = Substitute.For<IParseNode>();
        planIdNode.GetGuidValue().Returns(planId);
        deserializers["planId"](planIdNode);

        var resourceIdNode = Substitute.For<IParseNode>();
        resourceIdNode.GetLongValue().Returns(12345L);
        deserializers["resourceId"](resourceIdNode);

        var serviceNode = Substitute.For<IParseNode>();
        serviceNode.GetObjectValue(Arg.Any<ParsableFactory<SubscriptionService>>()).Returns(service);
        deserializers["service"](serviceNode);

        Assert.Equal(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), model.CreatedAt);
        Assert.Equal(testId, model.Id);
        Assert.Equal(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero), model.UpdatedAt);
        Assert.Equal(100, model.MonthlyRequest);
        Assert.Equal("subscription-1", model.Name);
        Assert.Equal(planId, model.PlanId);
        Assert.Equal(12345L, model.ResourceId);
        Assert.Same(service, model.Service);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Subscription_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Subscription
        {
            MonthlyRequest = 100,
            Name = "subscription-1",
            PlanId = Guid.NewGuid(),
            ResourceId = 12345L,
            Service = new SubscriptionService { Id = Guid.NewGuid(), Name = "service-1" },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Subscription.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MonthlyRequest, deserializedModel.MonthlyRequest);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.PlanId, deserializedModel.PlanId);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
        Assert.NotNull(deserializedModel.Service);
        Assert.Equal(originalModel.Service.Id, deserializedModel.Service.Id);
        Assert.Equal(originalModel.Service.Name, deserializedModel.Service.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Subscription_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Subscription
        {
            MonthlyRequest = 100,
            Name = "subscription-1",
            PlanId = Guid.NewGuid(),
            ResourceId = 12345L,
            Service = new SubscriptionService { Id = Guid.NewGuid(), Name = "service-1" },
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
        var deserializedModel = parseNode.GetObjectValue(Subscription.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MonthlyRequest, deserializedModel.MonthlyRequest);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.PlanId, deserializedModel.PlanId);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
        Assert.NotNull(deserializedModel.Service);
        Assert.Equal(originalModel.Service.Name, deserializedModel.Service.Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
