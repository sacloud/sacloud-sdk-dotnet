using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadApplicationモデルのテスト
/// </summary>
public class ReadApplicationTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadApplication_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ReadApplication();
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
    public void ReadApplication_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ReadApplication();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadApplication_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ReadApplication();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Collection型フィールド: components
        var componentsList = new List<ReadApplication_components> { new ReadApplication_components { Name = "component-1" } };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ReadApplication_components>>()).Returns(componentsList);
        deserializers["components"](parseNode);
        Assert.NotNull(model.Components);
        Assert.Single(model.Components);

        // DateTimeOffset型フィールド: created_at
        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["created_at"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        // String型フィールド: id
        parseNode.GetStringValue().Returns("app-123");
        deserializers["id"](parseNode);
        Assert.Equal("app-123", model.Id);

        // Int型フィールド: max_scale
        parseNode.GetIntValue().Returns(10);
        deserializers["max_scale"](parseNode);
        Assert.Equal(10, model.MaxScale);

        // Int型フィールド: min_scale
        parseNode.GetIntValue().Returns(1);
        deserializers["min_scale"](parseNode);
        Assert.Equal(1, model.MinScale);

        // String型フィールド: name
        parseNode.GetStringValue().Returns("app-name");
        deserializers["name"](parseNode);
        Assert.Equal("app-name", model.Name);

        // Int型フィールド: port
        parseNode.GetIntValue().Returns(8080);
        deserializers["port"](parseNode);
        Assert.Equal(8080, model.Port);

        // String型フィールド: public_url
        parseNode.GetStringValue().Returns("https://example.com");
        deserializers["public_url"](parseNode);
        Assert.Equal("https://example.com", model.PublicUrl);

        // String型フィールド: resource_id
        parseNode.GetStringValue().Returns("resource-123");
        deserializers["resource_id"](parseNode);
        Assert.Equal("resource-123", model.ResourceId);

        // Int型フィールド: scale_target_concurrency
        parseNode.GetIntValue().Returns(100);
        deserializers["scale_target_concurrency"](parseNode);
        Assert.Equal(100, model.ScaleTargetConcurrency);

        // Enum型フィールド: status
        parseNode.GetEnumValue<ReadApplication_status>().Returns(ReadApplication_status.Healthy);
        deserializers["status"](parseNode);
        Assert.Equal(ReadApplication_status.Healthy, model.Status);

        // Int型フィールド: timeout_seconds
        parseNode.GetIntValue().Returns(300);
        deserializers["timeout_seconds"](parseNode);
        Assert.Equal(300, model.TimeoutSeconds);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadApplication_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ReadApplication
        {
            Components = new List<ReadApplication_components> { new ReadApplication_components { Name = "component-1" } },
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            Id = "app-123",
            MaxScale = 10,
            MinScale = 1,
            Name = "app-name",
            Port = 8080,
            PublicUrl = "https://example.com",
            ResourceId = "resource-123",
            ScaleTargetConcurrency = 100,
            Status = ReadApplication_status.Healthy,
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
        var deserializedModel = parseNode.GetObjectValue(ReadApplication.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Components);
        Assert.Single(deserializedModel.Components);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.MaxScale, deserializedModel.MaxScale);
        Assert.Equal(originalModel.MinScale, deserializedModel.MinScale);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Port, deserializedModel.Port);
        Assert.Equal(originalModel.PublicUrl, deserializedModel.PublicUrl);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
        Assert.Equal(originalModel.ScaleTargetConcurrency, deserializedModel.ScaleTargetConcurrency);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
        Assert.Equal(originalModel.TimeoutSeconds, deserializedModel.TimeoutSeconds);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadApplication_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ReadApplication
        {
            Name = "app-name"
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
        var deserializedModel = parseNode.GetObjectValue(ReadApplication.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
