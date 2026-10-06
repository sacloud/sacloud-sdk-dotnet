using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Model;

/// <summary>
/// Errorモデルのテスト
/// </summary>
public class ErrorTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Error.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Error>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Error.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Error();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("error_code"));
        Assert.True(deserializers.ContainsKey("error_msg"));
        Assert.True(deserializers.ContainsKey("is_fatal"));
        Assert.True(deserializers.ContainsKey("serial"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Error();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Error();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("E001");
        deserializers["error_code"](parseNode);
        Assert.Equal("E001", model.ErrorCode);

        parseNode.GetStringValue().Returns("Bad Request");
        deserializers["error_msg"](parseNode);
        Assert.Equal("Bad Request", model.ErrorMsg);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_fatal"](parseNode);
        Assert.True(model.IsFatal);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Error
        {
            ErrorCode = "E001",
            ErrorMsg = "Test error",
            IsFatal = true,
            Serial = "abc123",
            Status = "400",
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Error.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ErrorCode, deserializedModel.ErrorCode);
        Assert.Equal(originalModel.ErrorMsg, deserializedModel.ErrorMsg);
        Assert.Equal(originalModel.IsFatal, deserializedModel.IsFatal);
        Assert.Equal(originalModel.Serial, deserializedModel.Serial);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Error();
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Error.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }

    /// <summary>
    /// MessageがErrorMsgを優先して返すことを確認
    /// </summary>
    [Fact]
    public void Message_WithErrorMsg_ReturnsErrorMsg()
    {
        var model = new Error
        {
            ErrorMsg = "API error message",
        };

        Assert.Equal("API error message", model.Message);
    }

    /// <summary>
    /// MessageがErrorCodeとErrorMsgを併記して返すことを確認
    /// </summary>
    [Fact]
    public void Message_WithErrorCodeAndErrorMsg_ReturnsComposedMessage()
    {
        var model = new Error
        {
            ErrorCode = "E001",
            ErrorMsg = "Invalid request",
        };

        Assert.Equal("E001: Invalid request", model.Message);
    }

    /// <summary>
    /// MessageがErrorCodeのみ設定時にErrorCodeを返すことを確認
    /// </summary>
    [Fact]
    public void Message_WithOnlyErrorCode_ReturnsErrorCode()
    {
        var model = new Error
        {
            ErrorCode = "E999",
        };

        Assert.Equal("E999", model.Message);
    }

    /// <summary>
    /// Messageが空白文字列を未設定として扱うことを確認
    /// </summary>
    [Fact]
    public void Message_WithWhitespaceValues_TreatsAsUnset()
    {
        var model = new Error
        {
            ErrorCode = "   ",
            ErrorMsg = "\t",
        };

        Assert.NotEqual("   : \t", model.Message);
    }
}
