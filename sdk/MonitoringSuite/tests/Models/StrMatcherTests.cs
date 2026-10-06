using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// StrMatcherモデルのテスト
/// </summary>
public class StrMatcherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void StrMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<StrMatcher>(StrMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void StrMatcher_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => StrMatcher.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void StrMatcher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new StrMatcher().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("field"));
        Assert.True(d.ContainsKey("operator"));
        Assert.True(d.ContainsKey("type"));
        Assert.True(d.ContainsKey("value"));
        Assert.True(d.ContainsKey("value_list"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void StrMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new StrMatcher().Serialize(null!));

    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void StrMatcher_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new StrMatcher().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void StrMatcher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new StrMatcher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["field"](parseNode);

        deserializers["operator"](parseNode);

        deserializers["type"](parseNode);

        parseNode.GetStringValue().Returns("test_value");
        deserializers["value"](parseNode);
        Assert.Equal("test_value", model.Value);

        deserializers["value_list"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void StrMatcher_Serialize_WritesExpectedFields()
    {
        var model = new StrMatcher { Value = "test_value" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("value", "test_value");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
