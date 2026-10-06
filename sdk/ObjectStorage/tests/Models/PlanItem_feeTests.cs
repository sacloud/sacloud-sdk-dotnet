using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PlanItem_feeモデルのテスト
/// </summary>
public class PlanItem_feeTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PlanItem_fee_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PlanItem_fee();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("daily"));
        Assert.True(deserializers.ContainsKey("for_month"));
        Assert.True(deserializers.ContainsKey("hourly"));
        Assert.True(deserializers.ContainsKey("monthly"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PlanItem_fee_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PlanItem_fee();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PlanItem_fee_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PlanItem_fee();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("daily"));

        parseNode.GetIntValue().Returns(123);

        deserializers["daily"](parseNode);
        Assert.NotNull(model.Daily);

        Assert.True(deserializers.ContainsKey("for_month"));

        parseNode.GetIntValue().Returns(123);

        deserializers["for_month"](parseNode);
        Assert.NotNull(model.ForMonth);

        Assert.True(deserializers.ContainsKey("hourly"));

        parseNode.GetIntValue().Returns(123);

        deserializers["hourly"](parseNode);
        Assert.NotNull(model.Hourly);

        Assert.True(deserializers.ContainsKey("monthly"));

        parseNode.GetIntValue().Returns(123);

        deserializers["monthly"](parseNode);
        Assert.NotNull(model.Monthly);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PlanItem_fee_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PlanItem_fee
        {
            Daily = 1,
            ForMonth = 1,
            Hourly = 1,
            Monthly = 1
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PlanItem_fee.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Daily, deserializedModel.Daily);
        Assert.Equal(originalModel.ForMonth, deserializedModel.ForMonth);
        Assert.Equal(originalModel.Hourly, deserializedModel.Hourly);
        Assert.Equal(originalModel.Monthly, deserializedModel.Monthly);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PlanItem_fee_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PlanItem_fee
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
        var deserializedModel = parseNode.GetObjectValue(PlanItem_fee.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
