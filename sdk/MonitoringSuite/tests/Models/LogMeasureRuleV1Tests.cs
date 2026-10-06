using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogMeasureRuleV1モデルのテスト
/// </summary>
public class LogMeasureRuleV1Tests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleV1_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogMeasureRuleV1>(LogMeasureRuleV1.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleV1_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogMeasureRuleV1().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleV1_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogMeasureRuleV1().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("matchers"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleV1_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogMeasureRuleV1();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["matchers"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleV1_Serialize_WritesExpectedFields()
    {
        var model = new LogMeasureRuleV1();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
