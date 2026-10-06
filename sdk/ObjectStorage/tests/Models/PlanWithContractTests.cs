using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PlanWithContractモデルのテスト
/// </summary>
public class PlanWithContractTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PlanWithContract_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PlanWithContract();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("contract"));
        Assert.True(deserializers.ContainsKey("plan"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PlanWithContract_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PlanWithContract();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PlanWithContract_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PlanWithContract();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("contract"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ContractSummary>>()).Returns(Substitute.For<ContractSummary>());

        deserializers["contract"](parseNode);
        Assert.NotNull(model.Contract);

        Assert.True(deserializers.ContainsKey("plan"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PlanSummary>>()).Returns(Substitute.For<PlanSummary>());

        deserializers["plan"](parseNode);
        Assert.NotNull(model.Plan);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PlanWithContract_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PlanWithContract
        {
            Contract = new ContractSummary
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
        var deserializedModel = parseNode.GetObjectValue(PlanWithContract.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Contract);
        Assert.Equal(originalModel.Contract.ResourceId, deserializedModel.Contract.ResourceId);
        Assert.Equal(originalModel.Contract.Status, deserializedModel.Contract.Status);
        Assert.NotNull(deserializedModel.Plan);
        Assert.Equal(originalModel.Plan.CapacityGib, deserializedModel.Plan.CapacityGib);
        Assert.Equal(originalModel.Plan.ClusterId, deserializedModel.Plan.ClusterId);
        Assert.Equal(originalModel.Plan.ServiceClassPath, deserializedModel.Plan.ServiceClassPath);
        Assert.Equal(originalModel.Plan.Type, deserializedModel.Plan.Type);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PlanWithContract_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PlanWithContract
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
        var deserializedModel = parseNode.GetObjectValue(PlanWithContract.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
