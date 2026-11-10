using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedAtLeastOneAttributeのテスト
/// </summary>
public class LocalizedAtLeastOneAttributeTests
{
    [LocalizedAtLeastOne(nameof(Primary), nameof(Secondary))]
    private sealed class SampleModel
    {
        [Display(Name = "主項目")]
        public string? Primary { get; set; }

        [Display(Name = "副項目")]
        public string? Secondary { get; set; }
    }

    /// <summary>
    /// Validatorがすべてnullのプロパティを無効と判定することを確認
    /// </summary>
    [Fact]
    public void Validate_WithAllPropertiesNull_ReturnsValidationError()
    {
        var model = new SampleModel();
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            model,
            new ValidationContext(model),
            validationResults,
            validateAllProperties: true);

        Assert.False(isValid);
        Assert.Equal("主項目または副項目のいずれかを設定してください。", validationResults.Single().ErrorMessage);
        Assert.Equal(new[] { nameof(SampleModel.Primary), nameof(SampleModel.Secondary) }, validationResults.Single().MemberNames);
    }

    /// <summary>
    /// Validatorが少なくとも1つの値を設定したモデルを有効と判定することを確認
    /// </summary>
    [Theory]
    [InlineData("primary", null)]
    [InlineData(null, "secondary")]
    [InlineData("primary", "secondary")]
    public void Validate_WithAnyPropertySet_PassesValidation(string? primary, string? secondary)
    {
        var model = new SampleModel
        {
            Primary = primary,
            Secondary = secondary
        };
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
    /// コンストラクターがプロパティ名不足の場合にArgumentExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithLessThanTwoPropertyNames_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => new LocalizedAtLeastOneAttribute("Primary"));

        Assert.Equal("propertyNames", exception.ParamName);
    }
}