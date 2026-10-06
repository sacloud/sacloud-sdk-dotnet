using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// CreateApplicationモデルのテスト
/// </summary>
public class CreateApplicationTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateApplication();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(12, deserializers.Count);
        Assert.True(deserializers.ContainsKey("components"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("max_scale"));
        Assert.True(deserializers.ContainsKey("min_scale"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("port"));
        Assert.True(deserializers.ContainsKey("public_url"));
        Assert.True(deserializers.ContainsKey("resource_id"));
        Assert.True(deserializers.ContainsKey("scale_target_concurrency"));
        Assert.True(deserializers.ContainsKey("status"));
        Assert.True(deserializers.ContainsKey("timeout_seconds"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateApplication_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateApplication();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateApplication_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateApplication();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var componentsList = new List<CreateApplication_components> { new CreateApplication_components() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<CreateApplication_components>>()).Returns(componentsList);
        deserializers["components"](parseNode);
        Assert.NotNull(model.Components);

        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["created_at"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        parseNode.GetStringValue().Returns("app-123");
        deserializers["id"](parseNode);
        Assert.Equal("app-123", model.Id);

        parseNode.GetIntValue().Returns(10);
        deserializers["max_scale"](parseNode);
        Assert.Equal(10, model.MaxScale);

        parseNode.GetIntValue().Returns(1);
        deserializers["min_scale"](parseNode);
        Assert.Equal(1, model.MinScale);

        parseNode.GetStringValue().Returns("app-name");
        deserializers["name"](parseNode);
        Assert.Equal("app-name", model.Name);

        parseNode.GetIntValue().Returns(8080);
        deserializers["port"](parseNode);
        Assert.Equal(8080, model.Port);

        parseNode.GetStringValue().Returns("https://example.com");
        deserializers["public_url"](parseNode);
        Assert.Equal("https://example.com", model.PublicUrl);

        parseNode.GetStringValue().Returns("resource-123");
        deserializers["resource_id"](parseNode);
        Assert.Equal("resource-123", model.ResourceId);

        parseNode.GetIntValue().Returns(100);
        deserializers["scale_target_concurrency"](parseNode);
        Assert.Equal(100, model.ScaleTargetConcurrency);

        parseNode.GetEnumValue<CreateApplication_status>().Returns(CreateApplication_status.Healthy);
        deserializers["status"](parseNode);
        Assert.Equal(CreateApplication_status.Healthy, model.Status);

        parseNode.GetIntValue().Returns(300);
        deserializers["timeout_seconds"](parseNode);
        Assert.Equal(300, model.TimeoutSeconds);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateApplication
        {
            Components = new List<CreateApplication_components>(),
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            Id = "app-123",
            MaxScale = 10,
            MinScale = 1,
            Name = "app-name",
            Port = 8080,
            PublicUrl = "https://example.com",
            ResourceId = "resource-123",
            ScaleTargetConcurrency = 100,
            Status = CreateApplication_status.Healthy,
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
        var deserializedModel = parseNode.GetObjectValue(CreateApplication.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateApplication_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateApplication { Name = "app-name" };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateApplication.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
    }
}
