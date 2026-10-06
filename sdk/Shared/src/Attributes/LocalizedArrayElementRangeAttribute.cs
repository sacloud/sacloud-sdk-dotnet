using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// コレクションの各要素が最小値以上かつ最大値以下であることを検証するバリデーション属性です。
    /// </summary>
    /// <example>
    /// <code>
    /// [LocalizedArrayElementRange(100, 999)]
    /// public List&lt;int&gt; IfStatusCode { get; set; }
    /// </code>
    /// </example>
    public class LocalizedArrayElementRangeAttribute : RangeAttribute
    {
        /// <summary>
        /// <see cref="LocalizedArrayElementRangeAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="minimum">要素の最小値。</param>
        /// <param name="maximum">要素の最大値。</param>
        public LocalizedArrayElementRangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {
            ErrorMessage = "{0}の各要素は{1}以上{2}以下で入力してください。";
        }

        /// <summary>
        /// コレクションの各要素が指定された範囲内かを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。<see cref="ICollection"/> を実装している必要があります。</param>
        /// <returns>
        /// 値が <see langword="null"/> またはコレクションでない場合、もしくはすべての要素が範囲内の場合は <see langword="true"/>。
        /// いずれかの要素が範囲外の場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            // null またはコレクションでない場合はバリデーションをスキップ
            if (value == null || !(value is ICollection collection))
            {
                return true;
            }

            // 各要素に対して RangeAttribute の検証を適用
            foreach (var item in collection)
            {
                if (!base.IsValid(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
