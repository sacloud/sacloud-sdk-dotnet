using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Model;

/// <summary>
/// Clusterモデルのテスト
/// </summary>
public class ClusterTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Cluster.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Cluster>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            Cluster.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいJSONフィールド名キーを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectKeys()
    {
        var model = new Cluster();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("api_zone"));
        Assert.True(deserializers.ContainsKey("control_panel_url"));
        Assert.True(deserializers.ContainsKey("display_name"));
        Assert.True(deserializers.ContainsKey("id"));
    }

    /// <summary>
    /// SerializeがnullのwriterでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Cluster();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersの各アクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Cluster();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("test-value");
        parseNode.GetIntValue().Returns(1);
        parseNode.GetEnumValue<Cluster_plan_family>().Returns(Cluster_plan_family.Standard);
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(["zone1"]);

        deserializers["api_zone"](parseNode);
        deserializers["control_panel_url"](parseNode);
        deserializers["display_name"](parseNode);
        deserializers["display_name_en_us"](parseNode);
        deserializers["display_name_ja"](parseNode);
        deserializers["display_order"](parseNode);
        deserializers["endpoint_base"](parseNode);
        deserializers["id"](parseNode);
        deserializers["plan_family"](parseNode);
        deserializers["region"](parseNode);
        deserializers["s3_endpoint"](parseNode);
        deserializers["s3_endpoint_for_control_panel"](parseNode);
        deserializers["storage_zone"](parseNode);

        Assert.Equal(["zone1"], model.ApiZone);
        Assert.Equal("test-value", model.ControlPanelUrl);
        Assert.Equal("test-value", model.DisplayName);
        Assert.Equal("test-value", model.DisplayNameEnUs);
        Assert.Equal("test-value", model.DisplayNameJa);
        Assert.Equal(1, model.DisplayOrder);
        Assert.Equal("test-value", model.EndpointBase);
        Assert.Equal("test-value", model.Id);
        Assert.Equal(Cluster_plan_family.Standard, model.PlanFamily);
        Assert.Equal("test-value", model.Region);
        Assert.Equal("test-value", model.S3Endpoint);
        Assert.Equal("test-value", model.S3EndpointForControlPanel);
        Assert.Equal(["zone1"], model.StorageZone);
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_AdditionalDataInitialized()
    {
        var model = new Cluster();

        Assert.NotNull(model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Cluster_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Cluster
        {
            ApiZone = ["zone1"],
            ControlPanelUrl = "https://panel.example.com",
            DisplayName = "Cluster",
            DisplayNameEnUs = "Cluster",
            DisplayNameJa = "クラスタ",
            DisplayOrder = 1,
            EndpointBase = "https://api.example.com",
            Id = "cluster-1",
            PlanFamily = Cluster_plan_family.Standard,
            Region = "is1a",
            S3Endpoint = "https://s3.example.com",
            S3EndpointForControlPanel = "https://s3-panel.example.com",
            StorageZone = ["storage1"]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Cluster.CreateFromDiscriminatorValue);

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

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Cluster_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Cluster
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" }
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Cluster.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
