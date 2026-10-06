using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// CreateWorkflowモデルのテスト
/// </summary>
public class CreateWorkflowTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateWorkflow.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateWorkflow>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => CreateWorkflow.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateWorkflow();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(9, deserializers.Count);
        Assert.True(deserializers.ContainsKey("ConcurrencyMode"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("Logging"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Publish"));
        Assert.True(deserializers.ContainsKey("RevisionAlias"));
        Assert.True(deserializers.ContainsKey("Runbook"));
        Assert.True(deserializers.ContainsKey("ServicePrincipalId"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateWorkflow();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateWorkflow();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("Test description");
        deserializers["Description"](parseNode);
        Assert.Equal("Test description", model.Description);

        parseNode.GetBoolValue().Returns(true);
        deserializers["Logging"](parseNode);
        Assert.True(model.Logging);

        parseNode.GetStringValue().Returns("Test workflow");
        deserializers["Name"](parseNode);
        Assert.Equal("Test workflow", model.Name);

        parseNode.GetBoolValue().Returns(false);
        deserializers["Publish"](parseNode);
        Assert.False(model.Publish);

        parseNode.GetStringValue().Returns("latest");
        deserializers["RevisionAlias"](parseNode);
        Assert.Equal("latest", model.RevisionAlias);

        parseNode.GetStringValue().Returns("test-runbook");
        deserializers["Runbook"](parseNode);
        Assert.Equal("test-runbook", model.Runbook);

        parseNode.GetStringValue().Returns("sp-123");
        deserializers["ServicePrincipalId"](parseNode);
        Assert.Equal("sp-123", model.ServicePrincipalId);

        var tagsList = new List<Tag> { new Tag { Name = "tag1" } };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Tag>>()).Returns(tagsList);
        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);
        Assert.Single(model.Tags);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new CreateWorkflow();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateWorkflow_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateWorkflow
        {
            Description = "Test description",
            Logging = true,
            Name = "Test workflow",
            Publish = false,
            RevisionAlias = "latest",
            Runbook = "test-runbook",
            ServicePrincipalId = "sp-123",
            Tags = new List<Tag> { new Tag { Name = "tag1" } }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateWorkflow.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Logging, deserializedModel.Logging);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Publish, deserializedModel.Publish);
        Assert.Equal(originalModel.RevisionAlias, deserializedModel.RevisionAlias);
        Assert.Equal(originalModel.Runbook, deserializedModel.Runbook);
        Assert.Equal(originalModel.ServicePrincipalId, deserializedModel.ServicePrincipalId);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Single(deserializedModel.Tags);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateWorkflow_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateWorkflow
        {
            Name = "Test workflow"
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
        var deserializedModel = parseNode.GetObjectValue(CreateWorkflow.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}

