using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PlanChangeReqBodyモデルのテスト
/// </summary>
public class PlanChangeReqBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PlanChangeReqBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PlanChangeReqBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("new_plan"));
        Assert.True(deserializers.ContainsKey("previous_contract"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PlanChangeReqBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PlanChangeReqBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PlanChangeReqBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PlanChangeReqBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("new_plan"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PlanChangeReqBody_new_plan>>()).Returns(Substitute.For<PlanChangeReqBody_new_plan>());

        deserializers["new_plan"](parseNode);
        Assert.NotNull(model.NewPlan);

        Assert.True(deserializers.ContainsKey("previous_contract"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PlanChangeReqBody_previous_contract>>()).Returns(Substitute.For<PlanChangeReqBody_previous_contract>());

        deserializers["previous_contract"](parseNode);
        Assert.NotNull(model.PreviousContract);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PlanChangeReqBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                ServiceClassPath = "sample",
                Type = PlanType.Archive,
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123",
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
        var deserializedModel = parseNode.GetObjectValue(PlanChangeReqBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.NewPlan);
        Assert.Equal(originalModel.NewPlan.ServiceClassPath, deserializedModel.NewPlan.ServiceClassPath);
        Assert.Equal(originalModel.NewPlan.Type, deserializedModel.NewPlan.Type);
        Assert.NotNull(deserializedModel.PreviousContract);
        Assert.Equal(originalModel.PreviousContract.ResourceId, deserializedModel.PreviousContract.ResourceId);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PlanChangeReqBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PlanChangeReqBody
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
        var deserializedModel = parseNode.GetObjectValue(PlanChangeReqBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
