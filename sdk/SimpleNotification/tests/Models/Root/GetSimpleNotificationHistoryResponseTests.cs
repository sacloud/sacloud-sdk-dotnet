using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Root;

/// <summary>
/// GetSimpleNotificationHistoryResponseモデルのテスト
/// </summary>
public class GetSimpleNotificationHistoryResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = GetSimpleNotificationHistoryResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<GetSimpleNotificationHistoryResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GetSimpleNotificationHistoryResponse.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new GetSimpleNotificationHistoryResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("NotificationHistory"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new GetSimpleNotificationHistoryResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new GetSimpleNotificationHistoryResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var history = new NotificationHistory();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<NotificationHistory>>()).Returns(history);
        deserializers["NotificationHistory"](parseNode);
        Assert.Same(history, model.NotificationHistory);
    }
}
