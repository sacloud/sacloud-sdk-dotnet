using System.Globalization;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 日本語にローカライズした <see cref="RegularExpressionAttribute"/>
    /// </summary>
    public class LocalizedRegularExpressionAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// <see cref="LocalizedRegularExpressionAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="pattern">検証に使用する正規表現パターン。</param>
        public LocalizedRegularExpressionAttribute(string pattern) : base(pattern)
        {
            ErrorMessage = "{0}の形式が正しくありません。";
        }

        /// <summary>
        /// 正規表現に基づいて値を検証します。
        /// </summary>
        /// <param name="value">検証対象の値。</param>
        /// <returns>
        /// 値が <see langword="null"/> の場合は <see langword="true"/>。
        /// それ以外は、空文字を含めて正規表現パターンに完全一致した場合のみ <see langword="true"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (stringValue == null)
            {
                return true;
            }

            return IsFullMatch(stringValue);
        }

        private bool IsFullMatch(string value)
        {
            var regex = new Regex(Pattern);
            var match = regex.Match(value);

            return match.Success && match.Index == 0 && match.Length == value.Length;
        }
    }
}
