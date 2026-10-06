using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// NotificationRoutingモデルのテスト
/// </summary>
public class NotificationRoutingTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void NotificationRouting_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<NotificationRouting>(NotificationRouting.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NotificationRouting_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => NotificationRouting.CreateFromDiscriminatorValue(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void NotificationRouting_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new NotificationRouting().GetFieldDeserializers();
        Assert.Equal(6, d.Count);
        Assert.True(d.ContainsKey("match_labels"));
        Assert.True(d.ContainsKey("notification_target"));
        Assert.True(d.ContainsKey("order"));
        Assert.True(d.ContainsKey("project_id"));
        Assert.True(d.ContainsKey("resend_interval_minutes"));
        Assert.True(d.ContainsKey("uid"));
    }
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NotificationRouting_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new NotificationRouting().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NotificationRouting_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NotificationRouting();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["match_labels"](parseNode);

        deserializers["notification_target"](parseNode);

        deserializers["order"](parseNode);

        deserializers["project_id"](parseNode);

        parseNode.GetIntValue().Returns(1);
        deserializers["resend_interval_minutes"](parseNode);
        Assert.Equal(1, model.ResendIntervalMinutes);

        deserializers["uid"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void NotificationRouting_Serialize_WritesExpectedFields()
    {
        var model = new NotificationRouting { ResendIntervalMinutes = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteIntValue("resend_interval_minutes", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
