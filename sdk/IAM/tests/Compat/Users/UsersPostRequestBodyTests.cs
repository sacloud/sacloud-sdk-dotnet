using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Compat.Users;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users;

/// <summary>
/// UsersPostRequestBodyモデルのテスト
/// </summary>
public class UsersPostRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UsersPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UsersPostRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            UsersPostRequestBody.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UsersPostRequestBody();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("email"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("password"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UsersPostRequestBody();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new UsersPostRequestBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-code", "test-desc", "test@example.com", "test-name", "test-password");

        deserializers["code"](parseNode);
        Assert.Equal("test-code", model.Code);

        deserializers["description"](parseNode);
        Assert.Equal("test-desc", model.Description);

        deserializers["email"](parseNode);
        Assert.Equal("test@example.com", model.Email);

        deserializers["name"](parseNode);
        Assert.Equal("test-name", model.Name);

        deserializers["password"](parseNode);
        Assert.Equal("test-password", model.Password);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UsersPostRequestBody
        {
            Code = "user001",
            Description = "Test user description",
            Email = "test@example.com",
            Name = "Test User",
            Password = "TestPassword123"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UsersPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Code, deserializedModel.Code);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Email, deserializedModel.Email);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Password, deserializedModel.Password);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UsersPostRequestBody
        {
            Name = "test-user"
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
        var deserializedModel = parseNode.GetObjectValue(UsersPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Contains("custom_field", deserializedModel.AdditionalData.Keys);
        Assert.Equal("custom_value", deserializedModel.AdditionalData["custom_field"]?.ToString());
    }
}
