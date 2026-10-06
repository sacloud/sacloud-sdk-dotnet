# CloudHSM API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する CloudHSM API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの CloudHSM を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本 README の内容を読めば、簡単な CloudHSM 一覧を取得する操作まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `CloudHSM`（CloudHSM）
- サービス概要:
  - 専用のハードウェアセキュリティモジュール（HSM）を利用し、暗号鍵の生成・保管や暗号処理をセキュアに行うためのサービスです。
  - さくらのクラウド環境と連携し、高いセキュリティ基準やコンプライアンス要件が求められるシステム向けの鍵管理環境を提供します。
- 本 SDK の目的:
   - さくらのクラウド「CloudHSM」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.CloudHSM` を提供します。
   - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、
	 通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
   - C# 7.3 以降
   - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
   - ファイルパス: `sdk/CloudHSM/openapi-spec/1.0.0/openapi.yaml`
   - OpenAPI バージョン: 3.0.3
- CloudHSM API バージョン:
   - `info.version`: `1.0.0`
- ベース URL:
   - `https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1`
   - 本 SDK からのすべての API 呼び出しは、上記ベース URL を基点として実行されます。
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

CloudHSM SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### 1-A. API キーを使用する

認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。

- [APIキー | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/api/apikey.html)

#### 1-B. サービスプリンシパルキーを使用して発行したアクセストークンを使用する

認証にサービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従いアクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
   - .NET Standard 2.0 でサポートされているバージョンであること
   - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```bash
dotnet add package Sacloud.Sdk.CloudHSM
```

## クイックスタート

ここでは、最小限のコードで CloudHSM API にアクセスし、CloudHSM 一覧を取得するまでの流れを示します。
認証には API キー を使用します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

   ```bash
   dotnet new console -n CloudHSMQuickStart
   cd CloudHSMQuickStart
   dotnet add package Sacloud.Sdk.CloudHSM
   ```

2. `Program.cs` に、以下のコード例を保存します。

   ```csharp
   using Microsoft.Extensions.Logging;
   using Microsoft.Kiota.Abstractions.Authentication;
   using Microsoft.Kiota.Http.HttpClientLibrary;
   using Sacloud.Sdk.CloudHSM;
   using Sacloud.Sdk.CloudHSM.Models;

   // 1. 認証情報を定義
   string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
   string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";
   BasicAuthenticationProvider authProvider =
	   new BasicAuthenticationProvider(
		   accessToken,
		   accessTokenSecret
	   );

   // 2. ロガーとクライアントを初期化
   ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
   {
	   builder.AddConsole();
   });
   HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
   CloudHSMClient client = new CloudHSMClient(adapter, loggerFactory);

   try
   {
	   // 3. CloudHSM 一覧を取得
	   PaginatedCloudHSMList cloudHSMs = await client.Cloudhsm.Cloudhsms.GetAsync();
	   if (cloudHSMs?.CloudHSMs != null)
	   {
		   foreach (CloudHSM cloudHSM in cloudHSMs.CloudHSMs)
		   {
			   Console.WriteLine($"- {cloudHSM.ID}: {cloudHSM.Name}");
		   }
	   }
   }
   catch (Exception ex)
   {
	   // 簡易的なエラーログ
	   Console.WriteLine($"Error while calling CloudHSM API: {ex.Message}");
   }
   ```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

   ```bash
   dotnet run
   ```

より詳細な認証手順や、リソースの追加・更新・削除などの操作については、後述の「使用方法」セクションを参照してください。

より多くの操作例や対話的なメニュー形式での実行例については、リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/CloudHSM/samples/`

このディレクトリには、実際の CloudHSM リソースに対して
SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、
サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/CloudHSM/samples/CloudHSM.Samples.csproj
```

実行前に、`Program.cs` 内の以下のプレースホルダーを適切な値に置き換えてください。

- `<YOUR_API_KEY_ACCESS_TOKEN>`
- `<YOUR_API_KEY_ACCESS_TOKEN_SECRET>`
- `<YOUR_JSON_WEB_TOKEN>`

## SDK のメソッドと API エンドポイントの対応表

### cloudhsm

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Cloudhsm.Cloudhsms.GetAsync` | GET | `/cloudhsm/cloudhsms` |
| `Cloudhsm.Cloudhsms.PostAsync` | POST | `/cloudhsm/cloudhsms` |
| `Cloudhsm.Cloudhsms[{cloudhsm_resource_id}].Clients.GetAsync` | GET | `/cloudhsm/cloudhsms/{cloudhsm_resource_id}/clients` |
| `Cloudhsm.Cloudhsms[{cloudhsm_resource_id}].Clients.PostAsync` | POST | `/cloudhsm/cloudhsms/{cloudhsm_resource_id}/clients` |
| `Cloudhsm.Cloudhsms[{cloudhsm_resource_id}].Clients[{id}].GetAsync` | GET | `/cloudhsm/cloudhsms/{cloudhsm_resource_id}/clients/{id}` |
| `Cloudhsm.Cloudhsms[{cloudhsm_resource_id}].Clients[{id}].PutAsync` | PUT | `/cloudhsm/cloudhsms/{cloudhsm_resource_id}/clients/{id}` |
| `Cloudhsm.Cloudhsms[{cloudhsm_resource_id}].Clients[{id}].DeleteAsync` | DELETE | `/cloudhsm/cloudhsms/{cloudhsm_resource_id}/clients/{id}` |
| `Cloudhsm.Cloudhsms[{resource_id}].GetAsync` | GET | `/cloudhsm/cloudhsms/{resource_id}` |
| `Cloudhsm.Cloudhsms[{resource_id}].PutAsync` | PUT | `/cloudhsm/cloudhsms/{resource_id}` |
| `Cloudhsm.Cloudhsms[{resource_id}].DeleteAsync` | DELETE | `/cloudhsm/cloudhsms/{resource_id}` |
| `Cloudhsm.Cloudhsms[{resource_id}].Peers.GetAsync` | GET | `/cloudhsm/cloudhsms/{resource_id}/peers` |
| `Cloudhsm.Cloudhsms[{resource_id}].Peers.PostAsync` | POST | `/cloudhsm/cloudhsms/{resource_id}/peers` |
| `Cloudhsm.Cloudhsms[{resource_id}].Peers[{peer_id}].DeleteAsync` | DELETE | `/cloudhsm/cloudhsms/{resource_id}/peers/{peer_id}` |
| `Cloudhsm.Licenses.GetAsync` | GET | `/cloudhsm/licenses` |
| `Cloudhsm.Licenses.PostAsync` | POST | `/cloudhsm/licenses` |
| `Cloudhsm.Licenses[{resource_id}].GetAsync` | GET | `/cloudhsm/licenses/{resource_id}` |
| `Cloudhsm.Licenses[{resource_id}].PutAsync` | PUT | `/cloudhsm/licenses/{resource_id}` |
| `Cloudhsm.Licenses[{resource_id}].DeleteAsync` | DELETE | `/cloudhsm/licenses/{resource_id}` |

## 使用方法

本 SDK を使用して CloudHSM API を呼び出す方法について記載します。

### 認証

#### API キーを使用する場合

1. 「事前セットアップ」で発行した API キーの「アクセストークン (UUID)」と「アクセストークンシークレット」を文字列として用意します。

   ```csharp
   using Microsoft.Kiota.Abstractions.Authentication;
   using Microsoft.Kiota.Http.HttpClientLibrary;

   string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
   string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";
   ```

2. Kiota が提供する `BasicAuthenticationProvider` を使用して、Basic 認証用の認証プロバイダを作成します。Base64 エンコードや `Authorization` ヘッダーの組み立ては内部で自動的に行われます。

   ```csharp
   BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);
   ```

3. 認証プロバイダを用いて、`HttpClientRequestAdapter` のインスタンスを作成します。

   ```csharp
   HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
   ```

#### サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合

1. 「事前セットアップ」で発行したアクセストークンを文字列として用意します。

   ```csharp
   using Microsoft.Kiota.Abstractions.Authentication;
   using Microsoft.Kiota.Http.HttpClientLibrary;

   string accessToken = "<YOUR_ACCESS_TOKEN>";
   ```

2. Kiota が提供する `ApiKeyAuthenticationProvider` を使用して、Bearer 認証用の認証プロバイダを作成します。

   ```csharp
   ApiKeyAuthenticationProvider authProvider =
	   new ApiKeyAuthenticationProvider(
		   $"Bearer {accessToken}",
		   "Authorization",
		   ApiKeyAuthenticationProvider.KeyLocation.Header
	   );
   ```

3. 認証プロバイダを用いて、`HttpClientRequestAdapter` のインスタンスを作成します。

   ```csharp
   HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
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
ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
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
   using Microsoft.Extensions.Logging;
   using Sacloud.Sdk.CloudHSM;

   ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
   {
	  // 必要に応じてログレベルや出力先などの設定を行う
   });
   ```

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`CloudHSMClient` のインスタンスを作成します。

   ```csharp
   CloudHSMClient client = new CloudHSMClient(adapter, loggerFactory);
   ```

   ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。

   ```csharp
   CloudHSMClient client = new CloudHSMClient(adapter);
   ```

### ログ出力と運用上の注意

本 SDK は `Microsoft.Extensions.Logging` を利用して、API 呼び出しの開始・終了やエラー情報などをログとして出力します。
運用環境では、適切なログレベルと出力先を設定することで、トラブルシュートや監視に役立てることができます。

```csharp
using Microsoft.Extensions.Logging;
using Sacloud.Sdk.CloudHSM;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
   builder.SetMinimumLevel(LogLevel.Information);
   builder.AddConsole();
});

CloudHSMClient client = new CloudHSMClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレット、パスワードなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

CloudHSM 一覧を取得する に対する各種操作の API 呼び出し例を示します。

#### CloudHSM の一覧を取得する

```csharp
using Sacloud.Sdk.CloudHSM;
using Sacloud.Sdk.CloudHSM.Models;

PaginatedCloudHSMList cloudHSMs = await client.Cloudhsm.Cloudhsms.GetAsync();
if (cloudHSMs?.CloudHSMs != null)
{
   foreach (CloudHSM cloudHSM in cloudHSMs.CloudHSMs)
   {
	  Console.WriteLine($"- {cloudHSM.ID}: {cloudHSM.Name}");
   }
}
```

#### CloudHSM を作成する

```csharp
using Sacloud.Sdk.CloudHSM;
using Sacloud.Sdk.CloudHSM.Models;

WrappedCreateCloudHSM requestBody = new WrappedCreateCloudHSM
{
   CloudHSM = new CreateCloudHSM
   {
	  Name = "example-cloudhsm",
	  IPv4NetworkAddress = "192.0.2.0",
	  IPv4PrefixLength = 24
   }
};

WrappedCreateCloudHSM result = await client.Cloudhsm.Cloudhsms.PostAsync(requestBody);
Console.WriteLine($"- {result?.CloudHSM?.ID}: {result?.CloudHSM?.Name}");
```

## エラー処理の簡易ガイド

### 例外の扱い

CloudHSM API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.CloudHSM.Models;

try
{
   PaginatedCloudHSMList result = await client.Cloudhsm.Cloudhsms.GetAsync();
   // 取得した結果を処理
}
catch (ApiException apiEx)
{
   Console.WriteLine($"API error: {apiEx.Message}");

   // 共通エラー型(ApiError)として返却されている場合は、レスポンスボディの詳細を参照できます。
   ApiError apiError = apiEx as ApiError;
   if (apiError != null)
   {
	  foreach (KeyValuePair<string, object> kv in apiError.ResponseBody)
	  {
		 Console.WriteLine($"  {kv.Key}: {kv.Value}");
	  }
   }
}
catch (Exception ex)
{
   Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

### 共通エラー型(ApiError)と詳細情報の確認

CloudHSM SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型
`Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、
   `ResponseBody` プロパティにエラーレスポンスボディの内容 (フィールド名と値のディクショナリ) を保持します。
- CloudHSM 固有のエラー形式も、この `ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから
`ResponseBody` を参照することで、APIごとの詳細なエラー情報を確認できます。

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キーを使用している場合**、次の点を確認してください。

- API キーに CloudHSM を操作するための適切な権限があること
- アクセストークン (UUID) とアクセストークンシークレットを正しくコピーしていること
- `"<ACCESS_TOKEN>:<ACCESS_TOKEN_SECRET>"` の形式で文字列連結し、UTF-8 で Base64 エンコードしていること
- `Authorization` ヘッダーが `Basic <base64>` 形式で送信されていること

**サービスプリンシパルキーを使用して発行したアクセストークンを使用している場合**、次の点を確認してください。

- アクセストークンの有効期限が切れていないこと（`ExpiresIn` または `TokenExpiredAt` で確認）
- `Authorization` ヘッダーが `Bearer <ACCESS_TOKEN>` 形式で送信されていること
- JWT の署名に使用したサービスプリンシパルキーが有効であること

それでも解消しない場合は、`ApiError` の内容やログを確認し、
レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。

### Q. タイムアウトや接続エラーが発生します

まず、開発環境から `https://secure.sakura.ad.jp` への通信経路（プロキシ設定やファイアウォール）に問題がないか確認してください。
ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、
`HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか？

本 SDK は .NET Standard 2.0 対応のライブラリです。
そのため、.NET 6 以降を含む .NET Standard 2.0 互換のランタイムで利用できます。
