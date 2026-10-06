using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// CurrentPlanモデルのテスト
/// </summary>
public class CurrentPlanTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CurrentPlan_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CurrentPlan.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CurrentPlan>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CurrentPlan_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => CurrentPlan.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CurrentPlan_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CurrentPlan();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(9, deserializers.Count);
        Assert.True(deserializers.ContainsKey("accountId"));
        Assert.True(deserializers.ContainsKey("activateFrom"));
        Assert.True(deserializers.ContainsKey("activateUntil"));
        Assert.True(deserializers.ContainsKey("contractId"));
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("planId"));
        Assert.True(deserializers.ContainsKey("planName"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CurrentPlan_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CurrentPlan();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CurrentPlan_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new CurrentPlan();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CurrentPlan_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CurrentPlan();
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
    public async Task CurrentPlan_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CurrentPlan
        {
            AccountId = "account-123",
            ActivateFrom = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            ActivateUntil = DateTimeOffset.Parse("2024-12-31T23:59:59Z"),
            ContractId = "contract-456",
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            Id = "plan-789",
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
        var deserializedModel = parseNode.GetObjectValue(CurrentPlan.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccountId, deserializedModel.AccountId);
        Assert.Equal(originalModel.ActivateFrom, deserializedModel.ActivateFrom);
        Assert.Equal(originalModel.ActivateUntil, deserializedModel.ActivateUntil);
        Assert.Equal(originalModel.ContractId, deserializedModel.ContractId);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.PlanId, deserializedModel.PlanId);
        Assert.Equal(originalModel.PlanName, deserializedModel.PlanName);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CurrentPlan_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CurrentPlan
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
        var deserializedModel = parseNode.GetObjectValue(CurrentPlan.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
