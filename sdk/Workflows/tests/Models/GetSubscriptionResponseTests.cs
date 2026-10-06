using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// GetSubscriptionResponseモデルのテスト
/// </summary>
public class GetSubscriptionResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void GetSubscriptionResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = GetSubscriptionResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<GetSubscriptionResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void GetSubscriptionResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => GetSubscriptionResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetSubscriptionResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new GetSubscriptionResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CurrentPlan"));
        Assert.True(deserializers.ContainsKey("is_ok"));
        Assert.True(deserializers.ContainsKey("MonthAppliedPlan"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void GetSubscriptionResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new GetSubscriptionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetSubscriptionResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new GetSubscriptionResponse();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetSubscriptionResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new GetSubscriptionResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var currentPlan = new CurrentPlan { PlanName = "Standard" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CurrentPlan>>()).Returns(currentPlan);
        deserializers["CurrentPlan"](parseNode);
        Assert.NotNull(model.CurrentPlan);
        Assert.Equal("Standard", model.CurrentPlan.PlanName);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.True(model.IsOk);

        var monthAppliedPlan = new MonthAppliedPlan { PlanName = "Standard" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<MonthAppliedPlan>>()).Returns(monthAppliedPlan);
        deserializers["MonthAppliedPlan"](parseNode);
        Assert.NotNull(model.MonthAppliedPlan);
        Assert.Equal("Standard", model.MonthAppliedPlan.PlanName);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task GetSubscriptionResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new GetSubscriptionResponse
        {
            CurrentPlan = new CurrentPlan { PlanName = "Standard" },
            IsOk = true,
            MonthAppliedPlan = new MonthAppliedPlan { PlanName = "Standard" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(GetSubscriptionResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.CurrentPlan);
        Assert.Equal(originalModel.IsOk, deserializedModel.IsOk);
        Assert.NotNull(deserializedModel.MonthAppliedPlan);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task GetSubscriptionResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new GetSubscriptionResponse
        {
            IsOk = true
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(GetSubscriptionResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
