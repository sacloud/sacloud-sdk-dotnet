using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// NotificationTargetモデルのテスト
/// </summary>
public class NotificationTargetTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void NotificationTarget_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<NotificationTarget>(NotificationTarget.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NotificationTarget_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => NotificationTarget.CreateFromDiscriminatorValue(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void NotificationTarget_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new NotificationTarget().GetFieldDeserializers();
        Assert.Equal(6, d.Count);
        Assert.True(d.ContainsKey("config"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("project_id"));
        Assert.True(d.ContainsKey("service_type"));
        Assert.True(d.ContainsKey("uid"));
        Assert.True(d.ContainsKey("url"));
    }
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NotificationTarget_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new NotificationTarget().Serialize(null!));
    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void NotificationTarget_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new NotificationTarget().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NotificationTarget_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NotificationTarget();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["config"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["project_id"](parseNode);

        deserializers["service_type"](parseNode);

        deserializers["uid"](parseNode);

        parseNode.GetStringValue().Returns("test_url");
        deserializers["url"](parseNode);
        Assert.Equal("test_url", model.Url);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void NotificationTarget_Serialize_WritesExpectedFields()
    {
        var model = new NotificationTarget { Description = "test_description", Url = "test_url" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("url", "test_url");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
