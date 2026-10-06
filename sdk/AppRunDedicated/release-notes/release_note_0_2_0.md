# AppRunDedicated リリースノート

## v0.2.0

OpenAPI 仕様 v1.2.1 への追随として、クラスター一覧取得のレスポンス型変更・不要クラスの削除・ロードバランサーポート設定の追加・リソース充足確認プロパティの追加を行いました。クラスター一覧を取得して詳細フィールドを参照していた既存コードには互換性影響があります。

## 重要: 互換性影響のある変更

### クラスター一覧レスポンスのクラスタ情報型が変わりました

クラスター一覧取得時のレスポンス（`ListClusterResponse`）に含まれる `Clusters` プロパティの要素型が変更されました。

- `Clusters` プロパティの要素型
  - 変更前: `ReadClusterDetail`（`HasLetsEncryptEmail`, `Ports`, `ServicePrincipalID` 等の詳細フィールドを含む）
  - 変更後: `ReadClusterSummary`（`ClusterID`, `Created`, `Name` のみ保持）

```cs
// 変更前
// ListClusterResponse.Clusters は List<ReadClusterDetail> だった
List<ReadClusterDetail> clusters = response.Clusters;
bool? hasLetsEncrypt = clusters[0].HasLetsEncryptEmail; // HasLetsEncryptEmail にアクセス可能

// 変更後
// ListClusterResponse.Clusters は List<ReadClusterSummary> に変わった
List<ReadClusterSummary> clusters = response.Clusters;
Guid? clusterId = clusters[0].ClusterID;  // ClusterID, Created, Name のみ参照可能
// clusters[0].HasLetsEncryptEmail; // コンパイルエラー: ReadClusterSummary にはこのプロパティがない
```

影響:

- `ListClusterResponse.Clusters` を `ReadClusterDetail` 型として扱っていた既存コードはコンパイルエラーになります
- `HasLetsEncryptEmail`, `Ports`, `ServicePrincipalID` などクラスター詳細フィールドを一覧取得後に参照していた箇所は修正が必要です

移行の考え方:

- クラスター一覧からは `ClusterID`, `Created`, `Name` のみ取得してください
- クラスター詳細情報が必要な場合は、`ClusterID` を使って個別のクラスター詳細取得 API を呼び出してください

### `ReadClusterDetail.AutoScalingGroups` プロパティが削除されました

クラスター詳細モデル `ReadClusterDetail` から `AutoScalingGroups` プロパティが削除されました。

- 変更前: `ReadClusterDetail` に `AutoScalingGroups`（`List<ReadAutoScalingGroupSummary>` 型）が存在
- 変更後: `ReadClusterDetail` に `AutoScalingGroups` プロパティなし

影響:

- `ReadClusterDetail.AutoScalingGroups` を参照していた既存コードはコンパイルエラーになります
- `ReadAutoScalingGroupSummary` クラス自体も削除されているため、このクラスを直接利用していた場合もコンパイルエラーになります

移行の考え方:

- オートスケーリンググループ情報の取得方法が API 側で変更されています。代替 API や取得方法についてはさくらのクラウドの AppRun Dedicated API ドキュメントを確認してください

## 追加・改善

### アプリケーション詳細でリソース充足状態を確認できるようになりました

アプリケーション詳細取得のレスポンス `ReadApplicationDetail` に `EnoughResources` プロパティが追加されました。デプロイ先クラスタの CPU・メモリリソースが十分かどうかを確認できます。

- `ReadApplicationDetail.EnoughResources` プロパティ（`EnoughResources` 型）
  - `EnoughResources.Cpu`（`bool?`）: `false` の場合は CPU 不足またはスケールアウト中、`null` の場合は不明
  - `EnoughResources.Memory`（`bool?`）: `false` の場合はメモリ不足またはスケールアウト中、`null` の場合は不明

```cs
// アプリケーション詳細取得後にリソース状態を確認する例
GetApplicationResponse response = await client.Applications[appId].GetAsync();
EnoughResources resources = response.Application.EnoughResources;
if (resources?.Cpu == false || resources?.Memory == false)
{
    // リソース不足またはスケールアウト中
}
```

### クラスター更新時にロードバランサーのポートを設定できるようになりました

クラスター更新リクエスト `UpdateCluster` に `Ports` プロパティが追加されました。ロードバランサーの外部公開ポートをプロトコルとあわせて最大 5 件設定できます。

- `UpdateCluster.Ports`（`List<UpdateLoadBalancerPort>` 型、最大 5 件）
  - `UpdateLoadBalancerPort.Port`（`int?`）: 外部公開ポート番号（1〜65535、5950〜5959 は予約済みのため指定不可）
  - `UpdateLoadBalancerPort.Protocol`（`UpdateLoadBalancerPort_protocol`）: プロトコル（`Http`, `Https`, `Tcp`）

```cs
// ロードバランサーポートを設定してクラスターを更新する例
UpdateCluster updateRequest = new UpdateCluster
{
    ServicePrincipalID = "sp-xxxx",
    Ports = new System.Collections.Generic.List<UpdateLoadBalancerPort>
    {
        new UpdateLoadBalancerPort { Port = 443, Protocol = UpdateLoadBalancerPort_protocol.Https },
        new UpdateLoadBalancerPort { Port = 80,  Protocol = UpdateLoadBalancerPort_protocol.Http }
    }
};
```

### 環境変数値の最大文字数が拡張されました

環境変数作成時の `Value` フィールドに設定できる文字数の上限が変更されました。

- `CreateEnvironmentVariable.Value` の最大文字数
  - 変更前: 255 文字
  - 変更後: 4096 文字

影響:

- 既存の 255 文字以内の環境変数値を持つ利用コードは影響を受けません
- 256 文字以上の環境変数値を設定したい場合は、今回の更新後に対応可能になります

## 仕様追随

- OpenAPI 仕様を v1.0.0 から v1.2.1 へ更新
- SDK バージョンを 0.1.2 から 0.2.0 へ更新

## 利用者向け確認ポイント

1. `ListClusterResponse.Clusters` を使用しているコードで `ReadClusterDetail` 型として参照しているプロパティがないか確認してください（コンパイルエラーになります）
2. `ReadClusterDetail.AutoScalingGroups` を参照しているコードがないか確認してください（コンパイルエラーになります）
3. `ReadAutoScalingGroupSummary` クラスを直接利用しているコードがないか確認してください（コンパイルエラーになります）
4. クラスター一覧取得後に詳細フィールドが必要な処理は、クラスター詳細取得 API へ移行できるか検討してください
5. `CreateEnvironmentVariable.Value` に 256 文字以上を設定する要件がある場合は、今回の更新で対応可能です
