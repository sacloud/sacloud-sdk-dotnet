using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ApplicationVersionSummaryモデルのテスト
/// </summary>
public class ApplicationVersionSummaryTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void ApplicationVersionDeploymentStatus_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ApplicationVersionSummary.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ApplicationVersionSummary>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ApplicationVersionDeploymentStatus_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            ApplicationVersionSummary.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ApplicationVersionDeploymentStatus_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ApplicationVersionSummary();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("activeNodeCount"));
        Assert.True(deserializers.ContainsKey("created"));
        Assert.True(deserializers.ContainsKey("image"));
        Assert.True(deserializers.ContainsKey("version"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ApplicationVersionDeploymentStatus_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ApplicationVersionSummary();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ApplicationVersionDeploymentStatus_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ApplicationVersionSummary();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(100L);
        deserializers["activeNodeCount"](parseNode);
        Assert.Equal(100L, model.ActiveNodeCount);

        parseNode.GetLongValue().Returns(1640000000L);
        deserializers["created"](parseNode);
        Assert.Equal(1640000000L, model.Created);

        parseNode.GetStringValue().Returns("test-image:latest");
        deserializers["image"](parseNode);
        Assert.Equal("test-image:latest", model.Image);

        parseNode.GetIntValue().Returns(42);
        deserializers["version"](parseNode);
        Assert.Equal(42, model.Version);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ApplicationVersionDeploymentStatus_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ApplicationVersionSummary
        {
            Image = "test-image:v1.0"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ApplicationVersionSummary.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Image, deserializedModel.Image);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ApplicationVersionDeploymentStatus_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ApplicationVersionSummary
        {
            Image = "test-image:v1.0"
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
        var deserializedModel = parseNode.GetObjectValue(ApplicationVersionSummary.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
