using System;

namespace Sacloud.Sdk.Shared.Validation
{
    /// <summary>
    /// パスパラメータのバリデーションオプションを定義します。
    /// </summary>
    public class PathParameterValidationOptions
    {
        /// <summary>
        /// パラメータの表示名
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 最小文字数(nullの場合はチェックしない)
        /// Lengthが指定されている場合、この設定は無視されます。
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// 最大文字数(nullの場合はチェックしない)
        /// Lengthが指定されている場合、この設定は無視されます。
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 特定の文字数(nullの場合はチェックしない)
        /// この値が指定されている場合、MinLengthとMaxLengthの設定は無視されます。
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// 正規表現パターン(nullの場合はチェックしない)
        /// </summary>
        public string RegexPattern { get; set; }

        /// <summary>
        /// 最小値(nullの場合はチェックしない)
        /// </summary>
        public int? MinValue { get; set; }

        /// <summary>
        /// 最大値(nullの場合はチェックしない)
        /// </summary>
        public long? MaxValue { get; set; }

        /// <summary>
        /// GUID形式かどうか(nullの場合はチェックしない)
        /// </summary>
        public bool IsGuid { get; set; } = false;

        /// <summary>
        /// 表示名を指定してインスタンスを作成します。
        /// </summary>
        /// <param name="displayName">パラメータの表示名</param>
        public PathParameterValidationOptions(string displayName)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }
    }
}