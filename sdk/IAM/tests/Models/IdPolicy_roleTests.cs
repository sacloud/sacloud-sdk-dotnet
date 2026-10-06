using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using IdPolicy_roleModel = Sacloud.Sdk.IAM.Models.IdPolicy_role;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IdPolicy_roleのテスト
/// </summary>
public class IdPolicy_roleTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = IdPolicy_roleModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IdPolicy_roleModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.IdPolicy_role.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"id", "type"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var idPolicyRole = new IdPolicy_roleModel();

        var deserializers = idPolicyRole.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("type"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var idPolicyRole = new IdPolicy_roleModel();

        Assert.Throws<ArgumentNullException>(() => idPolicyRole.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"id", "type"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var idPolicyRole = new IdPolicy_roleModel();
        var deserializers = idPolicyRole.GetFieldDeserializers();
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetStringValue().Returns("test-id-role-id");
        deserializers["id"](parseNodeId);
        
        var parseNodeType = Substitute.For<IParseNode>();
        parseNodeType.GetEnumValue<IdPolicy_role_type>().Returns(Sacloud.Sdk.IAM.Models.IdPolicy_role_type.Preset);
        deserializers["type"](parseNodeType);

        Assert.Equal("test-id-role-id", idPolicyRole.Id);
        Assert.Equal(Sacloud.Sdk.IAM.Models.IdPolicy_role_type.Preset, idPolicyRole.Type);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IdPolicy_role_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IdPolicy_roleModel
        {
            Id = "test-id-role-id",
            Type = IdPolicy_role_type.Preset
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IdPolicy_roleModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("test-id-role-id", deserializedModel.Id);
        Assert.Equal(IdPolicy_role_type.Preset, deserializedModel.Type);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IdPolicy_role_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IdPolicy_roleModel
        {
            Id = "test-id-role-id",
            Type = IdPolicy_role_type.Preset
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
        var deserializedModel = parseNode.GetObjectValue(IdPolicy_roleModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("test-id-role-id", deserializedModel.Id);
        Assert.Equal(IdPolicy_role_type.Preset, deserializedModel.Type);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void IdPolicy_role_Constructor_InitializesAdditionalData()
    {
        var idPolicyRole = new IdPolicy_roleModel();

        Assert.NotNull(idPolicyRole.AdditionalData);
        Assert.Empty(idPolicyRole.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataにデータを追加できることを確認
    /// </summary>
    [Fact]
    public void IdPolicy_role_AdditionalData_CanAddData()
    {
        var idPolicyRole = new IdPolicy_roleModel();

        idPolicyRole.AdditionalData["custom"] = "value";

        Assert.Single(idPolicyRole.AdditionalData);
        Assert.Equal("value", idPolicyRole.AdditionalData["custom"]);
    }

    /// <summary>
    /// Serializeがidプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdPolicy_role_Serialize_WritesIdProperty()
    {
        var idPolicyRole = new IdPolicy_roleModel
        {
            Id = "test-id-role-id"
        };
        var writer = Substitute.For<ISerializationWriter>();

        idPolicyRole.Serialize(writer);

        writer.Received(1).WriteStringValue("id", "test-id-role-id");
    }

    /// <summary>
    /// Serializeがtypeプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdPolicy_role_Serialize_WritesTypeProperty()
    {
        var idPolicyRole = new IdPolicy_roleModel
        {
            Type = Sacloud.Sdk.IAM.Models.IdPolicy_role_type.Preset
        };
        var writer = Substitute.For<ISerializationWriter>();

        idPolicyRole.Serialize(writer);

        writer.Received(1).WriteEnumValue<IdPolicy_role_type>("type", Sacloud.Sdk.IAM.Models.IdPolicy_role_type.Preset);
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdPolicy_role_Serialize_WritesAdditionalData()
    {
        var idPolicyRole = new IdPolicy_roleModel();
        idPolicyRole.AdditionalData["customKey"] = "customValue";
        var writer = Substitute.For<ISerializationWriter>();

        idPolicyRole.Serialize(writer);

        writer.Received(1).WriteAdditionalData(idPolicyRole.AdditionalData);
    }
}
