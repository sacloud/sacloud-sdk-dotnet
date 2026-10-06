namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 日本語にローカライズした <see cref="RequiredAttribute"/>
    /// </summary>
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        public LocalizedRequiredAttribute()
        {
            ErrorMessage = "{0}が入力されていません。";
        }
    }
}
