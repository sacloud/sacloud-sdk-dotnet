using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// Historyモデルのテスト
/// </summary>
public class HistoryTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void History_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<History>(History.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void History_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => History.CreateFromDiscriminatorValue(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void History_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new History().GetFieldDeserializers();
        Assert.Equal(11, d.Count);
        Assert.True(d.ContainsKey("endsAt"));
        Assert.True(d.ContainsKey("labels"));
        Assert.True(d.ContainsKey("open"));
        Assert.True(d.ContainsKey("project_id"));
        Assert.True(d.ContainsKey("query"));
        Assert.True(d.ContainsKey("rule_uid"));
        Assert.True(d.ContainsKey("severity"));
        Assert.True(d.ContainsKey("startsAt"));
        Assert.True(d.ContainsKey("threshold"));
        Assert.True(d.ContainsKey("uid"));
        Assert.True(d.ContainsKey("value"));
    }
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void History_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new History().Serialize(null!));
    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void History_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new History().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void History_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new History();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.UnixEpoch);
        deserializers["endsAt"](parseNode);
        Assert.Equal(DateTimeOffset.UnixEpoch, model.EndsAt);

        parseNode.GetStringValue().Returns("test_labels");
        deserializers["labels"](parseNode);
        Assert.Equal("test_labels", model.Labels);

        parseNode.GetBoolValue().Returns(true);
        deserializers["open"](parseNode);
        Assert.Equal(true, model.Open);

        deserializers["project_id"](parseNode);

        parseNode.GetStringValue().Returns("test_query");
        deserializers["query"](parseNode);
        Assert.Equal("test_query", model.Query);

        deserializers["rule_uid"](parseNode);

        deserializers["severity"](parseNode);

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.UnixEpoch);
        deserializers["startsAt"](parseNode);
        Assert.Equal(DateTimeOffset.UnixEpoch, model.StartsAt);

        parseNode.GetStringValue().Returns("test_threshold");
        deserializers["threshold"](parseNode);
        Assert.Equal("test_threshold", model.Threshold);

        deserializers["uid"](parseNode);

        parseNode.GetDoubleValue().Returns(1.0);
        deserializers["value"](parseNode);
        Assert.Equal(1.0, model.Value);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void History_Serialize_WritesExpectedFields()
    {
        var model = new History { EndsAt = DateTimeOffset.UnixEpoch, Labels = "test_labels", Open = true, Query = "test_query", StartsAt = DateTimeOffset.UnixEpoch, Threshold = "test_threshold", Value = 1.0 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteDateTimeOffsetValue("endsAt", DateTimeOffset.UnixEpoch);
        writer.Received(1).WriteStringValue("labels", "test_labels");
        writer.Received(1).WriteBoolValue("open", true);
        writer.Received(1).WriteStringValue("query", "test_query");
        writer.Received(1).WriteDateTimeOffsetValue("startsAt", DateTimeOffset.UnixEpoch);
        writer.Received(1).WriteStringValue("threshold", "test_threshold");
        writer.Received(1).WriteDoubleValue("value", 1.0);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
