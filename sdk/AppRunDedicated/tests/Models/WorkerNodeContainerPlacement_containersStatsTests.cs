using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// WorkerNodeContainerPlacement_containersStatsモデルのテスト
/// </summary>
public class WorkerNodeContainerPlacement_containersStatsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_containersStats_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WorkerNodeContainerPlacement_containersStats.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WorkerNodeContainerPlacement_containersStats>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_containersStats_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            WorkerNodeContainerPlacement_containersStats.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが基底クラスのフィールドを含むマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_containersStats_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WorkerNodeContainerPlacement_containersStats();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("collectedAtSec"));
        Assert.True(deserializers.ContainsKey("containers"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_containersStats_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WorkerNodeContainerPlacement_containersStats();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_containersStats_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WorkerNodeContainerPlacement_containersStats();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(1640000000L);
        deserializers["collectedAtSec"](parseNode);
        Assert.Equal(1640000000L, model.CollectedAtSec);

        var testContainers = new List<ApplicationCurrentContainer> { new ApplicationCurrentContainer() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ApplicationCurrentContainer>>())
            .Returns(testContainers);
        deserializers["containers"](parseNode);
        Assert.NotNull(model.Containers);
        Assert.Single(model.Containers);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NodeContainerPlacementInfo_containersStats_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WorkerNodeContainerPlacement_containersStats
        {
            CollectedAtSec = 1640000000L,
            Containers = new List<ApplicationCurrentContainer>
            {
                new ApplicationCurrentContainer()
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
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeContainerPlacement_containersStats.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CollectedAtSec, deserializedModel.CollectedAtSec);
        Assert.NotNull(deserializedModel.Containers);
        Assert.Single(deserializedModel.Containers);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NodeContainerPlacementInfo_containersStats_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WorkerNodeContainerPlacement_containersStats
        {
            CollectedAtSec = 1640000000L
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
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeContainerPlacement_containersStats.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
