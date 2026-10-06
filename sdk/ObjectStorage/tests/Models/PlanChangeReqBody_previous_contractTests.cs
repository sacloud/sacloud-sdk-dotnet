using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PlanChangeReqBody_previous_contractモデルのテスト
/// </summary>
public class PlanChangeReqBody_previous_contractTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PlanChangeReqBody_previous_contract_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PlanChangeReqBody_previous_contract();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("resource_id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PlanChangeReqBody_previous_contract_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PlanChangeReqBody_previous_contract();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PlanChangeReqBody_previous_contract_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PlanChangeReqBody_previous_contract();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("resource_id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["resource_id"](parseNode);
        Assert.NotNull(model.ResourceId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PlanChangeReqBody_previous_contract_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PlanChangeReqBody_previous_contract
        {
            ResourceId = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PlanChangeReqBody_previous_contract.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PlanChangeReqBody_previous_contract_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PlanChangeReqBody_previous_contract
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
        var deserializedModel = parseNode.GetObjectValue(PlanChangeReqBody_previous_contract.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
