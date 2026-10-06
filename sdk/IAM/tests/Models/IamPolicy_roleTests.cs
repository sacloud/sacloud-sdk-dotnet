using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using IamPolicy_roleModel = Sacloud.Sdk.IAM.Models.IamPolicy_role;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IamPolicy_roleのテスト
/// </summary>
public class IamPolicy_roleTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = IamPolicy_roleModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IamPolicy_roleModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.IamPolicy_role.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"id", "type"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var iamPolicyRole = new IamPolicy_roleModel();

        var deserializers = iamPolicyRole.GetFieldDeserializers();

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
        var iamPolicyRole = new IamPolicy_roleModel();

        Assert.Throws<ArgumentNullException>(() => iamPolicyRole.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"id", "type"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var iamPolicyRole = new IamPolicy_roleModel();
        var deserializers = iamPolicyRole.GetFieldDeserializers();
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetStringValue().Returns("test-iam-role-id");
        deserializers["id"](parseNodeId);
        
        var parseNodeType = Substitute.For<IParseNode>();
        parseNodeType.GetEnumValue<IamPolicy_role_type>().Returns(Sacloud.Sdk.IAM.Models.IamPolicy_role_type.Preset);
        deserializers["type"](parseNodeType);

        Assert.Equal("test-iam-role-id", iamPolicyRole.Id);
        Assert.Equal(Sacloud.Sdk.IAM.Models.IamPolicy_role_type.Preset, iamPolicyRole.Type);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IamPolicy_role_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IamPolicy_roleModel
        {
            Id = "test-iam-role-id",
            Type = IamPolicy_role_type.Preset
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IamPolicy_roleModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("test-iam-role-id", deserializedModel.Id);
        Assert.Equal(IamPolicy_role_type.Preset, deserializedModel.Type);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IamPolicy_role_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IamPolicy_roleModel
        {
            Id = "test-iam-role-id",
            Type = IamPolicy_role_type.Preset
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
        var deserializedModel = parseNode.GetObjectValue(IamPolicy_roleModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("test-iam-role-id", deserializedModel.Id);
        Assert.Equal(IamPolicy_role_type.Preset, deserializedModel.Type);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void IamPolicy_role_Constructor_InitializesAdditionalData()
    {
        var iamPolicyRole = new IamPolicy_roleModel();

        Assert.NotNull(iamPolicyRole.AdditionalData);
        Assert.Empty(iamPolicyRole.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataにデータを追加できることを確認
    /// </summary>
    [Fact]
    public void IamPolicy_role_AdditionalData_CanAddData()
    {
        var iamPolicyRole = new IamPolicy_roleModel();

        iamPolicyRole.AdditionalData["custom"] = "value";

        Assert.Single(iamPolicyRole.AdditionalData);
        Assert.Equal("value", iamPolicyRole.AdditionalData["custom"]);
    }

    /// <summary>
    /// Serializeがidプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamPolicy_role_Serialize_WritesIdProperty()
    {
        var iamPolicyRole = new IamPolicy_roleModel
        {
            Id = "test-iam-role-id"
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamPolicyRole.Serialize(writer);

        writer.Received(1).WriteStringValue("id", "test-iam-role-id");
    }

    /// <summary>
    /// Serializeがtypeプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamPolicy_role_Serialize_WritesTypeProperty()
    {
        var iamPolicyRole = new IamPolicy_roleModel
        {
            Type = Sacloud.Sdk.IAM.Models.IamPolicy_role_type.Preset
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamPolicyRole.Serialize(writer);

        writer.Received(1).WriteEnumValue<IamPolicy_role_type>("type", Sacloud.Sdk.IAM.Models.IamPolicy_role_type.Preset);
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamPolicy_role_Serialize_WritesAdditionalData()
    {
        var iamPolicyRole = new IamPolicy_roleModel();
        iamPolicyRole.AdditionalData["customKey"] = "customValue";
        var writer = Substitute.For<ISerializationWriter>();

        iamPolicyRole.Serialize(writer);

        writer.Received(1).WriteAdditionalData(iamPolicyRole.AdditionalData);
    }
}
