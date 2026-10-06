using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sacloud.Sdk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sacloud.Sdk.Shared.Helpers
{
    /// <summary>
    /// サービスプリンシパル認証でのアクセストークン取得を行うヘルパークラスです。
    /// IAM SDK への依存を避けるため、IAM API エンドポイントを HttpClient で直接呼び出します。
    /// </summary>
    public static class AccessTokenHelper
    {
        // アクセストークン発行の IAM API エンドポイント
        // IAM SDK への依存を避けるため、Kiota 経由ではなく直接 URL を定義する
        private static readonly string TokenEndpoint =
            "https://secure.sakura.ad.jp/cloud/api/iam/1.0/service-principals/oauth2/token";

        // httpClient が渡されない場合に使用するフォールバック用の静的インスタンス
        private static readonly HttpClient DefaultHttpClient = new HttpClient();

        /// <summary>
        /// JWT を使用してアクセストークンを非同期に取得します。
        /// </summary>
        /// <param name="jwt">認証に使用する JWT 文字列</param>
        /// <param name="httpClient">HTTP リクエストに使用する <see cref="HttpClient"/>。null の場合は既定の静的インスタンスを使用</param>
        /// <param name="loggerFactory">ロガー生成に使用するファクトリ。null の場合は <see cref="NullLoggerFactory"/> を使用</param>
        /// <param name="cancellationToken">操作をキャンセルするために使用するトークン</param>
        /// <returns>取得したアクセストークン情報を含む <see cref="ServicePrincipalOAuth2AccessToken"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="jwt"/> が null の場合にスローされます。</exception>
        /// <exception cref="ArgumentException"><paramref name="jwt"/> が空文字列または空白文字列の場合にスローされます。</exception>
        /// <exception cref="InvalidOperationException">アクセストークン取得リクエストが失敗した場合、またはアクセストークンがレスポンスに含まれていない場合にスローされます。</exception>
        public static async Task<ServicePrincipalOAuth2AccessToken> GetAccessTokenAsync(
            string jwt,
            HttpClient httpClient = null,
            ILoggerFactory loggerFactory = null,
            CancellationToken cancellationToken = default)
        {
            if (jwt == null)
            {
                throw new ArgumentNullException(nameof(jwt));
            }

            if (string.IsNullOrWhiteSpace(jwt))
            {
                throw new ArgumentException("JWT を指定してください。", nameof(jwt));
            }

            var resolvedLoggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            var logger = resolvedLoggerFactory.CreateLogger(typeof(AccessTokenHelper));

            logger.LogInformation("アクセストークンの取得を開始します。 {StartTime}", DateTime.UtcNow.ToString("o"));
            try
            {
                var resolvedHttpClient = httpClient ?? DefaultHttpClient;

                using (var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"),
                    new KeyValuePair<string, string>("assertion", jwt)
                }))
                using (var response = await resolvedHttpClient.PostAsync(TokenEndpoint, formContent, cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogError(
                            "アクセストークン取得に失敗しました。StatusCode: {StatusCode}, EndTime: {EndTime}",
                            (int)response.StatusCode,
                            DateTime.UtcNow.ToString("o"));
                        throw new InvalidOperationException(
                            $"アクセストークン取得に失敗しました。StatusCode: {(int)response.StatusCode}");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var accessTokenResponse = JsonSerializer.Deserialize<ServicePrincipalOAuth2AccessToken>(json);

                    if (string.IsNullOrWhiteSpace(accessTokenResponse?.AccessToken))
                    {
                        logger.LogError(
                            "アクセストークンがレスポンスに含まれていません。 {EndTime}",
                            DateTime.UtcNow.ToString("o"));
                        throw new InvalidOperationException("アクセストークンがレスポンスに含まれていません。");
                    }

                    logger.LogInformation("アクセストークンの取得が完了しました。 {EndTime}", DateTime.UtcNow.ToString("o"));
                    return accessTokenResponse;
                }
            }
            // try ブロック内でスローされた InvalidOperationException のログが二重出力されないように再スローのみ行う
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "アクセストークンの取得中にエラーが発生しました。 {EndTime}", DateTime.UtcNow.ToString("o"));
                throw;
            }
        }
    }
}
