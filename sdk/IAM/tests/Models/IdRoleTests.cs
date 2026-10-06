using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IdRoleモデルのテスト
/// </summary>
public class IdRoleTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void IdRole_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = IdRole.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IdRole>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void IdRole_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => IdRole.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを返すことを確認
    /// </summary>
    [Fact]
    public void IdRole_GetFieldDeserializers_ReturnsCorrectKeys()
    {
        var model = new IdRole();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("description"));
    }

    /// <summary>
    /// GetFieldDeserializersが"id"キーを含むことを確認
    /// </summary>
    [Fact]
    public void IdRole_GetFieldDeserializers_ContainsIdKey()
    {
        var model = new IdRole();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("id"));
    }

    /// <summary>
    /// GetFieldDeserializersが"name"キーを含むことを確認
    /// </summary>
    [Fact]
    public void IdRole_GetFieldDeserializers_ContainsNameKey()
    {
        var model = new IdRole();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// GetFieldDeserializersが"description"キーを含むことを確認
    /// </summary>
    [Fact]
    public void IdRole_GetFieldDeserializers_ContainsDescriptionKey()
    {
        var model = new IdRole();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("description"));
    }

    /// <summary>
    /// Serializeがnameプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdRole_Serialize_WritesNameProperty()
    {
        var model = new IdRole
        {
            Name = "Test Role"
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "Test Role");
    }

    /// <summary>
    /// Serializeがdescriptionプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdRole_Serialize_WritesDescriptionProperty()
    {
        var model = new IdRole
        {
            Description = "Test Description"
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("description", "Test Description");
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void IdRole_Serialize_WritesAdditionalData()
    {
        var model = new IdRole();
        model.AdditionalData["customKey"] = "customValue";
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void IdRole_AllActions_WorkCorrectly()
    {
        var model = new IdRole();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-id");
        deserializers["id"](parseNode);
        Assert.Equal("test-id", model.Id);

        parseNode.GetStringValue().Returns("Test Role");
        deserializers["name"](parseNode);
        Assert.Equal("Test Role", model.Name);

        parseNode.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNode);
        Assert.Equal("Test Description", model.Description);
    }

    /// <summary>
    /// シリアライズとデシリアライズのラウンドトリップが正しく動作することを確認
    /// </summary>
    [Fact]
    public async Task IdRole_SerializeDeserialize_RoundTrip()
    {
        var originalModel = new IdRole
        {
            Name = "Test Role",
            Description = "Test Description"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(IdRole.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを確認
    /// </summary>
    [Fact]
    public void IdRole_Constructor_InitializesAdditionalData()
    {
        var model = new IdRole();

        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void IdRole_AdditionalData_CanBeSet()
    {
        var model = new IdRole();
        model.AdditionalData["customKey"] = "customValue";

        Assert.True(model.AdditionalData.ContainsKey("customKey"));
        Assert.Equal("customValue", model.AdditionalData["customKey"]);
    }

    /// <summary>
    /// Nameプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void IdRole_NameProperty_CanBeSet()
    {
        var model = new IdRole
        {
            Name = "Test Role"
        };

        Assert.Equal("Test Role", model.Name);
    }

    /// <summary>
    /// Descriptionプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void IdRole_DescriptionProperty_CanBeSet()
    {
        var model = new IdRole
        {
            Description = "Test Description"
        };

        Assert.Equal("Test Description", model.Description);
    }

    /// <summary>
    /// Idプロパティがデシリアライザー経由で設定されることを確認
    /// </summary>
    [Fact]
    public void IdRole_IdProperty_SetViaDeserializer()
    {
        var model = new IdRole();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("test-id-123");

        deserializers["id"](parseNode);

        Assert.Equal("test-id-123", model.Id);
    }

    /// <summary>
    /// デシリアライズ時にAdditionalDataが保持されることを確認
    /// </summary>
    [Fact]
    public async Task IdRole_Deserialize_PreservesAdditionalData()
    {
        var jsonContent = @"{
            ""id"": ""test-id"",
            ""name"": ""Test Role"",
            ""description"": ""Test Description"",
            ""customField"": ""customValue""
        }";

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonContent));
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", stream);
        var deserializedModel = parseNode.GetObjectValue(IdRole.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("test-id", deserializedModel.Id);
        Assert.Equal("Test Role", deserializedModel.Name);
        Assert.Equal("Test Description", deserializedModel.Description);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void IdRole_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new IdRole();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }
}
