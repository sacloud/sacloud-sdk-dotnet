namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 日本語にローカライズした <see cref="MinLengthAttribute"/>
    /// </summary>
    public class LocalizedMinLengthAttribute : MinLengthAttribute
    {
        public LocalizedMinLengthAttribute(int minLength) : base(minLength)
        {
            ErrorMessage = "{0}は{1}文字以上で入力してください。";
        }
    }
}
