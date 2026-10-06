using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Executionモデルのテスト
/// </summary>
public class ExecutionTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Execution_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Execution.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Execution>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Execution_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Execution.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Execution_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Execution();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(17, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Args"));
        Assert.True(deserializers.ContainsKey("CanceledAt"));
        Assert.True(deserializers.ContainsKey("CancelRequestedAt"));
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Error"));
        Assert.True(deserializers.ContainsKey("ExecutionId"));
        Assert.True(deserializers.ContainsKey("FailedAt"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Result"));
        Assert.True(deserializers.ContainsKey("Revision"));
        Assert.True(deserializers.ContainsKey("RevisionAlias"));
        Assert.True(deserializers.ContainsKey("RunAt"));
        Assert.True(deserializers.ContainsKey("Status"));
        Assert.True(deserializers.ContainsKey("StepCount"));
        Assert.True(deserializers.ContainsKey("SucceededAt"));
        Assert.True(deserializers.ContainsKey("UpdatedAt"));
        Assert.True(deserializers.ContainsKey("Workflow"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Execution_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Execution();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Execution_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Execution();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-args");
        deserializers["Args"](parseNode);
        Assert.Equal("test-args", model.Args);

        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["CanceledAt"](parseNode);
        Assert.Equal(testDate, model.CanceledAt);

        parseNode.GetStringValue().Returns("execution-123");
        deserializers["ExecutionId"](parseNode);
        Assert.Equal("execution-123", model.ExecutionId);

        parseNode.GetIntValue().Returns(5);
        deserializers["Revision"](parseNode);
        Assert.Equal(5, model.Revision);

        parseNode.GetEnumValue<Execution_Status>().Returns(Execution_Status.Running);
        deserializers["Status"](parseNode);
        Assert.Equal(Execution_Status.Running, model.Status);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Execution_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new Execution();
        var testData = new Dictionary<string, object>
        {
            { "customField", "customValue" }
        };

        model.AdditionalData = testData;

        Assert.Equal(testData, model.AdditionalData);
        Assert.Equal("customValue", model.AdditionalData["customField"]);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Execution_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Execution
        {
            Args = "test-args",
            CanceledAt = DateTimeOffset.Parse("2024-01-10T08:00:00Z"),
            CancelRequestedAt = DateTimeOffset.Parse("2024-01-10T07:55:00Z"),
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            Error = "error message",
            ExecutionId = "execution-123",
            FailedAt = DateTimeOffset.Parse("2024-01-10T09:00:00Z"),
            Name = "execution-name",
            Result = "result data",
            Revision = 5,
            RevisionAlias = "latest",
            RunAt = DateTimeOffset.Parse("2024-01-10T08:30:00Z"),
            Status = Execution_Status.Running,
            SucceededAt = DateTimeOffset.Parse("2024-01-10T08:45:00Z"),
            UpdatedAt = DateTimeOffset.Parse("2024-01-15T11:00:00Z"),
            Workflow = new Workflow { Name = "test-workflow" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Execution.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Args, deserializedModel.Args);
        Assert.Equal(originalModel.CanceledAt, deserializedModel.CanceledAt);
        Assert.Equal(originalModel.CancelRequestedAt, deserializedModel.CancelRequestedAt);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Error, deserializedModel.Error);
        Assert.Equal(originalModel.ExecutionId, deserializedModel.ExecutionId);
        Assert.Equal(originalModel.FailedAt, deserializedModel.FailedAt);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Result, deserializedModel.Result);
        Assert.Equal(originalModel.Revision, deserializedModel.Revision);
        Assert.Equal(originalModel.RevisionAlias, deserializedModel.RevisionAlias);
        Assert.Equal(originalModel.RunAt, deserializedModel.RunAt);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
        Assert.Equal(originalModel.SucceededAt, deserializedModel.SucceededAt);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
        Assert.NotNull(deserializedModel.Workflow);
        Assert.Equal(originalModel.Workflow.Name, deserializedModel.Workflow.Name);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Execution_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Execution
        {
            ExecutionId = "execution-123",
            Name = "execution-name"
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
        var deserializedModel = parseNode.GetObjectValue(Execution.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}

