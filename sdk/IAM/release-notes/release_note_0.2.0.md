# IAM リリースノート

## v0.2.0

IAM SDK v0.2.0 では、ユーザープロビジョニング（SCIM設定）機能が新規追加されました。また、サービスプリンシパルキーの有効期限プロパティ名が変更されています。  `ServicePrincipalKey.ExpiredAt` を参照している既存コードは修正が必要です。  

## 重要: 互換性影響のある変更

### ServicePrincipalKey の有効期限プロパティ名変更

サービスプリンシパルキーを表す `ServicePrincipalKey` クラスの有効期限プロパティ名が変更されました。  

- `ServicePrincipalKey.ExpiredAt`  
  - 変更前: `ExpiredAt`（JSON フィールド: `expired_at`）  
  - 変更後: `KeyExpiresAt`（JSON フィールド: `key_expires_at`）  

```cs
// 変更前
ServicePrincipalKey key = await client.ServicePrincipals[id].Keys.PostAsync(requestBody);
string expiresAt = key.ExpiredAt;

// 変更後
ServicePrincipalKey key = await client.ServicePrincipals[id].Keys.PostAsync(requestBody);
string expiresAt = key.KeyExpiresAt;
```

影響:

- `ServicePrincipalKey.ExpiredAt` を参照しているコードはコンパイルエラーになります。  
- `ServicePrincipalKey.ExpiredAt` を参照している箇所を `KeyExpiresAt` に変更する必要があります。  

移行の考え方:

- `ExpiredAt` プロパティをすべて `KeyExpiresAt` に置き換えてください。

## 追加・改善

### ユーザープロビジョニング（SCIM設定）機能の追加

IAM API v1.1.0 への追随として、ユーザープロビジョニング（SCIM設定）の管理機能が追加されました。  
`IAMClient.ScimConfigurations` プロパティから操作できます。  

追加された操作:

| 操作 | メソッド | 説明 |
|------|---------|------|
| 一覧取得 | `ScimConfigurations.GetAsync()` | ユーザープロビジョニング設定の一覧を取得する。 |
| 作成 | `ScimConfigurations.PostAsync(body)` | ユーザープロビジョニング設定を作成する。（シークレットトークンが返却される） |
| 単体取得 | `ScimConfigurations[id].GetAsync()` | 指定 ID のユーザープロビジョニング設定を取得する。 |
| 更新 | `ScimConfigurations[id].PutAsync(body)` | 指定 ID のユーザープロビジョニング設定を更新する。 |
| 削除 | `ScimConfigurations[id].DeleteAsync()` | 指定 ID のユーザープロビジョニング設定を削除する。 |
| トークン再発行 | `ScimConfigurations[id].RegenerateToken.PostAsync()` | シークレットトークンを再発行する。（既存トークンは無効化される） |

追加されたレスポンス型:

- `ScimConfiguration`  
  - 作成時のレスポンス。  
  - シークレットトークン（`SecretToken`）を含む。  
  - 取得は読み取り専用。  
- `ScimConfigurationPublic`  
  - 取得・更新時のレスポンス。  
  - シークレットトークンは含まれない。  

```cs
// ユーザープロビジョニング設定を作成する
ScimConfigurationsPostRequestBody createBody = new ScimConfigurationsPostRequestBody
{
    Name = "my-provisioning",
};
ScimConfiguration created = await client.ScimConfigurations.PostAsync(createBody);
string secretToken = created.SecretToken; // 作成時のみ取得可能

// 一覧取得
ScimConfigurationsGetResponse list = await client.ScimConfigurations.GetAsync();

// 単体取得
ScimConfigurationPublic detail = await client.ScimConfigurations[created.Id.Value].GetAsync();

// シークレットトークン再発行（既存トークンは無効化される）
RegenerateTokenPostResponse regenerated = await client.ScimConfigurations[created.Id.Value].RegenerateToken.PostAsync();
```

## 仕様追随

- OpenAPI スペック: `1.0.0` → `1.1.0`  
- SDK バージョン: `0.1.1` → `0.2.0`  

## 利用者向け確認ポイント

1. サービスプリンシパルキーを使って有効期限を取得・表示しているコードを確認してください。  
2. `ServicePrincipalKey.ExpiredAt` を参照している箇所を `KeyExpiresAt` に変更してください。  
3. ユーザープロビジョニング機能を使用する場合、シークレットトークンは作成時のレスポンス（`ScimConfiguration.SecretToken`）でのみ取得できます。  
   作成後に確認する手段はないため、作成時に安全な場所へ保存してください。  
