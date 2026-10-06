using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// UserGroupDetailのテスト
/// </summary>
public class UserGroupDetailTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new UserGroupDetail();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = UserGroupDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UserGroupDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UserGroupDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"id", "isAssigned", "name"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new UserGroupDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("isAssigned"));
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UserGroupDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new UserGroupDetail();
        var deserializers = model.GetFieldDeserializers();
        var id = Guid.NewGuid();

        var idNode = Substitute.For<IParseNode>();
        idNode.GetGuidValue().Returns(id);
        deserializers["id"](idNode);

        var isAssignedNode = Substitute.For<IParseNode>();
        isAssignedNode.GetBoolValue().Returns(true);
        deserializers["isAssigned"](isAssignedNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("group-detail");
        deserializers["name"](nameNode);

        Assert.Equal(id, model.Id);
        Assert.True(model.IsAssigned);
        Assert.Equal("group-detail", model.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserGroupDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UserGroupDetail
        {
            Id = Guid.NewGuid(),
            IsAssigned = true,
            Name = "group-detail",
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserGroupDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.IsAssigned, deserializedModel.IsAssigned);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserGroupDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UserGroupDetail
        {
            Id = Guid.NewGuid(),
            IsAssigned = true,
            Name = "group-detail",
        };
        originalModel.AdditionalData["customField"] = "customValue";
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserGroupDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.IsAssigned, deserializedModel.IsAssigned);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
