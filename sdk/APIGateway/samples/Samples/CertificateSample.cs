using Sacloud.Sdk.APIGateway.Samples.Extensions;
using Sacloud.Sdk.APIGateway.Samples.Helpers;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class CertificateSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));
    private Guid certificateId = Guid.Empty;

    public string Description { get; } = "証明書カテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\n証明書のサンプルを実行します");
        Console.WriteLine("\n---------[証明書サンプル]---------");
        try
        {
            // POST 証明書を登録する
            Console.WriteLine("\nPOST /certificates | Certificate登録API");
            var createCertificateResponse = await client.Certificates.PostAsync(new Certificate
            {
                Name = "sample-certificate",
                Rsa = CreateTemporaryCertificateDetails()
            });
            certificateId = createCertificateResponse.Apigw.Certificate.Id
                ?? throw new NullReferenceException("証明書登録APIのレスポンスに証明書IDが含まれていません。");
            Console.WriteLine("証明書の登録が完了しました。");
            WriteCertificate(createCertificateResponse.Apigw.Certificate, 2);

            // GET 証明書一覧を取得する
            Console.WriteLine("\nGET /certificates | Certificate一覧照会API");
            var listCertificatesResponse = await client.Certificates.GetAsync();
            Console.WriteLine("証明書一覧の取得が完了しました。");
            Console.WriteLine("  - Certificates:");
            if (listCertificatesResponse.Apigw.Certificates is null || listCertificatesResponse.Apigw.Certificates.Count <= 0)
            {
                Console.WriteLine("    - (null)");
            }
            else
            {
                foreach (var certificate in listCertificatesResponse.Apigw.Certificates)
                {
                    WriteCertificate(certificate, 4);
                }
            }

            // PUT 指定した証明書を更新する
            Console.WriteLine("\nPUT /certificates/{certificateId} | Certificate更新API");
            await client.Certificates[certificateId].PutAsync(new Certificate
            {
                Name = "updated-sample-certificate",
                Rsa = CreateTemporaryCertificateDetails()
            });
            Console.WriteLine("指定した証明書の更新が完了しました。(レスポンスボディ無し)");

            Console.WriteLine("\n証明書のサンプルの実行が完了しました");
        }
        finally
        {
            // DELETE 指定した証明書を削除する
            if (certificateId != Guid.Empty)
            {
                Console.WriteLine("\nDELETE /certificates/{certificateId} | Certificate削除API");
                await client.Certificates[certificateId].DeleteAsync();
                Console.WriteLine("指定した証明書の削除が完了しました。(レスポンスボディ無し)");
            }

            Console.WriteLine("\n証明書のサンプルの実行が完了しました");
        }
    }

    private void WriteCertificate(Certificate? certificate, int indent = 2)
    {
        if (certificate is null)
        {
            Console.WriteIndentedLine("- (null)", indent);
            return;
        }

        Console.WriteIndentedLine($"- Id: {certificate.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- Name: {certificate.Name.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CreatedAt: {certificate.CreatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- UpdatedAt: {certificate.UpdatedAt.FormatNullable()}", indent + 2);
        WriteCertificateDetails(certificate.Rsa, "Rsa", indent + 2);
        WriteCertificateDetails(certificate.Ecdsa, "Ecdsa", indent + 2);
    }

    private void WriteCertificateDetails(CertificateDetails? certificateDetails, string type, int indent = 2)
    {
        if (certificateDetails is null)
        {
            Console.WriteIndentedLine($"- {type}: (null)", indent);
            return;
        }

        Console.WriteIndentedLine($"- {type}:", indent);
        Console.WriteIndentedLine($"- ExpiredAt: {certificateDetails.ExpiredAt.FormatNullable()}", indent + 2);
    }

    private CertificateDetails CreateTemporaryCertificateDetails()
    {
        var (certificatePem, privateKeyPem) = CertificateHelper.CreateTemporaryCertificatePair();
        return new CertificateDetails
        {
            Cert = certificatePem,
            Key = privateKeyPem
        };
    }
};
