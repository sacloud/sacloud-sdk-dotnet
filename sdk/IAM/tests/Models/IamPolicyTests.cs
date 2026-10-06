using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using IamPolicyModel = Sacloud.Sdk.IAM.Models.IamPolicy;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IamPolicyのテスト
/// </summary>
public class IamPolicyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = IamPolicyModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IamPolicyModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.IamPolicy.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"principals", "role"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var iamPolicy = new IamPolicyModel();

        var deserializers = iamPolicy.GetFieldDeserializers();

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
        var iamPolicy = new IamPolicyModel();

        Assert.Throws<ArgumentNullException>(() => iamPolicy.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"principals", "role"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var iamPolicy = new IamPolicyModel();
        var deserializers = iamPolicy.GetFieldDeserializers();
        
        var principal1 = new Principal();
        var principal2 = new Principal();
        var principals = new List<Principal> { principal1, principal2 };
        
        var parseNodePrincipals = Substitute.For<IParseNode>();
        parseNodePrincipals.GetCollectionOfObjectValues(
            Arg.Any<ParsableFactory<Principal>>())
            .Returns(principals);
        deserializers["principals"](parseNodePrincipals);
        
        var role = new IamPolicy_role();
        var parseNodeRole = Substitute.For<IParseNode>();
        parseNodeRole.GetObjectValue(
            Arg.Any<ParsableFactory<IamPolicy_role>>())
            .Returns(role);
        deserializers["role"](parseNodeRole);

        Assert.NotNull(iamPolicy.Principals);
        Assert.Equal(2, iamPolicy.Principals.Count);
        Assert.NotNull(iamPolicy.Role);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IamPolicy_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IamPolicyModel
        {
            Principals =
            [
                new Principal(),
                new Principal()
            ],
            Role = new IamPolicy_role()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IamPolicyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Principals);
        Assert.Equal(2, deserializedModel.Principals.Count);
        Assert.NotNull(deserializedModel.Role);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IamPolicy_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IamPolicyModel
        {
            Principals =
            [
                new Principal()
            ],
            Role = new IamPolicy_role()
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
        var deserializedModel = parseNode.GetObjectValue(IamPolicyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Principals);
        Assert.Single(deserializedModel.Principals);
        Assert.NotNull(deserializedModel.Role);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void IamPolicy_Constructor_InitializesAdditionalData()
    {
        var iamPolicy = new IamPolicyModel();

        Assert.NotNull(iamPolicy.AdditionalData);
        Assert.Empty(iamPolicy.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataにデータを追加できることを確認
    /// </summary>
    [Fact]
    public void IamPolicy_AdditionalData_CanAddData()
    {
        var iamPolicy = new IamPolicyModel();

        iamPolicy.AdditionalData["custom"] = "value";

        Assert.Single(iamPolicy.AdditionalData);
        Assert.Equal("value", iamPolicy.AdditionalData["custom"]);
    }

    /// <summary>
    /// Serializeがprincipalsプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamPolicy_Serialize_WritesPrincipalsProperty()
    {
        var iamPolicy = new IamPolicyModel
        {
            Principals =
            [
                new Principal()
            ]
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamPolicy.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues("principals", iamPolicy.Principals);
    }

    /// <summary>
    /// Serializeがroleプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamPolicy_Serialize_WritesRoleProperty()
    {
        var iamPolicy = new IamPolicyModel
        {
            Role = new IamPolicy_role()
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamPolicy.Serialize(writer);

        writer.Received(1).WriteObjectValue("role", iamPolicy.Role);
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamPolicy_Serialize_WritesAdditionalData()
    {
        var iamPolicy = new IamPolicyModel();
        iamPolicy.AdditionalData["customKey"] = "customValue";
        var writer = Substitute.For<ISerializationWriter>();

        iamPolicy.Serialize(writer);

        writer.Received(1).WriteAdditionalData(iamPolicy.AdditionalData);
    }
}
