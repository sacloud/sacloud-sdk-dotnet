using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;
using System.Text;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ReadLimitsResponseモデルのユニットテスト
/// </summary>
public class ReadLimitsResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ReadLimitsResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ReadLimitsResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ReadLimitsResponse.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ReadLimitsResponse();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("limit"));
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ReadLimitsResponse();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ReadLimitsResponse();
        var parseNode = Substitute.For<IParseNode>();

        var testLimit = new Limit();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Limit>>())
            .Returns(testLimit);

        var deserializers = model.GetFieldDeserializers();

        deserializers["limit"](parseNode);
        Assert.NotNull(model.Limit);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new ReadLimitsResponse
        {
            Limit = new Limit
            {
                ApplicationCount = 100L,
                ApplicationVersionCountPerApplication = 10L,
                AutoScalingGroupCount = 5L,
                ClusterCount = 3L,
                LoadBalancerNodeCount = 20L,
                WorkerNodeCount = 50L
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
        var deserialized = parseNode.GetObjectValue<ReadLimitsResponse>(ReadLimitsResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Limit);
        Assert.Equal(100L, deserialized.Limit.ApplicationCount);
        Assert.Equal(10L, deserialized.Limit.ApplicationVersionCountPerApplication);
        Assert.Equal(5L, deserialized.Limit.AutoScalingGroupCount);
        Assert.Equal(3L, deserialized.Limit.ClusterCount);
        Assert.Equal(20L, deserialized.Limit.LoadBalancerNodeCount);
        Assert.Equal(50L, deserialized.Limit.WorkerNodeCount);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new ReadLimitsResponse
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
        var deserialized = parseNode.GetObjectValue<ReadLimitsResponse>(ReadLimitsResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.Equal(2, deserialized.AdditionalData.Count);
    }
}
