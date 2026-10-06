# KMS API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する KMS (Key Management Service) API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウド KMS を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。本 README の内容を読めば、簡単なアプリケーション操作まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `KMS` (Key Management Service)
- サービス概要:
  - 暗号鍵の作成・管理・暗号化・復号を行うためのサービスです。
  - 鍵ローテーションやステータス変更、削除予約などの運用操作に対応します。
- 本 SDK の目的:
  - さくらのクラウド「KMS」向けの C# API クライアントライブラリ `Sacloud.Sdk.KMS` を提供します。
  - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
  - C# 7.3 以降
  - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
  - ファイルパス: `sdk/KMS/openapi-spec/1.0.0/openapi.json`
- KMSのAPIバージョン:
  - `info.version`:`1.0.0`
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
  - Bearer 認証
    - アクセストークン: サービスプリンシパルキーを使用して発行したアクセストークン
    - ヘッダー形式: `Authorization: Bearer <accessToken>`

## 事前セットアップ

### 認証情報の準備

KMS SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### 1-A. API キーを使用する
認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。

- [API キー | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/api/apikey.html#api)

#### 1-B. サービスプリンシパルキーを使用して発行したアクセストークンを使用する
認証に サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従い アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件を満たす .NET SDK
  - .NET Standard 2.0 でサポートされているバージョンであること
  - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```sh
dotnet add package Sacloud.Sdk.KMS
```

## クイックスタート

ここでは最小限のコードで KMS API にアクセスし、キー一覧を取得するまでの流れを示します。
認証には API キー を使用します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

```sh
dotnet new console -n KmsQuickStart
cd KmsQuickStart
dotnet add package Sacloud.Sdk.KMS
```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

```csharp
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.KMS;
using Sacloud.Sdk.KMS.Models;

// 1. 認証情報を設定
string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";

BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(
   accessToken,
   accessTokenSecret);

using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);

// 2. ロガーとクライアントを初期化
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

KMSClient client = new KMSClient(adapter, KMSClient.SakuraCloudZone.Is1a, loggerFactory);

try
{
    // 3. キー一覧を取得
    PaginatedKeyList keys = await client.Kms.Keys.GetAsync();
    
    Console.WriteLine("Keys:");
    if (keys?.Keys is not null)
    {
        foreach (Key key in keys.Keys)
        {
            Console.WriteLine($"- {key.ID}: {key.Name} ({key.Status})");
        }
    }
    else
    {
            Console.WriteLine("No keys found.");
    }
}
catch (Exception ex)
{
     // 簡易的なエラー処理
     Console.WriteLine($"Error while calling KMS API: {ex.Message}");
}
```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

```sh
dotnet run
```
より詳細な認証手順や、 KMS キーの作成・更新・削除などの操作については、 後述の「使用方法」セクションを参照してください。
より多くの操作例や対話的なメニュー形式での実行例については、 リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/KMS/samples/`

このディレクトリには、実際の KMS リソースに対して SDK を介して各種操作を行うコンソールアプリケーションが含まれています。 プロジェクトルート（Sacloud.Sdk.slnx があるディレクトリ）で次のコマンドを実行すると、 サンプルをビルド・実行できます。

```sh
dotnet run --project sdk/KMS/samples/KMS.Samples.csproj
```

実行前に、`Program.cs` 内の `<YOUR_API_KEY_ACCESS_TOKEN>` と `<YOUR_API_KEY_ACCESS_TOKEN_SECRET>` を適切な値に置き換えてください。

## SDK のメソッドと API エンドポイントの対応表

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Kms.Keys.GetAsync` | GET | `/kms/keys` |
| `Kms.Keys.PostAsync` | POST | `/kms/keys` |
| `Kms.Keys[{resource_id}].GetAsync` | GET | `/kms/keys/{resource_id}` |
| `Kms.Keys[{resource_id}].PutAsync` | PUT | `/kms/keys/{resource_id}` |
| `Kms.Keys[{resource_id}].DeleteAsync` | DELETE | `/kms/keys/{resource_id}` |
| `Kms.Keys[{resource_id}].Encrypt.PostAsync` | POST | `/kms/keys/{resource_id}/encrypt` |
| `Kms.Keys[{resource_id}].Decrypt.PostAsync` | POST | `/kms/keys/{resource_id}/decrypt` |
| `Kms.Keys[{resource_id}].Rotate.PostAsync` | POST | `/kms/keys/{resource_id}/rotate` |
| `Kms.Keys[{resource_id}].Status.PostAsync` | POST | `/kms/keys/{resource_id}/status` |
| `Kms.Keys[{resource_id}].ScheduleDestruction.PostAsync` | POST | `/kms/keys/{resource_id}/schedule-destruction` |

## 使用方法
本SDKを使用してKMSのAPIを呼び出す方法について記載します。

### 認証

#### API キーを使用する場合

1. 「事前セットアップ」で発行した API キーの「アクセストークン (UUID)」と「アクセストークンシークレット」を文字列として用意します。

```csharp
string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";
```

2. Kiota が提供する `BasicAuthenticationProvider` を使用して、Basic 認証用の認証プロバイダを作成します。 Base64 エンコードや `Authorization` ヘッダーの組み立ては内部で自動的に行われます。

```csharp
BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);
```

3. 認証プロバイダを用いて、 `HttpClientRequestAdapter` のインスタンスを作成します。

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

3. 認証プロバイダを用いて、 `HttpClientRequestAdapter` のインスタンスを作成します。

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
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
    // 必要に応じてログレベルや出力先などの設定を行う
});
```

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、 `KMSClient` のインスタンスを作成します。利用するゾーンを `SakuraCloudZone` で明示指定してください。

```csharp
KMSClient client = new KMSClient(adapter, KMSClient.SakuraCloudZone.Is1a, loggerFactory);
```
ログを受け取る必要がない場合は、`loggerFactory` 引数を省略してインスタンスを作成できます。
```csharp
KMSClient client = new KMSClient(adapter, KMSClient.SakuraCloudZone.Is1a);
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

KMSClient client = new KMSClient(adapter, KMSClient.SakuraCloudZone.Is1a, loggerFactory);

```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレット、パスワードなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

## 使用例

KMS キーに対する各種操作の API 呼び出し例を示します。

### キー一覧取得

```csharp
PaginatedKeyList keys = await client.Kms.Keys.GetAsync();
```

### キー作成

```csharp
using Sacloud.Sdk.KMS.Models;
using System.Collections.Generic;

WrappedCreateKey body = new WrappedCreateKey
{
    Key = new CreateKey
    {
        Name = "<YOUR_KEY_NAME>",
        Description = "KMS sample key",
        Tags = new List<string> { "sample", "kms" }
    }
};

WrappedCreateKey created = await client.Kms.Keys.PostAsync(body);
```

### キー詳細取得

```csharp
WrappedKey detail = await client.Kms.Keys["<YOUR_KEY_ID>"].GetAsync();
```

### キー暗号化

```csharp
using Sacloud.Sdk.KMS.Models;
using System;
using System.Text;

string plainText = "Hello KMS";
string plainBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

WrappedKeyPlain encryptBody = new WrappedKeyPlain
{
    Key = new KeyPlain
    {
        Algo = KeyEncryptAlgoEnum.Aes256Gcm,
        Plain = plainBase64
    }
};

WrappedKeyCipher encrypted = await client.Kms.Keys["<YOUR_KEY_ID>"].Encrypt.PostAsync(encryptBody);
```

### キー復号

```csharp
using Sacloud.Sdk.KMS.Models;
using System;
using System.Text;

WrappedKeyCipher decryptBody = new WrappedKeyCipher
{
    Key = new KeyCipher
    {
        Cipher = "<BASE64_CIPHER_TEXT>"
    }
};

WrappedKeyPlain decrypted = await client.Kms.Keys["<YOUR_KEY_ID>"].Decrypt.PostAsync(decryptBody);

string decoded = string.Empty;
if (decrypted != null && decrypted.Key != null && !string.IsNullOrWhiteSpace(decrypted.Key.Plain))
{
    decoded = Encoding.UTF8.GetString(Convert.FromBase64String(decrypted.Key.Plain));
}
```

## エラー処理とページングの簡易ガイド

KMS API 呼び出しで HTTP エラーやバリデーションエラーが発生した場合、 Kiota によって`Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。 例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using System;
using System.Collections.Generic;
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.KMS.Models;
using Sacloud.Sdk.Shared.Models;

try
{
    PaginatedKeyList keys = await client.Kms.Keys.GetAsync();
    // 取得した結果を処理
}
catch (ApiException apiEx)
{
    Console.WriteLine("API error: " + apiEx.Message);

    // 共通エラー型(ApiError)として返却されている場合は、レスポンスボディの詳細を参照できます。
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

### 共通エラー型(ApiError)と詳細情報の確認

KMS SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、
  `ResponseBody` プロパティにエラーレスポンスボディの内容 (フィールド名と値のディクショナリ) を保持します。
- KMS 固有のエラー形式も、`ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから
`ResponseBody` を参照することで、API ごとの詳細なエラー情報を確認できます。

### ページングと大量データ取得

`/kms/keys` の一覧取得結果には、`Count` / `From` / `Total` が含まれます。
大量件数を扱う場合は、これらの値を確認しながら取得状況を管理することを推奨します。

具体的なパラメータ名や制約値は、KMS API リファレンスおよび OpenAPI 定義を参照してください。

## よくある質問

### Q. `401 Unauthorized` / `403 Forbidden` が返ります

**API キーを使用している場合**、次の点を確認してください。

- KMS API 利用権限のある API キーを使用していること
- アクセストークン（UUID）とアクセストークンシークレットを正しくコピーしていること
- `"<ACCESS_TOKEN>:<ACCESS_TOKEN_SECRET>"` 形式で文字列連結し、UTF-8 で Base64 エンコードしていること
- `Authorization` ヘッダーが `Basic <base64>` 形式で送信されていること

**サービスプリンシパルキーを使用して発行したアクセストークンを使用している場合**、次の点を確認してください。

- アクセストークンの有効期限が切れていないこと（`ExpiresIn` または `TokenExpiredAt` で確認）
- `Authorization` ヘッダーが `Bearer <ACCESS_TOKEN>` 形式で送信されていること
- JWT の署名に使用したサービスプリンシパルキーが有効であること


それでも解消しない場合は、`ApiError` の内容やログを確認し、 レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。

### Q. タイムアウトや接続エラーが発生します
まず、開発環境から `https://secure.sakura.ad.jp` への通信経路（プロキシ設定やファイアウォール）に問題がないか確認してください。 ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、 `HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか？

本 SDK は .NET Standard 2.0 対応のライブラリです。そのため、 .NET 6 以降を含む、 .NET Standard 2.0 互換ランタイムで利用できます。

### Q. 他のサービス(AppRun専有型やEventBusなど)でも同じように使えますか？
本 README は KMS `(Sacloud.Sdk.KMS)` 向けの説明です。 同一リポジトリ内には AppRun専有型 や EventBus、Workflows など他サービス向けの SDK も含まれていますが、 それぞれ `sdk/<ServiceName>/` 配下に独自の README とサンプルが用意される想定です。 基本的な考え方（.NET プロジェクトへのパッケージ追加、認証、クライアント初期化、ログ設定など）は共通ですが、 サービスごとに利用する API やユースケースが異なるため、対象サービスの README も併せて参照してください。

KMS SDK を利用するだけであれば、通常は本パッケージ `Sacloud.Sdk.KMS` を参照すれば十分であり、 共通ライブラリプロジェクト (sdk/Shared) を意識する必要はありません。

### Q. ゾーンはどう選べばいいですか？

`KMSClient` コンストラクタの `SakuraCloudZone` で選択できます。

- `Is1a`
- `Is1b`
- `Tk1a`
- `Tk1b`

利用するリソース配置に合わせて選択してください。

