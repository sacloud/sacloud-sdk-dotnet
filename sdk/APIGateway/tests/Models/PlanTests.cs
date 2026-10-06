using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Planモデルのテスト
/// </summary>
public class PlanTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = Plan.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<Plan>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Plan.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"createdAt", "id", "updatedAt", "description", "maxRequests", "maxRequestsUnit", "maxServices", "name", "overage", "price"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new Plan();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(10, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("maxRequests"));
        Assert.True(deserializers.ContainsKey("maxRequestsUnit"));
        Assert.True(deserializers.ContainsKey("maxServices"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("overage"));
        Assert.True(deserializers.ContainsKey("price"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Plan();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersの各アクションが正しくプロパティを更新することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new Plan();
        var deserializers = model.GetFieldDeserializers();
        var testId = new Guid("12345678-1234-1234-1234-123456789012");
        var overage = new Overage();

        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        deserializers["createdAt"](parseNodeCreatedAt);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetGuidValue().Returns(testId);
        deserializers["id"](parseNodeId);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero));
        deserializers["updatedAt"](parseNodeUpdatedAt);

        var descriptionNode = Substitute.For<IParseNode>();
        descriptionNode.GetStringValue().Returns("Premium plan");
        deserializers["description"](descriptionNode);

        var maxRequestsNode = Substitute.For<IParseNode>();
        maxRequestsNode.GetIntValue().Returns(10000);
        deserializers["maxRequests"](maxRequestsNode);

        var maxRequestsUnitNode = Substitute.For<IParseNode>();
        maxRequestsUnitNode.GetEnumValue<Plan_maxRequestsUnit>().Returns(Plan_maxRequestsUnit.Month);
        deserializers["maxRequestsUnit"](maxRequestsUnitNode);

        var maxServicesNode = Substitute.For<IParseNode>();
        maxServicesNode.GetIntValue().Returns(50);
        deserializers["maxServices"](maxServicesNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("Premium");
        deserializers["name"](nameNode);

        var overageNode = Substitute.For<IParseNode>();
        overageNode.GetObjectValue(Arg.Any<ParsableFactory<Overage>>()).Returns(overage);
        deserializers["overage"](overageNode);

        var priceNode = Substitute.For<IParseNode>();
        priceNode.GetStringValue().Returns("99.99");
        deserializers["price"](priceNode);

        Assert.Equal(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), model.CreatedAt);
        Assert.Equal(testId, model.Id);
        Assert.Equal(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero), model.UpdatedAt);
        Assert.Equal("Premium plan", model.Description);
        Assert.Equal(10000, model.MaxRequests);
        Assert.Equal(Plan_maxRequestsUnit.Month, model.MaxRequestsUnit);
        Assert.Equal(50, model.MaxServices);
        Assert.Equal("Premium", model.Name);
        Assert.Same(overage, model.Overage);
        Assert.Equal("99.99", model.Price);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Plan_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Plan
        {
            Description = "Premium plan",
            MaxRequests = 10000,
            MaxRequestsUnit = Plan_maxRequestsUnit.Month,
            MaxServices = 50,
            Name = "Premium",
            Overage = new Overage { UnitPrice = "0.01", UnitRequests = 1000 },
            Price = "99.99",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Plan.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.MaxRequests, deserializedModel.MaxRequests);
        Assert.Equal(originalModel.MaxRequestsUnit, deserializedModel.MaxRequestsUnit);
        Assert.Equal(originalModel.MaxServices, deserializedModel.MaxServices);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Overage);
        Assert.Equal(originalModel.Overage.UnitPrice, deserializedModel.Overage.UnitPrice);
        Assert.Equal(originalModel.Overage.UnitRequests, deserializedModel.Overage.UnitRequests);
        Assert.Equal(originalModel.Price, deserializedModel.Price);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Plan_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Plan
        {
            Description = "Premium plan",
            MaxRequests = 10000,
            MaxRequestsUnit = Plan_maxRequestsUnit.Month,
            MaxServices = 50,
            Name = "Premium",
            Overage = new Overage(),
            Price = "99.99",
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
        var deserializedModel = parseNode.GetObjectValue(Plan.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.MaxRequests, deserializedModel.MaxRequests);
        Assert.Equal(originalModel.MaxRequestsUnit, deserializedModel.MaxRequestsUnit);
        Assert.Equal(originalModel.MaxServices, deserializedModel.MaxServices);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Overage);
        Assert.Equal(originalModel.Price, deserializedModel.Price);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
