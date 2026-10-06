using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// MoveProjectsモデルのテスト
/// </summary>
public class MoveProjectsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void MoveProjects_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = global::Sacloud.Sdk.IAM.Models.MoveProjects.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IAM.Models.MoveProjects>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void MoveProjects_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            global::Sacloud.Sdk.IAM.Models.MoveProjects.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void MoveProjects_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new IAM.Models.MoveProjects();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("parent_folder_id"));
        Assert.True(deserializers.ContainsKey("project_ids"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void MoveProjects_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new IAM.Models.MoveProjects();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MoveProjects_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new IAM.Models.MoveProjects();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var projectIds = new List<long?> { 1L, 2L, 3L };
        parseNode.GetCollectionOfPrimitiveValues<long?>().Returns(projectIds);
        deserializers["project_ids"](parseNode);
        Assert.NotNull(model.ProjectIds);
        Assert.Equal(3, model.ProjectIds.Count);
        Assert.Contains(1L, model.ProjectIds);
        Assert.Contains(2L, model.ProjectIds);
        Assert.Contains(3L, model.ProjectIds);

        parseNode.GetLongValue().Returns(100L);
        deserializers["parent_folder_id"](parseNode);
        Assert.Equal(100L, model.ParentFolderId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MoveProjects_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IAM.Models.MoveProjects
        {
            ParentFolderId = 100L,
            ProjectIds = [1L, 2L, 3L]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(global::Sacloud.Sdk.IAM.Models.MoveProjects.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ParentFolderId, deserializedModel.ParentFolderId);
        Assert.NotNull(deserializedModel.ProjectIds);
        Assert.Equal(3, deserializedModel.ProjectIds.Count);
        Assert.Contains(1L, deserializedModel.ProjectIds);
        Assert.Contains(2L, deserializedModel.ProjectIds);
        Assert.Contains(3L, deserializedModel.ProjectIds);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MoveProjects_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IAM.Models.MoveProjects
        {
            ParentFolderId = 50L,
            ProjectIds = [1L, 2L]
        };
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(global::Sacloud.Sdk.IAM.Models.MoveProjects.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
