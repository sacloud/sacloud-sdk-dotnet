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
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. 手動証明書を登録・更新・削除");
        Console.WriteLine("2. 証明書一覧を取得");
        Console.WriteLine("3. ACME 証明書を自動発行");
        Console.WriteLine("4. ACME 証明書の有効期限を更新");
        Console.WriteLine("5. ACME 証明書を再発行");
        Console.Write("選択 (1 ~ 5 から入力): ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await RunManualCertificateAsync();
                break;
            case "2":
                await ListCertificatesAsync();
                break;
            case "3":
                await IssueAcmeCertificateAsync();
                break;
            case "4":
                await RenewAcmeCertificateAsync();
                break;
            case "5":
                await ReissueAcmeCertificateAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }

        Console.WriteLine("\n証明書のサンプルの実行が完了しました");
    }

    private async Task RunManualCertificateAsync()
    {
        Console.WriteLine("\n---------[手動証明書サンプル]---------");
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
            await ListCertificatesAsync();

            // PUT 指定した証明書を更新する
            Console.WriteLine("\nPUT /certificates/{certificateId} | Certificate更新API");
            await client.Certificates[certificateId].PutAsync(new Certificate
            {
                Name = "updated-sample-certificate",
                Rsa = CreateTemporaryCertificateDetails()
            });
            Console.WriteLine("指定した証明書の更新が完了しました。(レスポンスボディ無し)");
        }
        finally
        {
            // DELETE 指定した証明書を削除する
            if (certificateId != Guid.Empty)
            {
                Console.WriteLine("\nDELETE /certificates/{certificateId} | Certificate削除API");
                await client.Certificates[certificateId].DeleteAsync();
                Console.WriteLine("指定した証明書の削除が完了しました。(レスポンスボディ無し)");
                certificateId = Guid.Empty;
            }
        }
    }

    private async Task ListCertificatesAsync()
    {
        // GET 証明書一覧を取得する
        Console.WriteLine("\nGET /certificates | Certificate一覧照会API");
        var listCertificatesResponse = await client.Certificates.GetAsync();
        var certificates = listCertificatesResponse.Apigw.Certificates;

        Console.WriteLine("証明書一覧の取得が完了しました。");
        Console.WriteLine($"  - AutoIssueLimit: {listCertificatesResponse.Apigw.AutoIssueLimit.FormatNullable()}");
        Console.WriteLine("  - Certificates:");
        if (certificates is null || certificates.Count <= 0)
        {
            Console.WriteLine("    - (null)");
            return;
        }

        foreach (var certificate in certificates)
        {
            WriteCertificate(certificate, 4);
        }
    }

    private async Task IssueAcmeCertificateAsync()
    {
        Console.WriteLine("\n---------[ACME 証明書自動発行サンプル]---------");

        // POST ACME 証明書を自動発行する
        Console.WriteLine("\nPOST /certificates/acme | Certificate自動発行API");
        await client.Certificates.Acme.PostAsync(new CertificatesAcme
        {
            Name = "<YOUR_ACME_CERTIFICATE_NAME>",
            Sans = ["<YOUR_ACME_CERTIFICATE_SAN>"]
        });

        Console.WriteLine("ACME 証明書の自動発行が受け付けられました。(202, レスポンスボディ無し)");
        Console.WriteLine("証明書一覧で Status を確認してから、有効期限更新または再発行を個別に実行してください。");
    }

    private async Task RenewAcmeCertificateAsync()
    {
        Console.WriteLine("\n---------[ACME 証明書有効期限更新サンプル]---------");
        var acmeCertificateId = await SelectAcmeCertificateIdAsync();
        if (acmeCertificateId is null)
        {
            return;
        }

        // POST ACME 証明書の有効期限を更新する
        Console.WriteLine("\nPOST /certificates/{certificateId}/acme/renew | Certificate自動発行有効期限更新API");
        await client.Certificates[acmeCertificateId.Value].Acme.Renew.PostAsync();

        Console.WriteLine("ACME 証明書の有効期限更新が受け付けられました。(202, レスポンスボディ無し)");
    }

    private async Task ReissueAcmeCertificateAsync()
    {
        Console.WriteLine("\n---------[ACME 証明書再発行サンプル]---------");
        var acmeCertificateId = await SelectAcmeCertificateIdAsync();
        if (acmeCertificateId is null)
        {
            return;
        }

        // POST ACME 証明書を再発行する
        Console.WriteLine("\nPOST /certificates/{certificateId}/acme/reissue | Certificate自動発行再発行API");
        await client.Certificates[acmeCertificateId.Value].Acme.Reissue.PostAsync(new CertificatesAcme
        {
            Name = "<YOUR_ACME_CERTIFICATE_NAME>",
            Sans = ["<YOUR_ACME_CERTIFICATE_SAN>"]
        });

        Console.WriteLine("ACME 証明書の再発行が受け付けられました。(202, レスポンスボディ無し)");
    }

    private async Task<Guid?> SelectAcmeCertificateIdAsync()
    {
        Console.WriteLine("\nGET /certificates | Certificate一覧照会API");
        var listCertificatesResponse = await client.Certificates.GetAsync();
        await Task.Delay(100);

        var certificates = listCertificatesResponse.Apigw.Certificates?
            .Where(x => x.Type == GetCertificates_certificates_type.Auto && x.Id.HasValue)
            .ToList();

        if (certificates is null || certificates.Count <= 0)
        {
            Console.WriteLine("自動発行された証明書が見つかりませんでした。");
            return null;
        }

        Console.WriteLine("\n---------[自動発行証明書の選択]---------");
        for (var i = 0; i < certificates.Count; i++)
        {
            var certificate = certificates[i];
            Console.WriteLine($"  {i + 1}. {certificate.Name.FormatNullable()} ({certificate.Status.FormatNullable()})");
        }
        Console.WriteLine("----------------------------------------");
        Console.Write("操作する証明書を選択してください（ cancel でキャンセル）: ");

        var input = Console.ReadLine();
        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        if (!int.TryParse(input, out var choice) || choice < 1 || choice > certificates.Count)
        {
            Console.WriteLine("\n無効な番号が選択されました。");
            return null;
        }

        return certificates[choice - 1].Id;
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

    private void WriteCertificate(GetCertificates_certificates certificate, int indent = 2)
    {
        WriteCertificate((Certificate)certificate, indent);
        Console.WriteIndentedLine($"- Type: {certificate.Type.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Status: {certificate.Status.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- StatusMessage: {certificate.StatusMessage.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine("- Sans:", indent + 2);
        if (certificate.Sans is null || certificate.Sans.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
            return;
        }

        foreach (var san in certificate.Sans)
        {
            Console.WriteIndentedLine($"- {san.FormatNullable()}", indent + 4);
        }
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
