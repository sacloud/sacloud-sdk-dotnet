using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogMeasureRuleModelRequestモデルのテスト
/// </summary>
public class LogMeasureRuleModelRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModelRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogMeasureRuleModelRequest>(LogMeasureRuleModelRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModelRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogMeasureRuleModelRequest().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModelRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogMeasureRuleModelRequest().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("query"));
        Assert.True(d.ContainsKey("version"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModelRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogMeasureRuleModelRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["query"](parseNode);

        deserializers["version"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleModelRequest_Serialize_WritesExpectedFields()
    {
        var model = new LogMeasureRuleModelRequest();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    [Fact]
    public void LogMeasureRuleModelRequest_Validation_AllRequiredFieldsMissing_ReturnsValidationError()
    {
        var model = new LogMeasureRuleModelRequest();
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogMeasureRuleModelRequest.Query)));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogMeasureRuleModelRequest.Version)));
    }
}
