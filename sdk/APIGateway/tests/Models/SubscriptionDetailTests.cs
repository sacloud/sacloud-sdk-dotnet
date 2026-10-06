using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// SubscriptionDetailのテスト
/// </summary>
public class SubscriptionDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = SubscriptionDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<SubscriptionDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SubscriptionDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親3+独自4=7キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new SubscriptionDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("monthlyRequest"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("resourceId"));
        Assert.True(deserializers.ContainsKey("service"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SubscriptionDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new SubscriptionDetail();
        var deserializers = model.GetFieldDeserializers();
        var testId = new Guid("12345678-1234-1234-1234-123456789012");
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
        nameNode.GetStringValue().Returns("subscription-detail");
        deserializers["name"](nameNode);

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
        Assert.Equal("subscription-detail", model.Name);
        Assert.Equal(12345L, model.ResourceId);
        Assert.Same(service, model.Service);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task SubscriptionDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new SubscriptionDetail
        {
            MonthlyRequest = 100,
            Name = "subscription-detail",
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
        var deserializedModel = parseNode.GetObjectValue(SubscriptionDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MonthlyRequest, deserializedModel.MonthlyRequest);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
        Assert.NotNull(deserializedModel.Service);
        Assert.Equal(originalModel.Service.Id, deserializedModel.Service.Id);
        Assert.Equal(originalModel.Service.Name, deserializedModel.Service.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task SubscriptionDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new SubscriptionDetail
        {
            MonthlyRequest = 100,
            Name = "subscription-detail",
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
        var deserializedModel = parseNode.GetObjectValue(SubscriptionDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MonthlyRequest, deserializedModel.MonthlyRequest);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
        Assert.NotNull(deserializedModel.Service);
        Assert.Equal(originalModel.Service.Name, deserializedModel.Service.Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
