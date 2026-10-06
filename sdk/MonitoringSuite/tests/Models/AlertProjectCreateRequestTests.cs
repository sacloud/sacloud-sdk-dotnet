using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// AlertProjectCreateRequestモデルのテスト
/// </summary>
public class AlertProjectCreateRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = AlertProjectCreateRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<AlertProjectCreateRequest>(result);
    }

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => AlertProjectCreateRequest.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new AlertProjectCreateRequest().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("name"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new AlertProjectCreateRequest().Serialize(null!));
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_Serialize_WritesExpectedFields()
    {
        var model = new AlertProjectCreateRequest { Name = "proj", Description = "desc" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("name", "proj");
        writer.Received(1).WriteStringValue("description", "desc");
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new AlertProjectCreateRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);
    }

    /// <summary>
    /// Nameがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_Validation_NameIsNull_ReturnsValidationError()
    {
        var model = new AlertProjectCreateRequest { Name = null };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(AlertProjectCreateRequest.Name)));
    }

    /// <summary>
    /// Nameが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_Validation_NameExceedsMaxLength_ReturnsValidationError()
    {
        var model = new AlertProjectCreateRequest { Name = new string('a', 65) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(AlertProjectCreateRequest.Name)));
    }

    /// <summary>
    /// Nameが最大長ちょうどの場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_Validation_NameAtMaxLength_PassesValidation()
    {
        var model = new AlertProjectCreateRequest { Name = new string('a', 64) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Descriptionが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_Validation_DescriptionExceedsMaxLength_ReturnsValidationError()
    {
        var model = new AlertProjectCreateRequest { Name = "valid", Description = new string('a', 513) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(AlertProjectCreateRequest.Description)));
    }

    /// <summary>
    /// すべてのフィールドが有効な場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void AlertProjectCreateRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new AlertProjectCreateRequest { Name = "valid-project", Description = "説明" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
