using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using ProjectModel = Sacloud.Sdk.IAM.Models.Project;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// Projectのテスト
/// </summary>
public class ProjectTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ProjectModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ProjectModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.Project.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"code", "created_at", "description", "id", "name", "parent_folder_id", "status", "updated_at"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var project = new ProjectModel();

        var deserializers = project.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("parent_folder_id"));
        Assert.True(deserializers.ContainsKey("status"));
        Assert.True(deserializers.ContainsKey("updated_at"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var project = new ProjectModel();

        Assert.Throws<ArgumentNullException>(() => project.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"code", "created_at", "description", "id", "name", "parent_folder_id", "status", "updated_at"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var project = new ProjectModel();
        var deserializers = project.GetFieldDeserializers();
        
        var parseNodeCode = Substitute.For<IParseNode>();
        parseNodeCode.GetStringValue().Returns("PRJ001");
        deserializers["code"](parseNodeCode);
        
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetStringValue().Returns("2026-01-01T00:00:00Z");
        deserializers["created_at"](parseNodeCreatedAt);

        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Project");
        deserializers["description"](parseNodeDescription);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123L);
        deserializers["id"](parseNodeId);

        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("My Project");
        deserializers["name"](parseNodeName);

        var parseNodeParentFolderId = Substitute.For<IParseNode>();
        parseNodeParentFolderId.GetLongValue().Returns(456L);
        deserializers["parent_folder_id"](parseNodeParentFolderId);

        var parseNodeStatus = Substitute.For<IParseNode>();
        parseNodeStatus.GetEnumValue<IAM.Models.Project_status>().Returns(Sacloud.Sdk.IAM.Models.Project_status.Available);
        deserializers["status"](parseNodeStatus);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetStringValue().Returns("2026-02-01T00:00:00Z");
        deserializers["updated_at"](parseNodeUpdatedAt);

        Assert.Equal("PRJ001", project.Code);
        Assert.Equal("2026-01-01T00:00:00Z", project.CreatedAt);
        Assert.Equal("Test Project", project.Description);
        Assert.Equal(123L, project.Id);
        Assert.Equal("My Project", project.Name);
        Assert.Equal(456L, project.ParentFolderId);
        Assert.Equal(Sacloud.Sdk.IAM.Models.Project_status.Available, project.Status);
        Assert.Equal("2026-02-01T00:00:00Z", project.UpdatedAt);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Project_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ProjectModel
        {
            Code = "PRJ002",
            CreatedAt = "2026-01-15T10:30:00Z",
            Description = "Sample Project",
            Name = "Sample",
            UpdatedAt = "2026-02-05T14:20:00Z"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Project.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Code, deserializedModel.Code);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Project_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ProjectModel
        {
            Code = "PRJ003",
            Name = "Test Project with Additional Data"
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Project.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Code, deserializedModel.Code);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }


}
