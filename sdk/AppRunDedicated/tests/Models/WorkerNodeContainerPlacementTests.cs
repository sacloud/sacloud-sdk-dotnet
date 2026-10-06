using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// WorkerNodeContainerPlacementモデルのテスト
/// </summary>
public class WorkerNodeContainerPlacementTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WorkerNodeContainerPlacement.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WorkerNodeContainerPlacement>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            WorkerNodeContainerPlacement.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WorkerNodeContainerPlacement();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("containersStats"));
        Assert.True(deserializers.ContainsKey("desired"));
        Assert.True(deserializers.ContainsKey("nodeID"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WorkerNodeContainerPlacement();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WorkerNodeContainerPlacement();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testContainersStats = new WorkerNodeContainerPlacement_containersStats();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<WorkerNodeContainerPlacement_containersStats>>())
            .Returns(testContainersStats);
        deserializers["containersStats"](parseNode);
        Assert.NotNull(model.ContainersStats);

        var testDesired = new WorkerNodeContainerPlacement_desired();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<WorkerNodeContainerPlacement_desired>>())
            .Returns(testDesired);
        deserializers["desired"](parseNode);
        Assert.NotNull(model.Desired);

        var testGuid = Guid.NewGuid();
        parseNode.GetGuidValue().Returns(testGuid);
        deserializers["nodeID"](parseNode);
        Assert.Equal(testGuid, model.NodeID);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NodeContainerPlacementInfo_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WorkerNodeContainerPlacement
        {
            ContainersStats = new WorkerNodeContainerPlacement_containersStats(),
            Desired = new WorkerNodeContainerPlacement_desired(),
            NodeID = Guid.NewGuid()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeContainerPlacement.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.ContainersStats);
        Assert.NotNull(deserializedModel.Desired);
        Assert.Equal(originalModel.NodeID, deserializedModel.NodeID);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NodeContainerPlacementInfo_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WorkerNodeContainerPlacement
        {
            NodeID = Guid.NewGuid()
        };
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeContainerPlacement.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
