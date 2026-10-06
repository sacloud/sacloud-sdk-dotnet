using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// UserGroupDetailUpdateMember1のテスト
/// </summary>
public class UserGroupDetailUpdateMember1Tests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new UserGroupDetailUpdateMember1();
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
        var result = UserGroupDetailUpdateMember1.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UserGroupDetailUpdateMember1>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UserGroupDetailUpdateMember1.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"id", "isAssigned"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new UserGroupDetailUpdateMember1();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("isAssigned"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UserGroupDetailUpdateMember1();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new UserGroupDetailUpdateMember1();
        var deserializers = model.GetFieldDeserializers();
        var id = Guid.NewGuid();

        var idNode = Substitute.For<IParseNode>();
        idNode.GetGuidValue().Returns(id);
        deserializers["id"](idNode);

        var isAssignedNode = Substitute.For<IParseNode>();
        isAssignedNode.GetBoolValue().Returns(true);
        deserializers["isAssigned"](isAssignedNode);

        Assert.Equal(id, model.Id);
        Assert.True(model.IsAssigned);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserGroupDetailUpdateMember1_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UserGroupDetailUpdateMember1
        {
            Id = Guid.NewGuid(),
            IsAssigned = true,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserGroupDetailUpdateMember1.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.IsAssigned, deserializedModel.IsAssigned);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserGroupDetailUpdateMember1_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UserGroupDetailUpdateMember1
        {
            Id = Guid.NewGuid(),
            IsAssigned = true,
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
        var deserializedModel = parseNode.GetObjectValue(UserGroupDetailUpdateMember1.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.IsAssigned, deserializedModel.IsAssigned);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
