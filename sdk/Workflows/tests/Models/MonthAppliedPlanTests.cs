using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// MonthAppliedPlanモデルのテスト
/// </summary>
public class MonthAppliedPlanTests
{
    [Fact]
    public void MonthAppliedPlan_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = MonthAppliedPlan.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<MonthAppliedPlan>(result);
    }

    [Fact]
    public void MonthAppliedPlan_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => MonthAppliedPlan.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void MonthAppliedPlan_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new MonthAppliedPlan();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count >= 8);
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void MonthAppliedPlan_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new MonthAppliedPlan();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MonthAppliedPlan_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new MonthAppliedPlan();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MonthAppliedPlan_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MonthAppliedPlan();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("account-123");
        deserializers["accountId"](parseNode);
        Assert.Equal("account-123", model.AccountId);

        var testDate1 = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate1);
        deserializers["activateFrom"](parseNode);
        Assert.Equal(testDate1, model.ActivateFrom);

        var testDate2 = DateTimeOffset.Parse("2024-12-31T23:59:59Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate2);
        deserializers["activateUntil"](parseNode);
        Assert.Equal(testDate2, model.ActivateUntil);

        parseNode.GetIntValue().Returns(1000);
        deserializers["basePrice"](parseNode);
        Assert.Equal(1000, model.BasePrice);

        parseNode.GetStringValue().Returns("contract-456");
        deserializers["contractId"](parseNode);
        Assert.Equal("contract-456", model.ContractId);

        var testDate3 = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate3);
        deserializers["createdAt"](parseNode);
        Assert.Equal(testDate3, model.CreatedAt);

        parseNode.GetStringValue().Returns("plan-789");
        deserializers["id"](parseNode);
        Assert.Equal("plan-789", model.Id);

        parseNode.GetIntValue().Returns(10000);
        deserializers["includedSteps"](parseNode);
        Assert.Equal(10000, model.IncludedSteps);

        parseNode.GetIntValue().Returns(1);
        deserializers["overagePricePerUnit"](parseNode);
        Assert.Equal(1, model.OveragePricePerUnit);

        parseNode.GetIntValue().Returns(100);
        deserializers["overageStepUnit"](parseNode);
        Assert.Equal(100, model.OverageStepUnit);

        parseNode.GetIntValue().Returns(2);
        deserializers["planGrade"](parseNode);
        Assert.Equal(2, model.PlanGrade);

        parseNode.GetIntValue().Returns(123);
        deserializers["planId"](parseNode);
        Assert.Equal(123, model.PlanId);

        parseNode.GetStringValue().Returns("Standard Plan");
        deserializers["planName"](parseNode);
        Assert.Equal("Standard Plan", model.PlanName);

        var testDate4 = DateTimeOffset.Parse("2024-02-01T12:00:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate4);
        deserializers["updatedAt"](parseNode);
        Assert.Equal(testDate4, model.UpdatedAt);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MonthAppliedPlan_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new MonthAppliedPlan
        {
            AccountId = "account-123",
            ActivateFrom = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            ActivateUntil = DateTimeOffset.Parse("2024-12-31T23:59:59Z"),
            BasePrice = 1000,
            ContractId = "contract-456",
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            Id = "plan-789",
            IncludedSteps = 10000,
            OveragePricePerUnit = 1,
            OverageStepUnit = 100,
            PlanGrade = 2,
            PlanId = 123,
            PlanName = "Standard Plan",
            UpdatedAt = DateTimeOffset.Parse("2024-02-01T12:00:00Z")
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(MonthAppliedPlan.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccountId, deserializedModel.AccountId);
        Assert.Equal(originalModel.ActivateFrom, deserializedModel.ActivateFrom);
        Assert.Equal(originalModel.ActivateUntil, deserializedModel.ActivateUntil);
        Assert.Equal(originalModel.BasePrice, deserializedModel.BasePrice);
        Assert.Equal(originalModel.ContractId, deserializedModel.ContractId);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.IncludedSteps, deserializedModel.IncludedSteps);
        Assert.Equal(originalModel.OveragePricePerUnit, deserializedModel.OveragePricePerUnit);
        Assert.Equal(originalModel.OverageStepUnit, deserializedModel.OverageStepUnit);
        Assert.Equal(originalModel.PlanGrade, deserializedModel.PlanGrade);
        Assert.Equal(originalModel.PlanId, deserializedModel.PlanId);
        Assert.Equal(originalModel.PlanName, deserializedModel.PlanName);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MonthAppliedPlan_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new MonthAppliedPlan
        {
            Id = "plan-789",
            PlanName = "Standard Plan"
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
        var deserializedModel = parseNode.GetObjectValue(MonthAppliedPlan.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
