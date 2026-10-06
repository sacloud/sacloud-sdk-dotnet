using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogMeasureRuleModelモデルのテスト
/// </summary>
public class LogMeasureRuleModelTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModel_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogMeasureRuleModel>(LogMeasureRuleModel.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModel_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogMeasureRuleModel().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModel_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogMeasureRuleModel().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("query"));
        Assert.True(d.ContainsKey("version"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModel_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogMeasureRuleModel();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["query"](parseNode);

        deserializers["version"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModel_Serialize_WritesExpectedFields()
    {
        var model = new LogMeasureRuleModel();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
