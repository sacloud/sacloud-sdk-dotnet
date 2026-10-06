using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using FolderModel = Sacloud.Sdk.IAM.Models.Folder;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// Folderのテスト
/// </summary>
public class FolderTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = FolderModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<FolderModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.Folder.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"created_at", "description", "id", "name", "parent_id", "updated_at"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var folder = new FolderModel();

        var deserializers = folder.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("parent_id"));
        Assert.True(deserializers.ContainsKey("updated_at"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var folder = new FolderModel();

        Assert.Throws<ArgumentNullException>(() => folder.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"created_at", "description", "id", "name", "parent_id", "updated_at"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var folder = new FolderModel();
        var deserializers = folder.GetFieldDeserializers();
        
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetStringValue().Returns("2024-01-01T00:00:00Z");
        deserializers["created_at"](parseNodeCreatedAt);
        
        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123456L);
        deserializers["id"](parseNodeId);
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Folder");
        deserializers["name"](parseNodeName);
        
        var parseNodeParentId = Substitute.For<IParseNode>();
        parseNodeParentId.GetLongValue().Returns(789L);
        deserializers["parent_id"](parseNodeParentId);
        
        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetStringValue().Returns("2024-01-02T00:00:00Z");
        deserializers["updated_at"](parseNodeUpdatedAt);

        Assert.Equal("2024-01-01T00:00:00Z", folder.CreatedAt);
        Assert.Equal("Test Description", folder.Description);
        Assert.Equal(123456L, folder.Id);
        Assert.Equal("Test Folder", folder.Name);
        Assert.Equal(789L, folder.ParentId);
        Assert.Equal("2024-01-02T00:00:00Z", folder.UpdatedAt);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Folder_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new FolderModel
        {
            Name = "Test Folder",
            Description = "Test Description",
            ParentId = 789L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Folder.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.ParentId, deserializedModel.ParentId);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Folder_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new FolderModel
        {
            Name = "Test Folder",
            Description = "Test Description",
            ParentId = 789L
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Folder.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.ParentId, deserializedModel.ParentId);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }


}
