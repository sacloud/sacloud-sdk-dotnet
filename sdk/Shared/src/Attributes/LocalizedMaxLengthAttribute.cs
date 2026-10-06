namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 日本語にローカライズした <see cref="MaxLengthAttribute"/>
    /// </summary>
    public class LocalizedMaxLengthAttribute : MaxLengthAttribute
    {
        public LocalizedMaxLengthAttribute(int maxLength) : base(maxLength)
        {
            ErrorMessage = "{0}は{1}文字以下で入力してください。";
        }
    }
}
