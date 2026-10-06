using Sacloud.Sdk.ObjectStorage.Models.Model;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// PlanSummaryモデルのテスト
/// </summary>
public class PlanSummaryTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PlanSummary_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PlanSummary();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("capacity_gib"));
        Assert.True(deserializers.ContainsKey("cluster_id"));
        Assert.True(deserializers.ContainsKey("service_class_path"));
        Assert.True(deserializers.ContainsKey("type"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PlanSummary_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PlanSummary();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PlanSummary_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PlanSummary();
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
    public async Task PlanSummary_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PlanSummary
        {
            CapacityGib = 1,
            ClusterId = "sample",
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
        var deserializedModel = parseNode.GetObjectValue(PlanSummary.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CapacityGib, deserializedModel.CapacityGib);
        Assert.Equal(originalModel.ClusterId, deserializedModel.ClusterId);
        Assert.Equal(originalModel.ServiceClassPath, deserializedModel.ServiceClassPath);
        Assert.Equal(originalModel.Type, deserializedModel.Type);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PlanSummary_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new PlanSummary
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
        var deserializedModel = parseNode.GetObjectValue(PlanSummary.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
