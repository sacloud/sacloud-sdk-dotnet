using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

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

        parseNode.GetStringValue().Returns("ERR001");
        deserializers["error_code"](parseNode);
        Assert.Equal("ERR001", model.ErrorCode);

        parseNode.GetStringValue().Returns("Error occurred");
        deserializers["error_msg"](parseNode);
        Assert.Equal("Error occurred", model.ErrorMsg);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_fatal"](parseNode);
        Assert.True(model.IsFatal);

        parseNode.GetStringValue().Returns("12345");
        deserializers["serial"](parseNode);
        Assert.Equal("12345", model.Serial);

        parseNode.GetStringValue().Returns("500");
        deserializers["status"](parseNode);
        Assert.Equal("500", model.Status);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Error_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Error
        {
            ErrorCode = "ERR001",
            ErrorMsg = "Error occurred",
            IsFatal = true,
            Serial = "12345",
            Status = "500"
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
    public async Task Error_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Error
        {
            ErrorCode = "ERR002",
            ErrorMsg = "Another error"
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
}
