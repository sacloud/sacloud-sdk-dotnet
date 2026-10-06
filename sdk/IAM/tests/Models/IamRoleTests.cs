using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using IamRoleModel = Sacloud.Sdk.IAM.Models.IamRole;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IamRoleのテスト
/// </summary>
public class IamRoleTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = IamRoleModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IamRoleModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.IamRole.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"category", "description", "id", "lowest_grantable_resource", "name"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var iamRole = new IamRoleModel();

        var deserializers = iamRole.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("category"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("lowest_grantable_resource"));
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var iamRole = new IamRoleModel();

        Assert.Throws<ArgumentNullException>(() => iamRole.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"category", "description", "id", "lowest_grantable_resource", "name"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var iamRole = new IamRoleModel();
        var deserializers = iamRole.GetFieldDeserializers();
        
        var parseNodeCategory = Substitute.For<IParseNode>();
        parseNodeCategory.GetStringValue().Returns("Test Category");
        deserializers["category"](parseNodeCategory);
        
        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetStringValue().Returns("test-iam-role-id");
        deserializers["id"](parseNodeId);

        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test IAM Role");
        deserializers["name"](parseNodeName);

        var parseNodeLowest = Substitute.For<IParseNode>();
        parseNodeLowest.GetEnumValue<IamRole_lowest_grantable_resource>().Returns(Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder);
        deserializers["lowest_grantable_resource"](parseNodeLowest);

        Assert.Equal("Test Category", iamRole.Category);
        Assert.Equal("Test Description", iamRole.Description);
        Assert.Equal("test-iam-role-id", iamRole.Id);
        Assert.Equal("Test IAM Role", iamRole.Name);
        Assert.Equal(Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder, iamRole.LowestGrantableResource);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IamRole_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new IamRoleModel
        {
            Category = "Test Category",
            Description = "Test Description",
            Name = "Test IAM Role",
            LowestGrantableResource = Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder,
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IamRoleModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("Test Category", deserializedModel.Category);
        Assert.Equal("Test Description", deserializedModel.Description);
        Assert.Equal("Test IAM Role", deserializedModel.Name);
        Assert.Equal(Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder, deserializedModel.LowestGrantableResource);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task IamRole_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new IamRoleModel
        {
            Category = "Test Category",
            Description = "Test Description",
            Name = "Test IAM Role",
            LowestGrantableResource = Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder,
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
        var deserializedModel = parseNode.GetObjectValue(IamRoleModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("Test Category", deserializedModel.Category);
        Assert.Equal("Test Description", deserializedModel.Description);
        Assert.Equal("Test IAM Role", deserializedModel.Name);
        Assert.Equal(Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder, deserializedModel.LowestGrantableResource);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void IamRole_Constructor_InitializesAdditionalData()
    {
        var iamRole = new IamRoleModel();

        Assert.NotNull(iamRole.AdditionalData);
        Assert.Empty(iamRole.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataにデータを追加できることを確認
    /// </summary>
    [Fact]
    public void IamRole_AdditionalData_CanAddData()
    {
        var iamRole = new IamRoleModel();

        iamRole.AdditionalData["custom"] = "value";

        Assert.Single(iamRole.AdditionalData);
        Assert.Equal("value", iamRole.AdditionalData["custom"]);
    }

    /// <summary>
    /// Serializeがcategoryプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamRole_Serialize_WritesCategoryProperty()
    {
        var iamRole = new IamRoleModel
        {
            Category = "Test Category"
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamRole.Serialize(writer);

        writer.Received(1).WriteStringValue("category", "Test Category");
    }

    /// <summary>
    /// Serializeがdescriptionプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamRole_Serialize_WritesDescriptionProperty()
    {
        var iamRole = new IamRoleModel
        {
            Description = "Test Description"
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamRole.Serialize(writer);

        writer.Received(1).WriteStringValue("description", "Test Description");
    }

    /// <summary>
    /// Serializeがnameプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamRole_Serialize_WritesNameProperty()
    {
        var iamRole = new IamRoleModel
        {
            Name = "Test IAM Role"
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamRole.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "Test IAM Role");
    }

    /// <summary>
    /// Serializeがlowest_grantable_resourceプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamRole_Serialize_WritesLowestGrantableResourceProperty()
    {
        var iamRole = new IamRoleModel
        {
            LowestGrantableResource = Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder
        };
        var writer = Substitute.For<ISerializationWriter>();

        iamRole.Serialize(writer);

        writer.Received(1).WriteEnumValue<IamRole_lowest_grantable_resource>("lowest_grantable_resource", Sacloud.Sdk.IAM.Models.IamRole_lowest_grantable_resource.Folder);
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void IamRole_Serialize_WritesAdditionalData()
    {
        var iamRole = new IamRoleModel();
        iamRole.AdditionalData["customKey"] = "customValue";
        var writer = Substitute.For<ISerializationWriter>();

        iamRole.Serialize(writer);

        writer.Received(1).WriteAdditionalData(iamRole.AdditionalData);
    }
}
