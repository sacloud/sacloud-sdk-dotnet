using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// コレクションの各要素が指定した列挙型に含まれる値であることを検証するバリデーション属性です。
    /// </summary>
    /// <example>
    /// <code>
    /// [LocalizedArrayElementEnumDataType(typeof(Status))]
    /// public List&lt;Status&gt; Statuses { get; set; }
    /// </code>
    /// </example>
    public class LocalizedArrayElementEnumDataTypeAttribute : DataTypeAttribute
    {
        /// <summary>
        /// <see cref="LocalizedArrayElementEnumDataTypeAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="enumType">各要素が属することを期待する列挙型。</param>
        public LocalizedArrayElementEnumDataTypeAttribute(Type enumType)
            : base("Enumerable")
        {
            EnumType = enumType;
            ErrorMessage = "{0}の各要素はEnumに含まれる値を入力してください。";
        }

        /// <summary>
        /// 検証対象の列挙型を取得します。
        /// </summary>
        public Type EnumType { get; }

        /// <summary>
        /// コレクションの各要素が指定した列挙型に含まれる値かどうかを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。<see cref="ICollection"/> を実装している必要があります。</param>
        /// <returns>
        /// 値が <see langword="null"/> またはコレクションでない場合、もしくはすべての要素が指定した列挙型に含まれる値である場合は <see langword="true"/>。
        /// いずれかの要素が指定した列挙型に含まれない場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            if (EnumType == null)
            {
                throw new InvalidOperationException("EnumTypeはnullにできません。");
            }

            if (!EnumType.IsEnum)
            {
                throw new InvalidOperationException($"EnumTypeは列挙型である必要があります。: {EnumType.FullName}");
            }

            // 値がnullまたはコレクションでない場合はバリデーションをスキップ
            if (value == null || !(value is ICollection collection))
            {
                return true;
            }

            foreach (var item in collection)
            {
                if (item == null)
                {
                    continue;
                }

                var stringValue = item as string;
                if (stringValue?.Length == 0)
                {
                    continue;
                }

                Type valueType = item.GetType();
                if (valueType.IsEnum && EnumType != valueType)
                {
                    return false;
                }

                if (!valueType.IsValueType && valueType != typeof(string))
                {
                    return false;
                }

                if (valueType == typeof(bool) ||
                    valueType == typeof(float) ||
                    valueType == typeof(double) ||
                    valueType == typeof(decimal))
                {
                    return false;
                }

                try
                {
                    if (!Enum.IsDefined(EnumType, item))
                    {
                        return false;
                    }
                }
                catch (ArgumentException)
                {
                    // Underlying 型が一致しない数値型が渡された場合など、ArgumentException が発生する場合は無効と判定
                    return false;
                }
            }

            return true;
        }
    }
}
