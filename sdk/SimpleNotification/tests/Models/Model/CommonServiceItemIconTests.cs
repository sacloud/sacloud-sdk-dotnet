using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Model;

/// <summary>
/// CommonServiceItemIconモデルのテスト
/// </summary>
public class CommonServiceItemIconTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CommonServiceItemIcon.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CommonServiceItemIcon>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CommonServiceItemIcon.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CommonServiceItemIcon();
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Scope"));
        Assert.True(deserializers.ContainsKey("URL"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CommonServiceItemIcon();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CommonServiceItemIcon { ID = "icon-1", Name = "test-icon", Scope = "shared", URL = "https://example.com/icon.png" };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(CommonServiceItemIcon.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal("icon-1", deserialized.ID);
        Assert.Equal("test-icon", deserialized.Name);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CommonServiceItemIcon();
        originalModel.AdditionalData.Add("custom", "value");

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(CommonServiceItemIcon.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.True(deserialized.AdditionalData.ContainsKey("custom"));
    }

    /// <summary>
    /// バリデーションでID必須を検証できることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMissingId_ThrowsValidationException()
    {
        var model = new CommonServiceItemIcon();

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(model, NullLogger.Instance));

        Assert.Contains("CommonServiceItem.Icon.ID", exception.Message);
    }
}
