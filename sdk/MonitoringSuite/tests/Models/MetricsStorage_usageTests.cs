using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsStorage_usageモデルのテスト
/// </summary>
public class MetricsStorage_usageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_usage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MetricsStorage_usage>(MetricsStorage_usage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_usage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MetricsStorage_usage().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_usage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MetricsStorage_usage().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("alert_rules"));
        Assert.True(d.ContainsKey("log_measure_rules"));
        Assert.True(d.ContainsKey("metrics_routings"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_usage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsStorage_usage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(1);
        deserializers["alert_rules"](parseNode);
        Assert.Equal(1, model.AlertRules);

        parseNode.GetIntValue().Returns(1);
        deserializers["log_measure_rules"](parseNode);
        Assert.Equal(1, model.LogMeasureRules);

        parseNode.GetIntValue().Returns(1);
        deserializers["metrics_routings"](parseNode);
        Assert.Equal(1, model.MetricsRoutings);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_usage_Serialize_WritesExpectedFields()
    {
        var model = new MetricsStorage_usage { AlertRules = 1, LogMeasureRules = 1, MetricsRoutings = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteIntValue("alert_rules", 1);
        writer.Received(1).WriteIntValue("log_measure_rules", 1);
        writer.Received(1).WriteIntValue("metrics_routings", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
