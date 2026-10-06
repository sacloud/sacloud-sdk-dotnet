using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// UpdateWorkerNodeDrainingRequestモデルのテスト
/// </summary>
public class UpdateWorkerNodeDrainingRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void UpdateWorkerNodeDrainingRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UpdateWorkerNodeDrainingRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UpdateWorkerNodeDrainingRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateWorkerNodeDrainingRequest_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            UpdateWorkerNodeDrainingRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateWorkerNodeDrainingRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateWorkerNodeDrainingRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("draining"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateWorkerNodeDrainingRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateWorkerNodeDrainingRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateWorkerNodeDrainingRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateWorkerNodeDrainingRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["draining"](parseNode);
        Assert.True(model.Draining);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateWorkerNodeDrainingRequest_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UpdateWorkerNodeDrainingRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Draining, deserializedModel.Draining);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateWorkerNodeDrainingRequest_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UpdateWorkerNodeDrainingRequest
        {
            Draining = false
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
        var deserializedModel = parseNode.GetObjectValue(UpdateWorkerNodeDrainingRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
