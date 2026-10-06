using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// コレクションの各文字列要素の長さが最小値以上かつ最大値以下であることを検証するバリデーション属性です。
    /// </summary>
    /// <example>
    /// <code>
    /// [LocalizedArrayElementLengthRange(1, 5)]
    /// public List&lt;string&gt; Tags { get; set; }
    /// </code>
    /// </example>
    public class LocalizedArrayElementLengthRangeAttribute : StringLengthAttribute
    {
        /// <summary>
        /// <see cref="LocalizedArrayElementLengthRangeAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="minimumLength">文字列の最小長。</param>
        /// <param name="maximumLength">文字列の最大長。</param>
        public LocalizedArrayElementLengthRangeAttribute(int minimumLength, int maximumLength)
            : base(maximumLength)
        {
            MinimumLength = minimumLength;
            ErrorMessage = "{0}の各要素は{2}文字以上{1}文字以下で入力してください。";
        }

        /// <summary>
        /// コレクションの各文字列要素の長さを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。<see cref="ICollection"/> を実装している必要があります。</param>
        /// <returns>
        /// 値が <see langword="null"/> またはコレクションでない場合、もしくはすべての要素の長さが範囲内の場合は <see langword="true"/>。
        /// いずれかの要素の長さが範囲外の場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            // null またはコレクションでない場合はバリデーションをスキップ
            if (value == null || !(value is ICollection collection))
            {
                return true;
            }

            // 各要素の文字列長が範囲内かどうかを検証する
            foreach (var item in collection)
            {
                if (item is string str)
                {
                    // 親クラス (StringLengthAttribute) の IsValid で MinimumLength/MaximumLength を使って検証
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
