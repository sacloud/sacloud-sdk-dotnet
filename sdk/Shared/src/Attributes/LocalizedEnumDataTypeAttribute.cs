namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Enumに含まれる値のチェックを行うバリデーション属性
    /// </summary>
    public class LocalizedEnumDataTypeAttribute : DataTypeAttribute
    {
        public LocalizedEnumDataTypeAttribute(Type enumType)
            : base("Enumerable")
        {
            EnumType = enumType;
            ErrorMessage = "{0}はEnumに含まれる値を入力してください。";
        }

        public Type EnumType { get; }

        public override bool IsValid(object value)
        {
            if (EnumType == null)
            {
                throw new InvalidOperationException("EnumTypeはnullにできません。");
            }

            if (!EnumType.IsEnum)
            {
                throw new InvalidOperationException($"EnumTypeは列挙型である必要があります。: {EnumType.FullName}");
            }

            if (value == null)
            {
                return true;
            }

            var stringValue = value as string;
            if (stringValue?.Length == 0)
            {
                return true;
            }

            Type valueType = value.GetType();
            if (valueType.IsEnum && EnumType != valueType)
            {
                return false;
            }


            if (!valueType.IsValueType && valueType != typeof(string))
            {
                return false;
            }

            if (valueType == typeof(bool) ||
                valueType == typeof(float) ||
                valueType == typeof(double) ||
                valueType == typeof(decimal))
            {
                return false;
            }

            return Enum.IsDefined(EnumType, value);
        }
    }
}
