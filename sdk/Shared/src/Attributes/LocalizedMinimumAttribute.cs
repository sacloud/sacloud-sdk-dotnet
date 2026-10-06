namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 最小値のみ検証する <see cref="RangeAttribute"/>
    /// </summary>
    public class LocalizedMinimumAttribute : RangeAttribute
    {
        public LocalizedMinimumAttribute(int minimum) : base(minimum, int.MaxValue)
        {
            ErrorMessage = "{0}の値は{1}以上で入力してください。";
        }
    }
}
