using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedKeyCipherモデルのテスト
/// </summary>
public class WrappedKeyCipherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void WrappedKeyCipher_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WrappedKeyCipher.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WrappedKeyCipher>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedKeyCipher_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedKeyCipher.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedKeyCipher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WrappedKeyCipher();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Key"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedKeyCipher_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WrappedKeyCipher();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedKeyCipher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedKeyCipher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var keyCipherObject = new KeyCipher { Cipher = "encrypted-data" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<KeyCipher>>()).Returns(keyCipherObject);
        deserializers["Key"](parseNode);
        Assert.NotNull(model.Key);
        Assert.Equal("encrypted-data", model.Key.Cipher);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedKeyCipher_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WrappedKeyCipher
        {
            Key = new KeyCipher
            {
                Cipher = "encrypted-cipher-content"
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
        var deserializedModel = parseNode.GetObjectValue(WrappedKeyCipher.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Key);
        Assert.Equal(originalModel.Key.Cipher, deserializedModel.Key.Cipher);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedKeyCipher_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WrappedKeyCipher
        {
            Key = new KeyCipher { Cipher = "test-cipher" }
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
        var deserializedModel = parseNode.GetObjectValue(WrappedKeyCipher.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
