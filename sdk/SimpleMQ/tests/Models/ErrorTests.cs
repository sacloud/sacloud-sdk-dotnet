using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleMQ.Models;

namespace Sacloud.Sdk.SimpleMQ.Tests.Models;

/// <summary>
/// Errorモデルのテスト
/// </summary>
public class ErrorTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Error_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Error.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Error>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Error_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            Error.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Error_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Error();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("message"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Error_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Error();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Error_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Error();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(400L);
        deserializers["code"](parseNode);
        Assert.Equal(400L, model.Code);

        parseNode.GetStringValue().Returns("Bad Request");
        deserializers["message"](parseNode);
        Assert.Equal("Bad Request", model.MessageEscaped);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Error_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Error
        {
            Code = 400,
            MessageEscaped = "Bad Request"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Error.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Code, deserializedModel.Code);
        Assert.Equal(originalModel.MessageEscaped, deserializedModel.MessageEscaped);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Error_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Error
        {
            Code = 500,
            MessageEscaped = "Internal Server Error"
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
        var deserializedModel = parseNode.GetObjectValue(Error.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// MessageプロパティがMessageEscapedの値を返すことを確認
    /// </summary>
    [Fact]
    public void Error_Message_ReturnsMessageEscapedValue()
    {
        var model = new Error
        {
            MessageEscaped = "Test Error"
        };

        Assert.Equal("Test Error", model.Message);
    }

    /// <summary>
    /// MessageプロパティがMessageEscapedがnullの場合に空文字を返すことを確認
    /// </summary>
    [Fact]
    public void Error_Message_ReturnsEmptyString_WhenMessageEscapedIsNull()
    {
        var model = new Error
        {
            MessageEscaped = null
        };

        Assert.Equal(string.Empty, model.Message);
    }
}
