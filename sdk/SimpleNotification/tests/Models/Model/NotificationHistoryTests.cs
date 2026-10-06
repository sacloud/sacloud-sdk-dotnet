using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Model;

/// <summary>
/// NotificationHistoryモデルのテスト
/// </summary>
public class NotificationHistoryTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = NotificationHistory.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<NotificationHistory>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => NotificationHistory.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new NotificationHistory();
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("message"));
        Assert.True(deserializers.ContainsKey("received_at"));
        Assert.True(deserializers.ContainsKey("request_id"));
        Assert.True(deserializers.ContainsKey("source_id"));
        Assert.True(deserializers.ContainsKey("statuses"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new NotificationHistory();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new NotificationHistory();
        originalModel.AdditionalData.Add("custom", "value");

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(NotificationHistory.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.True(deserialized.AdditionalData.ContainsKey("custom"));
    }
}
