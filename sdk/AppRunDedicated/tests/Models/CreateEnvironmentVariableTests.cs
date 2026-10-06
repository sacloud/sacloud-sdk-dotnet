using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// CreateEnvironmentVariableモデルのテスト
/// </summary>
public class CreateEnvironmentVariableTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateEnvironmentVariable_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateEnvironmentVariable.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateEnvironmentVariable>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateEnvironmentVariable_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            CreateEnvironmentVariable.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateEnvironmentVariable_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateEnvironmentVariable();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("key"));
        Assert.True(deserializers.ContainsKey("secret"));
        Assert.True(deserializers.ContainsKey("value"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateEnvironmentVariable_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateEnvironmentVariable();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateEnvironmentVariable_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateEnvironmentVariable();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-key");
        deserializers["key"](parseNode);
        Assert.Equal("test-key", model.Key);

        parseNode.GetBoolValue().Returns(true);
        deserializers["secret"](parseNode);
        Assert.True(model.Secret);

        parseNode.GetStringValue().Returns("test-value");
        deserializers["value"](parseNode);
        Assert.Equal("test-value", model.Value);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateEnvironmentVariable_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateEnvironmentVariable
        {
            Key = "test-key",
            Secret = true,
            Value = "test-value"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateEnvironmentVariable.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Key, deserializedModel.Key);
        Assert.Equal(originalModel.Secret, deserializedModel.Secret);
        Assert.Equal(originalModel.Value, deserializedModel.Value);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateEnvironmentVariable_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateEnvironmentVariable
        {
            Key = "test-key",
            Value = "test-value"
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateEnvironmentVariable.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
