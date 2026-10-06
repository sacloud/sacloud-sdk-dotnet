namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 文字列の長さが最小値以上かつ最大値以下であることを検証するバリデーション属性
    /// </summary>
    public class LocalizedLengthRangeAttribute : StringLengthAttribute
    {
        public LocalizedLengthRangeAttribute(int minimumLength, int maximumLength) : base(maximumLength)
        {
            MinimumLength = minimumLength;
            ErrorMessage = "{0}は{2}文字以上{1}文字以下で入力してください。";
        }
    }
}
