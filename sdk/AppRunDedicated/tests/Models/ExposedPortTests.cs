using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ExposedPortモデルのテスト
/// </summary>
public class ExposedPortTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void ExposedPort_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ExposedPort.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ExposedPort>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ExposedPort_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            ExposedPort.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ExposedPort_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ExposedPort();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("healthCheck"));
        Assert.True(deserializers.ContainsKey("host"));
        Assert.True(deserializers.ContainsKey("loadBalancerPort"));
        Assert.True(deserializers.ContainsKey("targetPort"));
        Assert.True(deserializers.ContainsKey("useLetsEncrypt"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ExposedPort_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ExposedPort();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ExposedPort_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ExposedPort();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testHealthCheck = new ExposedPort_healthCheck();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ExposedPort_healthCheck>>())
            .Returns(testHealthCheck);
        deserializers["healthCheck"](parseNode);
        Assert.NotNull(model.HealthCheck);

        var testHosts = new List<string> { "example.com", "www.example.com" };
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(testHosts);
        deserializers["host"](parseNode);
        Assert.NotNull(model.Host);
        Assert.Equal(2, model.Host.Count);

        parseNode.GetIntValue().Returns(443);
        deserializers["loadBalancerPort"](parseNode);
        Assert.Equal(443, model.LoadBalancerPort);

        parseNode.GetIntValue().Returns(8080);
        deserializers["targetPort"](parseNode);
        Assert.Equal(8080, model.TargetPort);

        parseNode.GetBoolValue().Returns(true);
        deserializers["useLetsEncrypt"](parseNode);
        Assert.True(model.UseLetsEncrypt);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ExposedPort_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ExposedPort
        {
            HealthCheck = new ExposedPort_healthCheck
            {
                Path = "/health",
                IntervalSeconds = 10,
                TimeoutSeconds = 5
            },
            Host = new List<string> { "example.com", "www.example.com" },
            LoadBalancerPort = 443,
            TargetPort = 8080,
            UseLetsEncrypt = true
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ExposedPort.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.HealthCheck);
        Assert.NotNull(deserializedModel.Host);
        Assert.Equal(2, deserializedModel.Host.Count);
        Assert.Equal(originalModel.LoadBalancerPort, deserializedModel.LoadBalancerPort);
        Assert.Equal(originalModel.TargetPort, deserializedModel.TargetPort);
        Assert.Equal(originalModel.UseLetsEncrypt, deserializedModel.UseLetsEncrypt);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ExposedPort_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ExposedPort
        {
            TargetPort = 8080
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
        var deserializedModel = parseNode.GetObjectValue(ExposedPort.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
