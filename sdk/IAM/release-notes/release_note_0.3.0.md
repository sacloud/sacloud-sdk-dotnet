# IAM リリースノート

## v0.3.0

IAM SDK v0.3.0 では、認証コンテキスト取得 API（`GET /auth/context`）が新規追加されました。  
また、IAM ロール取得 API（`GET /iam-roles`、`GET /iam-roles/{iam_role_id}`）のレスポンスボディに付与可能な最低階層を示すプロパティが追加されています。  
いずれも追加のみの変更であり、既存コードへの影響はありません。

## 追加・改善

### 認証コンテキスト取得機能の追加

APIキー・サービスプリンシパルが操作可能なプロジェクト ID を取得する機能が追加されました。  
`IAMClient.Auth.Context.GetAsync()` から操作できます。  
usacloud などの OSS プロダクトからの利用を主な用途としています。

| 操作 | メソッド | 説明 |
|------|---------|------|
| コンテキスト取得 | `Auth.Context.GetAsync()` | APIキーまたはサービスプリンシパルが操作可能なプロジェクト ID を取得する。 |

レスポンス型 `ContextGetResponse` のプロパティ:

| プロパティ | 型 | 説明 |
|-----------|-----|------|
| `AuthType` | `ContextGetResponse_auth_type?` | 認証種別。`Apikey`（APIキー）または `Service_principal`（サービスプリンシパル）。 |
| `LimitedToProjectId` | `long?` | 操作可能なプロジェクトの ID。将来的に制限が撤廃された場合は `null` になる可能性がある。 |
| `ResourceId` | `long?` | APIキーの ID またはサービスプリンシパルの ID。 |

```cs
// 認証コンテキストを取得する
ContextGetResponse context = await client.Auth.Context.GetAsync();
ContextGetResponse_auth_type? authType = context.AuthType;
long? limitedToProjectId = context.LimitedToProjectId;
long? resourceId = context.ResourceId;
```

### IAMロールへの最低付与可能階層プロパティ追加

IAM ロール取得 API のレスポンスボディ `IamRole` クラスに、そのロールを付与できる最低階層を示す `LowestGrantableResource` プロパティが追加されました。

| プロパティ | 型 | 説明 |
|-----------|-----|------|
| `LowestGrantableResource` | `IamRole_lowest_grantable_resource?` | `Organization`（組織）・`Folder`（フォルダ）・`Project`（プロジェクト）のいずれか。 |

既存の `IamRole` 取得コードは修正不要です。  
新プロパティは、IAMロールの付与先の選択肢を制御する際に参照できます。

## 仕様追随

- OpenAPI スペック: `1.1.0` → `1.3.0`
- SDK バージョン: `0.2.0` → `0.3.0`

## 利用者向け確認ポイント

既存コードへの互換性影響はありません。  
追加されたプロパティやメソッドを使用しない場合、コードの修正は不要です。
