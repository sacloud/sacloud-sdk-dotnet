using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedMutualExclusivityAttributeのテスト
/// </summary>
public class LocalizedMutualExclusivityAttributeTests
{
    private sealed class SampleModel
    {
        public string? Primary { get; set; }

        public string? Secondary { get; set; }
    }

    /// <summary>
    /// IsValidが0個または1個のプロパティを設定したモデルを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithZeroOrOnePropertySet_ReturnsTrue()
    {
        var attribute = new LocalizedMutualExclusivityAttribute(nameof(SampleModel.Primary), nameof(SampleModel.Secondary));

        Assert.True(attribute.IsValid(new SampleModel()));
        Assert.True(attribute.IsValid(new SampleModel { Primary = "primary" }));
    }

    /// <summary>
    /// IsValidが複数のプロパティを設定したモデルを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithMultiplePropertiesSet_ReturnsFalse()
    {
        var attribute = new LocalizedMutualExclusivityAttribute(nameof(SampleModel.Primary), nameof(SampleModel.Secondary));

        Assert.False(attribute.IsValid(new SampleModel { Primary = "primary", Secondary = "secondary" }));
    }

    /// <summary>
    /// IsValidがnullを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullValue_ReturnsTrue()
    {
        var attribute = new LocalizedMutualExclusivityAttribute(nameof(SampleModel.Primary), nameof(SampleModel.Secondary));

        Assert.True(attribute.IsValid(null));
    }

    /// <summary>
    /// FormatErrorMessageがプロパティ名を含む日本語メッセージを返すことを確認
    /// </summary>
    [Fact]
    public void FormatErrorMessage_ReturnsLocalizedMessageWithPropertyNames()
    {
        var attribute = new LocalizedMutualExclusivityAttribute(nameof(SampleModel.Primary), nameof(SampleModel.Secondary));

        Assert.Equal("PrimaryまたはSecondaryのいずれか1つのみを設定してください。", attribute.FormatErrorMessage("値"));
    }

    /// <summary>
    /// コンストラクターがプロパティ名不足の場合にArgumentExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithLessThanTwoPropertyNames_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => new LocalizedMutualExclusivityAttribute("Primary"));

        Assert.Equal("propertyNames", exception.ParamName);
    }
}