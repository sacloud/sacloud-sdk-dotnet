using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// SubscriptionPlanResponseのテスト
/// </summary>
public class SubscriptionPlanResponseTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new SubscriptionPlanResponse();
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
        var result = SubscriptionPlanResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<SubscriptionPlanResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SubscriptionPlanResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"maxRequests", "maxRequestsUnit", "maxServices", "overage", "planID", "planName", "price"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new SubscriptionPlanResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("maxRequests"));
        Assert.True(deserializers.ContainsKey("maxRequestsUnit"));
        Assert.True(deserializers.ContainsKey("maxServices"));
        Assert.True(deserializers.ContainsKey("overage"));
        Assert.True(deserializers.ContainsKey("planID"));
        Assert.True(deserializers.ContainsKey("planName"));
        Assert.True(deserializers.ContainsKey("price"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SubscriptionPlanResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new SubscriptionPlanResponse();
        var deserializers = model.GetFieldDeserializers();
        var planId = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var overage = new Overage
        {
            UnitPrice = "example_unit_price",
            UnitRequests = 1,
        };

        var maxRequestsNode = Substitute.For<IParseNode>();
        maxRequestsNode.GetIntValue().Returns(1000);
        deserializers["maxRequests"](maxRequestsNode);

        var maxRequestsUnitNode = Substitute.For<IParseNode>();
        maxRequestsUnitNode.GetEnumValue<SubscriptionPlanResponse_maxRequestsUnit>().Returns(SubscriptionPlanResponse_maxRequestsUnit.Minute);
        deserializers["maxRequestsUnit"](maxRequestsUnitNode);

        var maxServicesNode = Substitute.For<IParseNode>();
        maxServicesNode.GetIntValue().Returns(10);
        deserializers["maxServices"](maxServicesNode);

        var overageNode = Substitute.For<IParseNode>();
        overageNode.GetObjectValue(Arg.Any<ParsableFactory<Overage>>()).Returns(overage);
        deserializers["overage"](overageNode);

        var planIdNode = Substitute.For<IParseNode>();
        planIdNode.GetGuidValue().Returns(planId);
        deserializers["planID"](planIdNode);

        var planNameNode = Substitute.For<IParseNode>();
        planNameNode.GetStringValue().Returns("example_plan_name");
        deserializers["planName"](planNameNode);

        var priceNode = Substitute.For<IParseNode>();
        priceNode.GetStringValue().Returns("example_price");
        deserializers["price"](priceNode);

        Assert.Equal(1000, model.MaxRequests);
        Assert.Equal(SubscriptionPlanResponse_maxRequestsUnit.Minute, model.MaxRequestsUnit);
        Assert.Equal(10, model.MaxServices);
        Assert.Same(overage, model.Overage);
        Assert.Equal(planId, model.PlanID);
        Assert.Equal("example_plan_name", model.PlanName);
        Assert.Equal("example_price", model.Price);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task SubscriptionPlanResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new SubscriptionPlanResponse
        {
            MaxRequests = 1000,
            MaxRequestsUnit = SubscriptionPlanResponse_maxRequestsUnit.Minute,
            MaxServices = 10,
            Overage = new Overage
            {
                UnitPrice = "example_unit_price",
                UnitRequests = 1,
            },
            PlanID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            PlanName = "example_plan_name",
            Price = "example_price",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(SubscriptionPlanResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MaxRequests, deserializedModel.MaxRequests);
        Assert.Equal(originalModel.MaxRequestsUnit, deserializedModel.MaxRequestsUnit);
        Assert.Equal(originalModel.MaxServices, deserializedModel.MaxServices);
        Assert.NotNull(deserializedModel.Overage);
        Assert.Equal(originalModel.Overage.UnitPrice, deserializedModel.Overage.UnitPrice);
        Assert.Equal(originalModel.Overage.UnitRequests, deserializedModel.Overage.UnitRequests);
        Assert.Equal(originalModel.PlanID, deserializedModel.PlanID);
        Assert.Equal(originalModel.PlanName, deserializedModel.PlanName);
        Assert.Equal(originalModel.Price, deserializedModel.Price);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task SubscriptionPlanResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new SubscriptionPlanResponse
        {
            MaxRequests = 1000,
            MaxRequestsUnit = SubscriptionPlanResponse_maxRequestsUnit.Minute,
            MaxServices = 10,
            Overage = new Overage
            {
                UnitPrice = "example_unit_price",
                UnitRequests = 1,
            },
            PlanID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            PlanName = "example_plan_name",
            Price = "example_price",
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
        var deserializedModel = parseNode.GetObjectValue(SubscriptionPlanResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.MaxRequests, deserializedModel.MaxRequests);
        Assert.Equal(originalModel.MaxRequestsUnit, deserializedModel.MaxRequestsUnit);
        Assert.Equal(originalModel.MaxServices, deserializedModel.MaxServices);
        Assert.NotNull(deserializedModel.Overage);
        Assert.Equal(originalModel.Overage.UnitPrice, deserializedModel.Overage.UnitPrice);
        Assert.Equal(originalModel.Overage.UnitRequests, deserializedModel.Overage.UnitRequests);
        Assert.Equal(originalModel.PlanID, deserializedModel.PlanID);
        Assert.Equal(originalModel.PlanName, deserializedModel.PlanName);
        Assert.Equal(originalModel.Price, deserializedModel.Price);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
