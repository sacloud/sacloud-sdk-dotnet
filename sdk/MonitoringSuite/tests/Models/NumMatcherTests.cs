using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// NumMatcherモデルのテスト
/// </summary>
public class NumMatcherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void NumMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<NumMatcher>(NumMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NumMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new NumMatcher().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void NumMatcher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new NumMatcher().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("field"));
        Assert.True(d.ContainsKey("operator"));
        Assert.True(d.ContainsKey("type"));
        Assert.True(d.ContainsKey("value"));
        Assert.True(d.ContainsKey("value_list"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NumMatcher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NumMatcher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["field"](parseNode);

        deserializers["operator"](parseNode);

        deserializers["type"](parseNode);

        parseNode.GetDoubleValue().Returns(1.0);
        deserializers["value"](parseNode);
        Assert.Equal(1.0, model.Value);

        deserializers["value_list"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void NumMatcher_Serialize_WritesExpectedFields()
    {
        var model = new NumMatcher { Value = 1.0 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteDoubleValue("value", 1.0);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
