using System.Linq;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 指定されたプロパティのうち、1つだけが設定されていることを検証します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LocalizedMutualExclusivityAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public LocalizedMutualExclusivityAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames ?? throw new ArgumentNullException(nameof(propertyNames));

            if (_propertyNames.Length < 2)
            {
                throw new ArgumentException("少なくとも2つのプロパティ名を指定する必要があります。", nameof(propertyNames));
            }

            ErrorMessage = "{0}のいずれか1つのみを設定してください。";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var type = value.GetType();

            var nonNullCount = _propertyNames
                .Select(propertyName => type.GetProperty(propertyName))
                .Where(property => property != null)
                .Select(property => property.GetValue(value))
                .Count(propertyValue => propertyValue != null);

            // どちらも設定されていない、または1つだけ設定されている場合は有効
            return nonNullCount <= 1;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessage, string.Join("または", _propertyNames));
        }
    }
}
