using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedLogMeasureRuleRequestモデルのテスト
/// </summary>
public class PatchedLogMeasureRuleRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedLogMeasureRuleRequest>(PatchedLogMeasureRuleRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedLogMeasureRuleRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedLogMeasureRuleRequest().GetFieldDeserializers();
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
    public void PatchedLogMeasureRuleRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedLogMeasureRuleRequest();
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
    public void PatchedLogMeasureRuleRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedLogMeasureRuleRequest { Description = "test_description", LogStorageId = 1L, MetricsStorageId = 1L, Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteLongValue("log_storage_id", 1L);
        writer.Received(1).WriteLongValue("metrics_storage_id", 1L);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// Nameにパターン違反の文字列を指定した場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_Validation_NameInvalidPattern_ReturnsValidationError()
    {
        var model = new PatchedLogMeasureRuleRequest { Name = "Invalid Name!" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedLogMeasureRuleRequest.Name)));
    }

    /// <summary>
    /// Nameが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_Validation_NameExceedsMaxLength_ReturnsValidationError()
    {
        var model = new PatchedLogMeasureRuleRequest { Name = new string('a', 257) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedLogMeasureRuleRequest.Name)));
    }

    /// <summary>
    /// Descriptionが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_Validation_DescriptionExceedsMaxLength_ReturnsValidationError()
    {
        var model = new PatchedLogMeasureRuleRequest { Description = new string('a', 257) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedLogMeasureRuleRequest.Description)));
    }

    /// <summary>
    /// 全フィールドがnullの場合にバリデーションが成功することを確認（PATCH は全フィールド任意）
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_Validation_AllNullModel_PassesValidation()
    {
        var model = new PatchedLogMeasureRuleRequest();
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// 有効な値を設定した場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void PatchedLogMeasureRuleRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new PatchedLogMeasureRuleRequest { Name = "valid-rule_name123", Description = "説明" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
