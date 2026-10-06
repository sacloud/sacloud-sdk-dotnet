using System.Collections;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 配列の文字列要素が正規表現にマッチすることをチェックするバリデーション属性
    /// </summary>
    public class LocalizedArrayElementRegularExpressionAttribute : RegularExpressionAttribute
    {
        public LocalizedArrayElementRegularExpressionAttribute(string pattern) : base(pattern)
        {
            ErrorMessage = "{0}の要素の形式が正しくありません。";
        }

        /// <summary>
        /// 配列要素の文字列を正規表現で検証します。
        /// </summary>
        /// <param name="value">検証対象の値。<see cref="ICollection"/> を実装している必要があります。</param>
        /// <returns>
        /// 値が <see langword="null"/> またはコレクションでない場合は <see langword="true"/>。
        /// 文字列要素が空文字を含めて正規表現に完全一致する場合は <see langword="true"/>。
        /// いずれかの要素が一致しない場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            // 値がnullまたは配列でない場合はバリデーションをスキップ
            if (value == null || !(value is ICollection array))
            {
                return true;
            }

            // 各要素が正規表現にマッチするかチェック
            foreach (var item in array)
            {
                if (item is string str)
                {
                    // 要素が正規表現にマッチしない場合はバリデーションエラー
                    if (!IsFullMatch(str))
                    {
                        return false;
                    }
                }
            }

            // すべての要素が正規表現にマッチする場合は有効
            return true;
        }

        private bool IsFullMatch(string value)
        {
            var regex = new Regex(Pattern);
            var match = regex.Match(value);

            return match.Success && match.Index == 0 && match.Length == value.Length;
        }
    }
}
