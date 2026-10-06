using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsStorageCreateRequestモデルのテスト
/// </summary>
public class MetricsStorageCreateRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = MetricsStorageCreateRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<MetricsStorageCreateRequest>(result);
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new MetricsStorageCreateRequest().Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsStorageCreateRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_system"](parseNode);
        Assert.Equal(true, model.IsSystem);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_Serialize_WritesExpectedFields()
    {
        var model = new MetricsStorageCreateRequest { Description = "test_description", IsSystem = true, Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteBoolValue("is_system", true);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// Nameがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_Validation_NameIsNull_ReturnsValidationError()
    {
        var model = new MetricsStorageCreateRequest { Name = null, IsSystem = false };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MetricsStorageCreateRequest.Name)));
    }

    /// <summary>
    /// Nameが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_Validation_NameExceedsMaxLength_ReturnsValidationError()
    {
        var model = new MetricsStorageCreateRequest { Name = new string('a', 65), IsSystem = false };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MetricsStorageCreateRequest.Name)));
    }

    /// <summary>
    /// IsSystemがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_Validation_IsSystemIsNull_ReturnsValidationError()
    {
        var model = new MetricsStorageCreateRequest { Name = "valid-storage", IsSystem = null };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MetricsStorageCreateRequest.IsSystem)));
    }

    /// <summary>
    /// Descriptionが最大長を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_Validation_DescriptionExceedsMaxLength_ReturnsValidationError()
    {
        var model = new MetricsStorageCreateRequest { Name = "valid-storage", IsSystem = false, Description = new string('a', 513) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(MetricsStorageCreateRequest.Description)));
    }

    /// <summary>
    /// すべてのフィールドが有効な場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void MetricsStorageCreateRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new MetricsStorageCreateRequest { Name = "valid-storage", IsSystem = false, Description = "説明" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
