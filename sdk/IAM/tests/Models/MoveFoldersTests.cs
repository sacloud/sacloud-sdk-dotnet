using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// MoveFoldersモデルのテスト
/// </summary>
public class MoveFoldersTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void MoveFolders_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = global::Sacloud.Sdk.IAM.Models.MoveFolders.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IAM.Models.MoveFolders>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void MoveFolders_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            global::Sacloud.Sdk.IAM.Models.MoveFolders.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void MoveFolders_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new IAM.Models.MoveFolders();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("folder_ids"));
        Assert.True(deserializers.ContainsKey("parent_id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void MoveFolders_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new IAM.Models.MoveFolders();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MoveFolders_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new IAM.Models.MoveFolders();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var folderIds = new List<long?> { 1L, 2L, 3L };
        parseNode.GetCollectionOfPrimitiveValues<long?>().Returns(folderIds);
        deserializers["folder_ids"](parseNode);
        Assert.NotNull(model.FolderIds);
        Assert.Equal(3, model.FolderIds.Count);
        Assert.Contains(1L, model.FolderIds);
        Assert.Contains(2L, model.FolderIds);
        Assert.Contains(3L, model.FolderIds);

        parseNode.GetLongValue().Returns(100L);
        deserializers["parent_id"](parseNode);
        Assert.Equal(100L, model.ParentId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MoveFolders_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IAM.Models.MoveFolders
        {
            FolderIds = [1L, 2L, 3L],
            ParentId = 100L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(global::Sacloud.Sdk.IAM.Models.MoveFolders.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.FolderIds);
        Assert.Equal(3, deserializedModel.FolderIds.Count);
        Assert.Contains(1L, deserializedModel.FolderIds);
        Assert.Contains(2L, deserializedModel.FolderIds);
        Assert.Contains(3L, deserializedModel.FolderIds);
        Assert.Equal(originalModel.ParentId, deserializedModel.ParentId);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task MoveFolders_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IAM.Models.MoveFolders
        {
            FolderIds = [1L, 2L],
            ParentId = 50L
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
        var deserializedModel = parseNode.GetObjectValue(global::Sacloud.Sdk.IAM.Models.MoveFolders.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
