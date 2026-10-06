# SecretManager API クライアントプロジェクト

## 基本情報
さくらのクラウドが提供する SecretManager API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウド SecretManager を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本 README の内容を読めば、簡単な機密情報（シークレット）の操作まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `SecretManager`
- サービス概要:
  - 機密情報（シークレット）を安全に保存・管理するサービスです。
  - KMS（Key Management Service）の暗号化キーと連携してシークレットを保護します。
  - Vault（保管庫）ごとにシークレットを分離し、バージョン管理を行えます。
- 本 SDK の目的:
  - さくらのクラウド「SecretManager」向けの C# API クライアントライブラリ `Sacloud.Sdk.SecretManager` を提供します。
  - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
  - C# 7.3 以降
  - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
  - ファイルパス: `sdk/SecretManager/openapi-spec/1.0.0/openapi.json`
  - OpenAPI バージョン:`3.0.3`
- SecretManager API バージョン:
  - `info.version`: `1.0.0`
- ベース URL:
  - `https://secure.sakura.ad.jp/cloud/zone/{zone}/api/cloud/1.1`
  - 本 SDK からのすべての API 呼び出しは、上記ベース URL を基点として実行されます。
- 利用可能なゾーン:
  - `is1a` - 石狩第1ゾーン
  - `is1b` - 石狩第2ゾーン
  - `tk1a` - 東京第1ゾーン
  - `tk1b` - 東京第2ゾーン
- 認証方式（概要）:
    - HTTP Basic 認証
        - ユーザー名: API キーの「アクセストークン (UUID)」
        - パスワード: API キーの「アクセストークンシークレット」
        - ヘッダー形式: `Authorization: Basic <base64(user:password)>`
        - OpenAPI の `components.securitySchemes` には定義されていませんが、`info.description` に記載された仕様に基づいて Basic 認証として扱います。
    - Bearer 認証
        - アクセストークン： サービスプリンシパルキーを使用し発行したアクセストークン
        - ヘッダー形式： `Authorization: Bearer <AccessToken>`

## 事前セットアップ

### 認証情報の準備

SecretManager SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### API キーを使用する
認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。

- [API キー | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/api/apikey.html#api)

#### サービスプリンシパルキーを使用して発行したアクセストークンを使用する
認証に サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従い アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件を満たす .NET SDK
  - .NET Standard 2.0 でサポートされているバージョンであること
  - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```bash
dotnet add package Sacloud.Sdk.SecretManager
```

## クイックスタート

ここでは、最小限のコードで SecretManager API にアクセスし、Vault 一覧を取得するまでの流れを示します。
認証には API キー を使用します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

```bash
dotnet new console -n SecretManagerQuickStart
cd SecretManagerQuickStart
dotnet add package Sacloud.Sdk.SecretManager
```

2. `Program.cs` に、以下のコード例を保存します。(C# のトップレベルステートメント構文を使用しています)

```csharp
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.SecretManager;
using Sacloud.Sdk.SecretManager.Models;

string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";

BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(
    accessToken,
    accessTokenSecret);

using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

SecretManagerClient client = new SecretManagerClient(
    adapter,
    SecretManagerClient.SakuraCloudZone.Is1a,
    loggerFactory);

try
{
    PaginatedVaultList vaults = await client.Secretmanager.Vaults.GetAsync();
    Console.WriteLine("Vaults:");

    if (vaults?.Vaults is not null)
    {
        foreach (Vault vault in vaults.Vaults)
        {
            Console.WriteLine($"- {vault.ID}: {vault.Name}");
        }
    }
    else
    {
        Console.WriteLine("No vaults found.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error while calling SecretManager API: {ex.Message}");
}
```

3. 以下のコマンドで実行し、Vault 一覧が取得できることを確認します。

```bash
dotnet run
```

より詳細な認証手順や、Vault / Secret の作成・更新・削除などの操作については、後述の「使用方法」セクションを参照してください。

より多くの操作例や対話的なメニュー形式での実行例については、リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/SecretManager/samples/`

このディレクトリには、実際の SecretManager リソースに対して SDK を介して各種操作を行うコンソールアプリケーションが含まれています。プロジェクトルート（ `Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/SecretManager/samples/SecretManager.Samples.csproj
```

実行前に、`Program.cs` 内の以下のプレースホルダーを適切な値に置き換えてください。

- `<YOUR_API_KEY_ACCESS_TOKEN>`
- `<YOUR_API_KEY_ACCESS_TOKEN_SECRET>`
- `<YOUR_JSON_WEB_TOKEN>`（サービスプリンシパル認証を使用する場合）

## SDK のメソッドと API エンドポイントの対応表

### Vault（シークレット保管庫）関連

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Secretmanager.Vaults.GetAsync` | GET | `/secretmanager/vaults` |
| `Secretmanager.Vaults.PostAsync` | POST | `/secretmanager/vaults` |
| `Secretmanager.Vaults[{resource_id}].GetAsync` | GET | `/secretmanager/vaults/{resource_id}` |
| `Secretmanager.Vaults[{resource_id}].PutAsync` | PUT | `/secretmanager/vaults/{resource_id}` |
| `Secretmanager.Vaults[{resource_id}].DeleteAsync` | DELETE | `/secretmanager/vaults/{resource_id}` |

### Secret（シークレット）関連
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Secretmanager.Vaults[{resource_id}].Secrets.GetAsync` | GET | `/secretmanager/vaults/{resource_id}/secrets` |
| `Secretmanager.Vaults[{resource_id}].Secrets.PostAsync` | POST | `/secretmanager/vaults/{resource_id}/secrets` |
| `Secretmanager.Vaults[{resource_id}].Secrets.DeleteAsync` | DELETE | `/secretmanager/vaults/{resource_id}/secrets` |
| `Secretmanager.Vaults[{resource_id}].Secrets.Unveil.PostAsync` | POST | `/secretmanager/vaults/{resource_id}/secrets/unveil` |

## 使用方法

本 SDK を使用して SecretManager API を呼び出す方法について記載します。

### 認証

#### API キーを使用する場合

1. 「事前セットアップ」で発行した API キーの「アクセストークン (UUID)」と「アクセストークンシークレット」を文字列として用意します。

```csharp
string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";
```

2. Kiota が提供する `BasicAuthenticationProvider` を使用して、Basic 認証用の認証プロバイダを作成します。Base64 エンコードや `Authorization`  ヘッダーの組み立ては内部で自動的に行われます。

```csharp
BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);
```

3. 認証プロバイダを用いて `HttpClientRequestAdapter` のインスタンスを作成します。

```csharp
using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
```

#### サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合

1. 「事前セットアップ」で発行したアクセストークンを文字列として用意します。

    ```csharp
    string accessToken = "<YOUR_ACCESS_TOKEN>";
    ```

2. Kiota が提供する `ApiKeyAuthenticationProvider` を使用して、Bearer 認証用の認証プロバイダを作成します。

    ```csharp
    ApiKeyAuthenticationProvider authProvider = new ApiKeyAuthenticationProvider($"Bearer {accessToken}", "Authorization", ApiKeyAuthenticationProvider.KeyLocation.Header);
    ```

3. 認証プロバイダを用いて、`HttpClientRequestAdapter` のインスタンスを作成します。

    ```csharp
    using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
    ```

#### コードベースのアクセストークン取得

サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合、コードからアクセストークンを取得することが可能です。  
各サービス SDK の共通ライブラリである `Sacloud.Sdk.Shared` プロジェクトにある `Sacloud.Sdk.Shared.Helpers.AccessTokenHelper` クラスを利用することができます。  

```csharp
using Microsoft.Extensions.Logging;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

string jwt = "<YOUR_JWT>";

// ログの利用は任意のため、必要ない場合はILoggerFactoryのインスタンス生成は不要です。
// APIクライアントと同じILoggerFactoryのインスタンスを使用することを推奨します。
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

ServicePrincipalOAuth2AccessToken servicePrincipalOAuth2AccessToken = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory);
string accessToken = servicePrincipalOAuth2AccessToken.AccessToken;
```

JWT の生成方法については、下記ドキュメントを参照してください。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#id14)

なお、取得したアクセストークンには有効期限があります。  
期限切れの場合は `GetAccessTokenAsync` を再度呼び出してアクセストークンを取得し直してください。  
有効期限は `ServicePrincipalOAuth2AccessToken.ExpiresIn`（秒）で確認できます。  

### クライアントのセットアップ

1. 本 SDK で出力するログを受け取るための `ILoggerFactory` インスタンスを作成します。
  ログを受け取る必要がない場合はこの手順をスキップしてください。

```csharp
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    // 必要に応じてログレベルや出力先などの設定を行う
});
```

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、 `SecretManagerClient` のインスタンスを生成します。

```csharp
SecretManagerClient client = new SecretManagerClient(adapter, SecretManagerClient.SakuraCloudZone.Is1a, loggerFactory);
```

ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。

```csharp
SecretManagerClient client = new SecretManagerClient(adapter, SecretManagerClient.SakuraCloudZone.Is1a);
```

利用可能なゾーンは `Is1a` / `Is1b` / `Tk1a` / `Tk1b` です。ゾーン指定なしのコンストラクタを使用した場合は `is1a` が既定値として利用されます。

### ログ出力と運用上の注意

本 SDK は `Microsoft.Extensions.Logging` を利用して、API 呼び出しの開始・終了やエラー情報などをログとして出力します。
運用環境では、適切なログレベルと出力先を設定することで、トラブルシュートや監視に役立てることができます。

```csharp
using Microsoft.Extensions.Logging;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.SetMinimumLevel(LogLevel.Information);
    builder.AddConsole();
});

SecretManagerClient client = new SecretManagerClient(
    adapter,
    SecretManagerClient.SakuraCloudZone.Is1a,
    loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレット、パスワードなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

アプリケーションに対する各種操作の API 呼び出し例を示します。

#### Vault 一覧の取得

```csharp
PaginatedVaultList vaults = await client.Secretmanager.Vaults.GetAsync();
```

#### Vault の作成

```csharp
using Sacloud.Sdk.SecretManager.Models;
using System.Collections.Generic;

WrappedCreateVault body = new WrappedCreateVault
{
    Vault = new CreateVault
    {
        Name = "my-vault",
        Description = "サンプル Vault",
        KmsKeyID = "<YOUR_KMS_KEY_ID>",
        Tags = new List<string> { "sample" }
    }
};

WrappedCreateVault created = await client.Secretmanager.Vaults.PostAsync(body);
```

#### Vault の更新

```csharp
using Sacloud.Sdk.SecretManager.Models;
using System.Collections.Generic;

WrappedVault body = new WrappedVault
{
    Vault = new Vault
    {
        Name = "updated-vault-name",
        Description = "更新された説明",
        Tags = new List<string> { "updated" }
    }
};

WrappedVault updated = await client.Secretmanager.Vaults["<YOUR_VAULT_ID>"].PutAsync(body);
```

#### シークレットの作成

```csharp
using Sacloud.Sdk.SecretManager.Models;

WrappedCreateSecret body = new WrappedCreateSecret
{
    Secret = new CreateSecret
    {
        Name = "database-password",
        Value = "<YOUR_SECRET_VALUE>"
    }
};

WrappedCreateSecret created = await client.Secretmanager.Vaults["<YOUR_VAULT_ID>"].Secrets.PostAsync(body);
```

#### シークレット一覧の取得

```csharp
using Sacloud.Sdk.SecretManager.Models;

PaginatedSecretList secrets = await client.Secretmanager.Vaults["<YOUR_VAULT_ID>"].Secrets.GetAsync();
```

#### シークレットの削除

```csharp
using Sacloud.Sdk.SecretManager.Models;

WrappedDeleteSecret body = new WrappedDeleteSecret
{
    Secret = new DeleteSecret
    {
        Name = "database-password"
    }
};

await client.Secretmanager.Vaults["<YOUR_VAULT_ID>"].Secrets.DeleteAsync(body);
```

#### シークレットの取得（Unveil）

```csharp
using Sacloud.Sdk.SecretManager.Models;

WrappedUnvail body = new WrappedUnvail
{
    Secret = new Unvail
    {
        Name = "database-password",
        Version = 1
    }
};

WrappedUnvail unveiled = await client.Secretmanager.Vaults["<YOUR_VAULT_ID>"].Secrets.Unveil.PostAsync(body);
```

## エラー処理の簡易ガイド

### 例外の扱い

SecretManager API 呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx 応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using System;
using System.Collections.Generic;
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.SecretManager.Models;
using Sacloud.Sdk.Shared.Models;

try
{
    PaginatedVaultList vaults = await client.Secretmanager.Vaults.GetAsync();
}
catch (ApiException apiEx)
{
    Console.WriteLine("API error: " + apiEx.Message);

    if (apiEx is ApiError apiError)
    {
        foreach (KeyValuePair<string, object> kv in apiError.ResponseBody)
        {
            Console.WriteLine($"  {kv.Key}: {kv.Value}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Unexpected error: " + ex.Message);
}
```

### 共通エラー型 (`ApiError`)と詳細情報の確認

SecretManager SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、`ResponseBody` プロパティにエラーレスポンスボディの内容（フィールド名と値のディクショナリ）を保持します。
- SecretManager 固有のエラー形式も、 `ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから `ResponseBody` を参照することで、API ごとの詳細なエラー情報を確認できます。

### 大量データ取得

`Vaults.GetAsync()` / `Secrets.GetAsync()` の一覧結果には、取得結果を確認するためのメタ情報が含まれます。

具体的な仕様や制約値は、SecretManager API リファレンスおよび OpenAPI 定義を参照してください。

## よくある質問

### Q. シークレットのバージョン管理はどのように行われますか？

同じ名前でシークレットを再度作成（POST）すると新しいバージョンとして保存されます。`LatestVersion` で最新バージョンを確認でき、Unveil 時に特定バージョンを指定できます。

### Q. Vault を削除するとシークレットはどうなりますか？
Vault 配下のシークレットも削除されます。必要なシークレットは事前に退避してください。

### Q. Vault 作成時に KMS 鍵は必須ですか？

必須です。`KmsKeyID` に暗号化に利用する KMS 鍵のリソース ID を指定します。

### Q. 利用可能なゾーンはどれですか？

`is1a`、`is1b`、`tk1a`、`tk1b` が利用可能です。

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キーを使用している場合**、次の点を確認してください。

- SecretManager 用の API キーを発行していること
- アクセストークン (UUID) とアクセストークンシークレットを正しくコピーしていること
- `"<ACCESS_TOKEN>:<ACCESS_TOKEN_SECRET>"` の形式で文字列連結し、UTF-8 で Base64 エンコードしていること
- `Authorization` ヘッダーが `Basic <base64>` 形式で送信されていること

**サービスプリンシパルキーを使用して発行したアクセストークンを使用している場合**、次の点を確認してください。

- アクセストークンの有効期限が切れていないこと（`ExpiresIn` または `TokenExpiredAt` で確認）
- `Authorization` ヘッダーが `Bearer <ACCESS_TOKEN>` 形式で送信されていること
- JWT の署名に使用したサービスプリンシパルキーが有効であること

それでも解消しない場合は、`Error` の内容やログを確認し、レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。
