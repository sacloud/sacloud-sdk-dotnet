# オブジェクトストレージ リリースノート

## v0.2.0

オブジェクトストレージ API `1.7.0` に追従しました。  
既存の操作で使用するリクエスト型、戻り値型、およびパーミッション ID の指定方法が一部変更されているため、更新前に利用コードのコンパイルと動作確認を実施してください。  

## 重要: 互換性影響のある変更

### リクエストボディとレスポンスボディの型変更

リクエストボディとレスポンスボディとして直接使用するモデルの型名が変更されました。  
リクエストボディや戻り値の型を明示的に宣言している場合は、以下の一覧に従って型名を置き換えてください。  

| 対象メソッド | 変更対象 | 変更前 | 変更後 |
| --- | --- | --- | --- |
| `client.Clusters.GetAsync()` | レスポンスボディ | `Handler.ListClustersRes` | `Handler.ListClustersResponse` |
| `client.Clusters["<ClusterId>"].GetAsync()` | レスポンスボディ | `Handler.GetClusterRes` | `Handler.ReadClusterResponse` |
| `client.Account.Keys["<KeyId>"].GetAsync()` | レスポンスボディ | `AccountKey` | `ReadAccountKey` |
| `client.Buckets.GetAsync()` | レスポンスボディ | `BucketList` | `Handler.ListBucketsResponse` |
| `client.Buckets["<BucketName>"].PutAsync(<RequestBody>)` | リクエストボディ | `Handler.PutBucketReqBody` | `Handler.CreateBucketRequestBody` |
| `client.Buckets["<BucketName>"].PutAsync(<RequestBody>)` | レスポンスボディ | `Handler.PutBucketRes` | `Handler.CreateBucketResponse` |
| `client.Buckets["<BucketName>"].Replication.GetAsync()` | レスポンスボディ | `Handler.GetReplicationRes` | `Handler.ReadBucketReplicationResponse` |
| `client.Buckets["<BucketName>"].Replication.PostAsync(<RequestBody>)` | リクエストボディ | `PostBucketReplication` | `CreateBucketReplication` |
| `client.Buckets["<BucketName>"].Replication.PostAsync(<RequestBody>)` | レスポンスボディ | `Handler.PostReplicationRes` | `Handler.CreateBucketReplicationResponse` |
| `client.Buckets["<BucketName>"].ReplicableTargets.GetAsync()` | レスポンスボディ | `Handler.GetReplicableTargetsRes` | `Handler.ListBucketReplicableTargetsResponse` |
| `client.Buckets["<BucketName>"].Encryption.GetAsync()` | レスポンスボディ | `GetBucketEncryptionResponse` | `ReadBucketEncryptionResponse` |
| `client.Buckets["<BucketName>"].Encryption.PutAsync(<RequestBody>)` | レスポンスボディ | `PutBucketEncryptionResponse` | `UpdateBucketEncryptionResponse` |
| `client.Buckets["<BucketName>"].Encryption.PutAsync(<RequestBody>)` | リクエストボディ | `Handler.EncryptionConfigReqBody` | `Handler.UpdateBucketEncryptionRequestBody` |
| `client.Buckets["<BucketName>"].Plan.GetAsync()` | レスポンスボディ | `GetBucketPlanResponse` | `ReadBucketPlanResponse` |
| `client.Buckets["<BucketName>"].Plan.PutAsync(<RequestBody>)` | レスポンスボディ | `PutBucketPlanResponse` | `UpdateBucketPlanResponse` |
| `client.Permissions[<PermissionId>].Keys["<KeyId>"].GetAsync()` | レスポンスボディ | `PermissionKey` | `Handler.ReadPermissionKeyResponse` |
| `client.Permissions[<PermissionId>].Keys.PostAsync()` | レスポンスボディ | `PermissionKey` | `Handler.CreatePermissionKeyResponse` |
| `client.Permissions[<PermissionId>].Keys.GetAsync()` | レスポンスボディ | `PermissionKeys` | `Handler.ListPermissionKeysResponse` |
| `client.Plans.GetAsync()` | レスポンスボディ | `GetPlansResponse` | `ListPlansResponse` |
| `client.Metering.Buckets["<BucketName>"].GetAsync(<QueryParameter>)` | レスポンスボディ | `GetBucketMeteringResponse` | `ListBucketMeteringResponse` |

影響:
- 旧型を直接参照しているコードは、そのままではコンパイルできません。
- 変更前は、例えばパーミッションのアクセスキー操作では、個別取得と発行に `PermissionKey` という同じ型を使用していました。
  今回の変更では、API 操作ごとにレスポンス型が分離されたため、次のように各操作に対応する型を使用する必要があります。
  - 一覧取得: `PermissionKeys` → `Handler.ListPermissionKeysResponse`
  - 個別取得: `PermissionKey` → `Handler.ReadPermissionKeyResponse`
  - 新規発行: `PermissionKey` → `Handler.CreatePermissionKeyResponse`
- 旧型を新型へ置き換える際は、利用している操作に対応する型とプロパティ構成を確認してください。

移行の考え方:
- 上記の表を参照し、変数の型宣言・キャスト・`new` によるインスタンス生成・コレクション型の指定などを新しい型名に置き換えてください。
- リクエストボディの型を明示している場合は、表の「リクエストボディ」の変更前・変更後に従って置き換えてください。
  戻り値の型を明示している場合は、表の「レスポンスボディ」の変更前・変更後に従って置き換えてください。
- API 呼び出し結果のプロパティを明示的に参照している箇所は、更新後の型のプロパティ構成を確認してください。
- SDK の `RequestBuilder` が提供する `GetAsync`、`PostAsync`、`PutAsync` などの呼び出し方法は変更されていません。型を明示していない既存のメソッド呼び出しは、そのまま利用できます。

### モデルクラスの構造変更

モデルクラスの型と、モデルが保持するプロパティの変更があります。  
該当する型を変数宣言、`new`、キャスト、コレクション型などで直接参照している場合は置き換えてください。  

次の表で「対象モデル」が「`型` → `型`」と記載されている場合は、変更前の型から変更後の型へ変更されたことを示します。  
実際にクラス名が変更されたものについては、備考に記載します。  

| 対象モデル | 対象プロパティ | 変更前の型 | 変更後の型 | 備考 |
| --- | --- | --- | --- | --- |
| `ListClustersRes` → `ListClustersResponse` | `Data` | `List<Model.Cluster>` | `List<ClusterResponseBody>` | クラスタ一覧取得のレスポンスモデル |
| `GetClusterRes` → `ReadClusterResponse` | `Data` | `Model.Cluster` | `ClusterResponseBody` | クラスタ個別取得のレスポンスモデル |
| `Model.Cluster` | `IamEndpoint` | `string` | 削除 | |
| `Model.Cluster` | `IamEndpointForControlPanel` | `string` | 削除 | |
| `BucketList` → `Handler.ListBucketsResponse` | `Data` | `List<BucketList_data>` | `List<BucketListResponseBody>` | バケット一覧取得のレスポンスモデル |
| `BucketList_data` → `BucketListResponseBody` | `Plan` | `BucketList_data_plan` | `BucketListResponseBody_plan` | バケット一覧のプランモデル |
| `Handler.PutBucketReqBody` → `Handler.CreateBucketRequestBody` | `Plan` | `Handler.PutBucketReqBody_plan` | `Handler.CreateBucketRequestBody_plan` | バケット作成リクエストのプランモデル |
| `Handler.PutBucketRes` → `Handler.CreateBucketResponse` | `Data` | `Model.Bucket` | `BucketResponseBody` | バケット作成のレスポンスモデル |
| `Handler.GetReplicableTargetsRes` → `Handler.ListBucketReplicableTargetsResponse` | `Data` | `List<Model.Bucket>` | `List<BucketListResponseBody>` | レプリケーション先候補一覧のレスポンスモデル |
| `GetBucketEncryptionResponse` → `ReadBucketEncryptionResponse` | `Data` | `Handler.EncryptionConfigRes` | `Handler.ReadBucketEncryptionResponse` | バケット暗号化設定取得のレスポンスモデル |
| `PutBucketEncryptionResponse` → `UpdateBucketEncryptionResponse` | `Data` | `Handler.EncryptionConfigRes` | `Handler.UpdateBucketEncryptionResponse` | バケット暗号化設定更新のレスポンスモデル |
| `AccountKeys` | `Data[].Secret` | `string` | 削除 | アクセスキー一覧では返却されない |
| `AccountKey` → `ReadAccountKey` | `Data` | `AccountKey_data` | `ReadAccountKey_data` | アクセスキー個別取得のレスポンスモデル |
| `AccountKey_data` → `ReadAccountKey_data` | `Secret` | `string` | 削除 | アクセスキー個別取得では返却されない |
| `PermissionKeys` → `Handler.ListPermissionKeysResponse` | `Data` | `List<PermissionKeys_data>` | `List<PermissionKeyReadResponseBody>` | パーミッションキー一覧取得のレスポンスモデル |
| `PermissionKeys_data` → `PermissionKeyReadResponseBody` | `Id` | `long` | `string` | パーミッションキー一覧のID型変更 |
| `PermissionKeys_data` → `PermissionKeyReadResponseBody` | `Secret` | `string` | 削除 | パーミッションキー一覧では返却されない |
| `PermissionKey` → `Handler.ReadPermissionKeyResponse` | `Data` | `PermissionKey_data` | `PermissionKeyReadResponseBody` | パーミッションキー個別取得のレスポンスモデル |
| `PermissionKey_data` → `PermissionKeyReadResponseBody` | `Id` | `long` | `string` | パーミッションキー個別取得のID型変更 |
| `PermissionKey_data` → `PermissionKeyReadResponseBody` | `Secret` | `string` | 削除 | パーミッションキー個別取得では返却されない |
| `PermissionKey` → `Handler.CreatePermissionKeyResponse` | `Data` | `PermissionKey_data` | `PermissionKeyCreateResponseBody` | パーミッションキー発行のレスポンスモデル |
| `Permission_data` | `BucketControls` | `List<BucketControl>` | `List<BucketControlsItem>` | パーミッション個別取得のバケット制御モデル |
| `Permissions_data` | `BucketControls` | `List<BucketControl>` | `List<BucketControlsItem>` | パーミッション一覧取得のバケット制御モデル |
| `PermissionKey` | `Data` | `PermissionKey_data` | 削除 | `PermissionKey` が `Id`、`Secret`、`CreatedAt` を直接保持する |
| `PermissionKey` | `Id` | - | `string` | `PermissionKey.Data.Id`（`long`）が削除され、`PermissionKey` が直接保持する形で新規追加 |
| `PermissionKey` | `Secret` | - | `string` | `PermissionKey.Data.Secret` が削除され、`PermissionKey` が直接保持する形で新規追加 |
| `PermissionKey` | `CreatedAt` | - | `DateTimeOffset` | `PermissionKey.Data.CreatedAt` が削除され、`PermissionKey` が直接保持する形で新規追加 |
| `PermissionBucketControlsBody` | `BucketControls` | `List<BucketControl>` | `List<BucketControlsBodyItem>` | パーミッション設定リクエストのバケット制御モデル |

補足:
- パーミッションのアクセスキーのシークレット（`Secret`）は、新規発行時のレスポンスでのみ取得できます。
  一覧取得・個別取得のレスポンスには、このシークレットは含まれず、`Id` と `CreatedAt` のみ返却されます。

影響:
- 旧モデルや旧プロパティを直接参照しているコードは、更新後のSDKではコンパイルできません。
- 削除されたプロパティを参照しているコードは修正が必要です。
- 一覧取得・個別取得のレスポンスからパーミッションのアクセスキーの `Secret` を取得しているコードは、取得処理を新規発行時のレスポンスへ見直してください。

移行の考え方:
- 上記の表を参照し、変数の型宣言・キャスト・`new` によるインスタンス生成・コレクション型の指定などを新しい型に置き換えてください。
- 旧モデルや旧プロパティを参照しているコードは、削除されたプロパティを参照しないように修正してください。
- パーミッションのアクセスキーのシークレット（`Secret`）を取得しているコードは、一覧取得・個別取得ではなく、新規発行時のレスポンスから取得するように修正してください。

### パーミッション API のパスパラメータ型変更

パーミッション API のパスパラメータ（パーミッション ID）を指定するインデクサーの引数型を変更しました。  
- 変更前: `string` 型
- 変更後: `int` 型

```csharp
// 変更前
PermissionsItemRequestBuilder permission = client.Permissions["1234"];

// 変更後
PermissionsItemRequestBuilder permission = client.Permissions[1234];
```

影響:
- パーミッション ID を `string` 型で指定する既存コードはコンパイルできません。

移行の考え方:
- パーミッション ID を `int` 型で指定してください。

### バリデーション挙動の変更

一部のプロパティ、パスパラメータ、クエリパラメータのバリデーションルールが変更されています。  
既存の入力値が新しいバリデーションを通過できるか確認してください。  

#### プロパティのバリデーション変更

| プロパティ | 変更前バリデーション | 変更後バリデーション |
| - | - | - |
| `Models.BucketControlsBodyItem.BucketName` | 正規表現: `^[a-zA-Z][a-zA-Z0-9]{2,}` | 必須<br>正規表現: `^[a-z0-9][a-z0-9-]*[a-z0-9]$`<br>最小文字数: `3`<br>最大文字数: `64` |
| `Models.Handler.UpdateBucketEncryptionRequestBody.KmsKeyId` | 必須<br>正規表現: `\d{1,12}` | 必須<br>正規表現: `^\d{1,12}$` |
| `Models.PlanChangeReqBody_previous_contract.ResourceId` | 正規表現: `\d{1,12}` | 必須<br>正規表現: `^\d{1,12}$` |
| `Models.PermissionBucketControlsBody.BucketControls` | なし | 必須 |
| `Models.PermissionBucketControlsBody.DisplayName` | 最小文字数: `1` | 必須<br>正規表現: `^.+$` |
| `Models.BucketControlsBodyItem.CanRead` | なし | 必須 |
| `Models.BucketControlsBodyItem.CanWrite` | なし | 必須 |

#### パスパラメータのバリデーション変更

バケット名をパスパラメータとして受け取る一部の操作で、検証ルールが変更されました。  

| 対象操作 | 変更前バリデーション | 変更後バリデーション |
| - | - | - |
| `client.Buckets["<BucketName>"].Penalty.GetAsync()` | 正規表現: `^[a-zA-Z][a-zA-Z0-9]{2,}` | 正規表現: `^[a-z0-9][a-z0-9-]*[a-z0-9]$`<br>最小文字数: `3`<br>最大文字数: `64` |
| `client.Buckets["<BucketName>"].Plan.GetAsync()` | 正規表現: `^[a-zA-Z][a-zA-Z0-9]{2,}` | 正規表現: `^[a-z0-9][a-z0-9-]*[a-z0-9]$`<br>最小文字数: `3`<br>最大文字数: `64` |
| `client.Buckets["<BucketName>"].Plan.PutAsync(<RequestBody>)` | 正規表現: `^[a-zA-Z][a-zA-Z0-9]{2,}` | 正規表現: `^[a-z0-9][a-z0-9-]*[a-z0-9]$`<br>最小文字数: `3`<br>最大文字数: `64` |
| `client.Buckets["<BucketName>"].Quota.GetAsync()` | 正規表現: `^[a-zA-Z][a-zA-Z0-9]{2,}` | 正規表現: `^[a-z0-9][a-z0-9-]*[a-z0-9]$`<br>最小文字数: `3`<br>最大文字数: `64` |
| `client.Buckets["<BucketName>"].Usage.GetAsync()` | 正規表現: `^[a-zA-Z][a-zA-Z0-9]{2,}` | 正規表現: `^[a-z0-9][a-z0-9-]*[a-z0-9]$`<br>最小文字数: `3`<br>最大文字数: `64` |
| `client.Metering.Buckets["<BucketName>"].GetAsync(<QueryParameter>)` | 正規表現: `^[a-zA-Z][a-zA-Z0-9]{2,}` | 正規表現: `^[a-z0-9][a-z0-9-]*[a-z0-9]$`<br>最小文字数: `3`<br>最大文字数: `64` |

#### クエリパラメータのバリデーション変更

メータリングAPIのクエリパラメータ検証の適用条件が変更されました。  

| 対象操作 | 変更前 | 変更後 |
| - | - | - |
| `client.Metering.Buckets["<BucketName>"].GetAsync(<QueryParameter>)` | `requestConfiguration` を指定した場合にのみクエリパラメータ検証を実行 | `requestConfiguration` の有無にかかわらずクエリパラメータ検証を実行 |

上記の制約に違反する値は、SDK 内部のパラメータ検証またはモデル検証でエラーになる可能性があります。  

影響:
- 変更後の必須項目を設定していないリクエストや、正規表現・長さの制約に適合しない値は、SDK内の検証でエラーになる可能性があります。
- これまで使用できた大文字を含むバケット名や、先頭・末尾が英数字ではないバケット名は、パスパラメータの検証でエラーになる可能性があります。
- メータリングの `GetAsync` は、`requestConfiguration` を指定しない場合もクエリパラメータを検証するため、既存の呼び出しで `from` / `to` が未設定の場合はエラーになる可能性があります。

移行の考え方:
- プロパティのバリデーション表で変更対象となっている入力値を、変更後の必須条件・正規表現・長さ制約に合わせて見直してください。
- メータリングを利用している場合は、`GetAsync` のリクエスト設定で `from` と `to` を設定し、更新後のSDKで検証を含む呼び出しを確認してください。

## 追加・改善

### 未使用クラスの削除

次の SDK 内では使用されていないモデルクラスを削除しました。  
- `Error400`、`Error400_error`
- `Error401`、`Error401_error`
- `Error403`、`Error403_error`
- `Error404`、`Error404_error`
- `Error409`、`Error409_error`
- `ErrorDefault`、`ErrorDefault_error`
- `WithName`（`Error400_error.Errors` の要素型）
- `Buckets`（`Error401_error.Errors` の要素型）
- `Plans`（`ErrorDefault_error.Errors` の要素型）

影響:
- これらのクラスは SDK では使用されていないため、通常の利用コードへの影響はありません。
- 万一これらの型を直接参照しているコードがある場合はコンパイルエラーになりますが、その場合は参照を除去してください。

## 仕様追随

- OpenAPI 定義を `1.5.0` から `1.7.0` へ更新しました。
- SDK パッケージのバージョンは `0.2.0` です。

## 利用者向け確認ポイント

1. リクエストボディとレスポンスボディの型変更の表を参照し、`GetAsync`、`PostAsync`、`PutAsync` の戻り値型・リクエストボディ型を新しい型名に置き換えてください。
2. `client.Permissions["<PermissionId>"]` の引数を `string` から `int` に変更してください。
3. 削除された `Error*`、`Buckets`、`Plans`、`WithName` を直接参照している場合は参照を削除してください。
4. 「モデルクラスの構造変更」表を参照し、`Data` の型、`BucketControls`、クラスタの削除プロパティ、パーミッションキーの `Id` / `Secret` などを確認してください。
5. 変数宣言、キャスト、`new`、コレクション型、レスポンスの `Secret` 参照を見直してください。
6. 「バリデーション挙動の変更」表を参照し、リクエストモデルの必須項目・正規表現、バケット名のパスパラメータ、メータリングのクエリパラメータ検証を確認してください。
