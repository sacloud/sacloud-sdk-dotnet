using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadVersion_components_probeモデルのテスト
/// </summary>
public class ReadVersion_components_probeTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadVersion_components_probe_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ReadVersion_components_probe();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("http_get"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadVersion_components_probe_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ReadVersion_components_probe();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadVersion_components_probe_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ReadVersion_components_probe();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Object型フィールド: http_get
        var httpGet = new ReadVersion_components_probe_http_get();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ReadVersion_components_probe_http_get>>())
            .Returns(httpGet);
        deserializers["http_get"](parseNode);
        Assert.NotNull(model.HttpGet);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadVersion_components_probe_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ReadVersion_components_probe
        {
            HttpGet = new ReadVersion_components_probe_http_get
            {
                Path = "/health",
                Port = 8080
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
        var deserializedModel = parseNode.GetObjectValue(ReadVersion_components_probe.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.HttpGet);
        Assert.Equal(originalModel.HttpGet.Path, deserializedModel.HttpGet.Path);
        Assert.Equal(originalModel.HttpGet.Port, deserializedModel.HttpGet.Port);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadVersion_components_probe_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ReadVersion_components_probe
        {
            HttpGet = new ReadVersion_components_probe_http_get
            {
                Path = "/health"
            }
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
        var deserializedModel = parseNode.GetObjectValue(ReadVersion_components_probe.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
