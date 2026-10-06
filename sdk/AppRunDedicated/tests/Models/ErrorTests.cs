using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// Errorクラスのユニットテスト
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
    /// GetFieldDeserializersが正しいキーを返すことを確認
    /// </summary>
    [Fact]
    public void Error_GetFieldDeserializers_ReturnsCorrectKeys()
    {
        var error = new Error();
        
        var deserializers = error.GetFieldDeserializers();
        
        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("status"));
        Assert.True(deserializers.ContainsKey("title"));
    }

    /// <summary>
    /// GetFieldDeserializersのstatusアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Error_GetFieldDeserializers_StatusAction_WorksCorrectly()
    {
        var error = new Error();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetIntValue().Returns(500);
        
        var deserializers = error.GetFieldDeserializers();
        deserializers["status"](parseNode);
        
        Assert.Equal(500, error.Status);
    }

    /// <summary>
    /// GetFieldDeserializersのtitleアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Error_GetFieldDeserializers_TitleAction_WorksCorrectly()
    {
        var error = new Error();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("Internal Server Error");
        
        var deserializers = error.GetFieldDeserializers();
        deserializers["title"](parseNode);
        
        Assert.Equal("Internal Server Error", error.Title);
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを確認
    /// </summary>
    [Fact]
    public void Error_Serialize_WritesValues()
    {
        var error = new Error
        {
            Status = 404,
            Title = "Not Found"
        };
        var writer = Substitute.For<ISerializationWriter>();
        
        error.Serialize(writer);
        
        writer.Received(1).WriteIntValue("status", 404);
        writer.Received(1).WriteStringValue("title", "Not Found");
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Error_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var error = new Error();
        
        Assert.Throws<ArgumentNullException>(() => error.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Error_Constructor_InitializesAdditionalData()
    {
        var error = new Error();
        
        Assert.NotNull(error.AdditionalData);
        Assert.Empty(error.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataにデータを追加できることを確認
    /// </summary>
    [Fact]
    public void Error_AdditionalData_CanAddData()
    {
        var error = new Error();
        
        error.AdditionalData["custom"] = "value";
        
        Assert.Single(error.AdditionalData);
        Assert.Equal("value", error.AdditionalData["custom"]);
    }

    /// <summary>
    /// MessageプロパティがApiExceptionから継承されていることを確認
    /// </summary>
    [Fact]
    public void Error_Message_IsInheritedFromApiException()
    {
        var error = new Error();
        
        Assert.NotNull(error.Message);
        Assert.IsType<string>(error.Message);
    }
}
