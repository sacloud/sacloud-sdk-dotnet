using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedAlertProject_iconモデルのテスト
/// </summary>
public class WrappedAlertProject_iconTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedAlertProject_icon_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedAlertProject_icon>(WrappedAlertProject_icon.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedAlertProject_icon_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => WrappedAlertProject_icon.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void WrappedAlertProject_icon_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new WrappedAlertProject_icon().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("id"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedAlertProject_icon_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedAlertProject_icon().Serialize(null!));

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedAlertProject_icon_Serialize_WritesId()
    {
        var m = new WrappedAlertProject_icon { Id = "icon-1" };
        var w = Substitute.For<ISerializationWriter>();
        m.Serialize(w);
        w.Received(1).WriteStringValue("id", "icon-1");
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedAlertProject_icon_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedAlertProject_icon();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_id");
        deserializers["id"](parseNode);
        Assert.Equal("test_id", model.Id);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedAlertProject_icon_Serialize_WritesExpectedFields()
    {
        var model = new WrappedAlertProject_icon { Id = "test_id" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("id", "test_id");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
