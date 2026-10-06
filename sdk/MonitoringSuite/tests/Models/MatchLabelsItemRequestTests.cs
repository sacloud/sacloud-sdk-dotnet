using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MatchLabelsItemRequestモデルのテスト
/// </summary>
public class MatchLabelsItemRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MatchLabelsItemRequest>(MatchLabelsItemRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MatchLabelsItemRequest().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MatchLabelsItemRequest().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("value"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MatchLabelsItemRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        parseNode.GetStringValue().Returns("test_value");
        deserializers["value"](parseNode);
        Assert.Equal("test_value", model.Value);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Serialize_WritesExpectedFields()
    {
        var model = new MatchLabelsItemRequest { Name = "test_name", Value = "test_value" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteStringValue("value", "test_value");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// Nameがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Validation_NameIsNull_ReturnsValidationError()
    {
        var model = new MatchLabelsItemRequest { Name = null, Value = "val" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MatchLabelsItemRequest.Name)));
    }

    /// <summary>
    /// Nameが空文字の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Validation_NameIsEmpty_ReturnsValidationError()
    {
        var model = new MatchLabelsItemRequest { Name = "", Value = "val" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MatchLabelsItemRequest.Name)));
    }

    /// <summary>
    /// Nameが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Validation_NameExceedsMaxLength_ReturnsValidationError()
    {
        var model = new MatchLabelsItemRequest { Name = new string('a', 257), Value = "val" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MatchLabelsItemRequest.Name)));
    }

    /// <summary>
    /// Valueがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Validation_ValueIsNull_ReturnsValidationError()
    {
        var model = new MatchLabelsItemRequest { Name = "key", Value = null };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MatchLabelsItemRequest.Value)));
    }

    /// <summary>
    /// Valueが空文字の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Validation_ValueIsEmpty_ReturnsValidationError()
    {
        var model = new MatchLabelsItemRequest { Name = "key", Value = "" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MatchLabelsItemRequest.Value)));
    }

    /// <summary>
    /// Valueが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Validation_ValueExceedsMaxLength_ReturnsValidationError()
    {
        var model = new MatchLabelsItemRequest { Name = "key", Value = new string('a', 257) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MatchLabelsItemRequest.Value)));
    }

    /// <summary>
    /// すべてのフィールドが有効な場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void MatchLabelsItemRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new MatchLabelsItemRequest { Name = "severity", Value = "critical" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
