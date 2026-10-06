using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// WorkerNodeContainerPlacement_desiredモデルのテスト
/// </summary>
public class WorkerNodeContainerPlacement_desiredTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_desired_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WorkerNodeContainerPlacement_desired.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WorkerNodeContainerPlacement_desired>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_desired_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            WorkerNodeContainerPlacement_desired.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが基底クラスのフィールドを含むマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_desired_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WorkerNodeContainerPlacement_desired();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("containers"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_desired_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WorkerNodeContainerPlacement_desired();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NodeContainerPlacementInfo_desired_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WorkerNodeContainerPlacement_desired();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testContainers = new List<ApplicationDesiredContainer> { new ApplicationDesiredContainer() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ApplicationDesiredContainer>>())
            .Returns(testContainers);
        deserializers["containers"](parseNode);
        Assert.NotNull(model.Containers);
        Assert.Single(model.Containers);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NodeContainerPlacementInfo_desired_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WorkerNodeContainerPlacement_desired
        {
            Containers = new List<ApplicationDesiredContainer>
            {
                new ApplicationDesiredContainer()
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
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeContainerPlacement_desired.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Containers);
        Assert.Single(deserializedModel.Containers);
    }

    /// <summary>
    /// AdditionalDataを含むシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task NodeContainerPlacementInfo_desired_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WorkerNodeContainerPlacement_desired
        {
            Containers = new List<ApplicationDesiredContainer>
            {
                new ApplicationDesiredContainer()
            }
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
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeContainerPlacement_desired.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
