using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using IdPolicyModel = Sacloud.Sdk.IAM.Models.IdPolicy;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IdPolicyのテスト
/// </summary>
public class IdPolicyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = IdPolicyModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IdPolicyModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.IdPolicy.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"principals", "role"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var idPolicy = new IdPolicyModel();

        var deserializers = idPolicy.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("principals"));
        Assert.True(deserializers.ContainsKey("role"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var idPolicy = new IdPolicyModel();

        Assert.Throws<ArgumentNullException>(() => idPolicy.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"principals", "role"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var idPolicy = new IdPolicyModel();
        var deserializers = idPolicy.GetFieldDeserializers();
        
        var principalsList = new List<Principal>
        {
            new Principal()
        };
        var parseNodePrincipals = Substitute.For<IParseNode>();
        parseNodePrincipals.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Principal>>())
            .Returns(principalsList);
        deserializers["principals"](parseNodePrincipals);
        
        var testRole = new IdPolicy_role();
        var parseNodeRole = Substitute.For<IParseNode>();
        parseNodeRole.GetObjectValue(Arg.Any<ParsableFactory<IdPolicy_role>>())
            .Returns(testRole);
        deserializers["role"](parseNodeRole);

        Assert.NotNull(idPolicy.Principals);
        Assert.Single(idPolicy.Principals);
        Assert.NotNull(idPolicy.Role);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IdPolicy_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IdPolicyModel
        {
            Principals =
            [
                new Principal()
            ],
            Role = new IdPolicy_role
            {
                Id = "test-id-role-id"
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IdPolicyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Principals);
        Assert.Single(deserializedModel.Principals);
        Assert.NotNull(deserializedModel.Role);
        Assert.Equal("test-id-role-id", deserializedModel.Role.Id);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IdPolicy_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IdPolicyModel
        {
            Principals =
            [
                new Principal()
            ],
            Role = new IdPolicy_role
            {
                Id = "test-id-role-id"
            }
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
        var deserializedModel = parseNode.GetObjectValue(IdPolicyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Principals);
        Assert.Single(deserializedModel.Principals);
        Assert.NotNull(deserializedModel.Role);
        Assert.Equal("test-id-role-id", deserializedModel.Role.Id);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void IdPolicy_Constructor_InitializesAdditionalData()
    {
        var idPolicy = new IdPolicyModel();

        Assert.NotNull(idPolicy.AdditionalData);
        Assert.Empty(idPolicy.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataにデータを追加できることを確認
    /// </summary>
    [Fact]
    public void IdPolicy_AdditionalData_CanAddData()
    {
        var idPolicy = new IdPolicyModel();

        idPolicy.AdditionalData["custom"] = "value";

        Assert.Single(idPolicy.AdditionalData);
        Assert.Equal("value", idPolicy.AdditionalData["custom"]);
    }

    /// <summary>
    /// Serializeがprincipalsプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdPolicy_Serialize_WritesPrincipalsProperty()
    {
        var idPolicy = new IdPolicyModel
        {
            Principals =
            [
                new Principal()
            ]
        };
        var writer = Substitute.For<ISerializationWriter>();

        idPolicy.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues("principals", idPolicy.Principals);
    }

    /// <summary>
    /// Serializeがroleプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdPolicy_Serialize_WritesRoleProperty()
    {
        var idPolicy = new IdPolicyModel
        {
            Role = new IdPolicy_role()
        };
        var writer = Substitute.For<ISerializationWriter>();

        idPolicy.Serialize(writer);

        writer.Received(1).WriteObjectValue("role", idPolicy.Role);
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdPolicy_Serialize_WritesAdditionalData()
    {
        var idPolicy = new IdPolicyModel();
        idPolicy.AdditionalData["customKey"] = "customValue";
        var writer = Substitute.For<ISerializationWriter>();

        idPolicy.Serialize(writer);

        writer.Received(1).WriteAdditionalData(idPolicy.AdditionalData);
    }
}
