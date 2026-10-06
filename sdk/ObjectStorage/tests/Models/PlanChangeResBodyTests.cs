using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PlanChangeResBodyモデルのテスト
/// </summary>
public class PlanChangeResBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PlanChangeResBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PlanChangeResBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("new_contract"));
        Assert.True(deserializers.ContainsKey("plan"));
        Assert.True(deserializers.ContainsKey("previous_contract"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PlanChangeResBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PlanChangeResBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PlanChangeResBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PlanChangeResBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("new_contract"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ContractSummary>>()).Returns(Substitute.For<ContractSummary>());

        deserializers["new_contract"](parseNode);
        Assert.NotNull(model.NewContract);

        Assert.True(deserializers.ContainsKey("plan"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PlanSummary>>()).Returns(Substitute.For<PlanSummary>());

        deserializers["plan"](parseNode);
        Assert.NotNull(model.Plan);

        Assert.True(deserializers.ContainsKey("previous_contract"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ContractSummary>>()).Returns(Substitute.For<ContractSummary>());

        deserializers["previous_contract"](parseNode);
        Assert.NotNull(model.PreviousContract);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PlanChangeResBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PlanChangeResBody
        {
            NewContract = new ContractSummary
            {
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                DeletedAt = DateTimeOffset.Parse("2026-01-02T00:00:00+00:00"),
                ResourceId = "123",
                Status = ContractSummary_status.Active,
            },
            Plan = new PlanSummary
            {
                CapacityGib = 1,
                ClusterId = "cluster",
                ServiceClassPath = "sample",
                Type = PlanType.Archive,
            },
            PreviousContract = new ContractSummary
            {
                CreatedAt = DateTimeOffset.Parse("2026-01-03T00:00:00+00:00"),
                DeletedAt = DateTimeOffset.Parse("2026-01-04T00:00:00+00:00"),
                ResourceId = "456",
                Status = ContractSummary_status.Terminated,
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PlanChangeResBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.NewContract);
        Assert.Equal(originalModel.NewContract.ResourceId, deserializedModel.NewContract.ResourceId);
        Assert.Equal(originalModel.NewContract.Status, deserializedModel.NewContract.Status);
        Assert.NotNull(deserializedModel.Plan);
        Assert.Equal(originalModel.Plan.CapacityGib, deserializedModel.Plan.CapacityGib);
        Assert.Equal(originalModel.Plan.ClusterId, deserializedModel.Plan.ClusterId);
        Assert.Equal(originalModel.Plan.ServiceClassPath, deserializedModel.Plan.ServiceClassPath);
        Assert.Equal(originalModel.Plan.Type, deserializedModel.Plan.Type);
        Assert.NotNull(deserializedModel.PreviousContract);
        Assert.Equal(originalModel.PreviousContract.ResourceId, deserializedModel.PreviousContract.ResourceId);
        Assert.Equal(originalModel.PreviousContract.Status, deserializedModel.PreviousContract.Status);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PlanChangeResBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PlanChangeResBody
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PlanChangeResBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
