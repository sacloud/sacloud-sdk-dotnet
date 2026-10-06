using System.Security.Cryptography;
using System.Text;

namespace Sacloud.Sdk.IAM.Samples.Helpers;

static class JsonWebTokenHelper
{
    private static string ToJwtString(this string x) => x.TrimEnd('=').Replace('+', '-').Replace('/', '_');

    /// <summary>
    /// RS256 アルゴリズムを使用して JWT を生成します。
    /// </summary>
    /// <param name="servicePrincipalResourceId">サービスプリンシパルのリソース ID</param>
    /// <param name="kid">署名に使用する鍵の KID</param>
    /// <param name="privateKeyPem">PEM 形式の RSA 秘密鍵</param>
    /// <returns>生成された JWT 文字列</returns>
    /// <exception cref="ArgumentNullException"><paramref name="servicePrincipalResourceId"/>、<paramref name="kid"/>、または <paramref name="privateKeyPem"/> が <c>null</c> の場合にスローされます。</exception>
    public static string GenerateJsonWebToken(string servicePrincipalResourceId, string kid, string privateKeyPem)
    {
        ArgumentNullException.ThrowIfNull(privateKeyPem);
        ArgumentNullException.ThrowIfNull(kid);
        ArgumentNullException.ThrowIfNull(servicePrincipalResourceId);

        // ヘッダー
        string jsonHeader = @$"{{
            ""alg"": ""RS256"",
            ""kid"": ""{kid}"",
            ""typ"": ""JWT""
        }}";
        string header = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonHeader)).ToJwtString();

        // ペイロード
        long issuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long expiredAt = issuedAt + 300; // 5分後に有効期限切れ
        string jsonPayload = @$"{{
            ""aud"": ""https://secure.sakura.ad.jp/cloud/api/iam/1.0/service-principals/oauth2/token"",
            ""exp"": {expiredAt},
            ""iat"": {issuedAt},
            ""iss"": ""{servicePrincipalResourceId}"",
            ""sub"": ""{servicePrincipalResourceId}""
        }}";
        string payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPayload)).ToJwtString();

        // 署名
        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem);
        byte[] signatureBytes = rsa.SignData(
            Encoding.UTF8.GetBytes($"{header}.{payload}"),
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
        string signature = Convert.ToBase64String(signatureBytes).ToJwtString();

        var jwt = $"{header}.{payload}.{signature}";

        return jwt;
    }
}
