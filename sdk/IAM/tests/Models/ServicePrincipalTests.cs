using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using ServicePrincipalModel = Sacloud.Sdk.IAM.Models.ServicePrincipal;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalのテスト
/// </summary>
public class ServicePrincipalTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ServicePrincipalModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicePrincipalModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServicePrincipalModel.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ServicePrincipalModel();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("project_id"));
        Assert.True(deserializers.ContainsKey("updated_at"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServicePrincipalModel();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ServicePrincipalModel();
        var deserializers = model.GetFieldDeserializers();
        
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
        parseNodeName.GetStringValue().Returns("Test Name");
        deserializers["name"](parseNodeName);
        
        var parseNodeProjectId = Substitute.For<IParseNode>();
        parseNodeProjectId.GetLongValue().Returns(789L);
        deserializers["project_id"](parseNodeProjectId);
        
        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetStringValue().Returns("2024-01-02T00:00:00Z");
        deserializers["updated_at"](parseNodeUpdatedAt);

        Assert.Equal("2024-01-01T00:00:00Z", model.CreatedAt);
        Assert.Equal("Test Description", model.Description);
        Assert.Equal(123456L, model.Id);
        Assert.Equal("Test Name", model.Name);
        Assert.Equal(789L, model.ProjectId);
        Assert.Equal("2024-01-02T00:00:00Z", model.UpdatedAt);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipal_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ServicePrincipalModel
        {
            CreatedAt = "2024-01-01T00:00:00Z",
            Description = "Test Description",
            Name = "Test Name",
            ProjectId = 789L,
            UpdatedAt = "2024-01-02T00:00:00Z"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.ProjectId, deserializedModel.ProjectId);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ServicePrincipal_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ServicePrincipalModel
        {
            Name = "Test Name"
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
        var deserializedModel = parseNode.GetObjectValue(ServicePrincipalModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }


}
