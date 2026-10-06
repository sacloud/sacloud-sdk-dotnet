using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Shared.Extensions;

namespace Sacloud.Sdk.Shared.Tests.Extensions;

/// <summary>
/// ISerializationWriterExtensionのテスト
/// </summary>
public class ISerializationWriterExtensionTests
{
    /// <summary>
    /// WriteNullableIntValueがnull値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableIntValue_WithNullValue_WritesNullValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        int? value = null;

        writer.WriteNullableIntValue("testKey", value);

        writer.Received(1).WriteNullValue("testKey");
        writer.DidNotReceive().WriteIntValue(Arg.Any<string>(), Arg.Any<int>());
    }

    /// <summary>
    /// WriteNullableIntValueが正の非null値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableIntValue_WithPositiveValue_WritesIntValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        int? value = 42;

        writer.WriteNullableIntValue("testKey", value);

        writer.Received(1).WriteIntValue("testKey", 42);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableIntValueがゼロ値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableIntValue_WithZeroValue_WritesIntValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        int? value = 0;

        writer.WriteNullableIntValue("testKey", value);

        writer.Received(1).WriteIntValue("testKey", 0);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableIntValueが負の値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableIntValue_WithNegativeValue_WritesIntValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        int? value = -100;

        writer.WriteNullableIntValue("testKey", value);

        writer.Received(1).WriteIntValue("testKey", -100);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableIntValueが境界値int.MaxValueを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableIntValue_WithMaxValue_WritesIntValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        int? value = int.MaxValue;

        writer.WriteNullableIntValue("testKey", value);

        writer.Received(1).WriteIntValue("testKey", int.MaxValue);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableIntValueが境界値int.MinValueを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableIntValue_WithMinValue_WritesIntValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        int? value = int.MinValue;

        writer.WriteNullableIntValue("testKey", value);

        writer.Received(1).WriteIntValue("testKey", int.MinValue);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableLongValueがnull値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableLongValue_WithNullValue_WritesNullValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        long? value = null;

        writer.WriteNullableLongValue("testKey", value);

        writer.Received(1).WriteNullValue("testKey");
        writer.DidNotReceive().WriteLongValue(Arg.Any<string>(), Arg.Any<long>());
    }

    /// <summary>
    /// WriteNullableLongValueが正の非null値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableLongValue_WithPositiveValue_WritesLongValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        long? value = 999999999999;

        writer.WriteNullableLongValue("testKey", value);

        writer.Received(1).WriteLongValue("testKey", 999999999999);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableLongValueがゼロ値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableLongValue_WithZeroValue_WritesLongValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        long? value = 0;

        writer.WriteNullableLongValue("testKey", value);

        writer.Received(1).WriteLongValue("testKey", 0);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableLongValueが負の値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableLongValue_WithNegativeValue_WritesLongValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        long? value = -999999999999;

        writer.WriteNullableLongValue("testKey", value);

        writer.Received(1).WriteLongValue("testKey", -999999999999);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableLongValueが境界値long.MaxValueを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableLongValue_WithMaxValue_WritesLongValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        long? value = long.MaxValue;

        writer.WriteNullableLongValue("testKey", value);

        writer.Received(1).WriteLongValue("testKey", long.MaxValue);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableLongValueが境界値long.MinValueを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableLongValue_WithMinValue_WritesLongValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        long? value = long.MinValue;

        writer.WriteNullableLongValue("testKey", value);

        writer.Received(1).WriteLongValue("testKey", long.MinValue);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableStringValueがnull値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableStringValue_WithNullValue_WritesNullValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        string? value = null;

        writer.WriteNullableStringValue("testKey", value);

        writer.Received(1).WriteNullValue("testKey");
        writer.DidNotReceive().WriteStringValue(Arg.Any<string>(), Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableStringValueが非null値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableStringValue_WithNonNullValue_WritesStringValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        string? value = "test string";

        writer.WriteNullableStringValue("testKey", value);

        writer.Received(1).WriteStringValue("testKey", "test string");
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableStringValueが空文字列を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableStringValue_WithEmptyString_WritesStringValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        string? value = string.Empty;

        writer.WriteNullableStringValue("testKey", value);

        writer.Received(1).WriteStringValue("testKey", string.Empty);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableStringValueが空白文字を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableStringValue_WithWhitespace_WritesStringValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        string? value = "   ";

        writer.WriteNullableStringValue("testKey", value);

        writer.Received(1).WriteStringValue("testKey", "   ");
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableStringValueが長い文字列を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableStringValue_WithLongString_WritesStringValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        string? value = new string('a', 1000);

        writer.WriteNullableStringValue("testKey", value);

        writer.Received(1).WriteStringValue("testKey", value);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableStringValueが特殊文字を含む文字列を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableStringValue_WithSpecialCharacters_WritesStringValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        string? value = "Hello\nWorld\t\"Test\"";

        writer.WriteNullableStringValue("testKey", value);

        writer.Received(1).WriteStringValue("testKey", "Hello\nWorld\t\"Test\"");
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableDateTimeOffsetValueがnull値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableDateTimeOffsetValue_WithNullValue_WritesNullValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        DateTimeOffset? value = null;

        writer.WriteNullableDateTimeOffsetValue("testKey", value);

        writer.Received(1).WriteNullValue("testKey");
        writer.DidNotReceive().WriteDateTimeOffsetValue(Arg.Any<string>(), Arg.Any<DateTimeOffset>());
    }

    /// <summary>
    /// WriteNullableDateTimeOffsetValueが非null値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableDateTimeOffsetValue_WithNonNullValue_WritesDateTimeOffsetValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        DateTimeOffset? value = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

        writer.WriteNullableDateTimeOffsetValue("testKey", value);

        writer.Received(1).WriteDateTimeOffsetValue("testKey", value.Value);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableDateTimeOffsetValueがDateTimeOffset.MinValueを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableDateTimeOffsetValue_WithMinValue_WritesDateTimeOffsetValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        DateTimeOffset? value = DateTimeOffset.MinValue;

        writer.WriteNullableDateTimeOffsetValue("testKey", value);

        writer.Received(1).WriteDateTimeOffsetValue("testKey", DateTimeOffset.MinValue);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableDateTimeOffsetValueがDateTimeOffset.MaxValueを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableDateTimeOffsetValue_WithMaxValue_WritesDateTimeOffsetValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        DateTimeOffset? value = DateTimeOffset.MaxValue;

        writer.WriteNullableDateTimeOffsetValue("testKey", value);

        writer.Received(1).WriteDateTimeOffsetValue("testKey", DateTimeOffset.MaxValue);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableDateTimeOffsetValueがタイムゾーンオフセットを持つ値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableDateTimeOffsetValue_WithPositiveOffset_WritesDateTimeOffsetValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        DateTimeOffset? value = new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.FromHours(9));

        writer.WriteNullableDateTimeOffsetValue("testKey", value);

        writer.Received(1).WriteDateTimeOffsetValue("testKey", value.Value);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableDateTimeOffsetValueが空文字キーでも正しく動作することを確認
    /// </summary>
    [Fact]
    public void WriteNullableDateTimeOffsetValue_WithEmptyKey_WritesCorrectly()
    {
        var writer = Substitute.For<ISerializationWriter>();
        DateTimeOffset? value = DateTimeOffset.UtcNow;

        writer.WriteNullableDateTimeOffsetValue(string.Empty, value);

        writer.Received(1).WriteDateTimeOffsetValue(string.Empty, value.Value);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableDateTimeOffsetValueが空文字キーにnull値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableDateTimeOffsetValue_WithEmptyKeyAndNullValue_WritesNullValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        DateTimeOffset? value = null;

        writer.WriteNullableDateTimeOffsetValue(string.Empty, value);

        writer.Received(1).WriteNullValue(string.Empty);
        writer.DidNotReceive().WriteDateTimeOffsetValue(Arg.Any<string>(), Arg.Any<DateTimeOffset>());
    }

    /// <summary>
    /// WriteNullableObjectValueがnull値を正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableObjectValue_WithNullValue_WritesNullValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        IParsable? value = null;

        writer.WriteNullableObjectValue("testKey", value);

        writer.Received(1).WriteNullValue("testKey");
        writer.DidNotReceive().WriteObjectValue(Arg.Any<string>(), Arg.Any<IParsable>());
    }

    /// <summary>
    /// WriteNullableObjectValueが非nullオブジェクトを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void WriteNullableObjectValue_WithNonNullValue_WritesObjectValue()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var value = Substitute.For<IParsable>();

        writer.WriteNullableObjectValue("testKey", value);

        writer.Received(1).WriteObjectValue("testKey", value);
        writer.DidNotReceive().WriteNullValue(Arg.Any<string>());
    }

    /// <summary>
    /// WriteNullableIntValueとWriteNullableObjectValueが同じキーで順次呼び出された場合に正しく動作することを確認
    /// </summary>
    [Fact]
    public void WriteNullable_MultipleCallsWithSameKey_WorksCorrectly()
    {
        var writer = Substitute.For<ISerializationWriter>();
        int? intValue = 123;
        IParsable? objValue = Substitute.For<IParsable>();

        writer.WriteNullableIntValue("testKey", intValue);
        writer.WriteNullableObjectValue("testKey", objValue);

        writer.Received(1).WriteIntValue("testKey", 123);
        writer.Received(1).WriteObjectValue("testKey", objValue);
    }
}
