using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.AppRunDedicated.Applications;

namespace Sacloud.Sdk.AppRunDedicated.Samples;

static class ApplicationSample
{
    private const string suffix = "-app-s";

    public static async Task RunSampleAsync(AppRunDedicatedClient client, string servicePrincipalID)
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
        // [アプリケーション]
        // ------------------------------

        // POST 新しいアプリケーションの作成
        var createApplicationResponse = await client.Applications.PostAsync(new CreateApplicationRequest
        {
            Name = "app" + suffix,
            ClusterID = clusterId,
        });
        var applicationId = createApplicationResponse.Application.ApplicationID
            ?? throw new InvalidOperationException("ApplicationIDがNullです");
        Console.WriteLine($"アプリケーションが作成されました: {applicationId}");

        // GET アカウント内のアプリケーション一覧の取得
        var listApplicationResponse = await client.Applications.GetAsync(x =>
            x.QueryParameters = new ApplicationsRequestBuilder.ApplicationsRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var applications = listApplicationResponse.Applications
            ?? throw new InvalidOperationException("ApplicationsがNullです");
        foreach(var x in applications)
        {
            Console.WriteLine($"- Name: {x.Name}, ID: {x.ApplicationID}");
        }

        // GET アプリケーションの詳細情報を取得する
        var getApplicationResponse = await client.Applications[applicationId].GetAsync();
        var application = getApplicationResponse.Application
            ?? throw new InvalidOperationException("ApplicationがNullです");
        Console.WriteLine($"アプリケーション詳細を取得しました: Name={application.Name}, ID={application.ApplicationID}, Cpu={application.EnoughResources.Cpu}, Memory={application.EnoughResources.Memory}");

        // PUT アプリケーション情報の更新
        await client.Applications[applicationId].PutAsync(new UpdateApplicationRequest
        {
            ActiveVersion = null
        });

        // GET アプリケーションのノードごとのコンテナ配置情報を取得
        var listContainersResponse = await client.Applications[applicationId].Containers.GetAsync();
        var containers = listContainersResponse.Nodes
            ?? throw new InvalidOperationException("NodesがNullです");
        foreach(var x in containers)
        {
            Console.WriteLine($"- NodeID: {x.NodeID}");
        }

        // ------------------------------
        // [クリーンアップ]
        // ------------------------------

        /*
        // DELETE アプリケーションの削除
        await client.Applications[applicationId].DeleteAsync();

        // DELETE クラスタの削除
        await client.Clusters[clusterId].DeleteAsync();
        */
    }
}
