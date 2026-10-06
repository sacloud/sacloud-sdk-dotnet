using System.Net;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// IPv4アドレスの形式であることのチェックを行うバリデーション属性
    /// </summary>
    public class LocalizedIpv4Address : ValidationAttribute
    {
        // 厳密なIPv4形式の正規表現（省略形、各オクテットの不要な先頭ゼロ、各オクテットの0-255の範囲外の値を拒否）
        private static readonly Regex IPv4Regex = new Regex(
            @"^(?:(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\.){3}(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])$",
            RegexOptions.Compiled);

        public LocalizedIpv4Address()
        {
            ErrorMessage = "{0}には有効なIPv4アドレス形式を入力してください。";
        }

        public override bool IsValid(object value)
        {
            // nullの場合はバリデーションをスキップ
            if (value == null)
            {
                return true;
            }

            // 空文字列の場合はバリデーションをスキップ
            var stringValue = value.ToString();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return true;
            }

            // 正規表現で形式チェック（省略形、各オクテットの不要な先頭ゼロ、各オクテットの0-255の範囲外の値を拒否）
            if (!IPv4Regex.IsMatch(stringValue))
            {
                return false;
            }

            // 文字列がIPv4アドレス形式かどうかをチェック
            // IPAddress.TryParse()メソッドではIPv6アドレスも有効と判定されてしまうため、AddressFamilyプロパティでIPv4かどうかを確認する
            // https://learn.microsoft.com/ja-jp/dotnet/api/system.net.sockets.addressfamily?view=netstandard-2.0
            return IPAddress.TryParse(stringValue, out var ipAddress) && ipAddress.AddressFamily == Net.Sockets.AddressFamily.InterNetwork;
        }
    }
}
