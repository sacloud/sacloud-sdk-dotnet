using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedExactlyOneAttributeのテスト
/// </summary>
public class LocalizedExactlyOneAttributeTests
{
    [LocalizedExactlyOne(nameof(Primary), nameof(Secondary))]
    private sealed class SampleModel
    {
        [Display(Name = "主項目")]
        public string? Primary { get; set; }

        [Display(Name = "副項目")]
        public string? Secondary { get; set; }
    }

    private sealed class SampleModelWithoutDisplay
    {
        public string? Primary { get; set; }

        public string? Secondary { get; set; }
    }

    private sealed class SampleModelWithMissingProperty
    {
        public string? Primary { get; set; }
    }

    /// <summary>
    /// コンストラクターがプロパティ名不足の場合にArgumentExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithLessThanTwoPropertyNames_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => new LocalizedExactlyOneAttribute("Primary"));

        Assert.Equal("propertyNames", exception.ParamName);
        Assert.Equal("少なくとも2つのプロパティ名を指定する必要があります。 (Parameter 'propertyNames')", exception.Message);
    }

    /// <summary>
    /// GetValidationResultがnullの値で成功を返すことを確認
    /// </summary>
    [Fact]
    public void GetValidationResult_WithNullValue_ReturnsSuccess()
    {
        var attribute = new LocalizedExactlyOneAttribute("Primary", "Secondary");
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(null, validationContext);

        Assert.Null(result);
    }

    /// <summary>
    /// GetValidationResultが未存在プロパティ名を設定エラーとして返すことを確認
    /// </summary>
    [Fact]
    public void GetValidationResult_WithMissingPropertyName_ReturnsConfigurationError()
    {
        var attribute = new LocalizedExactlyOneAttribute(nameof(SampleModelWithMissingProperty.Primary), "Missing");
        var model = new SampleModelWithMissingProperty();

        var result = attribute.GetValidationResult(model, new ValidationContext(model));

        Assert.NotNull(result);
        Assert.Equal(
            "LocalizedExactlyOneAttributeの設定に誤りがあります。次のプロパティが見つかりません: Missing",
            result.ErrorMessage);
        Assert.Equal(new[] { "Missing" }, result.MemberNames);
    }

    /// <summary>
    /// GetValidationResultがDisplay属性のないプロパティ名をエラーメッセージに使用することを確認
    /// </summary>
    [Fact]
    public void GetValidationResult_WithoutDisplayAttribute_UsesPropertyName()
    {
        var attribute = new LocalizedExactlyOneAttribute(
            nameof(SampleModelWithoutDisplay.Primary),
            nameof(SampleModelWithoutDisplay.Secondary));
        var model = new SampleModelWithoutDisplay();

        var result = attribute.GetValidationResult(model, new ValidationContext(model));

        Assert.NotNull(result);
        Assert.Equal("PrimaryまたはSecondaryのいずれか1つのみを設定してください。", result.ErrorMessage);
    }

    /// <summary>
    /// Validatorが0個のプロパティを設定したモデルを無効と判定することを確認
    /// </summary>
    [Fact]
    public void Validate_WithNoPropertiesSet_ReturnsValidationError()
    {
        var model = new SampleModel();
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            model,
            new ValidationContext(model),
            validationResults,
            validateAllProperties: true);

        Assert.False(isValid);
        Assert.Equal("主項目または副項目のいずれか1つのみを設定してください。", validationResults.Single().ErrorMessage);
    }

    /// <summary>
    /// Validatorが1つのプロパティを設定したモデルを有効と判定することを確認
    /// </summary>
    [Fact]
    public void Validate_WithOnePropertySet_PassesValidation()
    {
        var model = new SampleModel { Primary = "primary" };
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            model,
            new ValidationContext(model),
            validationResults,
            validateAllProperties: true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    /// <summary>
    /// Validatorが複数のプロパティを設定したモデルを無効と判定することを確認
    /// </summary>
    [Fact]
    public void Validate_WithMultiplePropertiesSet_ReturnsValidationError()
    {
        var model = new SampleModel { Primary = "primary", Secondary = "secondary" };
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            model,
            new ValidationContext(model),
            validationResults,
            validateAllProperties: true);

        Assert.False(isValid);
        Assert.Equal("主項目または副項目のいずれか1つのみを設定してください。", validationResults.Single().ErrorMessage);
    }
}