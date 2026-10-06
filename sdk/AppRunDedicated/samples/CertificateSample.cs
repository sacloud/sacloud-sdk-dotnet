using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Certificates;

namespace Sacloud.Sdk.AppRunDedicated.Samples;

static class CertificateSample
{
    private const string suffix = "-cert-s";

    public class CertificateSampleInput
    {
        public string CertificatePem { get; set; } = string.Empty;
        public string PrivatekeyPem { get; set; } = string.Empty;
    }

    public static async Task RunSampleAsync(AppRunDedicatedClient client, string servicePrincipalID, CertificateSampleInput input)
    {
        // ------------------------------
        // [セットアップ]
        // ------------------------------

        Console.WriteLine("--------[セットアップ]--------");

        // POST クラスタの作成
        var createClusterResponse = await client.Clusters.PostAsync(new CreateClusterRequest
        {
            Name = "cluster" + suffix,
            Ports = [],
            ServicePrincipalID = servicePrincipalID
        });
        var clusterId = createClusterResponse.Cluster.ClusterID
            ?? throw new InvalidOperationException("ClusterIDがNullです");
        Console.WriteLine($"クラスタが作成されました: {clusterId}");

        Console.WriteLine("--------[セットアップ完了]--------");

        // ------------------------------
        // [証明書]
        // ------------------------------

        // POST 証明書の作成
        var certificateResponse = await client.Clusters[clusterId].Certificates.PostAsync(new CreateCertificateRequest
        {
            Name = "cert" + suffix,
            CertificatePem = input.CertificatePem,
            PrivatekeyPem = input.PrivatekeyPem,
            
        });
        var certificateId = certificateResponse.Certificate.CertificateID
            ?? throw new InvalidOperationException("CertificateIDがNullです");
        Console.WriteLine($"証明書が作成されました: {certificateId}");

        // GET 証明書の詳細を取得
        var getCertificateResponse = await client.Clusters[clusterId].Certificates[certificateId].GetAsync();
        var certificate = getCertificateResponse.Certificate
            ?? throw new InvalidOperationException("CertificateがNullです");
        Console.WriteLine($"証明書詳細を取得しました: Name={certificate.Name}, ID={certificate.CertificateID}");

        // GET 証明書の一覧を取得
        var listCertificateResponse = await client.Clusters[clusterId].Certificates.GetAsync(x =>
            x.QueryParameters = new CertificatesRequestBuilder.CertificatesRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var certificates = listCertificateResponse.Certificates
            ?? throw new InvalidOperationException("CertificatesがNullです");
        Console.WriteLine($"証明書一覧を取得しました: {certificates.Count}件");
        foreach (var x in certificates)
        {
            Console.WriteLine($"- Name: {x.Name}, ID: {x.CertificateID}");
        }

        // PUT 証明書の更新
        await client.Clusters[clusterId].Certificates[certificateId].PutAsync(new UpdateCertificateRequest
        {
            Name = "cert-updated" + suffix,
            CertificatePem = input.CertificatePem,
            PrivatekeyPem = input.PrivatekeyPem
        });

        // ------------------------------
        // [クリーンアップ]
        // ------------------------------

        /*
        // DELETE 証明書の削除
        await client.Clusters[clusterId].Certificates[certificateId].DeleteAsync();

        // DELETE クラスタの削除
        await client.Clusters[clusterId].DeleteAsync();
        */
    }
}
