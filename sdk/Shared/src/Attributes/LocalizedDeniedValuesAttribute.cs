using System.Linq;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 許可されていない値のチェックを行うバリデーション属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class LocalizedDeniedValuesAttribute : ValidationAttribute
    {
        private readonly object[] _deniedValues;

        public LocalizedDeniedValuesAttribute(params object[] deniedValues)
        {
            _deniedValues = deniedValues;
            ErrorMessage = "{0}には許可された値を入力してください。";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            
            return !_deniedValues.Contains(value);
        }
    }
}
