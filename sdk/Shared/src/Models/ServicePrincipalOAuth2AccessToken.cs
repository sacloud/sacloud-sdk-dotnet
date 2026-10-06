using System;
using System.Text.Json.Serialization;

namespace Sacloud.Sdk.Shared.Models
{
    /// <summary>
    /// サービスプリンシパルの OAuth2 アクセストークンレスポンスを表すモデルクラスです。
    /// </summary>
    public class ServicePrincipalOAuth2AccessToken
    {
        /// <summary>アクセストークン</summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>アクセストークンの有効期限（秒単位）</summary>
        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }

        /// <summary>アクセストークンの有効期限 (ISO 8601 形式)</summary>
        [JsonPropertyName("token_expired_at")]
        public DateTimeOffset? TokenExpiredAt { get; set; }

        /// <summary>トークンの種類</summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}
