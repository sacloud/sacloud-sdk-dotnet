using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// EnumMatcherモデルのテスト
/// </summary>
public class EnumMatcherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void EnumMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<EnumMatcher>(EnumMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void EnumMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new EnumMatcher().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void EnumMatcher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new EnumMatcher().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("field"));
        Assert.True(d.ContainsKey("type"));
        Assert.True(d.ContainsKey("value"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void EnumMatcher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new EnumMatcher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["field"](parseNode);

        deserializers["type"](parseNode);

        deserializers["value"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void EnumMatcher_Serialize_WritesExpectedFields()
    {
        var model = new EnumMatcher();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
