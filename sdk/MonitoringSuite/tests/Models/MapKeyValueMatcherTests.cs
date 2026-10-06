using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MapKeyValueMatcherモデルのテスト
/// </summary>
public class MapKeyValueMatcherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MapKeyValueMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MapKeyValueMatcher>(MapKeyValueMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MapKeyValueMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MapKeyValueMatcher().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MapKeyValueMatcher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MapKeyValueMatcher().GetFieldDeserializers();
        Assert.Equal(4, d.Count);
        Assert.True(d.ContainsKey("field"));
        Assert.True(d.ContainsKey("key"));
        Assert.True(d.ContainsKey("type"));
        Assert.True(d.ContainsKey("value"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MapKeyValueMatcher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MapKeyValueMatcher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["field"](parseNode);

        parseNode.GetStringValue().Returns("test_key");
        deserializers["key"](parseNode);
        Assert.Equal("test_key", model.Key);

        deserializers["type"](parseNode);

        parseNode.GetStringValue().Returns("test_value");
        deserializers["value"](parseNode);
        Assert.Equal("test_value", model.Value);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MapKeyValueMatcher_Serialize_WritesExpectedFields()
    {
        var model = new MapKeyValueMatcher { Key = "test_key", Value = "test_value" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("key", "test_key");
        writer.Received(1).WriteStringValue("value", "test_value");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
