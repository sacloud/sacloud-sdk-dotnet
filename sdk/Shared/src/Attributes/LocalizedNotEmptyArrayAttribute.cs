using System.Collections;

namespace System.ComponentModel.DataAnnotations
{

    /// <summary>
    /// 空配列でないことのチェックを行うバリデーション属性
    /// </summary>
    public class LocalizedNotEmptyArrayAttribute : RequiredAttribute
    {
        public LocalizedNotEmptyArrayAttribute()
        {
            ErrorMessage = "{0}が入力されていません。";
        }

        public override bool IsValid(object value)
        {
            // nullの場合はバリデーションをスキップ
            if (value == null)
            {
                return true;
            }

            // IEnumerableとしてチェック
            if (value is IEnumerable enumerable)
            {
                // 少なくとも1つの要素があるかチェック
                foreach (var _ in enumerable)
                {
                    return true;
                }
                // 要素がない場合は無効
                return false;
            }

            // IEnumerableでない場合はバリデーションをスキップ
            return true;
        }
    }
}
