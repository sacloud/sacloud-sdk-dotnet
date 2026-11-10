using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedEnumDataTypeAttributeのテスト
/// </summary>
public class LocalizedEnumDataTypeAttributeTests
{
    private enum SampleStatus
    {
        Active,
        Disabled
    }

    private enum OtherStatus
    {
        Active
    }

    /// <summary>
    /// IsValidが定義済みのEnum値を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDefinedEnumValue_ReturnsTrue()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.True(attribute.IsValid(SampleStatus.Active));
    }

    /// <summary>
    /// IsValidが定義済みの数値を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDefinedNumericValue_ReturnsTrue()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.True(attribute.IsValid(1));
    }

    /// <summary>
    /// IsValidが未定義の値を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithUndefinedValue_ReturnsFalse()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(2));
    }

    /// <summary>
    /// IsValidが異なるEnum型の値を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDifferentEnumType_ReturnsFalse()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(OtherStatus.Active));
    }

    /// <summary>
    /// IsValidがEnum以外の参照型を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNonEnumReferenceType_ReturnsFalse()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(new object()));
    }

    /// <summary>
    /// IsValidがEnum値として扱えない数値型およびbool型を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithUnsupportedValueType_ReturnsFalse()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(true));
        Assert.False(attribute.IsValid(1.5f));
        Assert.False(attribute.IsValid(1.5d));
        Assert.False(attribute.IsValid(1.5m));
    }

    /// <summary>
    /// IsValidがnullと空文字列を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrEmptyString_ReturnsTrue()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(string.Empty));
    }

    /// <summary>
    /// IsValidがEnum以外の型を指定された場合にInvalidOperationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNonEnumType_ThrowsInvalidOperationException()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(typeof(string));

        Assert.Throws<InvalidOperationException>(() => attribute.IsValid(SampleStatus.Active));
    }

    /// <summary>
    /// IsValidがnullのEnumTypeでInvalidOperationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullEnumType_ThrowsInvalidOperationException()
    {
        var attribute = new LocalizedEnumDataTypeAttribute(null!);

        var exception = Assert.Throws<InvalidOperationException>(() => attribute.IsValid(SampleStatus.Active));

        Assert.Equal("EnumTypeはnullにできません。", exception.Message);
    }
}