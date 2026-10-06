namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 日本語にローカライズした <see cref="RangeAttribute"/>
    /// </summary>
    public class LocalizedRangeAttribute : RangeAttribute
    {
        public LocalizedRangeAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
            ErrorMessage = "{0}の値は{1}以上{2}以下で入力してください。";
        }
    }
}
