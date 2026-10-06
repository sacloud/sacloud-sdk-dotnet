using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Projects;

namespace Sacloud.Sdk.IAM.Tests.Projects;

/// <summary>
/// ProjectsPostRequestBodyのテスト
/// </summary>
public class ProjectsPostRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ProjectsPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ProjectsPostRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ProjectsPostRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new ProjectsPostRequestBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("parent_folder_id"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new ProjectsPostRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var body = new ProjectsPostRequestBody();
        var deserializers = body.GetFieldDeserializers();

        var parseNodeCode = Substitute.For<IParseNode>();
        parseNodeCode.GetStringValue().Returns("test-project");
        deserializers["code"](parseNodeCode);

        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);

        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Project");
        deserializers["name"](parseNodeName);

        var parseNodeParentFolderId = Substitute.For<IParseNode>();
        parseNodeParentFolderId.GetLongValue().Returns(100L);
        deserializers["parent_folder_id"](parseNodeParentFolderId);

        Assert.Equal("test-project", body.Code);
        Assert.Equal("Test Description", body.Description);
        Assert.Equal("Test Project", body.Name);
        Assert.Equal(100L, body.ParentFolderId);
    }



    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanSetAndGet()
    {
        var body = new ProjectsPostRequestBody();
        var additionalData = new Dictionary<string, object>
        {
            { "custom_field", "custom_value" }
        };

        body.AdditionalData = additionalData;

        Assert.NotNull(body.AdditionalData);
        Assert.True(body.AdditionalData.ContainsKey("custom_field"));
        Assert.Equal("custom_value", body.AdditionalData["custom_field"]);
    }

    /// <summary>
    /// Codeプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Code_CanSetAndGet()
    {
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project"
        };

        Assert.Equal("test-project", body.Code);
    }

    /// <summary>
    /// Descriptionプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Description_CanSetAndGet()
    {
        var body = new ProjectsPostRequestBody
        {
            Description = "Test Description"
        };

        Assert.Equal("Test Description", body.Description);
    }

    /// <summary>
    /// Nameプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Name_CanSetAndGet()
    {
        var body = new ProjectsPostRequestBody
        {
            Name = "Test Project"
        };

        Assert.Equal("Test Project", body.Name);
    }

    /// <summary>
    /// ParentFolderIdプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void ParentFolderId_CanSetAndGet()
    {
        var body = new ProjectsPostRequestBody
        {
            ParentFolderId = 100
        };

        Assert.Equal(100, body.ParentFolderId);
    }

    /// <summary>
    /// すべてのプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void AllProperties_CanBeSet()
    {
        var body = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project",
            Description = "Test Description",
            ParentFolderId = 100
        };

        Assert.Equal("test-project", body.Code);
        Assert.Equal("Test Project", body.Name);
        Assert.Equal("Test Description", body.Description);
        Assert.Equal(100, body.ParentFolderId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project",
            Description = "Test Description",
            ParentFolderId = 100
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(ProjectsPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal("test-project", deserialized.Code);
        Assert.Equal("Test Project", deserialized.Name);
        Assert.Equal("Test Description", deserialized.Description);
        Assert.Equal(100, deserialized.ParentFolderId);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new ProjectsPostRequestBody
        {
            Code = "test-project",
            Name = "Test Project",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(ProjectsPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal("test-project", deserialized.Code);
        Assert.Equal("Test Project", deserialized.Name);
        Assert.NotNull(deserialized.AdditionalData);
    }
}
