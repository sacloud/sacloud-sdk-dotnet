namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 最大値のみ検証する <see cref="RangeAttribute"/>
    /// </summary>
    public class LocalizedMaximumAttribute : RangeAttribute
    {
        public LocalizedMaximumAttribute(int maximum) : base(int.MinValue, maximum)
        {
            ErrorMessage = "{0}の値は{2}以下で入力してください。";
        }
    }
}
