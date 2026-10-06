using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MapValueNumMatcherモデルのテスト
/// </summary>
public class MapValueNumMatcherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MapValueNumMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MapValueNumMatcher>(MapValueNumMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MapValueNumMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MapValueNumMatcher().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MapValueNumMatcher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MapValueNumMatcher().GetFieldDeserializers();
        Assert.Equal(6, d.Count);
        Assert.True(d.ContainsKey("field"));
        Assert.True(d.ContainsKey("key"));
        Assert.True(d.ContainsKey("operator"));
        Assert.True(d.ContainsKey("type"));
        Assert.True(d.ContainsKey("value"));
        Assert.True(d.ContainsKey("value_list"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MapValueNumMatcher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MapValueNumMatcher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["field"](parseNode);

        parseNode.GetStringValue().Returns("test_key");
        deserializers["key"](parseNode);
        Assert.Equal("test_key", model.Key);

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
    public void MapValueNumMatcher_Serialize_WritesExpectedFields()
    {
        var model = new MapValueNumMatcher { Key = "test_key", Value = 1.0 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("key", "test_key");
        writer.Received(1).WriteDoubleValue("value", 1.0);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
