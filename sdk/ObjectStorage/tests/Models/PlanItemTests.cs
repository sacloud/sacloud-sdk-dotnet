using Sacloud.Sdk.ObjectStorage.Models.Model;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PlanItemモデルのテスト
/// </summary>
public class PlanItemTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PlanItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PlanItem();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("capacity_gib"));
        Assert.True(deserializers.ContainsKey("cluster_id"));
        Assert.True(deserializers.ContainsKey("fee"));
        Assert.True(deserializers.ContainsKey("service_class_path"));
        Assert.True(deserializers.ContainsKey("type"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PlanItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PlanItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PlanItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PlanItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("capacity_gib"));

        parseNode.GetIntValue().Returns(123);

        deserializers["capacity_gib"](parseNode);
        Assert.NotNull(model.CapacityGib);

        Assert.True(deserializers.ContainsKey("cluster_id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["cluster_id"](parseNode);
        Assert.NotNull(model.ClusterId);

        Assert.True(deserializers.ContainsKey("fee"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PlanItem_fee>>()).Returns(Substitute.For<PlanItem_fee>());

        deserializers["fee"](parseNode);
        Assert.NotNull(model.Fee);

        Assert.True(deserializers.ContainsKey("service_class_path"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["service_class_path"](parseNode);
        Assert.NotNull(model.ServiceClassPath);

        Assert.True(deserializers.ContainsKey("type"));

        parseNode.GetEnumValue<PlanType>().Returns(PlanType.Standard);

        deserializers["type"](parseNode);
        Assert.NotNull(model.Type);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PlanItem_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PlanItem
        {
            CapacityGib = 1,
            ClusterId = "sample",
            Fee = new PlanItem_fee
            {
                Daily = 1,
                ForMonth = 2,
                Hourly = 3,
                Monthly = 4,
            },
            ServiceClassPath = "sample",
            Type = PlanType.Archive
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PlanItem.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CapacityGib, deserializedModel.CapacityGib);
        Assert.Equal(originalModel.ClusterId, deserializedModel.ClusterId);
        Assert.NotNull(deserializedModel.Fee);
        Assert.Equal(originalModel.Fee.Daily, deserializedModel.Fee.Daily);
        Assert.Equal(originalModel.Fee.ForMonth, deserializedModel.Fee.ForMonth);
        Assert.Equal(originalModel.Fee.Hourly, deserializedModel.Fee.Hourly);
        Assert.Equal(originalModel.Fee.Monthly, deserializedModel.Fee.Monthly);
        Assert.Equal(originalModel.ServiceClassPath, deserializedModel.ServiceClassPath);
        Assert.Equal(originalModel.Type, deserializedModel.Type);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PlanItem_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PlanItem
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
        var deserializedModel = parseNode.GetObjectValue(PlanItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
