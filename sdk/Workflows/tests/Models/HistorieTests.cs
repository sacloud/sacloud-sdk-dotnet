using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Historieモデルのテスト
/// </summary>
public class HistorieTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Historie_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Historie.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Historie>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Historie_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Historie.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Historie_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Historie();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("JobId"));
        Assert.True(deserializers.ContainsKey("Meta"));
        Assert.True(deserializers.ContainsKey("StackTrace"));
        Assert.True(deserializers.ContainsKey("ThreadId"));
        Assert.True(deserializers.ContainsKey("Type"));
        Assert.True(deserializers.ContainsKey("Variables"));
        Assert.True(deserializers.ContainsKey("WorkflowExecutionId"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Historie_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Historie();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Historie_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Historie();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["CreatedAt"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        parseNode.GetStringValue().Returns("job-123");
        deserializers["JobId"](parseNode);
        Assert.Equal("job-123", model.JobId);

        parseNode.GetStringValue().Returns("test-meta");
        deserializers["Meta"](parseNode);
        Assert.Equal("test-meta", model.Meta);

        parseNode.GetStringValue().Returns("stack-trace-info");
        deserializers["StackTrace"](parseNode);
        Assert.Equal("stack-trace-info", model.StackTrace);

        parseNode.GetStringValue().Returns("thread-456");
        deserializers["ThreadId"](parseNode);
        Assert.Equal("thread-456", model.ThreadId);

        parseNode.GetEnumValue<Historie_Type>().Returns(Historie_Type.WorkflowWillStart);
        deserializers["Type"](parseNode);
        Assert.Equal(Historie_Type.WorkflowWillStart, model.Type);

        parseNode.GetStringValue().Returns("variables-data");
        deserializers["Variables"](parseNode);
        Assert.Equal("variables-data", model.Variables);

        parseNode.GetStringValue().Returns("exec-789");
        deserializers["WorkflowExecutionId"](parseNode);
        Assert.Equal("exec-789", model.WorkflowExecutionId);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Historie_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new Historie();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Historie_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Historie
        {
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            JobId = "job-123",
            Meta = "test-meta",
            StackTrace = "stack-trace-info",
            ThreadId = "thread-456",
            Type = Historie_Type.WorkflowWillStart,
            Variables = "variables-data",
            WorkflowExecutionId = "exec-789"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Historie.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.JobId, deserializedModel.JobId);
        Assert.Equal(originalModel.Meta, deserializedModel.Meta);
        Assert.Equal(originalModel.StackTrace, deserializedModel.StackTrace);
        Assert.Equal(originalModel.ThreadId, deserializedModel.ThreadId);
        Assert.Equal(originalModel.Type, deserializedModel.Type);
        Assert.Equal(originalModel.Variables, deserializedModel.Variables);
        Assert.Equal(originalModel.WorkflowExecutionId, deserializedModel.WorkflowExecutionId);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Historie_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Historie
        {
            Type = Historie_Type.WorkflowWillStart
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
        var deserializedModel = parseNode.GetObjectValue(Historie.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}

