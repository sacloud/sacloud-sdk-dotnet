using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// BoolMatcherモデルのテスト
/// </summary>
public class BoolMatcherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void BoolMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<BoolMatcher>(BoolMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void BoolMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new BoolMatcher().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void BoolMatcher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new BoolMatcher().GetFieldDeserializers();
        Assert.Equal(4, d.Count);
        Assert.True(d.ContainsKey("field"));
        Assert.True(d.ContainsKey("operator"));
        Assert.True(d.ContainsKey("type"));
        Assert.True(d.ContainsKey("value"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BoolMatcher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BoolMatcher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["field"](parseNode);

        deserializers["operator"](parseNode);

        deserializers["type"](parseNode);

        parseNode.GetBoolValue().Returns(true);
        deserializers["value"](parseNode);
        Assert.Equal(true, model.Value);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void BoolMatcher_Serialize_WritesExpectedFields()
    {
        var model = new BoolMatcher { Value = true };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteBoolValue("value", true);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
