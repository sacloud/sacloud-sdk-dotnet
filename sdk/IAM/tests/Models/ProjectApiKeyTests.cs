using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using ProjectApiKeyModel = Sacloud.Sdk.IAM.Models.ProjectApiKey;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ProjectApiKeyのテスト
/// </summary>
public class ProjectApiKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ProjectApiKeyModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ProjectApiKeyModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.ProjectApiKey.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"access_token", "created_at", "description", "iam_roles", "id", "name", "project_id", "server_resource_id", "updated_at", "zone_id"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var projectApiKey = new ProjectApiKeyModel();

        var deserializers = projectApiKey.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(10, deserializers.Count);
        Assert.True(deserializers.ContainsKey("access_token"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("iam_roles"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("project_id"));
        Assert.True(deserializers.ContainsKey("server_resource_id"));
        Assert.True(deserializers.ContainsKey("updated_at"));
        Assert.True(deserializers.ContainsKey("zone_id"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var projectApiKey = new ProjectApiKeyModel();

        Assert.Throws<ArgumentNullException>(() => projectApiKey.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"access_token", "created_at", "description", "iam_roles", "id", "name", "project_id", "server_resource_id", "updated_at", "zone_id"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var projectApiKey = new ProjectApiKeyModel();
        var deserializers = projectApiKey.GetFieldDeserializers();
        
        var parseNodeAccessToken = Substitute.For<IParseNode>();
        parseNodeAccessToken.GetStringValue().Returns("token123");
        deserializers["access_token"](parseNodeAccessToken);
        
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetStringValue().Returns("2026-01-01T00:00:00Z");
        deserializers["created_at"](parseNodeCreatedAt);

        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test API Key");
        deserializers["description"](parseNodeDescription);

        var parseNodeIamRoles = Substitute.For<IParseNode>();
        parseNodeIamRoles.GetCollectionOfPrimitiveValues<string>().Returns(new List<string> { "role1", "role2" });
        deserializers["iam_roles"](parseNodeIamRoles);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(789L);
        deserializers["id"](parseNodeId);

        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("My API Key");
        deserializers["name"](parseNodeName);

        var parseNodeProjectId = Substitute.For<IParseNode>();
        parseNodeProjectId.GetLongValue().Returns(101L);
        deserializers["project_id"](parseNodeProjectId);

        var parseNodeServerResourceId = Substitute.For<IParseNode>();
        parseNodeServerResourceId.GetStringValue().Returns("server001");
        deserializers["server_resource_id"](parseNodeServerResourceId);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetStringValue().Returns("2026-02-01T00:00:00Z");
        deserializers["updated_at"](parseNodeUpdatedAt);

        var parseNodeZoneId = Substitute.For<IParseNode>();
        parseNodeZoneId.GetStringValue().Returns("zone01");
        deserializers["zone_id"](parseNodeZoneId);

        Assert.Equal("token123", projectApiKey.AccessToken);
        Assert.Equal("2026-01-01T00:00:00Z", projectApiKey.CreatedAt);
        Assert.Equal("Test API Key", projectApiKey.Description);
        Assert.NotNull(projectApiKey.IamRoles);
        Assert.Equal(2, projectApiKey.IamRoles.Count);
        Assert.Equal(789L, projectApiKey.Id);
        Assert.Equal("My API Key", projectApiKey.Name);
        Assert.Equal(101L, projectApiKey.ProjectId);
        Assert.Equal("server001", projectApiKey.ServerResourceId);
        Assert.Equal("2026-02-01T00:00:00Z", projectApiKey.UpdatedAt);
        Assert.Equal("zone01", projectApiKey.ZoneId);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ProjectApiKey_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ProjectApiKeyModel
        {
            AccessToken = "token456",
            CreatedAt = "2026-01-15T10:30:00Z",
            Description = "Sample API Key",
            IamRoles = ["admin", "viewer"],
            Name = "Sample Key",
            ProjectId = 202L,
            ServerResourceId = "server002",
            UpdatedAt = "2026-02-05T14:20:00Z",
            ZoneId = "zone02"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.ProjectApiKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccessToken, deserializedModel.AccessToken);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.NotNull(deserializedModel.IamRoles);
        Assert.Equal(originalModel.IamRoles.Count, deserializedModel.IamRoles.Count);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.ProjectId, deserializedModel.ProjectId);
        Assert.Equal(originalModel.ServerResourceId, deserializedModel.ServerResourceId);
        Assert.Equal(originalModel.UpdatedAt, deserializedModel.UpdatedAt);
        Assert.Equal(originalModel.ZoneId, deserializedModel.ZoneId);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task ProjectApiKey_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ProjectApiKeyModel
        {
            AccessToken = "token789",
            Name = "Test Key with Additional Data",
            ProjectId = 303L
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.ProjectApiKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccessToken, deserializedModel.AccessToken);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.ProjectId, deserializedModel.ProjectId);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }


}
