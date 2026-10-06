# API ゲートウェイ API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する API ゲートウェイ API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの API ゲートウェイを .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本 README の内容を読めば、クライアント初期化から主要リソースの操作イメージまで一通り把握できる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `API Gateway`（さくらのクラウド API ゲートウェイ）
- サービス概要:
    - APIゲートウェイは、APIの公開、モニタリングを簡単に行えるフルマネージドサービスです。
    - APIのデプロイや管理を簡略化し、開発効率を向上します。
- 本 SDK の目的:
    - さくらのクラウド「API ゲートウェイ」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.APIGateway` を提供します。
    - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
    - C# 7.3 以降
    - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
    - ファイルパス: `sdk/APIGateway/openapi-spec/2.0.1/apigw-api.yaml`
    - OpenAPI バージョン: 3.0.3
- API Gateway API バージョン:
    - `info.version`: `2.0.1`
- ベース URL:
    - `https://secure.sakura.ad.jp/cloud/api/apigw/1.0`
    - 本 SDK からのすべての API 呼び出しは、上記ベース URL を基点として実行されます。
- 認証方式（概要）:
    - HTTP Basic 認証
        - ユーザー名: API キーの「アクセストークン (UUID)」
        - パスワード: API キーの「アクセストークンシークレット」
        - ヘッダー形式: `Authorization: Basic <base64(user:password)>`
    - Bearer認証
        - アクセストークン： サービスプリンシパルキーを使用し発行したアクセストークン
        - ヘッダー形式： `Authorization: Bearer <AccessToken>`

## 事前セットアップ

### 認証情報の準備

API ゲートウェイ SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### 1-A. API キーを使用する
認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。

- [API Gateway API - さくらのクラウド API ポータル](https://manual.sakura.ad.jp/api/cloud/portal/?api=apigw-api)

#### 1-B. サービスプリンシパルキーを使用して発行したアクセストークンを使用する
認証に サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従い アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
    - .NET Standard 2.0 でサポートされているバージョンであること
    - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```bash
dotnet add package Sacloud.Sdk.APIGateway
```

## クイックスタート

ここでは、最小限のコードで API Gateway API にアクセスし、サービスの一覧を取得するまでの流れを示します。
認証には API キー を使用します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

    ```bash
    dotnet new console -n APIGatewayQuickStart
    cd APIGatewayQuickStart
    dotnet add package Sacloud.Sdk.APIGateway
    dotnet add package Microsoft.Extensions.Logging.Console
    ```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

    ```csharp
    using Microsoft.Extensions.Logging;
    using Microsoft.Kiota.Abstractions.Authentication;
    using Microsoft.Kiota.Http.HttpClientLibrary;
    using Sacloud.Sdk.APIGateway.Services;
    using Sacloud.Sdk.APIGateway.Models;
    using Sacloud.Sdk.APIGateway;

    // 1. 認証情報を設定
    string accessToken = "<YOUR_ACCESS_TOKEN>";
    string accessTokenSecret = "<YOUR_ACCESS_TOKEN_SECRET>";

    BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(
        accessToken,
        accessTokenSecret);

    using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);

    // 2. ロガーとクライアントを初期化
    using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    APIGatewayClient client = new APIGatewayClient(adapter, loggerFactory);

    try
    {
        // 3. サービスの一覧を取得
        ServicesGetResponse services = await client.Services.GetAsync();

        Console.WriteLine("Services:");
        if (services.Apigw.Services is not null)
        {
            foreach (ServiceDetailResponse service in services.Apigw.Services)
            {
                Console.WriteLine($"- {service.Id}: {service.Name}");
            }
        }
        else
        {
            Console.WriteLine("No services found.");
        }
    }
    catch (Exception ex)
    {
        // 簡易的なエラー処理
        Console.WriteLine($"Error while calling API Gateway API: {ex.Message}");
    }
    ```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

    ```bash
    dotnet run
    ```

より多くの操作については、後述の「使用方法」セクションを参照してください。
より多くの操作例や対話的なメニュー形式での実行例については、リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/APIGateway/samples/`

このディレクトリには、実際の API ゲートウェイ リソースに対して SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/APIGateway/samples/APIGateway.Samples.csproj
```

実行前に、`Program.cs` 内の `YOUR_ACCESS_TOKEN` および `YOUR_ACCESS_TOKEN_SECRET` を実際の値に置き換えてください。

## SDK のメソッドと API エンドポイントの対応表

### サービス

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Services.GetAsync` | GET | `/services` |
| `Services.PostAsync` | POST | `/services` |
| `Services[{service_id}].GetAsync` | GET | `/services/{service_id}` |
| `Services[{service_id}].PutAsync` | PUT | `/services/{service_id}` |
| `Services[{service_id}].DeleteAsync` | DELETE | `/services/{service_id}` |
| `Services[{service_id}].Routes.GetAsync` | GET | `/services/{service_id}/routes` |
| `Services[{service_id}].Routes.PostAsync` | POST | `/services/{service_id}/routes` |
| `Services[{service_id}].Routes[{route_id}].GetAsync` | GET | `/services/{service_id}/routes/{route_id}` |
| `Services[{service_id}].Routes[{route_id}].PutAsync` | PUT | `/services/{service_id}/routes/{route_id}` |
| `Services[{service_id}].Routes[{route_id}].DeleteAsync` | DELETE | `/services/{service_id}/routes/{route_id}` |
| `Services[{service_id}].Routes[{route_id}].Authorization.GetAsync` | GET | `/services/{service_id}/routes/{route_id}/authorization` |
| `Services[{service_id}].Routes[{route_id}].Authorization.PutAsync` | PUT | `/services/{service_id}/routes/{route_id}/authorization` |
| `Services[{service_id}].Routes[{route_id}].Request.GetAsync` | GET | `/services/{service_id}/routes/{route_id}/request` |
| `Services[{service_id}].Routes[{route_id}].Request.PutAsync` | PUT | `/services/{service_id}/routes/{route_id}/request` |
| `Services[{service_id}].Routes[{route_id}].Response.GetAsync` | GET | `/services/{service_id}/routes/{route_id}/response` |
| `Services[{service_id}].Routes[{route_id}].Response.PutAsync` | PUT | `/services/{service_id}/routes/{route_id}/response` |

### ユーザー

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Users.GetAsync` | GET | `/users` |
| `Users.PostAsync` | POST | `/users` |
| `Users[{user_id}].GetAsync` | GET | `/users/{user_id}` |
| `Users[{user_id}].PutAsync` | PUT | `/users/{user_id}` |
| `Users[{user_id}].DeleteAsync` | DELETE | `/users/{user_id}` |
| `Users[{user_id}].Authentication.GetAsync` | GET | `/users/{user_id}/authentication` |
| `Users[{user_id}].Authentication.PutAsync` | PUT | `/users/{user_id}/authentication` |
| `Users[{user_id}].Groups.GetAsync` | GET | `/users/{user_id}/groups` |
| `Users[{user_id}].Groups.PutAsync` | PUT | `/users/{user_id}/groups` |

### グループ

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Groups.GetAsync` | GET | `/groups` |
| `Groups.PostAsync` | POST | `/groups` |
| `Groups[{group_id}].GetAsync` | GET | `/groups/{group_id}` |
| `Groups[{group_id}].PutAsync` | PUT | `/groups/{group_id}` |
| `Groups[{group_id}].DeleteAsync` | DELETE | `/groups/{group_id}` |

### ドメイン

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Domains.GetAsync` | GET | `/domains` |
| `Domains.PostAsync` | POST | `/domains` |
| `Domains[{domain_id}].PutAsync` | PUT | `/domains/{domain_id}` |
| `Domains[{domain_id}].DeleteAsync` | DELETE | `/domains/{domain_id}` |

### 証明書

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Certificates.GetAsync` | GET | `/certificates` |
| `Certificates.PostAsync` | POST | `/certificates` |
| `Certificates[{certificate_id}].PutAsync` | PUT | `/certificates/{certificate_id}` |
| `Certificates[{certificate_id}].DeleteAsync` | DELETE | `/certificates/{certificate_id}` |

### プラン

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Plans.GetAsync` | GET | `/plans` |

### サブスクリプション

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Subscriptions.GetAsync` | GET | `/subscriptions` |
| `Subscriptions.PostAsync` | POST | `/subscriptions` |
| `Subscriptions[{subscription_id}].GetAsync` | GET | `/subscriptions/{subscription_id}` |
| `Subscriptions[{subscription_id}].PutAsync` | PUT | `/subscriptions/{subscription_id}` |
| `Subscriptions[{subscription_id}].DeleteAsync` | DELETE | `/subscriptions/{subscription_id}` |

### OIDC認証設定

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Oidc.GetAsync` | GET | `/oidc` |
| `Oidc.PostAsync` | POST | `/oidc` |
| `Oidc[{oidc_id}].GetAsync` | GET | `/oidc/{oidc_id}` |
| `Oidc[{oidc_id}].PutAsync` | PUT | `/oidc/{oidc_id}` |
| `Oidc[{oidc_id}].DeleteAsync` | DELETE | `/oidc/{oidc_id}` |

## 使用方法

本 SDK を使用して API Gateway API を呼び出す方法について記載します。

### 認証

#### API キーを使用する場合

1. 「事前セットアップ」で発行した API キーの「アクセストークン (UUID)」と「アクセストークンシークレット」を文字列として用意します。

    ```csharp
    string accessToken = "<YOUR_ACCESS_TOKEN>";
    string accessTokenSecret = "<YOUR_ACCESS_TOKEN_SECRET>";
    ```

2. Kiota が提供する `BasicAuthenticationProvider` を使用して、Basic 認証用の認証プロバイダを作成します。Base64 エンコードや `Authorization` ヘッダーの組み立ては内部で自動的に行われます。

    ```csharp
    BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);
    ```

3. 認証プロバイダを用いて、`HttpClientRequestAdapter` のインスタンスを作成します。

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

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`APIGatewayClient` のインスタンスを作成します。

    ```csharp
    APIGatewayClient client = new APIGatewayClient(adapter, loggerFactory);
    ```

    ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。

    ```csharp
    APIGatewayClient client = new APIGatewayClient(adapter);
    ```

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

APIGatewayClient client = new APIGatewayClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレット、証明書秘密鍵、レジストリパスワードなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

サービスに対する各種操作の API 呼び出し例を示します。

#### サービスの一覧取得

```csharp
ServicesGetResponse services = await client.Services.GetAsync();
```

#### サービスの詳細取得

```csharp
WithServiceGetResponse service = await client.Services[new Guid("<YOUR_SERVICE_ID>")].GetAsync();
```

#### サービスの作成

```csharp
ServicesPostResponse createdService = await client.Services.PostAsync(new ServiceDetailRequest
{
    Name = "<YOUR_SERVICE_NAME>",
    Protocol = Service_protocol.Https,
    Host = "<YOUR_UPSTREAM_HOST>",
    Subscription = new ServiceSubscriptionRequest
    {
        Id = new Guid("<YOUR_SUBSCRIPTION_ID>")
    }
});
```

#### サービスの詳細情報更新

```csharp
await client.Services[new Guid("<YOUR_SERVICE_ID>")].PutAsync(new ServiceDetail
{
    Name = "<YOUR_NEW_SERVICE_NAME>",
    Protocol = Service_protocol.Https,
    Host = "<YOUR_UPSTREAM_HOST>",
    Tags = new List<string> { "<YOUR_TAG_1>", "<YOUR_TAG_2>" }
});
```

#### サービスの削除

```csharp
await client.Services[new Guid("<YOUR_SERVICE_ID>")].DeleteAsync();
```

## エラー処理の簡易ガイド

### 例外の扱い

API Gateway API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;

try
{
    ServicesGetResponse services = await client.Services.GetAsync();
    // 取得した結果を処理
}
catch (ApiError apiError)
{
    Console.WriteLine($"API error: {apiError.Message}");
    foreach (var keyValue in apiError.ResponseBody)
    {
        Console.WriteLine($"  {keyValue.Key}: {keyValue.Value}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キーを使用している場合**、次の点を確認してください。

- API ゲートウェイ用の API キーを発行していること
- アクセストークン (UUID) とアクセストークンシークレットを正しくコピーしていること
- `"<ACCESS_TOKEN>:<ACCESS_TOKEN_SECRET>"` の形式で文字列連結し、UTF-8 で Base64 エンコードしていること
- `Authorization` ヘッダーが `Basic <base64>` 形式で送信されていること

**サービスプリンシパルキーを使用して発行したアクセストークンを使用している場合**、次の点を確認してください。

- アクセストークンの有効期限が切れていないこと（`ExpiresIn` または `TokenExpiredAt` で確認）
- `Authorization` ヘッダーが `Bearer <ACCESS_TOKEN>` 形式で送信されていること
- JWT の署名に使用したサービスプリンシパルキーが有効であること

それでも解消しない場合は、`Error` の内容やログを確認し、レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。

### Q. タイムアウトや接続エラーが発生します

まず、開発環境から `https://secure.sakura.ad.jp` への通信経路（プロキシ設定やファイアウォール）に問題がないか確認してください。
ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、
`HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか？

本 SDK は .NET Standard 2.0 対応のライブラリです。
そのため、.NET 6 以降を含む .NET Standard 2.0 互換のランタイムで利用できます。

### Q. 他のサービス(AppRun共用型やEventBusなど)でも同じように使えますか？

本 README は API ゲートウェイ（`Sacloud.Sdk.APIGateway`）向けの説明です。
同一リポジトリ内には AppRun共用型 や EventBus、Workflows など他サービス向けの SDK も含まれていますが、
それぞれ `sdk/<ServiceName>/` 配下に独自の README とサンプルが用意される想定です。
基本的な考え方（.NET プロジェクトへのパッケージ追加、認証、クライアント初期化、ログ設定など）は共通ですが、
サービスごとに利用する API やユースケースが異なるため、対象サービスの README も併せて参照してください。

API ゲートウェイ SDK を利用するだけであれば、通常は本パッケージ `Sacloud.Sdk.APIGateway` を参照すれば十分であり、
共通ライブラリプロジェクト (`sdk/Shared`) を意識する必要はありません。
