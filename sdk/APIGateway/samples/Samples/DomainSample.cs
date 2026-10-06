using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;
using Sacloud.Sdk.APIGateway.Samples.Helpers;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class DomainSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private Guid certificateId = Guid.Empty;
    private Guid domainId = Guid.Empty;

    public string Description { get; } = "ドメインカテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\nドメインのサンプルを実行します");
        try
        {
            // サンプルで利用する依存リソースの作成
            Console.WriteLine("\n---------[事前準備]---------");

            // サンプル用証明書の登録
            Console.WriteLine("\nサンプル用証明書を登録中...");
            var (certificatePem, privateKeyPem) = CertificateHelper.CreateTemporaryCertificatePair();
            var createCertificateResponse = await client.Certificates.PostAsync(new Certificate
            {
                Name = "sample-domain-certificate",
                Rsa = new CertificateDetails
                {
                    Cert = certificatePem,
                    Key = privateKeyPem
                }
            });
            certificateId = createCertificateResponse.Apigw.Certificate.Id
                ?? throw new NullReferenceException("証明書登録APIのレスポンスに証明書IDが含まれていません。");

            Console.WriteLine("サンプル用証明書の登録が完了しました。");

            // サンプルの実行
            Console.WriteLine("\n---------[ドメインサンプル]---------");

            var domainName = "example.com";

            // POST ドメインを登録する
            Console.WriteLine("\nPOST /domains | Domain登録API");
            var createDomainResponse = await client.Domains.PostAsync(new Domain
            {
                DomainName = domainName,
                CertificateId = certificateId
            });
            var domain = createDomainResponse.Apigw.Domain;
            domainId = domain.Id
                ?? throw new NullReferenceException("ドメイン登録APIのレスポンスにドメインIDが含まれていません。");

            Console.WriteLine("ドメインの登録が完了しました。");
            WriteDomain(domain, 2);

            // GET ドメイン一覧を取得する
            Console.WriteLine("\nGET /domains | Domain一覧照会API");
            var listDomainsResponse = await client.Domains.GetAsync();

            Console.WriteLine("ドメイン一覧の取得が完了しました。");
            Console.WriteLine("  - Domains:");
            if (listDomainsResponse.Apigw.Domains is null || listDomainsResponse.Apigw.Domains.Count <= 0)
            {
                Console.WriteLine("    - (null)");
            }
            else
            {
                foreach (var listedDomain in listDomainsResponse.Apigw.Domains)
                {
                    WriteDomain(listedDomain, 4);
                }
            }

            // PUT 指定したドメインの証明書を更新する
            Console.WriteLine("\nPUT /domains/{domainId} | Domain更新API");
            await client.Domains[domainId].PutAsync(new DomainPUT
            {
                CertificateId = certificateId
            });

            Console.WriteLine("指定したドメインの証明書情報の更新が完了しました。(レスポンスボディ無し)");

        }
        finally
        {
            // DELETE 指定したドメインを削除する
            if (domainId != Guid.Empty)
            {
                Console.WriteLine("\nDELETE /domains/{domainId} | Domain削除API");
                await client.Domains[domainId].DeleteAsync();
                Console.WriteLine("指定したドメインの削除が完了しました。(レスポンスボディ無し)");
            }

            Console.WriteLine("\n---------[クリーンアップ]--------");

            // サンプル用証明書の削除
            if (certificateId != Guid.Empty)
            {
                Console.WriteLine("\nサンプル用証明書を削除中...");
                await client.Certificates[certificateId].DeleteAsync();
                Console.WriteLine("サンプル用証明書の削除が完了しました。");
            }

            Console.WriteLine("\nドメインのサンプルの実行が完了しました");
        }
    }

    private void WriteDomain(Domain domain, int indent = 2)
    {
        Console.WriteIndentedLine($"- Id: {domain.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- DomainName: {domain.DomainName.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CertificateId: {domain.CertificateId.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CertificateName: {domain.CertificateName.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CreatedAt: {domain.CreatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- UpdatedAt: {domain.UpdatedAt.FormatNullable()}", indent + 2);
    }
};
