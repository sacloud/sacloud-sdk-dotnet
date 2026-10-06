namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 特定の文字数であることのチェックを行うバリデーション属性
    /// </summary>
    public class LocalizedLengthAttribute : StringLengthAttribute
    {
        public LocalizedLengthAttribute(int length) : base(length)
        {
            MinimumLength = length;
            ErrorMessage = "{0}は{1}文字で入力してください。";
        }
    }
}
