using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Sacloud.Sdk.APIGateway.Samples.Helpers;

/// <summary>
/// 証明書関連のヘルパーメソッドを提供します。
/// </summary>
public static class CertificateHelper
{
    /// <summary>
    /// テスト用の自己署名証明書と秘密鍵のペアを作成します。有効期限は1分です。
    /// </summary>
    /// <returns>証明書と秘密鍵の PEM 形式の文字列ペア。</returns>
    public static (string CertificatePem, string PrivateKeyPem) CreateTemporaryCertificatePair()
    {
        using var rsa = RSA.Create(2048);

        var certificateRequest = new CertificateRequest(
            "CN=TemporaryCertificate",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        var certificate = certificateRequest.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(1));

        string certificatePem = certificate.ExportCertificatePem();
        string privateKeyPem = rsa.ExportPkcs8PrivateKeyPem();

        return (certificatePem, privateKeyPem);
    }
}
