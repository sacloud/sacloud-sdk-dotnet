using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// PatchApplicationBodyモデルのテスト
/// </summary>
public class PatchApplicationBodyTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PatchApplicationBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("all_traffic_available"));
        Assert.True(deserializers.ContainsKey("components"));
        Assert.True(deserializers.ContainsKey("max_scale"));
        Assert.True(deserializers.ContainsKey("min_scale"));
        Assert.True(deserializers.ContainsKey("port"));
        Assert.True(deserializers.ContainsKey("scale_target_concurrency"));
        Assert.True(deserializers.ContainsKey("timeout_seconds"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PatchApplicationBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchApplicationBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Bool型フィールド: all_traffic_available
        parseNode.GetBoolValue().Returns(true);
        deserializers["all_traffic_available"](parseNode);
        Assert.True(model.AllTrafficAvailable);

        // Collection型フィールド: components
        var componentsList = new List<PatchApplicationBody_components>
        {
            new PatchApplicationBody_components { Name = "component-1" }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<PatchApplicationBody_components>>())
            .Returns(componentsList);
        deserializers["components"](parseNode);
        Assert.NotNull(model.Components);
        Assert.Single(model.Components);

        // Int型フィールド: max_scale
        parseNode.GetIntValue().Returns(10);
        deserializers["max_scale"](parseNode);
        Assert.Equal(10, model.MaxScale);

        // Int型フィールド: min_scale
        parseNode.GetIntValue().Returns(1);
        deserializers["min_scale"](parseNode);
        Assert.Equal(1, model.MinScale);

        // Int型フィールド: port
        parseNode.GetIntValue().Returns(8080);
        deserializers["port"](parseNode);
        Assert.Equal(8080, model.Port);

        // Int型フィールド: scale_target_concurrency
        parseNode.GetIntValue().Returns(100);
        deserializers["scale_target_concurrency"](parseNode);
        Assert.Equal(100, model.ScaleTargetConcurrency);

        // Int型フィールド: timeout_seconds
        parseNode.GetIntValue().Returns(300);
        deserializers["timeout_seconds"](parseNode);
        Assert.Equal(300, model.TimeoutSeconds);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchApplicationBody_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PatchApplicationBody
        {
            AllTrafficAvailable = true,
            Components = new List<PatchApplicationBody_components>
            {
                new PatchApplicationBody_components { Name = "component-1" }
            },
            MaxScale = 10,
            MinScale = 1,
            Port = 8080,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 300
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PatchApplicationBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AllTrafficAvailable, deserializedModel.AllTrafficAvailable);
        Assert.NotNull(deserializedModel.Components);
        Assert.Single(deserializedModel.Components);
        Assert.Equal(originalModel.Components[0].Name, deserializedModel.Components[0].Name);
        Assert.Equal(originalModel.MaxScale, deserializedModel.MaxScale);
        Assert.Equal(originalModel.MinScale, deserializedModel.MinScale);
        Assert.Equal(originalModel.Port, deserializedModel.Port);
        Assert.Equal(originalModel.ScaleTargetConcurrency, deserializedModel.ScaleTargetConcurrency);
        Assert.Equal(originalModel.TimeoutSeconds, deserializedModel.TimeoutSeconds);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchApplicationBody_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PatchApplicationBody
        {
            Port = 8080
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PatchApplicationBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
