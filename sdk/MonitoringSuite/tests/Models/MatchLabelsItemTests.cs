using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MatchLabelsItemモデルのテスト
/// </summary>
public class MatchLabelsItemTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItem_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MatchLabelsItem>(MatchLabelsItem.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MatchLabelsItem().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MatchLabelsItem().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("value"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MatchLabelsItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        parseNode.GetStringValue().Returns("test_value");
        deserializers["value"](parseNode);
        Assert.Equal("test_value", model.Value);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItem_Serialize_WritesExpectedFields()
    {
        var model = new MatchLabelsItem { Name = "test_name", Value = "test_value" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteStringValue("value", "test_value");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
