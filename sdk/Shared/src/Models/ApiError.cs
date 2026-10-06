using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using System;
using System.Collections.Generic;

namespace Sacloud.Sdk.Shared.Models
{
    /// <summary>
    /// 各APIのエラー時に共通で使用するエラーレスポンスボディのモデルクラス
    /// </summary>
    public partial class ApiError : ApiException, IAdditionalDataHolder, IParsable
    {
        /// <summary>
        /// エラーレスポンスボディの各フィールドを管理する内部用のディクショナリ
        /// </summary>
        private IDictionary<string, object> _responseBody;

        /// <summary>
        /// エラーレスポンスボディの各フィールドを管理するディクショナリ
        /// </summary>
        public IDictionary<string, object> ResponseBody => _responseBody;

        /// <summary>
        /// KiotaのAdditionalData管理の仕組みをResponseBodyに適用
        /// </summary>
        IDictionary<string, object> IAdditionalDataHolder.AdditionalData
        {
            get => _responseBody;
            set => _responseBody = value ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// <see cref="ApiError"/> の新しいインスタンスを生成し、既定値を設定します。
        /// </summary>
        public ApiError()
        {
            _responseBody = new Dictionary<string, object>();
        }

        /// <summary>
        /// 識別子の値に基づいて適切なクラスの新しいインスタンスを作成します。
        /// </summary>
        /// <returns>新しいインスタンス</returns>
        /// <param name="parseNode">解析ノード</param>
        public static ApiError CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            if (ReferenceEquals(parseNode, null)) throw new ArgumentNullException(nameof(parseNode));
            return new ApiError();
        }

        /// <summary>
        /// デシリアライズ情報を取得します。
        /// </summary>
        /// <returns>フィールド名とデシリアライズアクションのディクショナリ</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>();
        }

        /// <summary>
        /// 現在のオブジェクトの情報をシリアライズします。
        /// </summary>
        /// <param name="writer">シリアライゼーションライター</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            if (ReferenceEquals(writer, null)) throw new ArgumentNullException(nameof(writer));
            writer.WriteAdditionalData(_responseBody);
        }
    }
}
