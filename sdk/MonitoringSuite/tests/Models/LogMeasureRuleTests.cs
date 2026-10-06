using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogMeasureRuleモデルのテスト
/// </summary>
public class LogMeasureRuleTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRule_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogMeasureRule>(LogMeasureRule.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRule_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogMeasureRule().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRule_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogMeasureRule().GetFieldDeserializers();
        Assert.Equal(10, d.Count);
        Assert.True(d.ContainsKey("created_at"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("log_storage"));
        Assert.True(d.ContainsKey("metrics_storage"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("project_id"));
        Assert.True(d.ContainsKey("rule"));
        Assert.True(d.ContainsKey("uid"));
        Assert.True(d.ContainsKey("updated_at"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogMeasureRule_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogMeasureRule();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["created_at"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["id"](parseNode);

        deserializers["log_storage"](parseNode);

        deserializers["metrics_storage"](parseNode);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["project_id"](parseNode);

        deserializers["rule"](parseNode);

        deserializers["uid"](parseNode);

        deserializers["updated_at"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRule_Serialize_WritesExpectedFields()
    {
        var model = new LogMeasureRule { Description = "test_description", Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
