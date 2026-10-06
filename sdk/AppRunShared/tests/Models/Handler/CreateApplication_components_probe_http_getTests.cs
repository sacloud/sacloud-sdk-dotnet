using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// CreateApplication_components_probe_http_getモデルのテスト
/// </summary>
public class CreateApplication_components_probe_http_getTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_probe_http_get_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateApplication_components_probe_http_get();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("headers"));
        Assert.True(deserializers.ContainsKey("path"));
        Assert.True(deserializers.ContainsKey("port"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_probe_http_get_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateApplication_components_probe_http_get();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateApplication_components_probe_http_get_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateApplication_components_probe_http_get();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var headersList = new List<CreateApplication_components_probe_http_get_headers>
        {
            new CreateApplication_components_probe_http_get_headers { Name = "Content-Type", Value = "application/json" }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<CreateApplication_components_probe_http_get_headers>>()).Returns(headersList);
        deserializers["headers"](parseNode);
        Assert.NotNull(model.Headers);
        Assert.Single(model.Headers);

        parseNode.GetStringValue().Returns("/health");
        deserializers["path"](parseNode);
        Assert.Equal("/health", model.Path);

        parseNode.GetIntValue().Returns(8080);
        deserializers["port"](parseNode);
        Assert.Equal(8080, model.Port);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_components_probe_http_get_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateApplication_components_probe_http_get
        {
            Headers = new List<CreateApplication_components_probe_http_get_headers> { new CreateApplication_components_probe_http_get_headers() },
            Path = "/health",
            Port = 8080
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplication_components_probe_http_get.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Headers);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.Equal(originalModel.Port, deserializedModel.Port);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_components_probe_http_get_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateApplication_components_probe_http_get { Path = "/health" };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplication_components_probe_http_get.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
    }
}
