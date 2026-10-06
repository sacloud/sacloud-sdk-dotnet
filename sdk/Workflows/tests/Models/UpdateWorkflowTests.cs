using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// UpdateWorkflowモデルのテスト
/// </summary>
public class UpdateWorkflowTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = UpdateWorkflow.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UpdateWorkflow>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => UpdateWorkflow.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateWorkflow();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("ConcurrencyMode"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("Logging"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Publish"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateWorkflow();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new UpdateWorkflow();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UpdateWorkflow();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test description");
        deserializers["Description"](parseNode);
        Assert.Equal("test description", model.Description);

        parseNode.GetBoolValue().Returns(true);
        deserializers["Logging"](parseNode);
        Assert.True(model.Logging);

        parseNode.GetStringValue().Returns("workflow-name");
        deserializers["Name"](parseNode);
        Assert.Equal("workflow-name", model.Name);

        parseNode.GetBoolValue().Returns(false);
        deserializers["Publish"](parseNode);
        Assert.False(model.Publish);

        var tagsList = new List<Tag> { new Tag { Name = "tag1" } };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Tag>>()).Returns(tagsList);
        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);
        Assert.Single(model.Tags);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateWorkflow_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UpdateWorkflow
        {
            Description = "test description",
            Logging = true,
            Name = "workflow-name",
            Publish = false,
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
        var deserializedModel = parseNode.GetObjectValue(UpdateWorkflow.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Logging, deserializedModel.Logging);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Publish, deserializedModel.Publish);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Single(deserializedModel.Tags);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task UpdateWorkflow_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UpdateWorkflow
        {
            Name = "workflow-name"
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
        var deserializedModel = parseNode.GetObjectValue(UpdateWorkflow.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
