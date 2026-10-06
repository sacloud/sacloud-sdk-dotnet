using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogMeasureRuleRequestモデルのテスト
/// </summary>
public class LogMeasureRuleRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogMeasureRuleRequest>(LogMeasureRuleRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogMeasureRuleRequest().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogMeasureRuleRequest().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("log_storage_id"));
        Assert.True(d.ContainsKey("metrics_storage_id"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("rule"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogMeasureRuleRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        parseNode.GetLongValue().Returns(1L);
        deserializers["log_storage_id"](parseNode);
        Assert.Equal(1L, model.LogStorageId);

        parseNode.GetLongValue().Returns(1L);
        deserializers["metrics_storage_id"](parseNode);
        Assert.Equal(1L, model.MetricsStorageId);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["rule"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Serialize_WritesExpectedFields()
    {
        var model = new LogMeasureRuleRequest { Description = "test_description", LogStorageId = 1L, MetricsStorageId = 1L, Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteNullableLongValue("log_storage_id", 1L);
        writer.Received(1).WriteNullableLongValue("metrics_storage_id", 1L);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// LogStorageIdとMetricsStorageIdがnullの場合にSerializeがnull値のフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Serialize_WithNullLogStorageIdAndMetricsStorageId_WritesNullValues()
    {
        var model = new LogMeasureRuleRequest { LogStorageId = null, MetricsStorageId = null };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteNullableLongValue("log_storage_id", null);
        writer.Received(1).WriteNullableLongValue("metrics_storage_id", null);
    }

    /// <summary>
    /// LogStorageIdとMetricsStorageIdが未定義の場合にSerializeがnull値のフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Serialize_WithUndefinedLogStorageIdAndMetricsStorageId_WritesNullValues()
    {
        var model = new LogMeasureRuleRequest();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteNullableLongValue("log_storage_id", null);
        writer.Received(1).WriteNullableLongValue("metrics_storage_id", null);
    }

    /// <summary>
    /// Ruleがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Validation_RuleIsNull_ReturnsValidationError()
    {
        var model = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 1L,
            Rule = null
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogMeasureRuleRequest.Rule)));
    }

    /// <summary>
    /// Nameが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Validation_NameExceedsMaxLength_ReturnsValidationError()
    {
        var model = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 1L,
            Rule = new LogMeasureRuleModelRequest(),
            Name = new string('a', 257)
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogMeasureRuleRequest.Name)));
    }

    /// <summary>
    /// Nameにパターン違反の文字列を指定した場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Validation_NameInvalidPattern_ReturnsValidationError()
    {
        var model = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 1L,
            Rule = new LogMeasureRuleModelRequest(),
            Name = "Invalid Name!"
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogMeasureRuleRequest.Name)));
    }

    /// <summary>
    /// Nameが有効なパターンの場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void LogMeasureRuleRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new LogMeasureRuleRequest
        {
            LogStorageId = 1L,
            MetricsStorageId = 1L,
            Rule = new LogMeasureRuleModelRequest(),
            Name = "valid-rule_name123",
            Description = "説明"
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
