
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.AppRunDedicated.Clusters;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Worker_nodes;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Load_balancers;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Load_balancers.Item.Load_balancer_nodes;


namespace Sacloud.Sdk.AppRunDedicated.Samples;

static class ClusterSample
{
    private const string suffix = "-clst-s";

    public static async Task RunSampleAsync(AppRunDedicatedClient client, string servicePrincipalID, string letsEncryptEmail)
    {
        // ------------------------------
        // [クラスタ]
        // ------------------------------

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

        // GET クラスタ詳細の取得
        var getClusterResponse = await client.Clusters[clusterId].GetAsync();
        var cluster = getClusterResponse.Cluster
            ?? throw new InvalidOperationException("ClusterがNullです");
        Console.WriteLine($"クラスタ詳細を取得しました: Name={cluster.Name}, ID={cluster.ClusterID}");

        // GET クラスタのロードバランサ設定を取得
        var getClusterLoadBalancerResponse = await client.Clusters[clusterId].Load_balancer.GetAsync();
        var clusterLoadBalancer = getClusterLoadBalancerResponse.LoadBalancer
            ?? throw new InvalidOperationException("LoadBalancerがNullです");
        Console.WriteLine($"クラスタのロードバランサ設定を取得しました: Let’s Encryptメール設定済み={clusterLoadBalancer.HasLetsEncryptEmail}, ポート数={clusterLoadBalancer.Ports?.Count ?? 0}");

        // PATCH クラスタのロードバランサ設定を更新
        // 注意: Ports は置換専用（ReplaceOnly）のため、指定した内容で既存のポート設定がすべて置き換わります。
        // 既存のポートを維持したまま追加・変更したい場合は、事前に GET で取得した設定と合わせて Ports 全体を指定してください。
        await client.Clusters[clusterId].Load_balancer.PatchAsync(new PatchClusterLoadBalancerRequest
        {
            LetsEncryptEmail = letsEncryptEmail,
            Ports = [
                new ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly
                {
                    Port = 80,
                    Protocol = ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Http
                }
            ]
        });
        Console.WriteLine("クラスタのロードバランサ設定を更新しました");

        // 以下は Let’s Encrypt メールアドレスの設定を解除するサンプルです。
        // 現状の SDK では LetsEncryptEmail が null の場合、未指定（デフォルト）の場合も null が送信されます。
        // 既存のメール設定を維持したい場合は、事前に GET で取得した値を LetsEncryptEmail に設定して送信してください。
        // このサンプルを有効化すると、直後の GET で取得する設定が上記 PATCH の結果と異なってしまい動作確認の流れが分かりにくくなるため、
        // 既定ではコメントアウトしています。動作確認する場合はコメントを解除してください。
        /*
        // PATCH Let’s Encryptメールアドレスの設定を解除
        await client.Clusters[clusterId].Load_balancer.PatchAsync(new PatchClusterLoadBalancerRequest
        {
            LetsEncryptEmail = null
        });
        */

        // GET 更新後のクラスタのロードバランサ設定を取得
        var getUpdatedClusterLoadBalancerResponse = await client.Clusters[clusterId].Load_balancer.GetAsync();
        var updatedClusterLoadBalancer = getUpdatedClusterLoadBalancerResponse.LoadBalancer
            ?? throw new InvalidOperationException("LoadBalancerがNullです");
        Console.WriteLine($"更新後のクラスタのロードバランサ設定を取得しました: Let’s Encryptメール設定済み={updatedClusterLoadBalancer.HasLetsEncryptEmail}, ポート数={updatedClusterLoadBalancer.Ports?.Count ?? 0}");

        // GET クラスタ一覧の取得
        var listClusterResponse = await client.Clusters.GetAsync(x =>
            x.QueryParameters = new ClustersRequestBuilder.ClustersRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var clusters = listClusterResponse.Clusters
            ?? throw new InvalidOperationException("ClustersがNullです");
        Console.WriteLine($"クラスタ一覧を取得しました: {clusters.Count}件");
        foreach (var x in clusters)
        {
            Console.WriteLine($"- Name: {x.Name}, ID: {x.ClusterID}");
        }
        
        // PUT クラスタの更新
        await client.Clusters[clusterId].PutAsync(new UpdateClusterRequest
        {
            ServicePrincipalID = servicePrincipalID,
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 8080,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            }
        });

        // ------------------------------
        // [オートスケーリンググループ]
        // ------------------------------

        // POST オートスケーリンググループの作成
        var createAutoScalingGroupResponse = await client.Clusters[clusterId].Asg.PostAsync(new CreateAutoScalingGroupRequest
        {
            Name = "asg" + suffix,
            Zone = "tk1b",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 1,
            Interfaces = [
                new AutoScalingGroupNodeInterface()
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = true
                }
            ]
        });

        var autoScalingGroupId = createAutoScalingGroupResponse.AutoScalingGroup.AutoScalingGroupID
            ?? throw new InvalidOperationException("AutoScalingGroupIDがNullです");
        Console.WriteLine($"オートスケーリンググループが作成されました: {autoScalingGroupId}");

        // GET オートスケーリンググループ詳細の取得
        var getAutoScalingGroupResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].GetAsync();
        var autoScalingGroup = getAutoScalingGroupResponse.AutoScalingGroup
            ?? throw new InvalidOperationException("AutoScalingGroupがNullです");
        Console.WriteLine($"オートスケーリンググループ詳細を取得しました: Name={autoScalingGroup.Name}, ID={autoScalingGroup.AutoScalingGroupID}");

        // GET オートスケーリンググループ一覧の取得
        var listAutoScalingGroupResponse = await client.Clusters[clusterId].Asg.GetAsync(x =>
            x.QueryParameters = new AsgRequestBuilder.AsgRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var autoScalingGroups = listAutoScalingGroupResponse.AutoScalingGroups
            ?? throw new InvalidOperationException("AutoScalingGroupsがNullです");
        Console.WriteLine($"オートスケーリンググループ一覧を取得しました: {autoScalingGroups.Count}件");
        foreach (var x in autoScalingGroups)
        {
            Console.WriteLine($"- Name: {x.Name}, ID: {x.AutoScalingGroupID}");
        }

        // ------------------------------
        // [ロードバランサ]
        // ------------------------------

        // POST ロードバランサの作成
        var createLoadBalancerResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].Load_balancers.PostAsync(new CreateLoadBalancerRequest
        {
            Name = "lb" + suffix,
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = [
                new LoadBalancerInterface()
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                }
            ]
        });
        var loadBalancerId = createLoadBalancerResponse.LoadBalancer.LoadBalancerID
            ?? throw new InvalidOperationException("LoadBalancerIDがNullです");
        Console.WriteLine($"ロードバランサが作成されました: {loadBalancerId}");

        // GET ロードバランサ詳細の取得
        var getLoadBalancerResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].Load_balancers[loadBalancerId].GetAsync();
        var loadBalancer = getLoadBalancerResponse.LoadBalancer
            ?? throw new InvalidOperationException("LoadBalancerがNullです");
        Console.WriteLine($"ロードバランサ詳細を取得しました: Name={loadBalancer.Name}, ID={loadBalancer.LoadBalancerID}");

        // GET ロードバランサ一覧の取得
        var listLoadBalancerResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].Load_balancers.GetAsync(x =>
            x.QueryParameters = new Load_balancersRequestBuilder.Load_balancersRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var loadBalancers = listLoadBalancerResponse.LoadBalancers
            ?? throw new InvalidOperationException("LoadBalancersがNullです");
        Console.WriteLine($"ロードバランサ一覧を取得しました: {loadBalancers.Count}件");
        foreach (var x in loadBalancers)
        {
            Console.WriteLine($"- Name: {x.Name}, ID: {x.LoadBalancerID}");
        }

        // ------------------------------
        // [ロードバランサノード]
        // ------------------------------

        // GET ロードバランサノードの一覧を取得
        var listLoadBalancerNodesResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].Load_balancers[loadBalancerId].Load_balancer_nodes.GetAsync(x =>
            x.QueryParameters = new Load_balancer_nodesRequestBuilder.Load_balancer_nodesRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var loadBalancerNodes = listLoadBalancerNodesResponse.LoadBalancerNodes
            ?? throw new InvalidOperationException("LoadBalancerNodesがNullです");
        Console.WriteLine($"ロードバランサノード一覧を取得しました: {loadBalancerNodes.Count}件");
        foreach (var x in loadBalancerNodes)
        {
            Console.WriteLine($"- ID: {x.LoadBalancerNodeID}, Status: {x.Status}");
        }

        // GET ロードバランサ―ノードの詳細を取得
        var loadBalancerNodeId = loadBalancerNodes.FirstOrDefault()?.LoadBalancerNodeID
            ?? throw new InvalidOperationException("LoadBalancerNodeIDがNullです");
        var getLoadBalancerNodeResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].Load_balancers[loadBalancerId].Load_balancer_nodes[loadBalancerNodeId].GetAsync();
        var loadBalancerNode = getLoadBalancerNodeResponse.LoadBalancerNode
            ?? throw new InvalidOperationException("LoadBalancerNodeがNullです");
        Console.WriteLine($"ロードバランサノード詳細を取得しました: ID={loadBalancerNode.LoadBalancerNodeID}, Status={loadBalancerNode.Status}");

        // ------------------------------
        // [ワーカノード]
        // ------------------------------

        // GET オートスケーリンググループに所属するワーカノード一覧を取得する
        var listNodesResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].Worker_nodes.GetAsync(x =>
            x.QueryParameters = new Worker_nodesRequestBuilder.Worker_nodesRequestBuilderGetQueryParameters
            {
                MaxItems = 20
            }
        );
        var nodes = listNodesResponse.WorkerNodes
            ?? throw new InvalidOperationException("WorkerNodesがNullです");
        Console.WriteLine($"ワーカノード一覧を取得しました: {nodes.Count}件");
        foreach (var x in nodes)
        {
            Console.WriteLine($"- ID: {x.WorkerNodeID}, Status: {x.Status}");
        }

        // GET オートスケーリンググループに所属するワーカノードの詳細を取得する
        var workerNodeId = nodes.FirstOrDefault()?.WorkerNodeID
            ?? throw new InvalidOperationException("WorkerNodeIDがNullです");
        var getNodeResponse = await client.Clusters[clusterId].Asg[autoScalingGroupId].Worker_nodes[workerNodeId].GetAsync();
        var node = getNodeResponse.WorkerNode
            ?? throw new InvalidOperationException("WorkerNodeがNullです");
        Console.WriteLine($"ワーカノード詳細を取得しました: ID={node.WorkerNodeID}, Status={node.Status}");

        // PUT ワーカノードのdraining状態を変更する
        await client.Clusters[clusterId].Asg[autoScalingGroupId].Worker_nodes[workerNodeId].Draining.PutAsync(new UpdateWorkerNodeDrainingRequest
        {
            Draining = false
        });

        // ------------------------------
        // [クリーンアップ]
        // ------------------------------

        /*
        // DELETE ロードバランサ―の削除
        await client.Clusters[clusterId].Asg[autoScalingGroupId].Load_balancers[loadBalancerId].DeleteAsync();

        // DELETE オートスケーリンググループの削除
        await client.Clusters[clusterId].Asg[autoScalingGroupId].DeleteAsync();

        // DELETE クラスタの削除
        await client.Clusters[clusterId].DeleteAsync();
        */
    }
}
