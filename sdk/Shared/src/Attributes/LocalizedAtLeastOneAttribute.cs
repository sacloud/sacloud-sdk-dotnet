using System.Linq;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 指定されたプロパティのうち、少なくとも1つが設定されていることを検証します。
    /// すべてのプロパティが <see langword="null"/> の場合は検証エラーとなります。
    /// </summary>
    /// <example>
    /// <code>
    /// [LocalizedAtLeastOne(nameof(PropertyA), nameof(PropertyB))]
    /// public class MyRequest
    /// {
    ///     [Display(Name = "プロパティA")]
    ///     public string PropertyA { get; set; }
    ///
    ///     [Display(Name = "プロパティB")]
    ///     public string PropertyB { get; set; }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LocalizedAtLeastOneAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        /// <summary>
        /// <see cref="LocalizedAtLeastOneAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="propertyNames">検証対象のプロパティ名。2つ以上指定してください。</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyNames"/> が <see langword="null"/> の場合。</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyNames"/> の要素数が2未満の場合。</exception>
        public LocalizedAtLeastOneAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames ?? throw new ArgumentNullException(nameof(propertyNames));

            if (_propertyNames.Length < 2)
            {
                throw new ArgumentException("少なくとも2つのプロパティ名を指定する必要があります。", nameof(propertyNames));
            }
        }

        /// <summary>
        /// 指定されたプロパティのうち、少なくとも1つが非 <see langword="null"/> であるかを検証します。
        /// </summary>
        /// <param name="value">検証対象のオブジェクト。</param>
        /// <param name="validationContext">検証コンテキスト。</param>
        /// <returns>
        /// 少なくとも1つのプロパティが非 <see langword="null"/> の場合は <see cref="ValidationResult.Success"/>、
        /// すべてのプロパティが <see langword="null"/> の場合はエラーを含む <see cref="ValidationResult"/>。
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var type = value.GetType();
            var propertyLookups = _propertyNames
                .Select(propertyName => new
                {
                    PropertyName = propertyName,
                    PropertyInfo = type.GetProperty(propertyName)
                })
                .ToList();

            var missingPropertyNames = propertyLookups
                .Where(lookup => lookup.PropertyInfo == null)
                .Select(lookup => lookup.PropertyName)
                .ToArray();

            if (missingPropertyNames.Length > 0)
            {
                var configurationErrorMessage = string.Format(
                    "LocalizedAtLeastOneAttributeの設定に誤りがあります。次のプロパティが見つかりません: {0}",
                    string.Join(", ", missingPropertyNames));

                return new ValidationResult(configurationErrorMessage, missingPropertyNames);
            }

            var propertyInfos = propertyLookups
                .Where(lookup => lookup.PropertyInfo != null)
                .Select(lookup => lookup.PropertyInfo)
                .Cast<Reflection.PropertyInfo>()
                .ToList();

            var nonNullCount = propertyInfos
                .Count(property => property.GetValue(value) != null);

            // 少なくとも1つが非 null であれば検証成功
            if (nonNullCount >= 1)
            {
                return ValidationResult.Success;
            }

            var propertyDisplayNames = propertyInfos
                .Select(propertyInfo =>
                {
                    var displayAttribute = (DisplayAttribute)GetCustomAttribute(propertyInfo, typeof(DisplayAttribute));
                    if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.GetName()))
                    {
                        return displayAttribute.GetName();
                    }

                    return propertyInfo.Name;
                })
                .ToArray();
            var errorMessage = string.Format(
                "{0}のいずれかを設定してください。",
                string.Join("または", propertyDisplayNames));

            return new ValidationResult(errorMessage, propertyInfos.Select(property => property.Name));
        }
    }
}
