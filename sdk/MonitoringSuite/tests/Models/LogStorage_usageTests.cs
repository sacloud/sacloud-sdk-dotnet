using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogStorage_usageモデルのテスト
/// </summary>
public class LogStorage_usageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogStorage_usage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogStorage_usage>(LogStorage_usage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogStorage_usage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogStorage_usage().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogStorage_usage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogStorage_usage().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("log_measure_rules"));
        Assert.True(d.ContainsKey("log_routings"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogStorage_usage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogStorage_usage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(1);
        deserializers["log_measure_rules"](parseNode);
        Assert.Equal(1, model.LogMeasureRules);

        parseNode.GetIntValue().Returns(1);
        deserializers["log_routings"](parseNode);
        Assert.Equal(1, model.LogRoutings);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogStorage_usage_Serialize_WritesExpectedFields()
    {
        var model = new LogStorage_usage { LogMeasureRules = 1, LogRoutings = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteIntValue("log_measure_rules", 1);
        writer.Received(1).WriteIntValue("log_routings", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
