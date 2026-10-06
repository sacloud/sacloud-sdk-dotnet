
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.AppRunDedicated.Applications.Item.Versions;

namespace Sacloud.Sdk.AppRunDedicated.Samples;

class ApplicationVersionSample
{
    public class ApplicationVersionSampleInput()
    {
        public string ContainerImage { get; set; } = string.Empty;
        public string ContainerRegistryUsername { get; set; } = string.Empty;
        public string ContainerRegistryPassword { get; set; } = string.Empty;
    }

    private static readonly string suffix = "-appver-s";

    public static async Task RunSampleAsync(AppRunDedicatedClient client, string servicePrincipalID, ApplicationVersionSampleInput input)
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

        // POST 新しいアプリケーションの作成
        var createApplicationResponse = await client.Applications.PostAsync(new CreateApplicationRequest
        {
            Name = "appver" + suffix,
            ClusterID = clusterId,
        });
        var applicationId = createApplicationResponse.Application.ApplicationID
            ?? throw new InvalidOperationException("ApplicationIDがNullです");
        Console.WriteLine($"アプリケーションが作成されました: {applicationId}");

        Console.WriteLine("--------[セットアップ完了]--------");

        // ------------------------------
        // [アプリケーションバージョン]
        // ------------------------------

        // POST 新しいアプリケーションバージョンの作成
        var createApplicationVersionResponse = await client.Applications[applicationId].Versions.PostAsync(new CreateApplicationVersionRequest
        {
            ScalingMode = ScalingMode.Manual,
            FixedScale = 1,
            RegistryPasswordAction = RegistryPasswordAction.New,
            RegistryUsername = input.ContainerRegistryUsername,
            RegistryPassword = input.ContainerRegistryPassword,
            Cpu = 100,
            Memory = 128,
            Image = input.ContainerImage,
            Cmd = [],
            ExposedPorts = [
                new ExposedPort()
                {
                    TargetPort = 8080,
                    LoadBalancerPort = null,
                    UseLetsEncrypt = false,
                    HealthCheck = null
                }
            ],
            Env = []
        });
        var applicationVersion = createApplicationVersionResponse.ApplicationVersion.Version
            ?? throw new InvalidOperationException("ApplicationVersionがNullです");
        Console.WriteLine($"アプリケーションバージョンが作成されました: Version={applicationVersion}");

        // GET アプリケーションバージョンの詳細取得
        var getApplicationVersionResponse = await client.Applications[applicationId].Versions[applicationVersion].GetAsync();
        var applicationVersionDetail = getApplicationVersionResponse.ApplicationVersion
            ?? throw new InvalidOperationException("ApplicationVersionがNullです");
        Console.WriteLine($"アプリケーションバージョン詳細を取得しました: Version={applicationVersionDetail.Version}");

        // GET アプリケーションバージョン一覧の取得
        var listApplicationVersionsResponse = await client.Applications[applicationId].Versions.GetAsync(x =>
            x.QueryParameters = new VersionsRequestBuilder.VersionsRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var applicationVersions = listApplicationVersionsResponse.Versions
            ?? throw new InvalidOperationException("ApplicationVersionsがNullです");
        Console.WriteLine($"アプリケーションバージョン一覧を取得しました: {applicationVersions.Count}件");
        foreach(var x in applicationVersions)
        {
            Console.WriteLine($"- Version: {x.Version}");
        }

        // ------------------------------
        // [クリーンアップ]
        // ------------------------------

        /*
        // アプリケーションバージョンの削除
        await client.Applications[applicationId].Versions[applicationVersion].DeleteAsync();

        // DELETE アプリケーションの削除
        await client.Applications[applicationId].DeleteAsync();

        // DELETE クラスタの削除
        await client.Clusters[clusterId].DeleteAsync();
        */
    }
}
