using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// UpdateBucketPlanResponseモデルのテスト
/// </summary>
public class UpdateBucketPlanResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateBucketPlanResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateBucketPlanResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateBucketPlanResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateBucketPlanResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateBucketPlanResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateBucketPlanResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PlanChangeResBody>>()).Returns(Substitute.For<PlanChangeResBody>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateBucketPlanResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateBucketPlanResponse
        {
            Data = new PlanChangeResBody
            {
                NewContract = new ContractSummary
                {
                    ResourceId = "new-contract-id",
                    Status = ContractSummary_status.Active,
                },
                Plan = new PlanSummary
                {
                    CapacityGib = 200,
                    ClusterId = "cluster-id",
                    ServiceClassPath = "storage/archive",
                    Type = PlanType.Archive,
                },
                PreviousContract = new ContractSummary
                {
                    ResourceId = "previous-contract-id",
                    Status = ContractSummary_status.Terminated,
                },
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
        var deserializedModel = parseNode.GetObjectValue(UpdateBucketPlanResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.NotNull(deserializedModel.Data.NewContract);
        Assert.Equal(originalModel.Data.NewContract.ResourceId, deserializedModel.Data.NewContract.ResourceId);
        Assert.Equal(originalModel.Data.NewContract.Status, deserializedModel.Data.NewContract.Status);
        Assert.NotNull(deserializedModel.Data.Plan);
        Assert.Equal(originalModel.Data.Plan.CapacityGib, deserializedModel.Data.Plan.CapacityGib);
        Assert.Equal(originalModel.Data.Plan.ServiceClassPath, deserializedModel.Data.Plan.ServiceClassPath);
        Assert.Equal(originalModel.Data.Plan.Type, deserializedModel.Data.Plan.Type);
        Assert.NotNull(deserializedModel.Data.PreviousContract);
        Assert.Equal(originalModel.Data.PreviousContract.ResourceId, deserializedModel.Data.PreviousContract.ResourceId);
        Assert.Equal(originalModel.Data.PreviousContract.Status, deserializedModel.Data.PreviousContract.Status);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task UpdateBucketPlanResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new UpdateBucketPlanResponse
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
        var deserializedModel = parseNode.GetObjectValue(UpdateBucketPlanResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
