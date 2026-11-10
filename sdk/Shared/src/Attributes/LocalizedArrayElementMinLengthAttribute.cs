using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// コレクションの各文字列要素が最小文字数以上であることを検証するバリデーション属性です。
    /// </summary>
    /// <example>
    /// <code>
    /// [LocalizedArrayElementMinLength(1)]
    /// public List&lt;string&gt; Tags { get; set; }
    /// </code>
    /// </example>
    public class LocalizedArrayElementMinLengthAttribute : MinLengthAttribute
    {
        /// <summary>
        /// <see cref="LocalizedArrayElementMinLengthAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="minLength">文字列の最小長。</param>
        public LocalizedArrayElementMinLengthAttribute(int minLength)
            : base(minLength)
        {
            ErrorMessage = "{0}の各要素は{1}文字以上で入力してください。";
        }

        /// <summary>
        /// コレクションの各文字列要素の長さを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。<see cref="ICollection"/> を実装している必要があります。</param>
        /// <returns>
        /// 値が <see langword="null"/> またはコレクションでない場合、もしくはすべての要素の長さが最小値以上の場合は <see langword="true"/>。
        /// いずれかの要素の長さが最小値未満の場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null || !(value is ICollection collection))
            {
                return true;
            }

            foreach (var item in collection)
            {
                if (item is string str && !base.IsValid(str))
                {
                    return false;
                }
            }

            return true;
        }
    }
}