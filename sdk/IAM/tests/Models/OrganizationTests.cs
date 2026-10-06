using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using OrganizationModel = Sacloud.Sdk.IAM.Models.Organization;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// Organizationのテスト
/// </summary>
public class OrganizationTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OrganizationModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OrganizationModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.Organization.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"id", "name"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var org = new OrganizationModel();

        var deserializers = org.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var org = new OrganizationModel();

        Assert.Throws<ArgumentNullException>(() => org.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"id", "name"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var org = new OrganizationModel();
        var deserializers = org.GetFieldDeserializers();
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123456L);
        deserializers["id"](parseNodeId);
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Organization");
        deserializers["name"](parseNodeName);

        Assert.Equal(123456L, org.Id);
        Assert.Equal("Test Organization", org.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Organization_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new OrganizationModel
        {
            Name = "Test Organization"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Organization.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Organization_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new OrganizationModel
        {
            Name = "Test Organization"
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Organization.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
