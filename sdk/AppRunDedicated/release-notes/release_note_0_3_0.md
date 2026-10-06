# AppRunDedicated リリースノート

## v0.3.0

AppRunDedicated SDK を OpenAPI 仕様バージョン 1.3.1 に追随させました。今回の更新では、リソース制限値を取得する新しい API エンドポイントが追加されました。また、多数のモデル型（データクラス）の名称が変更されており、既存のコードをお使いの方は型名の更新が必要です。

## 重要: 互換性影響のある変更

### モデル型名の変更

多数のモデル型（データクラス）の名称が変更されました。これらの型を直接参照しているコードはコンパイルエラーとなるため、型名の更新が必要です。

主な変更は以下のパターンに沿っています。

**レスポンス型の統一**

| 変更前 | 変更後 |
|--------|--------|
| `GetApplicationResponse` | `ReadApplicationResponse` |
| `GetApplicationVersionResponse` | `ReadApplicationVersionResponse` |
| `GetAutoScalingGroupResponse` | `ReadAutoScalingGroupResponse` |
| `GetCertificateResponse` | `ReadCertificateResponse` |
| `GetClusterResponse` | `ReadClusterResponse` |
| `GetLoadBalancerResponse` | `ReadLoadBalancerResponse` |
| `GetLoadBalancerNodeResponse` | `ReadLoadBalancerNodeResponse` |
| `GetWorkerNodeResponse` | `ReadWorkerNodeResponse` |
| `GetApplicationContainersResponse` | `ListApplicationContainersResponse` |

**一覧取得レスポンス型の統一**

| 変更前 | 変更後 |
|--------|--------|
| `ListApplicationResponse` | `ListApplicationsResponse` |
| `ListApplicationVersionResponse` | `ListApplicationVersionsResponse` |
| `ListAutoScalingGroupResponse` | `ListAutoScalingGroupsResponse` |
| `ListCertificateResponse` | `ListCertificatesResponse` |
| `ListClusterResponse` | `ListClustersResponse` |
| `ListLbServiceClassResponse` | `ListLoadBalancerServiceClassesResponse` |
| `ListWorkerServiceClassResponse` | `ListWorkerServiceClassesResponse` |

**リクエスト型の統一**

| 変更前 | 変更後 |
|--------|--------|
| `CreateApplication` | `CreateApplicationRequest` |
| `CreateApplicationVersion` | `CreateApplicationVersionRequest` |
| `CreateAutoScalingGroup` | `CreateAutoScalingGroupRequest` |
| `CreateCertificate` | `CreateCertificateRequest` |
| `CreateCluster` | `CreateClusterRequest` |
| `CreateLoadBalancer` | `CreateLoadBalancerRequest` |
| `UpdateApplication` | `UpdateApplicationRequest` |
| `UpdateCertificate` | `UpdateCertificateRequest` |
| `UpdateCluster` | `UpdateClusterRequest` |

**詳細情報型の統一**

| 変更前 | 変更後 |
|--------|--------|
| `ReadApplicationDetail` | `ReadApplication` |
| `ReadApplicationVersionDetail` | `ReadApplicationVersion` |
| `ReadAutoScalingGroupDetail` | `ReadAutoScalingGroup` |
| `ReadClusterDetail` | `ReadCluster` |
| `ReadLoadBalancerDetail` | `ReadLoadBalancer` |
| `ReadWorkerNodeDetail` | `WorkerNode` |

**作成結果識別子型の統一**

| 変更前 | 変更後 |
|--------|--------|
| `CreatedApplication` | `ApplicationIdentifier` |
| `CreatedAutoScalingGroup` | `AutoScalingGroupIdentifier` |
| `CreatedCertificate` | `CertificateIdentifier` |
| `CreatedCluster` | `ClusterIdentifier` |
| `CreatedLoadBalancer` | `LoadBalancerIdentifier` |

**その他の型変更**

| 変更前 | 変更後 |
|--------|--------|
| `ReadApplicationVersionSummary` | `ApplicationVersionIdentifier` |
| `ApplicationVersionDeploymentStatus` | `ApplicationVersionSummary` |
| `ApplicationContainerSummary` | `ApplicationCurrentContainer` |
| `ApplicationContainersStats` | `ApplicationCurrentContainers` |
| `ApplicationPeekDesiredContainersResponse` | `ApplicationDesiredContainers` |
| `UpdateLoadBalancerPort` | `ClusterLoadBalancerPort` |
| `CreateLoadBalancerPort_protocol` | `ClusterLoadBalancerPort_protocol` |
| `ReadWorkerNodeSummary` | `WorkerNodeSummary` |
| `ReadWorkerNodeNetworkInterface` | `LoadBalancerNodeInterface` |
| `ReadLoadBalancerNodeInterface` | `WorkerNodeNetworkInterface` |
| `ReadLoadBalancerNodeInterfaceAddress` | `LoadBalancerNodeInterfaceAddress` |
| `ReadWorkerNodeInterfaceAddress` | `WorkerNodeInterfaceAddress` |
| `ReadLoadBalancerNode` | `LoadBalancerNode` |
| `ReadLbServiceClass` | `LoadBalancerServiceClass` |
| `ReadWorkerServiceClass` | `WorkerServiceClass` |
| `NodeContainerPlacementInfo` | `WorkerNodeContainerPlacement` |
| `NodeContainerPlacementInfo_containersStats` | `WorkerNodeContainerPlacement_containersStats` |
| `NodeContainerPlacementInfo_desired` | `WorkerNodeContainerPlacement_desired` |
| `RunningContainer` | `WorkerNodeRunningContainer` |

```cs
// 変更前（コンパイルエラーとなる例）
ReadAutoScalingGroupDetail asg = await client.Clusters[clusterId].Asg[asgId].GetAsync();
ListApplicationResponse apps = await client.Applications.GetAsync();
CreateApplication createReq = new CreateApplication { Name = "my-app" };

// 変更後
ReadAutoScalingGroup asg = await client.Clusters[clusterId].Asg[asgId].GetAsync();
ListApplicationsResponse apps = await client.Applications.GetAsync();
CreateApplicationRequest createReq = new CreateApplicationRequest { Name = "my-app" };
```

影響:

- 上記の旧型名を使用しているコードはすべてコンパイルエラーとなります。
- メソッドの戻り値型・変数宣言・型キャスト・メソッド引数など、型名を明示しているすべての箇所が対象です。

移行の考え方:

- 上記の対応表に従い、旧型名を新型名に置き換えてください。

### 削除された型

以下の型は削除されました。代替型への移行が必要です。

| 削除された型 | 移行先の目安 |
|-------------|------------|
| `CreateLoadBalancerPort` | `ClusterLoadBalancerPort`（ポート番号とプロトコルを持つ共通型に統合） |
| `ReadLoadBalancerPort` | `ClusterLoadBalancerPort` |
| `ReadLoadBalancerPort_protocol` | `ClusterLoadBalancerPort_protocol` |
| `UpdateLoadBalancerPort_protocol` | `ClusterLoadBalancerPort_protocol` |
| `ReadLoadBalancerNodeSummary` | `LoadBalancerNode` または `ReadLoadBalancerNodeResponse` を参照 |

影響:

- 削除された型を参照しているコードはコンパイルエラーとなります。

移行の考え方:

- ロードバランサーのポート設定には `ClusterLoadBalancerPort` と `ClusterLoadBalancerPort_protocol` を使用してください。
- `ReadLoadBalancerNodeSummary` を使用していた箇所は、取得するデータの用途に応じて `LoadBalancerNode`（詳細情報）または `ReadLoadBalancerNodeResponse`（単一取得レスポンス）への移行を検討してください。

## 追加・改善

### リソース制限値取得 API の追加（`GET /limits`）

プロジェクトに設定されているリソース制限値を取得できるようになりました。

```cs
ReadLimitsResponse limitsResponse = await client.Limits.GetAsync();
```

## 仕様追随

- AppRunDedicated OpenAPI 仕様バージョン 1.3.1 に追随しました（変更前: 1.2.1）。
- SDK バージョン: `0.2.2` → `0.3.0`

## 利用者向け確認ポイント

1. **型名の更新**: 「重要: 互換性影響のある変更」セクションの対応表を参照し、旧型名を新型名に置き換えてください。
2. **削除された型**: `CreateLoadBalancerPort`、`ReadLoadBalancerPort`、`ReadLoadBalancerPort_protocol`、`UpdateLoadBalancerPort_protocol`、`ReadLoadBalancerNodeSummary` を使用していた場合は、代替型への移行が必要です。
3. **新機能の活用**: `client.Limits.GetAsync()` でリソース制限値を取得できます。必要に応じて利用を検討してください。
