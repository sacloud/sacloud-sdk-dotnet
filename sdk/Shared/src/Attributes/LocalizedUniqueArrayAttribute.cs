using System.Collections;
using System.Collections.Generic;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 配列の要素がそれぞれ一意であることを検証する属性です。
    /// </summary>
    public class LocalizedUniqueArrayAttribute : ValidationAttribute
    {
        /// <summary>
        /// 属性を初期化します。
        /// </summary>
        public LocalizedUniqueArrayAttribute()
        {
            ErrorMessage = "{0}の要素は重複できません。";
        }

        /// <summary>
        /// コレクション内の要素に重複がないことを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。</param>
        /// <returns>
        /// 値が null または <see cref="ICollection"/> でない場合、もしくは要素が一意の場合は true。
        /// 重複する要素がある場合は false。
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null || !(value is ICollection collection))
            {
                return true;
            }

            var items = new HashSet<object>();
            foreach (var item in collection)
            {
                if (!items.Add(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}