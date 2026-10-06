using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Workflowモデルのテスト
/// </summary>
public class WorkflowTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Workflow_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Workflow.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Workflow>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Workflow_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Workflow.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Workflow_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Workflow();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(10, deserializers.Count);
        Assert.True(deserializers.ContainsKey("ConcurrencyMode"));
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("Id"));
        Assert.True(deserializers.ContainsKey("Logging"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Publish"));
        Assert.True(deserializers.ContainsKey("ServicePrincipalId"));
        Assert.True(deserializers.ContainsKey("Tags"));
        Assert.True(deserializers.ContainsKey("UpdatedAt"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Workflow_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Workflow();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Workflow_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Workflow();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetEnumValue<Workflow_ConcurrencyMode>().Returns(Workflow_ConcurrencyMode.Parallel);
        deserializers["ConcurrencyMode"](parseNode);
        Assert.Equal(Workflow_ConcurrencyMode.Parallel, model.ConcurrencyMode);

        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["CreatedAt"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        parseNode.GetStringValue().Returns("Test description");
        deserializers["Description"](parseNode);
        Assert.Equal("Test description", model.Description);

        parseNode.GetStringValue().Returns("workflow-123");
        deserializers["Id"](parseNode);
        Assert.Equal("workflow-123", model.Id);

        parseNode.GetBoolValue().Returns(true);
        deserializers["Logging"](parseNode);
        Assert.True(model.Logging);

        parseNode.GetStringValue().Returns("Test workflow");
        deserializers["Name"](parseNode);
        Assert.Equal("Test workflow", model.Name);

        parseNode.GetBoolValue().Returns(false);
        deserializers["Publish"](parseNode);
        Assert.False(model.Publish);

        parseNode.GetStringValue().Returns("sp-123");
        deserializers["ServicePrincipalId"](parseNode);
        Assert.Equal("sp-123", model.ServicePrincipalId);

        var testTags = new List<Tag> { new Tag() };
        parseNode.GetCollectionOfObjectValues<Tag>(Arg.Any<ParsableFactory<Tag>>()).Returns(testTags);
        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);

        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["UpdatedAt"](parseNode);
        Assert.Equal(testDate, model.UpdatedAt);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Workflow_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new Workflow();
        var testData = new Dictionary<string, object>
        {
            { "customField", "customValue" }
        };

        model.AdditionalData = testData;

        Assert.Equal(testData, model.AdditionalData);
        Assert.Equal("customValue", model.AdditionalData["customField"]);
    }
}
