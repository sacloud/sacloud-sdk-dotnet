using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;
using System.Text;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// Limitモデルのユニットテスト
/// </summary>
public class LimitTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Limit.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Limit>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Limit.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Limit();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("applicationCount"));
        Assert.True(deserializers.ContainsKey("applicationVersionCountPerApplication"));
        Assert.True(deserializers.ContainsKey("autoScalingGroupCount"));
        Assert.True(deserializers.ContainsKey("clusterCount"));
        Assert.True(deserializers.ContainsKey("loadBalancerNodeCount"));
        Assert.True(deserializers.ContainsKey("workerNodeCount"));
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Limit();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Limit();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(100L, 10L, 5L, 3L, 20L, 50L);

        var deserializers = model.GetFieldDeserializers();

        deserializers["applicationCount"](parseNode);
        Assert.Equal(100L, model.ApplicationCount);

        deserializers["applicationVersionCountPerApplication"](parseNode);
        Assert.Equal(10L, model.ApplicationVersionCountPerApplication);

        deserializers["autoScalingGroupCount"](parseNode);
        Assert.Equal(5L, model.AutoScalingGroupCount);

        deserializers["clusterCount"](parseNode);
        Assert.Equal(3L, model.ClusterCount);

        deserializers["loadBalancerNodeCount"](parseNode);
        Assert.Equal(20L, model.LoadBalancerNodeCount);

        deserializers["workerNodeCount"](parseNode);
        Assert.Equal(50L, model.WorkerNodeCount);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new Limit
        {
            ApplicationCount = 100L,
            ApplicationVersionCountPerApplication = 10L,
            AutoScalingGroupCount = 5L,
            ClusterCount = 3L,
            LoadBalancerNodeCount = 20L,
            WorkerNodeCount = 50L
        };

        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue<Limit>(Limit.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(100L, deserialized.ApplicationCount);
        Assert.Equal(10L, deserialized.ApplicationVersionCountPerApplication);
        Assert.Equal(5L, deserialized.AutoScalingGroupCount);
        Assert.Equal(3L, deserialized.ClusterCount);
        Assert.Equal(20L, deserialized.LoadBalancerNodeCount);
        Assert.Equal(50L, deserialized.WorkerNodeCount);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new Limit
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" },
                { "numericField", 123 }
            }
        };

        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue<Limit>(Limit.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.Equal(2, deserialized.AdditionalData.Count);
    }
}
