using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// コレクションの各要素が null ではないことを検証するバリデーション属性です。
    /// </summary>
    public class LocalizedNotNullArrayElementsAttribute : ValidationAttribute
    {
        /// <summary>
        /// <see cref="LocalizedNotNullArrayElementsAttribute"/> の新しいインスタンスを初期化します。
        /// </summary>
        public LocalizedNotNullArrayElementsAttribute()
        {
            ErrorMessage = "{0}の各要素はnullにできません。";
        }

        /// <summary>
        /// コレクションの各要素が null でないことを検証します。
        /// </summary>
        /// <param name="value">検証対象の値。</param>
        /// <returns>
        /// 値が null またはコレクションでない場合、もしくは null 要素がない場合は <see langword="true"/>。
        /// null 要素がある場合は <see langword="false"/>。
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null || !(value is ICollection collection))
            {
                return true;
            }

            foreach (var item in collection)
            {
                if (item == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}