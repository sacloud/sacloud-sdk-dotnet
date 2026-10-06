using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Models;

namespace Sacloud.Sdk.SecretManager.Tests.Models;

/// <summary>
/// CreateSecretモデルのテスト
/// </summary>
public class CreateSecretTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateSecret_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateSecret.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateSecret>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateSecret_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => CreateSecret.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateSecret_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateSecret();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("LatestVersion"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Value"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateSecret_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateSecret();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateSecret_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateSecret();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(5);
        deserializers["LatestVersion"](parseNode);

        parseNode.GetStringValue().Returns("test-secret-name");
        deserializers["Name"](parseNode);
        Assert.Equal("test-secret-name", model.Name);

        parseNode.GetStringValue().Returns("test-secret-value");
        deserializers["Value"](parseNode);
        Assert.Equal("test-secret-value", model.Value);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateSecret_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateSecret
        {
            Name = "test-secret-name",
            Value = "test-secret-value"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateSecret.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Value, deserializedModel.Value);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateSecret_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateSecret
        {
            Name = "test-secret-name"
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
        var deserializedModel = parseNode.GetObjectValue(CreateSecret.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
