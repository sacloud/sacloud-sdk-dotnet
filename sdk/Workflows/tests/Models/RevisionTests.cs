using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Revisionモデルのテスト
/// </summary>
public class RevisionTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Revision_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Revision.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Revision>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Revision_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Revision.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Revision_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Revision();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("RevisionAlias"));
        Assert.True(deserializers.ContainsKey("RevisionId"));
        Assert.True(deserializers.ContainsKey("Runbook"));
        Assert.True(deserializers.ContainsKey("UpdatedAt"));
        Assert.True(deserializers.ContainsKey("WorkflowId"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Revision_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Revision();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Revision_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Revision();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testDate1 = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate1);
        deserializers["CreatedAt"](parseNode);
        Assert.Equal(testDate1, model.CreatedAt);

        parseNode.GetStringValue().Returns("latest");
        deserializers["RevisionAlias"](parseNode);
        Assert.Equal("latest", model.RevisionAlias);

        parseNode.GetIntValue().Returns(5);
        deserializers["RevisionId"](parseNode);
        Assert.Equal(5, model.RevisionId);

        parseNode.GetStringValue().Returns("test-runbook");
        deserializers["Runbook"](parseNode);
        Assert.Equal("test-runbook", model.Runbook);

        var testDate2 = DateTimeOffset.Parse("2024-01-16T12:00:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate2);
        deserializers["UpdatedAt"](parseNode);
        Assert.Equal(testDate2, model.UpdatedAt);

        parseNode.GetStringValue().Returns("workflow-123");
        deserializers["WorkflowId"](parseNode);
        Assert.Equal("workflow-123", model.WorkflowId);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Revision_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new Revision();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Revision_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Revision
        {
            CreatedAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z"),
            RevisionAlias = "latest",
            RevisionId = 5,
            Runbook = "test-runbook",
            UpdatedAt = DateTimeOffset.Parse("2024-01-16T12:00:00Z"),
            WorkflowId = "workflow-123"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Revision.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.RevisionAlias, deserializedModel.RevisionAlias);
        Assert.Equal(originalModel.RevisionId, deserializedModel.RevisionId);
        Assert.Equal(originalModel.Runbook, deserializedModel.Runbook);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
        Assert.Equal(originalModel.WorkflowId, deserializedModel.WorkflowId);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Revision_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Revision
        {
            RevisionId = 5
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
        var deserializedModel = parseNode.GetObjectValue(Revision.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}

