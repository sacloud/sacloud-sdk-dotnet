# AppRunShared リリースノート

## v0.4.0

OpenAPI 仕様 1.5.0 への追従に伴う更新です。  
API 側でエンドポイントの命名規則が統一されたことに伴い、SDK が提供するリクエスト／レスポンスの型名が変更されました。  
プロパティの構成やバリデーション、レスポンス内容自体に変更はありませんが、型名を直接コード中で参照している場合は修正が必要です。  

## 重要: 互換性影響のある変更

### リクエストボディ・レスポンスボディの型名変更

各エンドポイントに対応する型名が、操作内容（作成／取得／更新／一覧取得）を表す名前に統一されました。  
呼び出しに使用するメソッド名（`GetAsync`、`PostAsync`、`PutAsync`、`PatchAsync` など）自体は変更されていないため、戻り値を明示的な型で受け取っていない場合は影響ありません。  
戻り値やリクエストボディの型を明示的に宣言している場合、キャストなどで直接参照している場合などは、以下の一覧に従って型名を置き換えてください。  

- アプリケーション一覧取得（`client.Applications.GetAsync()`）
  - レスポンスボディ型
    - 変更なし（`Handler.ListApplications` のまま）
- アプリケーション作成（`client.Applications.PostAsync(...)`）
  - リクエストボディ型
    - 変更前: `PostApplicationBody`
    - 変更後: `CreateApplicationBody`
  - レスポンスボディ型
    - 変更前: `PostApplication`
    - 変更後: `CreateApplication`
- アプリケーション取得（`client.Applications["ID"].GetAsync()`）
  - レスポンスボディ型
    - 変更前: `GetApplication`
    - 変更後: `ReadApplication`
- アプリケーション更新（`client.Applications["ID"].PatchAsync(...)`）
  - リクエストボディ型
    - 変更なし（`PatchApplicationBody` のまま）
  - レスポンスボディ型
    - 変更なし（`Handler.PatchApplication` のまま）
- アプリケーションステータス取得（`client.Applications["ID"].Status.GetAsync()`）
  - レスポンスボディ型
    - 変更前: `GetApplicationStatus`
    - 変更後: `ReadApplicationStatus`
- パケットフィルタ取得（`client.Applications["ID"].Packet_filter.GetAsync()`）
  - レスポンスボディ型
    - 変更前: `GetPacketFilter`
    - 変更後: `ReadPacketFilter`
- パケットフィルタ変更（`client.Applications["ID"].Packet_filter.PatchAsync(...)`）
  - リクエストボディ型（`Sacloud.Sdk.AppRunShared.Models` 名前空間）
    - 変更前: `PatchPacketFilter`
    - 変更後: `PatchPacketFilterBody`
  - レスポンスボディ型
    - 変更なし（`Handler.PatchPacketFilter` のまま）
- トラフィック分散一覧取得（`client.Applications["ID"].Traffics.GetAsync()`）
  - レスポンスボディ型
    - 変更前: `ListTraffics`
    - 変更後: `ListTraffic`
- トラフィック分散変更（`client.Applications["ID"].Traffics.PutAsync(...)`）
  - リクエストボディ型（内部で合成型メンバーを使用）
    - 変更前: `PutTrafficsBody`（`PutTrafficsBodyMember1`/`PutTrafficsBodyMember2` を使用）
    - 変更後: `UpdateTrafficBody`（`UpdateTrafficBodyMember1`/`UpdateTrafficBodyMember2` を使用）
  - レスポンスボディ型
    - 変更前: `PutTraffics`
    - 変更後: `UpdateTraffic`
- バージョン取得（`client.Applications["ID"].Versions["VER"].GetAsync()`）
  - レスポンスボディ型
    - 変更前: `GetVersion`
    - 変更後: `ReadVersion`
- バージョンステータス取得（`client.Applications["ID"].Versions["VER"].Status.GetAsync()`）
  - レスポンスボディ型
    - 変更前: `GetVersionStatus`
    - 変更後: `ReadVersionStatus`
- ユーザー情報取得（`client.User.GetAsync()`）
  - レスポンスボディ型
    - 変更前: `GetUser`
    - 変更後: `ReadUser`
- ユーザー作成（`client.User.PostAsync()`）
  - レスポンスボディ型
    - 変更前: `PostUser`
    - 変更後: `CreateUser`

上記の各型が内部に持つコンポーネント情報・ステータス・プローブ設定などのネストされた型（`_components`、`_status`、`_probe` などを名前に含む型）も、それぞれ属する親の型と同じ命名規則（例: `GetApplication_components` → `ReadApplication_components`）でまとめて改名されています。  
プロパティの構成自体に変更はありません。  

以下は移行パターンの代表例です。  
上記の一覧にある他の操作も、同じ考え方（型名の置き換え）で移行できます。  

```cs
// 変更前: アプリケーションの作成と取得
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Handler;

PostApplicationBody createBody = new PostApplicationBody
{
    Name = "my-app",
    MaxScale = 1,
    MinScale = 0
};
PostApplication createdApplication = await client.Applications.PostAsync(createBody);

GetApplication fetchedApplication = await client.Applications["APPLICATION_ID"].GetAsync();
```
```cs
// 変更後: アプリケーションの作成と取得
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Handler;

CreateApplicationBody createBody = new CreateApplicationBody
{
    Name = "my-app",
    MaxScale = 1,
    MinScale = 0
};
CreateApplication createdApplication = await client.Applications.PostAsync(createBody);

ReadApplication fetchedApplication = await client.Applications["APPLICATION_ID"].GetAsync();
```

```cs
// 変更前: トラフィック分散の変更
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Applications.Item.Traffics;

TrafficsRequestBuilder.PutTrafficsBody trafficItem = new TrafficsRequestBuilder.PutTrafficsBody
{
    PutTrafficsBodyMember2 = new PutTrafficsBodyMember2
    {
        VersionName = "version1.0",
        Percent = 100
    }
};
List<TrafficsRequestBuilder.PutTrafficsBody> requestBody = new List<TrafficsRequestBuilder.PutTrafficsBody> { trafficItem };
await client.Applications["APPLICATION_ID"].Traffics.PutAsync(requestBody);
```
```cs
// 変更後: トラフィック分散の変更
using Sacloud.Sdk.AppRunShared.Applications.Item.Traffics;
using Sacloud.Sdk.AppRunShared.Models;

TrafficsRequestBuilder.UpdateTrafficBody trafficItem = new TrafficsRequestBuilder.UpdateTrafficBody
{
    UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2
    {
        VersionName = "version1.0",
        Percent = 100
    }
};
List<TrafficsRequestBuilder.UpdateTrafficBody> requestBody = new List<TrafficsRequestBuilder.UpdateTrafficBody> { trafficItem };
await client.Applications["APPLICATION_ID"].Traffics.PutAsync(requestBody);
```

影響:

- 上記の一覧にある型名（ネストされたコンポーネント型を含む）を変数の型宣言・キャスト・ジェネリック型指定などで直接参照しているコードは、そのままではコンパイルエラーになります。
- 戻り値を `var` で受け取る、またはメソッドチェーンでプロパティに直接アクセスするなど、型名を明示していないコードは修正不要です。
- プロパティの構造、バリデーション内容、API から返却される値の内容には変更がないため、型名の置き換えのみで移行できます。
  ネストされた型を参照しているプロパティ（`Components` など）は、参照先のクラス名変更に伴い宣言する型名も変わります。

移行の考え方:

- 上記の一覧を参照し、変数の型宣言・`new` によるインスタンス生成・コレクション型の指定などを新しい型名に置き換えてください。
- `TrafficsRequestBuilder.PutTrafficsBody` および `PutTrafficsBodyMember1`/`PutTrafficsBodyMember2` を使用している場合は、それぞれ `TrafficsRequestBuilder.UpdateTrafficBody`、`UpdateTrafficBodyMember1`/`UpdateTrafficBodyMember2` に置き換えてください。
- パケットフィルタ変更の呼び出しでは、リクエストボディの型のみ `PatchPacketFilterBody` に置き換えてください。
  戻り値の型（`Handler.PatchPacketFilter`）は変更不要です。

## 仕様追随

- OpenAPI 仕様バージョン: 1.4.0 → 1.5.0
- SDK バージョン: 0.3.0 → 0.4.0
- 今回の追従はエンドポイントの命名規則統一による型名変更が中心で、新規のプロパティ追加やバリデーション内容の変更はありません。

## 利用者向け確認ポイント

1. `Sacloud.Sdk.AppRunShared.Models` および `Sacloud.Sdk.AppRunShared.Models.Handler` 名前空間の型を変数宣言・キャストなどで直接参照しているコードがないか確認し、上記の一覧に従って新しい型名に置き換えてください。
2. `TrafficsRequestBuilder.PutTrafficsBody`、`PutTrafficsBodyMember1`、`PutTrafficsBodyMember2` を使用しているコードがあれば、`UpdateTrafficBody`、`UpdateTrafficBodyMember1`、`UpdateTrafficBodyMember2` に置き換えてください。
3. パケットフィルタ変更（`Packet_filter.PatchAsync`）のリクエストボディを `PatchPacketFilter` 型で組み立てているコードがあれば、`PatchPacketFilterBody` に置き換えてください（レスポンスの型は変更不要です）。
4. 戻り値を `var` で受け取っている、またはメソッドチェーンでプロパティへ直接アクセスしているコードは、再ビルドのみで動作を継続できます。
