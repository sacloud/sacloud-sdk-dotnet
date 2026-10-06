using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// コレクションの各文字列要素が最大文字数以下であることを検証するバリデーション属性です。
    /// </summary>
    /// <example>
    /// <code>
    /// [LocalizedArrayElementMaxLength(5)]
    /// public List&lt;string&gt; Tags { get; set; }
    /// </code>
    /// </example>
    public class LocalizedArrayElementMaxLengthAttribute : MaxLengthAttribute
    {
        /// <summary>
        /// <see cref="LocalizedArrayElementMaxLengthAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="maxLength">文字列の最大長。</param>
        public LocalizedArrayElementMaxLengthAttribute(int maxLength)
            : base(maxLength)
        {
            ErrorMessage = "{0}の各要素は{1}文字以下で入力してください。";
        }

        /// <summary>
        /// コレクションの各文字列要素の長さを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。<see cref="ICollection"/> を実装している必要があります。</param>
        /// <returns>
        /// 値が <see langword="null"/> またはコレクションでない場合、もしくはすべての要素の長さが最大文字数以下の場合は <see langword="true"/>。
        /// いずれかの要素の長さが最大文字数を超える場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            // null またはコレクションでない場合はバリデーションをスキップ
            if (value == null || !(value is ICollection collection))
            {
                return true;
            }

            // 各要素の文字列長が最大値以下かどうかを検証する
            foreach (var item in collection)
            {
                if (item is string str)
                {
                    // 親クラス (MaxLengthAttribute) の IsValid で Length を使って検証
                    if (!base.IsValid(str))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
