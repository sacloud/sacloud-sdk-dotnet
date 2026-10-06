# AppRunDedicated リリースノート

## v0.4.0

この更新では、AppRunDedicated SDK が OpenAPI 仕様 1.4.0 に追随し、クラスタのロードバランサ設定を SDK から取得・部分更新できるようにしました。既存の利用コードへの大きな破壊的変更はなく、追加された機能を利用する場合に必要な確認ポイントをまとめています。

## 追加・改善

### クラスタのロードバランサ設定取得・更新 API を追加

クラスタのロードバランサ設定に関する API が SDK に追加されました。これにより、クライアント側から設定の参照と部分更新を行いやすくなります。

```cs
var clusterId = Guid.NewGuid();
var response = await client.Clusters[clusterId].Load_balancer.GetAsync();

var patchRequest = new PatchClusterLoadBalancerRequest
{
    LetsEncryptEmail = "admin@example.com",
    Ports = new List<ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly>
    {
        new ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly
        {
            Port = 443,
            Protocol = ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Https,
        },
    },
};

await client.Clusters[clusterId].Load_balancer.PatchAsync(patchRequest);
```

- 変更前: クラスタのロードバランサ設定を SDK から取得・更新する手段がありませんでした。
- 変更後: `GetAsync()` と `PatchAsync()` で設定値を参照し、部分更新を実行できます。

影響:

- 既存のコードがそのまま動作しなくなるような破壊的変更ではありません。
- ただし、`LetsEncryptEmail` と `Ports` を指定する場合は、ポート番号とプロトコルの値が API の要件に合っているかを確認してください。

移行の考え方:

- 既存の呼び出しコードを変更する必要はありませんが、ロードバランサ設定を利用している場合は `client.Clusters[clusterId].Load_balancer` から取得・更新の処理に切り替えるとよいです。
- `ports` の `Port` は `1-65535` の範囲で指定し、`5950-5959` は予約済みのため使用しないようにしてください。

## 仕様追随

- AppRunDedicated OpenAPI 仕様が 1.3.1 から 1.4.0 に更新されました。
- SDK バージョン: `0.3.0` → `0.4.0`

## 利用者向け確認ポイント

1. **既存コードへの影響**: 今回の更新は新機能の追加（非破壊）であるため、既存の機能のみを利用している場合はコードの修正は不要です。
2. **クラスタのロードバランサ設定の参照・更新**:
   - 設定を参照・更新したい場合は、`client.Clusters[clusterId].Load_balancer` の `GetAsync()` および `PatchAsync()` を利用してください。
   - `PatchAsync()` で設定を更新する際は、以下の 2 点にご注意ください。
     - **予約ポートの指定不可**: `Ports` のポート番号は `1〜65535` の範囲で指定し、予約されている `5950〜5959` は指定しないでください。
     - **Let's Encrypt の前提条件**: `LetsEncryptEmail` を設定して証明書発行を有効にする場合、HTTP-01 チャレンジのため 80 番ポートを HTTP プロトコルで外部公開する必要があります。