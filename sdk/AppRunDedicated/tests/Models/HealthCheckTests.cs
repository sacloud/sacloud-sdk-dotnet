using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// HealthCheckモデルのテスト
/// </summary>
public class HealthCheckTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void HealthCheck_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = HealthCheck.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<HealthCheck>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void HealthCheck_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            HealthCheck.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void HealthCheck_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new HealthCheck();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("intervalSeconds"));
        Assert.True(deserializers.ContainsKey("path"));
        Assert.True(deserializers.ContainsKey("timeoutSeconds"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void HealthCheck_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new HealthCheck();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void HealthCheck_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new HealthCheck();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(10);
        deserializers["intervalSeconds"](parseNode);
        Assert.Equal(10, model.IntervalSeconds);

        parseNode.GetStringValue().Returns("/health");
        deserializers["path"](parseNode);
        Assert.Equal("/health", model.Path);

        parseNode.GetIntValue().Returns(5);
        deserializers["timeoutSeconds"](parseNode);
        Assert.Equal(5, model.TimeoutSeconds);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task HealthCheck_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new HealthCheck
        {
            IntervalSeconds = 30,
            Path = "/api/health",
            TimeoutSeconds = 10
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(HealthCheck.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.IntervalSeconds, deserializedModel.IntervalSeconds);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.Equal(originalModel.TimeoutSeconds, deserializedModel.TimeoutSeconds);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task HealthCheck_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new HealthCheck
        {
            Path = "/health"
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
        var deserializedModel = parseNode.GetObjectValue(HealthCheck.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
