using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Attributes;

/// <summary>
/// LocalizedArrayElementEnumDataTypeAttributeのテスト
/// </summary>
public class LocalizedArrayElementEnumDataTypeAttributeTests
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
    /// IsValidが定義済みのEnum値を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDefinedEnumValues_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.True(attribute.IsValid(new[] { SampleStatus.Active, SampleStatus.Disabled }));
    }

    /// <summary>
    /// IsValidがnull要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullElement_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));
        object?[] values = { null, SampleStatus.Active };

        Assert.True(attribute.IsValid(values));
    }

    /// <summary>
    /// IsValidが空文字列要素を含むコレクションを有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithEmptyStringElement_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));
        object[] values = { string.Empty, SampleStatus.Active };

        Assert.True(attribute.IsValid(values));
    }

    /// <summary>
    /// IsValidが未定義の値を含むコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithUndefinedValue_ReturnsFalse()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(new[] { 0, 2 }));
    }

    /// <summary>
    /// IsValidがEnumの基底型と異なる数値型の要素を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithMismatchedUnderlyingType_ReturnsFalse()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(new object[] { 0L }));
    }

    /// <summary>
    /// IsValidが異なるEnum型の値を含むコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithDifferentEnumType_ReturnsFalse()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(new[] { OtherStatus.Active }));
    }

    /// <summary>
    /// IsValidがEnum以外の参照型要素を含むコレクションを無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNonEnumReferenceTypeElement_ReturnsFalse()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(new object[] { new object() }));
    }

    /// <summary>
    /// IsValidがEnum値として扱えない数値型およびbool型の要素を無効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithUnsupportedValueTypeElement_ReturnsFalse()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.False(attribute.IsValid(new object[] { true }));
        Assert.False(attribute.IsValid(new object[] { 1.5f }));
        Assert.False(attribute.IsValid(new object[] { 1.5d }));
        Assert.False(attribute.IsValid(new object[] { 1.5m }));
    }

    /// <summary>
    /// IsValidがnullまたはコレクション以外を有効と判定することを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullOrNonCollection_ReturnsTrue()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(SampleStatus));

        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(new object()));
    }

    /// <summary>
    /// IsValidがnullのEnumTypeでInvalidOperationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNullEnumType_ThrowsInvalidOperationException()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(null!);

        var exception = Assert.Throws<InvalidOperationException>(() => attribute.IsValid(new[] { 0 }));

        Assert.Equal("EnumTypeはnullにできません。", exception.Message);
    }

    /// <summary>
    /// IsValidがEnum以外の型を指定された場合にInvalidOperationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void IsValid_WithNonEnumType_ThrowsInvalidOperationException()
    {
        var attribute = new LocalizedArrayElementEnumDataTypeAttribute(typeof(string));

        var exception = Assert.Throws<InvalidOperationException>(() => attribute.IsValid(new[] { 0 }));

        Assert.Equal("EnumTypeは列挙型である必要があります。: System.String", exception.Message);
    }
}