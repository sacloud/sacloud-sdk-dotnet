using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedKeyモデルのテスト
/// </summary>
public class WrappedKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void WrappedKey_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WrappedKey.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WrappedKey>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedKey_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedKey.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedKey_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WrappedKey();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Key"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedKey_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WrappedKey();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedKey_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedKey();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var keyObject = new Key { Name = "test-key" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Key>>()).Returns(keyObject);
        deserializers["Key"](parseNode);
        Assert.NotNull(model.Key);
        Assert.Equal("test-key", model.Key.Name);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedKey_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WrappedKey
        {
            Key = new Key
            {
                Name = "test-key-name",
                Description = "Test key description"
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(WrappedKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Key);
        Assert.Equal(originalModel.Key.Name, deserializedModel.Key.Name);
        Assert.Equal(originalModel.Key.Description, deserializedModel.Key.Description);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedKey_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WrappedKey
        {
            Key = new Key { Name = "test-key" }
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(WrappedKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
