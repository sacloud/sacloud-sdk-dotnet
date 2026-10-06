using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MapKeyExistsMatcherモデルのテスト
/// </summary>
public class MapKeyExistsMatcherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MapKeyExistsMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MapKeyExistsMatcher>(MapKeyExistsMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MapKeyExistsMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MapKeyExistsMatcher().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MapKeyExistsMatcher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MapKeyExistsMatcher().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("field"));
        Assert.True(d.ContainsKey("key"));
        Assert.True(d.ContainsKey("type"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MapKeyExistsMatcher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MapKeyExistsMatcher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["field"](parseNode);

        parseNode.GetStringValue().Returns("test_key");
        deserializers["key"](parseNode);
        Assert.Equal("test_key", model.Key);

        deserializers["type"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MapKeyExistsMatcher_Serialize_WritesExpectedFields()
    {
        var model = new MapKeyExistsMatcher { Key = "test_key" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("key", "test_key");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
