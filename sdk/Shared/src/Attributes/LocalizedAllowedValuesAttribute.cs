using System.Linq;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 値が許可された候補のいずれかと一致することを検証するバリデーション属性です。
    /// </summary>
    /// <example>
    /// <code>
    /// [LocalizedAllowedValues("Enabled", "Disabled")]
    /// public string Status { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class LocalizedAllowedValuesAttribute : ValidationAttribute
    {
        private readonly object[] _allowedValues;

        /// <summary>
        /// <see cref="LocalizedAllowedValuesAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="allowedValues">許可する値の一覧。</param>
        public LocalizedAllowedValuesAttribute(params object[] allowedValues)
        {
            _allowedValues = allowedValues
                ?? throw new ArgumentNullException(nameof(allowedValues), "許可値の配列はnullにできません。");
            var allowedValuesList = string.Join(", ", _allowedValues);
            ErrorMessage = $"{{0}}には許可された値（{allowedValuesList}）を入力してください。";
        }

        /// <summary>
        /// 値が許可された候補に含まれるかどうかを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。</param>
        /// <returns>
        /// 値が <see langword="null"/> の場合、または許可された候補のいずれかと一致する場合は <see langword="true"/>。
        /// それ以外の場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return _allowedValues.Contains(value);
        }
    }
}
