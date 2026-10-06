using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Compat.ApiKeys;

namespace Sacloud.Sdk.IAM.Tests.Compat.ApiKeys;

/// <summary>
/// ApiKeysPostRequestBodyモデルのテスト
/// </summary>
public class ApiKeysPostRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ApiKeysPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ApiKeysPostRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            ApiKeysPostRequestBody.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ApiKeysPostRequestBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("iam_roles"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("project_id"));
        Assert.True(deserializers.ContainsKey("server_resource_id"));
        Assert.True(deserializers.ContainsKey("zone_id"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ApiKeysPostRequestBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ApiKeysPostRequestBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-name", "test-desc", "test-server-id", "is1a");
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(new List<string> { "role1", "role2" });
        parseNode.GetLongValue().Returns(12345L);

        deserializers["name"](parseNode);
        Assert.Equal("test-name", model.Name);

        deserializers["description"](parseNode);
        Assert.Equal("test-desc", model.Description);

        deserializers["iam_roles"](parseNode);
        Assert.NotNull(model.IamRoles);
        Assert.Equal(2, model.IamRoles.Count);

        deserializers["project_id"](parseNode);
        Assert.Equal(12345L, model.ProjectId);

        deserializers["server_resource_id"](parseNode);
        Assert.Equal("test-server-id", model.ServerResourceId);

        deserializers["zone_id"](parseNode);
        Assert.Equal("is1a", model.ZoneId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ApiKeysPostRequestBody
        {
            Name = "test-api-key",
            Description = "Test description",
            IamRoles = ["role1", "role2"],
            ProjectId = 12345L,
            ServerResourceId = "server-123",
            ZoneId = "is1a"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ApiKeysPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.ProjectId, deserializedModel.ProjectId);
        Assert.Equal(originalModel.ServerResourceId, deserializedModel.ServerResourceId);
        Assert.Equal(originalModel.ZoneId, deserializedModel.ZoneId);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ApiKeysPostRequestBody
        {
            Name = "test-api-key"
        };
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ApiKeysPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
