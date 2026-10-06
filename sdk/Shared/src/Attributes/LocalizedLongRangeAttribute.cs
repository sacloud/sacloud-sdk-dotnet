using System.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 日本語にローカライズした long 型のプロパティに対する <see cref="RangeAttribute"/>
    /// </summary>
    public class LocalizedLongRangeAttribute : RangeAttribute
    {
        public LocalizedLongRangeAttribute(long minimum, long maximum) : base(typeof(long), minimum.ToString(CultureInfo.InvariantCulture), maximum.ToString(CultureInfo.InvariantCulture))
        {
            ErrorMessage = "{0}の値は{1}以上{2}以下で入力してください。";
        }
    }
}
