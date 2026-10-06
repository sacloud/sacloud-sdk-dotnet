# AppRunShared リリースノート

## v0.3.0

OpenAPI 仕様 1.4.0 への追従に伴う更新です。  
コンポーネントにシークレット（`secret`）プロパティが追加されました。  
また各操作で個別に定義されていた環境変数モデルを共通の型へ統一するリネームが行われたため、これらの型を直接参照しているコードには修正が必要です。  
既存コードへの影響箇所があるため、下記の互換性変更を確認してください。  

## 重要： 互換性影響のある変更

### 環境変数モデルの型名変更

複数の操作（GET/PATCH/POST）でそれぞれ個別に定義されていた環境変数モデルが、共通の型に統一されました。  
既存コードでこれらの型を直接参照している場合は、以下に従って置き換えが必要です。  

#### リクエストボディ（コンポーネントに渡す環境変数）

- `PostApplicationBody_components.Env` の要素型：
  - 変更前： `Sacloud.Sdk.AppRunShared.Models.PostApplicationBody_components_env`
  - 変更後： `Sacloud.Sdk.AppRunShared.Models.Request.EnvItem`
- `PatchApplicationBody_components.Env` の要素型：
  - 変更前： `Sacloud.Sdk.AppRunShared.Models.PatchApplicationBody_components_env`
  - 変更後： `Sacloud.Sdk.AppRunShared.Models.Request.EnvItem`

```cs
// 変更前
using Sacloud.Sdk.AppRunShared.Models;

PostApplicationBody_components postApplicationBodyComponent = new PostApplicationBody_components
{
    Env = new List<PostApplicationBody_components_env>
    {
        new PostApplicationBody_components_env
        {
            Key = "ENV_KEY",
            Value = "ENV_VALUE"
        }
    }
};

PatchApplicationBody_components patchApplicationBodyComponent = new PatchApplicationBody_components
{
    Env = new List<PatchApplicationBody_components_env>
    {
        new PatchApplicationBody_components_env
        {
            Key = "ENV_KEY",
            Value = "ENV_VALUE"
        }
    }
};
```
```cs
// 変更後
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Request;

PostApplicationBody_components postApplicationBodyComponent = new PostApplicationBody_components
{
    Env = new List<EnvItem>
    {
        new EnvItem
        {
            Key = "ENV_KEY",
            Value = "ENV_VALUE"
        }
    }
};

PatchApplicationBody_components patchApplicationBodyComponent = new PatchApplicationBody_components
{
    Env = new List<EnvItem>
    {
        new EnvItem
        {
            Key = "ENV_KEY",
            Value = "ENV_VALUE"
        }
    }
};
```

#### レスポンスボディ（コンポーネントから取得する環境変数）

- `GetApplication_components.Env` の要素型：
  - 変更前： `Sacloud.Sdk.AppRunShared.Models.Handler.GetApplication_components_env`
  - 変更後： `Sacloud.Sdk.AppRunShared.Models.Response.EnvItem`（共通型）
- `GetVersion_components.Env` の要素型：
  - 変更前： `Sacloud.Sdk.AppRunShared.Models.Handler.GetVersion_components_env`
  - 変更後： `Sacloud.Sdk.AppRunShared.Models.Response.EnvItem`（共通型）
- `PatchApplication_components.Env` の要素型：
  - 変更前： `Sacloud.Sdk.AppRunShared.Models.Handler.PatchApplication_components_env`
  - 変更後： `Sacloud.Sdk.AppRunShared.Models.Response.EnvItem`（共通型）
- `PostApplication_components.Env` の要素型：
  - 変更前： `Sacloud.Sdk.AppRunShared.Models.Handler.PostApplication_components_env`
  - 変更後： `Sacloud.Sdk.AppRunShared.Models.Response.EnvItem`（共通型）

```cs
// 変更前
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Handler;

PostApplication postApplicationResponse = await client.Applications.PostAsync(new PostApplicationBody());
GetApplication getApplicationResponse = await client.Applications["APPLICATION_ID"].GetAsync();
GetVersion getVersionResponse = await client.Applications["APPLICATION_ID"].Versions["VERSION_ID"].GetAsync();
PatchApplication patchApplicationResponse = await client.Applications["APPLICATION_ID"].PatchAsync(new PatchApplicationBody());

List<PostApplication_components_env> postApplicationResponseEnv = postApplicationResponse.Components[0].Env;
List<GetApplication_components_env> getApplicationResponseEnv = getApplicationResponse.Components[0].Env;
List<GetVersion_components_env> getVersionResponseEnv = getVersionResponse.Components[0].Env;
List<PatchApplication_components_env> patchApplicationResponseEnv = patchApplicationResponse.Components[0].Env;
```
```cs
// 変更後
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Response;
using Sacloud.Sdk.AppRunShared.Models.Handler;

PostApplication postApplicationResponse = await client.Applications.PostAsync(new PostApplicationBody());
GetApplication getApplicationResponse = await client.Applications["APPLICATION_ID"].GetAsync();
GetVersion getVersionResponse = await client.Applications["APPLICATION_ID"].Versions["VERSION_ID"].GetAsync();
PatchApplication patchApplicationResponse = await client.Applications["APPLICATION_ID"].PatchAsync(new PatchApplicationBody());

List<EnvItem> postApplicationResponseEnv = postApplicationResponse.Components[0].Env;
List<EnvItem> getApplicationResponseEnv = getApplicationResponse.Components[0].Env;
List<EnvItem> getVersionResponseEnv = getVersionResponse.Components[0].Env;
List<EnvItem> patchApplicationResponseEnv = patchApplicationResponse.Components[0].Env;
```

影響：

- 以下モデルは削除されているため、これらを参照しているコードはコンパイルエラーになります。
  - `PatchApplicationBody_components_env`
  - `PostApplicationBody_components_env`
  - `Handler.GetApplication_components_env`
  - `Handler.GetVersion_components_env`
  - `Handler.PatchApplication_components_env`
  - `Handler.PostApplication_components_env`
- 新しい共通型のプロパティ構成（`Key`, `Value`）は旧クラスと同一であるため、値の代入・参照ロジック自体の変更は不要です。

移行の考え方：

- 旧型を参照している変数の型宣言、`new` によるインスタンス生成、コレクション型の指定などを、上記の新しい型名に置き換えてください。
- `using Sacloud.Sdk.AppRunShared.Models.Request;` または `using Sacloud.Sdk.AppRunShared.Models.Response;` を using ディレクティブに追加することで、`EnvItem` として参照できます。
  両方を同時に using している場合は `EnvItem` の名前解決が曖昧になるため、`Request.EnvItem` または `Response.EnvItem` と修飾して参照してください。

---

### 未使用クラスの削除

`CloudctrlError`、`DefaultError`、`DefaultError_error`、`User`、`User_location_type`（`Sacloud.Sdk.AppRunShared.Models.Model` 名前空間配下のモデル）は、SDK として実際には使用されていなかった残置クラスのため、削除されました。

影響：

- これらのクラスは SDK の公開 API では使用されていないため、通常の利用コードへの影響はありません。
- 万一これらの型を直接参照しているコードがある場合はコンパイルエラーになりますが、その場合は参照を除去してください。

---

### バリデーション挙動の変更

一部プロパティのバリデーションルールが変更されています。  
既存の入力値が新しいバリデーションを通過できるか確認してください。  

- 以下プロパティから「空配列を禁止する」バリデーションが削除されました。空配列が許容されるようになります。
  - `PatchApplicationBody.Components`
  - `PostApplicationBody.Components`
- 以下プロパティから「必須」バリデーションが削除されました。
  - `PatchApplicationBody_components_probe_http_get_headers.Name`
  - `PatchApplicationBody_components_probe_http_get_headers.Value`
  - `PostApplicationBody_components_probe_http_get_headers.Name`
  - `PostApplicationBody_components_probe_http_get_headers.Value`
- 以下プロパティに「最小文字数 1」のバリデーションが追加されました。空文字列が渡された場合にバリデーションエラーとなります。
  - `PatchApplicationBody_components.Name`
  - `PostApplicationBody_components.Name`
  - `PostApplicationBody.Name`
  - `PatchApplicationBody_components_deploy_source_container_registry.Image`
  - `PostApplicationBody_components_deploy_source_container_registry.Image`

## 追加・改善

### シークレット（`secret`）プロパティの追加

コンポーネントにシークレットプロパティが追加されました。  
シークレットの詳細については以下ドキュメントを参照してください。  
- [コントロールパネル操作ガイド | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/apprun/operation.html#apprun-operation-container-configuration-variable)

#### リクエスト（アプリケーション作成・更新時）

- `PostApplicationBody_components.Secret`（型： `List<PostApplicationBody_components_secret>`）
- `PatchApplicationBody_components.Secret`（型： `List<PatchApplicationBody_components_secret>`）
  - 各要素の `Key` には `K_SERVICE`、`K_CONFIGURATION`、`K_REVISION`、`PORT` を設定できません。
  - PATCH リクエストで既に存在するキーの `Value` を省略した場合、直前のバージョンで保存された値がそのまま使用されます。

```cs
// アプリケーション作成時にシークレット環境変数を設定する例
using Sacloud.Sdk.AppRunShared.Models;

PostApplicationBody_components component = new PostApplicationBody_components
{
    Name = "my-component",
    Secret = new List<PostApplicationBody_components_secret>
    {
        new PostApplicationBody_components_secret
        {
          Key = "DB_PASSWORD",
          Value = "secret-value"
        }
    }
};
```

#### レスポンス（アプリケーション・バージョン取得時）

- `GetApplication_components.Secret`（型： `List<Response.SecretItem>`）
- `GetVersion_components.Secret`（型： `List<Response.SecretItem>`）
- `PatchApplication_components.Secret`（型： `List<Response.SecretItem>`）
- `PostApplication_components.Secret`（型： `List<Response.SecretItem>`）
  - `Response.SecretItem` は `Key` プロパティのみを持ちます。
    `Value` は API から返却されません。

```cs
// アプリケーション取得時にシークレット環境変数のキーを取得する例
using Sacloud.Sdk.AppRunShared.Models.Response;
using Sacloud.Sdk.AppRunShared.Models.Handler;

GetApplication getApplicationResponse = await client.Applications["APPLICATION_ID"].GetAsync();
List<SecretItem> secrets = getApplicationResponse.Components[0].Secret;
foreach (var secret in secrets)
{
    Console.WriteLine($"- {secret.Key}");
}
```

## 仕様追随

- OpenAPI 仕様バージョン： 1.3.0 → 1.4.0
- SDK バージョン： 0.2.1 → 0.3.0
- その他：
  - コンテナレジストリ関連プロパティのドキュメントコメントがOpenAPI 仕様に合わせて変更されました。
    既存コードの動作に影響はありません。

## 利用者向け確認ポイント

1. `PatchApplicationBody_components_env`、`PostApplicationBody_components_env` を参照しているコードがないか確認し、`Request.EnvItem` に置き換えてください。
2. `Handler.GetApplication_components_env`、`Handler.GetVersion_components_env` 等、操作ごとの旧レスポンス型を参照しているコードがないか確認し、`Response.EnvItem` に置き換えてください。
3. バリデーションの変更により、コンポーネント名（`Name`）やコンテナレジストリのイメージ名（`Image`）に空文字列を渡しているコードがある場合は、バリデーションエラーになります（最小文字数 1 が追加されました）。
4. `CloudctrlError`、`DefaultError`、`DefaultError_error`、`User`、`User_location_type` を参照しているコードがあれば削除が必要です。
