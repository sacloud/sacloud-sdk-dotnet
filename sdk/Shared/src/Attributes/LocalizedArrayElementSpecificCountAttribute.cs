using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 配列の要素数のチェックを行うバリデーション属性
    /// </summary>
    public class LocalizedArrayElementSpecificCountAttribute : MaxLengthAttribute
    {
        public LocalizedArrayElementSpecificCountAttribute(int specificCount)
            : base(specificCount)
        {
            ErrorMessage = "{0}の要素数は{1}にしてください。";
        }

        public override bool IsValid(object value)
        {
            // 値がnullまたは配列でない場合はバリデーションをスキップ
            if (value == null || !(value is ICollection array))
            {
                return true;
            }

            // 要素数が特定の数でない場合はバリデーションエラー
            if (array.Count != Length)
            {
                return false;
            }

            return true;
        }
    }
}
