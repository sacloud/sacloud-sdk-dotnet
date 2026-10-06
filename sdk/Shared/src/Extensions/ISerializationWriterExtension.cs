using Microsoft.Kiota.Abstractions.Serialization;
using System;

namespace Sacloud.Sdk.Shared.Extensions
{
    /// <summary>
    /// <see cref="ISerializationWriter"/> に対して、Nullable 型の値を適切に JSON にシリアライズするための拡張メソッドを提供します。
    /// </summary>
    /// <remarks>
    /// Kiota で生成されたコードでは Nullable 型の値を明示的に null として書き込む機能が不足しているため、
    /// この拡張メソッドを使用して null 値を適切に JSON に含めることができます。
    /// </remarks>
    public static class ISerializationWriterExtension
    {
        /// <summary>
        /// Nullable な int 値を JSON に書き込みます。
        /// </summary>
        /// <param name="writer">シリアライゼーションライター。</param>
        /// <param name="key">書き込むプロパティキー。</param>
        /// <param name="value">書き込む int 値。null の場合は null 値として書き込まれます。</param>
        /// <remarks>
        /// 値が null の場合は明示的に null 値として JSON に書き込まれます。
        /// 値が null でない場合は、その値がそのまま書き込まれます。
        /// </remarks>
        public static void WriteNullableIntValue(this ISerializationWriter writer, string key, int? value)
        {
            if (value == null)
            {
                writer.WriteNullValue(key);
            }
            else
            {
                writer.WriteIntValue(key, value.Value);
            }
        }

        /// <summary>
        /// Nullable な long 値を JSON に書き込みます。
        /// </summary>
        /// <param name="writer">シリアライゼーションライター。</param>
        /// <param name="key">書き込むプロパティキー。</param>
        /// <param name="value">書き込む long 値。null の場合は null 値として書き込まれます。</param>
        /// <remarks>
        /// 値が null の場合は明示的に null 値として JSON に書き込まれます。
        /// 値が null でない場合は、その値がそのまま書き込まれます。
        /// </remarks>
        public static void WriteNullableLongValue(this ISerializationWriter writer, string key, long? value)
        {
            if (value == null)
            {
                writer.WriteNullValue(key);
            }
            else
            {
                writer.WriteLongValue(key, value.Value);
            }
        }

        /// <summary>
        /// Nullable な string 値を JSON に書き込みます。
        /// </summary>
        /// <param name="writer">シリアライゼーションライター。</param>
        /// <param name="key">書き込むプロパティキー。</param>
        /// <param name="value">書き込む string 値。null の場合は null 値として書き込まれます。</param>
        /// <remarks>
        /// 値が null の場合は明示的に null 値として JSON に書き込まれます。
        /// 値が null でない場合は、その値がそのまま書き込まれます。
        /// </remarks>
        public static void WriteNullableStringValue(this ISerializationWriter writer, string key, string value)
        {
            if (value == null)
            {
                writer.WriteNullValue(key);
            }
            else
            {
                writer.WriteStringValue(key, value);
            }
        }

        /// <summary>
        /// Nullable な DateTimeOffset 値を JSON に書き込みます。
        /// </summary>
        /// <param name="writer">シリアライゼーションライター。</param>
        /// <param name="key">書き込むプロパティキー。</param>
        /// <param name="value">書き込む DateTimeOffset 値。null の場合は null 値として書き込まれます。</param>
        /// <remarks>
        /// 値が null の場合は明示的に null 値として JSON に書き込まれます。
        /// 値が null でない場合は、その値がそのまま書き込まれます。
        /// </remarks>
        public static void WriteNullableDateTimeOffsetValue(this ISerializationWriter writer, string key, DateTimeOffset? value)
        {
            if (value == null)
            {
                writer.WriteNullValue(key);
            }
            else
            {
                writer.WriteDateTimeOffsetValue(key, value.Value);
            }
        }

        /// <summary>
        /// Nullable なオブジェクト値を JSON に書き込みます。
        /// </summary>
        /// <typeparam name="T">書き込むオブジェクトの型。<see cref="IParsable"/> を実装した参照型である必要があります。</typeparam>
        /// <param name="writer">シリアライゼーションライター。</param>
        /// <param name="key">書き込むプロパティキー。</param>
        /// <param name="value">書き込むオブジェクト値。null の場合は null 値として書き込まれます。</param>
        /// <remarks>
        /// 値が null の場合は明示的に null 値として JSON に書き込まれます。
        /// 値が null でない場合は、<see cref="IParsable"/> のシリアライゼーション機能を使用してオブジェクトが書き込まれます。
        /// </remarks>
        public static void WriteNullableObjectValue<T>(this ISerializationWriter writer, string key, T value)
            where T : class, IParsable
        {
            if (value == null)
            {
                writer.WriteNullValue(key);
            }
            else
            {
                writer.WriteObjectValue(key, value);
            }
        }

        /// <summary>
        /// Nullable な プリミティブ値のコレクションを JSON に書き込みます。
        /// </summary>
        /// <param name="writer">シリアライゼーションライター。</param>
        /// <param name="key">書き込むプロパティキー。</param>
        /// <param name="values">書き込むプリミティブ値のコレクション。null の場合は null 値として書き込まれます。</param>
        /// <remarks>
        /// 値が null の場合は明示的に null 値として JSON に書き込まれます。
        /// 値が null でない場合は、その値がそのまま書き込まれます。
        /// </remarks>
        public static void WriteNullableCollectionOfPrimitiveValues<T>(this ISerializationWriter writer, string key, System.Collections.Generic.IEnumerable<T> values)
        {
            if (values == null)
            {
                writer.WriteNullValue(key);
            }
            else
            {
                writer.WriteCollectionOfPrimitiveValues<T>(key, values);
            }
        }
    }
}
