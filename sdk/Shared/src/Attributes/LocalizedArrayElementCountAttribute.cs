using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 配列の要素数のチェックを行うバリデーション属性
    /// </summary>
    public class LocalizedArrayElementCountAttribute : MaxLengthAttribute
    {
        public LocalizedArrayElementCountAttribute(int maxCount)
            : base(maxCount)
        {
            ErrorMessage = "{0}の要素数は{1}以下にしてください。";
        }

        public override bool IsValid(object value)
        {
            // 値がnullまたは配列でない場合はバリデーションをスキップ
            if (value == null || !(value is ICollection array))
            {
                return true;
            }

            // 要素数が最大値を超えている場合はバリデーションエラー
            if (array.Count > Length)
            {
                return false;
            }

            return true;
        }
    }
}
