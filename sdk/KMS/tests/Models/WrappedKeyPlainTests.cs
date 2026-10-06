using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedKeyPlainモデルのテスト
/// </summary>
public class WrappedKeyPlainTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void WrappedKeyPlain_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WrappedKeyPlain.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WrappedKeyPlain>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedKeyPlain_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedKeyPlain.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedKeyPlain_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WrappedKeyPlain();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Key"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedKeyPlain_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WrappedKeyPlain();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedKeyPlain_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedKeyPlain();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var keyPlainObject = new KeyPlain { Plain = "plain-data", Algo = KeyEncryptAlgoEnum.Aes256Gcm };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<KeyPlain>>()).Returns(keyPlainObject);
        deserializers["Key"](parseNode);
        Assert.NotNull(model.Key);
        Assert.Equal("plain-data", model.Key.Plain);
        Assert.Equal(KeyEncryptAlgoEnum.Aes256Gcm, model.Key.Algo);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedKeyPlain_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WrappedKeyPlain
        {
            Key = new KeyPlain
            {
                Plain = "base64-plain-content",
                Algo = KeyEncryptAlgoEnum.Aes256Cbc
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
        var deserializedModel = parseNode.GetObjectValue(WrappedKeyPlain.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Key);
        Assert.Equal(originalModel.Key.Plain, deserializedModel.Key.Plain);
        Assert.Equal(originalModel.Key.Algo, deserializedModel.Key.Algo);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedKeyPlain_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WrappedKeyPlain
        {
            Key = new KeyPlain { Plain = "test-plain" }
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
        var deserializedModel = parseNode.GetObjectValue(WrappedKeyPlain.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
