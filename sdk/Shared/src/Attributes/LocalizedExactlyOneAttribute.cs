using System.Linq;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 指定されたプロパティのうち、ちょうど1つだけが設定されていることを検証します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LocalizedExactlyOneAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public LocalizedExactlyOneAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames ?? throw new ArgumentNullException(nameof(propertyNames));

            if (_propertyNames.Length < 2)
            {
                throw new ArgumentException("少なくとも2つのプロパティ名を指定する必要があります。", nameof(propertyNames));
            }
        }

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
                    "LocalizedExactlyOneAttributeの設定に誤りがあります。次のプロパティが見つかりません: {0}",
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

            if (nonNullCount == 1)
            {
                return ValidationResult.Success;
            }

            var propertyDisplayNames = propertyInfos
                .Select(GetDisplayName)
                .ToArray();
            var errorMessage = string.Format(
                "{0}のいずれか1つのみを設定してください。",
                string.Join("または", propertyDisplayNames));

            return new ValidationResult(errorMessage, propertyInfos.Select(property => property.Name));
        }

        private static string GetDisplayName(Reflection.PropertyInfo propertyInfo)
        {
            var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayAttribute));
            if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.GetName()))
            {
                return displayAttribute.GetName();
            }

            return propertyInfo.Name;
        }
    }
}