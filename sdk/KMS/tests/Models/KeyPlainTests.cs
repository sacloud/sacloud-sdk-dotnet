using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyPlainモデルのテスト
/// </summary>
public class KeyPlainTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void KeyPlain_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = KeyPlain.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<KeyPlain>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void KeyPlain_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => KeyPlain.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void KeyPlain_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new KeyPlain();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Algo"));
        Assert.True(deserializers.ContainsKey("Plain"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void KeyPlain_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new KeyPlain();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void KeyPlain_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new KeyPlain();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetEnumValue<KeyEncryptAlgoEnum>().Returns(KeyEncryptAlgoEnum.Aes256Gcm);
        deserializers["Algo"](parseNode);
        Assert.Equal(KeyEncryptAlgoEnum.Aes256Gcm, model.Algo);

        parseNode.GetStringValue().Returns("base64-encoded-plain-data");
        deserializers["Plain"](parseNode);
        Assert.Equal("base64-encoded-plain-data", model.Plain);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task KeyPlain_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new KeyPlain
        {
            Algo = KeyEncryptAlgoEnum.Aes256Cbc,
            Plain = "base64-plain-content"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(KeyPlain.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Algo, deserializedModel.Algo);
        Assert.Equal(originalModel.Plain, deserializedModel.Plain);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task KeyPlain_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new KeyPlain
        {
            Plain = "test-plain"
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
        var deserializedModel = parseNode.GetObjectValue(KeyPlain.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
