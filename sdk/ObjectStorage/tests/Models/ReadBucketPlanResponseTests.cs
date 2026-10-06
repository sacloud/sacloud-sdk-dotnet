using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// ReadBucketPlanResponseモデルのテスト
/// </summary>
public class ReadBucketPlanResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadBucketPlanResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ReadBucketPlanResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadBucketPlanResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ReadBucketPlanResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadBucketPlanResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ReadBucketPlanResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PlanWithContract>>()).Returns(Substitute.For<PlanWithContract>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadBucketPlanResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ReadBucketPlanResponse
        {
            Data = new PlanWithContract
            {
                Contract = new ContractSummary
                {
                    CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                    ResourceId = "contract-id",
                    Status = ContractSummary_status.Active,
                },
                Plan = new PlanSummary
                {
                    CapacityGib = 100,
                    ClusterId = "cluster-id",
                    ServiceClassPath = "storage/standard",
                    Type = PlanType.Standard,
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
        var deserializedModel = parseNode.GetObjectValue(ReadBucketPlanResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.NotNull(deserializedModel.Data.Contract);
        Assert.Equal(originalModel.Data.Contract.ResourceId, deserializedModel.Data.Contract.ResourceId);
        Assert.Equal(originalModel.Data.Contract.Status, deserializedModel.Data.Contract.Status);
        Assert.NotNull(deserializedModel.Data.Plan);
        Assert.Equal(originalModel.Data.Plan.CapacityGib, deserializedModel.Data.Plan.CapacityGib);
        Assert.Equal(originalModel.Data.Plan.ServiceClassPath, deserializedModel.Data.Plan.ServiceClassPath);
        Assert.Equal(originalModel.Data.Plan.Type, deserializedModel.Data.Plan.Type);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ReadBucketPlanResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ReadBucketPlanResponse
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
        var deserializedModel = parseNode.GetObjectValue(ReadBucketPlanResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
