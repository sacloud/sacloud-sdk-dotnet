using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Root;

/// <summary>
/// GetCommonServiceItemStatusResponse_NotificationStatusモデルのテスト
/// </summary>
public class GetCommonServiceItemStatusResponse_NotificationStatusTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = GetCommonServiceItemStatusResponse_NotificationStatus.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<GetCommonServiceItemStatusResponse_NotificationStatus>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GetCommonServiceItemStatusResponse_NotificationStatus.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new GetCommonServiceItemStatusResponse_NotificationStatus();
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("IsValid"));
        Assert.True(deserializers.ContainsKey("ModifiedAt"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new GetCommonServiceItemStatusResponse_NotificationStatus();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new GetCommonServiceItemStatusResponse_NotificationStatus
        {
            IsValid = true,
            ModifiedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(GetCommonServiceItemStatusResponse_NotificationStatus.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(originalModel.IsValid, deserialized.IsValid);
        Assert.Equal(originalModel.ModifiedAt, deserialized.ModifiedAt);
    }
}
