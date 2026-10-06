using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// DeleteWorkflowRevisionAliasResponseモデルのテスト
/// </summary>
public class DeleteWorkflowRevisionAliasResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void DeleteWorkflowRevisionAliasResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = DeleteWorkflowRevisionAliasResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<DeleteWorkflowRevisionAliasResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DeleteWorkflowRevisionAliasResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => DeleteWorkflowRevisionAliasResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void DeleteWorkflowRevisionAliasResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new DeleteWorkflowRevisionAliasResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("is_ok"));
        Assert.True(deserializers.ContainsKey("Revision"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DeleteWorkflowRevisionAliasResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new DeleteWorkflowRevisionAliasResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void DeleteWorkflowRevisionAliasResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new DeleteWorkflowRevisionAliasResponse();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void DeleteWorkflowRevisionAliasResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new DeleteWorkflowRevisionAliasResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.True(model.IsOk);

        var revision = new Revision { RevisionId = 1 };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Revision>>()).Returns(revision);
        deserializers["Revision"](parseNode);
        Assert.NotNull(model.Revision);
        Assert.Equal(1, model.Revision.RevisionId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task DeleteWorkflowRevisionAliasResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new DeleteWorkflowRevisionAliasResponse
        {
            IsOk = true,
            Revision = new Revision { RevisionId = 1 }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(DeleteWorkflowRevisionAliasResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.IsOk, deserializedModel.IsOk);
        Assert.NotNull(deserializedModel.Revision);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task DeleteWorkflowRevisionAliasResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new DeleteWorkflowRevisionAliasResponse
        {
            IsOk = true
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
        var deserializedModel = parseNode.GetObjectValue(DeleteWorkflowRevisionAliasResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
