using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// WorkerNodeRunningContainerモデルのテスト
/// </summary>
public class WorkerNodeRunningContainerTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void RunningContainer_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WorkerNodeRunningContainer.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WorkerNodeRunningContainer>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void RunningContainer_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            WorkerNodeRunningContainer.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void RunningContainer_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WorkerNodeRunningContainer();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("applicationID"));
        Assert.True(deserializers.ContainsKey("applicationVersion"));
        Assert.True(deserializers.ContainsKey("containerID"));
        Assert.True(deserializers.ContainsKey("image"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("startedAt"));
        Assert.True(deserializers.ContainsKey("state"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void RunningContainer_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WorkerNodeRunningContainer();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void RunningContainer_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WorkerNodeRunningContainer();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var appId = Guid.NewGuid();
        parseNode.GetGuidValue().Returns(appId);
        deserializers["applicationID"](parseNode);
        Assert.Equal(appId, model.ApplicationID);

        parseNode.GetIntValue().Returns(5);
        deserializers["applicationVersion"](parseNode);
        Assert.Equal(5, model.ApplicationVersion);

        parseNode.GetStringValue().Returns("container-abc123");
        deserializers["containerID"](parseNode);
        Assert.Equal("container-abc123", model.ContainerID);

        parseNode.GetStringValue().Returns("nginx:latest");
        deserializers["image"](parseNode);
        Assert.Equal("nginx:latest", model.Image);

        parseNode.GetStringValue().Returns("web-server");
        deserializers["name"](parseNode);
        Assert.Equal("web-server", model.Name);

        parseNode.GetLongValue().Returns(1640000000L);
        deserializers["startedAt"](parseNode);
        Assert.Equal(1640000000L, model.StartedAt);

        parseNode.GetStringValue().Returns("running");
        deserializers["state"](parseNode);
        Assert.Equal("running", model.State);

        parseNode.GetStringValue().Returns("Up 2 hours");
        deserializers["status"](parseNode);
        Assert.Equal("Up 2 hours", model.Status);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task RunningContainer_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WorkerNodeRunningContainer
        {
            ApplicationID = Guid.NewGuid(),
            ApplicationVersion = 3,
            ContainerID = "container-xyz789",
            Image = "nginx:1.21",
            Name = "web-container",
            StartedAt = 1640000000L,
            State = "running",
            Status = "Up 3 hours"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeRunningContainer.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ApplicationID, deserializedModel.ApplicationID);
        Assert.Equal(originalModel.ApplicationVersion, deserializedModel.ApplicationVersion);
        Assert.Equal(originalModel.ContainerID, deserializedModel.ContainerID);
        Assert.Equal(originalModel.Image, deserializedModel.Image);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.StartedAt, deserializedModel.StartedAt);
        Assert.Equal(originalModel.State, deserializedModel.State);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task RunningContainer_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WorkerNodeRunningContainer
        {
            Name = "test-container"
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
        var deserializedModel = parseNode.GetObjectValue(WorkerNodeRunningContainer.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
