using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// ClusterResponseBodyモデルのテスト
/// </summary>
public class ClusterResponseBodyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ClusterResponseBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ClusterResponseBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(13, deserializers.Count);
        Assert.True(deserializers.ContainsKey("api_zone"));
        Assert.True(deserializers.ContainsKey("control_panel_url"));
        Assert.True(deserializers.ContainsKey("display_name"));
        Assert.True(deserializers.ContainsKey("display_name_en_us"));
        Assert.True(deserializers.ContainsKey("display_name_ja"));
        Assert.True(deserializers.ContainsKey("display_order"));
        Assert.True(deserializers.ContainsKey("endpoint_base"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("plan_family"));
        Assert.True(deserializers.ContainsKey("region"));
        Assert.True(deserializers.ContainsKey("s3_endpoint"));
        Assert.True(deserializers.ContainsKey("s3_endpoint_for_control_panel"));
        Assert.True(deserializers.ContainsKey("storage_zone"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ClusterResponseBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ClusterResponseBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ClusterResponseBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ClusterResponseBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(new List<string> { "zone-a" });
        parseNode.GetStringValue().Returns("sample");
        parseNode.GetIntValue().Returns(10);
        parseNode.GetEnumValue<Cluster_plan_family>().Returns(Cluster_plan_family.Standard);

        foreach (var deserializer in deserializers.Values)
        {
            deserializer(parseNode);
        }

        Assert.Equal(new[] { "zone-a" }, model.ApiZone);
        Assert.Equal("sample", model.ControlPanelUrl);
        Assert.Equal("sample", model.DisplayName);
        Assert.Equal("sample", model.DisplayNameEnUs);
        Assert.Equal("sample", model.DisplayNameJa);
        Assert.Equal(10, model.DisplayOrder);
        Assert.Equal("sample", model.EndpointBase);
        Assert.Equal("sample", model.Id);
        Assert.Equal(Cluster_plan_family.Standard, model.PlanFamily);
        Assert.Equal("sample", model.Region);
        Assert.Equal("sample", model.S3Endpoint);
        Assert.Equal("sample", model.S3EndpointForControlPanel);
        Assert.Equal(new[] { "zone-a" }, model.StorageZone);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ClusterResponseBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ClusterResponseBody
        {
            ApiZone = ["api-zone"],
            ControlPanelUrl = "https://panel.example",
            DisplayName = "Cluster",
            DisplayNameEnUs = "Cluster",
            DisplayNameJa = "クラスタ",
            DisplayOrder = 10,
            EndpointBase = "https://cluster.example",
            Id = "cluster-1",
            PlanFamily = Cluster_plan_family.Standard,
            Region = "east",
            S3Endpoint = "https://s3.example",
            S3EndpointForControlPanel = "https://s3-panel.example",
            StorageZone = ["storage-zone"],
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ClusterResponseBody.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ApiZone, deserializedModel.ApiZone);
        Assert.Equal(originalModel.ControlPanelUrl, deserializedModel.ControlPanelUrl);
        Assert.Equal(originalModel.DisplayName, deserializedModel.DisplayName);
        Assert.Equal(originalModel.DisplayNameEnUs, deserializedModel.DisplayNameEnUs);
        Assert.Equal(originalModel.DisplayNameJa, deserializedModel.DisplayNameJa);
        Assert.Equal(originalModel.DisplayOrder, deserializedModel.DisplayOrder);
        Assert.Equal(originalModel.EndpointBase, deserializedModel.EndpointBase);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.PlanFamily, deserializedModel.PlanFamily);
        Assert.Equal(originalModel.Region, deserializedModel.Region);
        Assert.Equal(originalModel.S3Endpoint, deserializedModel.S3Endpoint);
        Assert.Equal(originalModel.S3EndpointForControlPanel, deserializedModel.S3EndpointForControlPanel);
        Assert.Equal(originalModel.StorageZone, deserializedModel.StorageZone);
    }

    [Fact]
    public async Task ClusterResponseBody_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ClusterResponseBody
        {
            AdditionalData = new Dictionary<string, object> { ["extra_key"] = "extra-value" },
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        writer.GetSerializedContent().CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ClusterResponseBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Contains("extra_key", deserializedModel.AdditionalData.Keys);
    }
}
