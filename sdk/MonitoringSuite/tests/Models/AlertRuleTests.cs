using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// AlertRuleモデルのテスト
/// </summary>
public class AlertRuleTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void AlertRule_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<AlertRule>(AlertRule.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void AlertRule_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => AlertRule.CreateFromDiscriminatorValue(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void AlertRule_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new AlertRule().GetFieldDeserializers();
        Assert.Equal(14, d.Count);
        Assert.True(d.ContainsKey("enabled_critical"));
        Assert.True(d.ContainsKey("enabled_warning"));
        Assert.True(d.ContainsKey("format"));
        Assert.True(d.ContainsKey("metrics_storage_id"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("open"));
        Assert.True(d.ContainsKey("project_id"));
        Assert.True(d.ContainsKey("query"));
        Assert.True(d.ContainsKey("template"));
        Assert.True(d.ContainsKey("threshold_critical"));
        Assert.True(d.ContainsKey("threshold_duration_critical"));
        Assert.True(d.ContainsKey("threshold_duration_warning"));
        Assert.True(d.ContainsKey("threshold_warning"));
        Assert.True(d.ContainsKey("uid"));
    }
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void AlertRule_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new AlertRule().Serialize(null!));
    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void AlertRule_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new AlertRule().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void AlertRule_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new AlertRule();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["enabled_critical"](parseNode);
        Assert.Equal(true, model.EnabledCritical);

        parseNode.GetBoolValue().Returns(true);
        deserializers["enabled_warning"](parseNode);
        Assert.Equal(true, model.EnabledWarning);

        parseNode.GetStringValue().Returns("test_format");
        deserializers["format"](parseNode);
        Assert.Equal("test_format", model.Format);

        parseNode.GetLongValue().Returns(1L);
        deserializers["metrics_storage_id"](parseNode);
        Assert.Equal(1L, model.MetricsStorageId);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["open"](parseNode);

        deserializers["project_id"](parseNode);

        parseNode.GetStringValue().Returns("test_query");
        deserializers["query"](parseNode);
        Assert.Equal("test_query", model.Query);

        parseNode.GetStringValue().Returns("test_template");
        deserializers["template"](parseNode);
        Assert.Equal("test_template", model.Template);

        parseNode.GetStringValue().Returns("test_threshold_critical");
        deserializers["threshold_critical"](parseNode);
        Assert.Equal("test_threshold_critical", model.ThresholdCritical);

        parseNode.GetLongValue().Returns(1L);
        deserializers["threshold_duration_critical"](parseNode);
        Assert.Equal(1L, model.ThresholdDurationCritical);

        parseNode.GetLongValue().Returns(1L);
        deserializers["threshold_duration_warning"](parseNode);
        Assert.Equal(1L, model.ThresholdDurationWarning);

        parseNode.GetStringValue().Returns("test_threshold_warning");
        deserializers["threshold_warning"](parseNode);
        Assert.Equal("test_threshold_warning", model.ThresholdWarning);

        deserializers["uid"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void AlertRule_Serialize_WritesExpectedFields()
    {
        var model = new AlertRule { EnabledCritical = true, EnabledWarning = true, Format = "test_format", MetricsStorageId = 1L, Name = "test_name", Query = "test_query", Template = "test_template", ThresholdCritical = "test_threshold_critical", ThresholdDurationCritical = 1L, ThresholdDurationWarning = 1L, ThresholdWarning = "test_threshold_warning" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteBoolValue("enabled_critical", true);
        writer.Received(1).WriteBoolValue("enabled_warning", true);
        writer.Received(1).WriteStringValue("format", "test_format");
        writer.Received(1).WriteLongValue("metrics_storage_id", 1L);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteStringValue("query", "test_query");
        writer.Received(1).WriteStringValue("template", "test_template");
        writer.Received(1).WriteStringValue("threshold_critical", "test_threshold_critical");
        writer.Received(1).WriteLongValue("threshold_duration_critical", 1L);
        writer.Received(1).WriteLongValue("threshold_duration_warning", 1L);
        writer.Received(1).WriteStringValue("threshold_warning", "test_threshold_warning");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
