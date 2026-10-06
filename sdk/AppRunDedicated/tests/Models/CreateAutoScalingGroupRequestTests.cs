using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// CreateAutoScalingGroupRequestモデルのテスト
/// </summary>
public class CreateAutoScalingGroupRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateAutoScalingGroup_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateAutoScalingGroupRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateAutoScalingGroupRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateAutoScalingGroup_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            CreateAutoScalingGroupRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateAutoScalingGroup_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateAutoScalingGroupRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("interfaces"));
        Assert.True(deserializers.ContainsKey("maxNodes"));
        Assert.True(deserializers.ContainsKey("minNodes"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("nameServers"));
        Assert.True(deserializers.ContainsKey("workerServiceClassPath"));
        Assert.True(deserializers.ContainsKey("zone"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateAutoScalingGroup_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateAutoScalingGroupRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateAutoScalingGroup_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateAutoScalingGroupRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var interfacesList = new List<AutoScalingGroupNodeInterface>
        {
            new AutoScalingGroupNodeInterface()
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<AutoScalingGroupNodeInterface>>())
            .Returns(interfacesList);
        deserializers["interfaces"](parseNode);
        Assert.NotNull(model.Interfaces);

        parseNode.GetIntValue().Returns(10);
        deserializers["maxNodes"](parseNode);
        Assert.Equal(10, model.MaxNodes);

        parseNode.GetIntValue().Returns(1);
        deserializers["minNodes"](parseNode);
        Assert.Equal(1, model.MinNodes);

        parseNode.GetStringValue().Returns("test-asg");
        deserializers["name"](parseNode);
        Assert.Equal("test-asg", model.Name);

        var nameServersList = new List<string> { "8.8.8.8" };
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(nameServersList);
        deserializers["nameServers"](parseNode);
        Assert.NotNull(model.NameServers);

        parseNode.GetStringValue().Returns("/worker/standard");
        deserializers["workerServiceClassPath"](parseNode);
        Assert.Equal("/worker/standard", model.WorkerServiceClassPath);

        parseNode.GetStringValue().Returns("is1a");
        deserializers["zone"](parseNode);
        Assert.Equal("is1a", model.Zone);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateAutoScalingGroup_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateAutoScalingGroupRequest
        {
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface()
            },
            MaxNodes = 10,
            MinNodes = 1,
            Name = "test-asg",
            NameServers = new List<string> { "8.8.8.8" },
            WorkerServiceClassPath = "/worker/standard",
            Zone = "is1a"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateAutoScalingGroupRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Interfaces);
        Assert.Equal(originalModel.MaxNodes, deserializedModel.MaxNodes);
        Assert.Equal(originalModel.MinNodes, deserializedModel.MinNodes);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.NameServers);
        Assert.Equal(originalModel.WorkerServiceClassPath, deserializedModel.WorkerServiceClassPath);
        Assert.Equal(originalModel.Zone, deserializedModel.Zone);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateAutoScalingGroup_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            Zone = "is1a"
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
        var deserializedModel = parseNode.GetObjectValue(CreateAutoScalingGroupRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
