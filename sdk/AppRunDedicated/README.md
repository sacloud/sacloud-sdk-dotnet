# AppRun専有型 API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する AppRun専有型 API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの AppRun専有型を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本 README の内容を読めば、クライアント初期化から主要リソースの操作イメージまで一通り把握できる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `AppRunDedicated`（AppRun専有型）
- サービス概要:
    - 自動作成される専用の仮想サーバ（ワーカノード）上でコンテナをデプロイ・実行する、専有型のコンテナオーケストレーションサービスです。
    - 複数のユーザー間でリソースを共有しないため、セキュリティ面やアプリケーションの性能・安定性がより求められるケースにマッチします。
- 本 SDK の目的:
    - さくらのクラウド「AppRun専有型」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.AppRunDedicated` を提供します。
    - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
    - C# 7.3 以降
    - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
    - ファイルパス: `sdk/AppRunDedicated/openapi-spec/1.4.0/openapi.yaml`
    - OpenAPI バージョン: 3.0.0
- AppRun専有型 API バージョン:
    - `info.version`: `1.4.0`
- ベース URL:
    - `https://secure.sakura.ad.jp/cloud/api/apprun-dedicated/1.0`
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

AppRun専有型 SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### 1. 共通の準備

AppRun専有型でクラスタの作成や更新を行う場合は、サービスプリンシパルを作成し、適切な IAM ポリシーを設定する必要があります。  
下記ドキュメントの「サービスプリンシパルの設定」を参照し、設定を行ってください。  

- [クイックスタート | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/apprun-dedicated/getting_started.html#id12)

README 内のクラスタ作成例およびサンプルプロジェクトでは、作成したサービスプリンシパルの ID を利用します。  

#### 2-A. API キーを使用する
認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。

- [APIリファレンス | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/api/cloud/apprun-dedicated/)

#### 2-B. サービスプリンシパルキーを使用して発行したアクセストークンを使用する
認証に サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従い アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
    - .NET Standard 2.0 でサポートされているバージョンであること
    - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```bash
dotnet add package Sacloud.Sdk.AppRunDedicated
```

## クイックスタート

ここでは、最小限のコードで AppRun専有型 API にアクセスし、アプリケーション一覧を取得するまでの流れを示します。
認証には API キー を使用します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

    ```bash
    dotnet new console -n AppRunDedicatedQuickStart
    cd AppRunDedicatedQuickStart
    dotnet add package Sacloud.Sdk.AppRunDedicated
    ```

2. `Program.cs` に、以下のコード例を保存します。

    ```csharp
    using Microsoft.Extensions.Logging;
    using Microsoft.Kiota.Abstractions.Authentication;
    using Microsoft.Kiota.Http.HttpClientLibrary;
    using Sacloud.Sdk.AppRunDedicated;
    using Sacloud.Sdk.AppRunDedicated.Models;

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

    AppRunDedicatedClient client = new AppRunDedicatedClient(adapter, loggerFactory);

    try
    {
        // 3. アプリケーションの一覧を取得
        ListApplicationsResponse applications = await client.Applications.GetAsync(config =>
        {
            config.QueryParameters.MaxItems = 20;
        });

        Console.WriteLine("Applications:");
        if (applications.Applications is not null)
        {
            foreach (ReadApplication application in applications.Applications)
            {
                Console.WriteLine($"- {application.ApplicationID}: {application.Name}");
            }
        }
        else
        {
            Console.WriteLine("No applications found.");
        }
    }
    catch (Exception ex)
    {
        // 簡易的なエラー処理
        Console.WriteLine($"Error while calling AppRunDedicated API: {ex.Message}");
    }
    ```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

    ```bash
    dotnet run
    ```

より多くの操作例については、後述の「使用方法」セクションと、リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/AppRunDedicated/samples/`

このディレクトリには、実際の AppRun専有型リソースに対して SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/AppRunDedicated/samples/AppRunDedicated.Samples.csproj
```

実行前に、`Program.cs` 内の以下のプレースホルダーを適切な値に置き換えてください。

- `<YOUR_ACCESS_TOKEN>`
- `<YOUR_ACCESS_TOKEN_SECRET>`
- `<YOUR_SERVICE_PRINCIPAL_ID>`
- `<YOUR_CONTAINER_IMAGE>`
- `<YOUR_CONTAINER_REGISTRY_USERNAME>`
- `<YOUR_CONTAINER_REGISTRY_PASSWORD>`
- `<YOUR_CERTIFICATE_PEM>`
- `<YOUR_PRIVATEKEY_PEM>`

## SDK のメソッドと API エンドポイントの対応表

### Application

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Applications.GetAsync` | GET | `/applications` |
| `Applications.PostAsync` | POST | `/applications` |
| `Applications[{application_id}].GetAsync` | GET | `/applications/{application_id}` |
| `Applications[{application_id}].PutAsync` | PUT | `/applications/{application_id}` |
| `Applications[{application_id}].DeleteAsync` | DELETE | `/applications/{application_id}` |
| `Applications[{application_id}].Containers.GetAsync` | GET | `/applications/{application_id}/containers` |

### Application Version

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Applications[{application_id}].Versions.GetAsync` | GET | `/applications/{application_id}/versions` |
| `Applications[{application_id}].Versions.PostAsync` | POST | `/applications/{application_id}/versions` |
| `Applications[{application_id}].Versions[{version}].GetAsync` | GET | `/applications/{application_id}/versions/{version}` |
| `Applications[{application_id}].Versions[{version}].DeleteAsync` | DELETE | `/applications/{application_id}/versions/{version}` |

### Certificate

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Clusters[{cluster_id}].Certificates.GetAsync` | GET | `/clusters/{cluster_id}/certificates` |
| `Clusters[{cluster_id}].Certificates.PostAsync` | POST | `/clusters/{cluster_id}/certificates` |
| `Clusters[{cluster_id}].Certificates[{certificate_id}].GetAsync` | GET | `/clusters/{cluster_id}/certificates/{certificate_id}` |
| `Clusters[{cluster_id}].Certificates[{certificate_id}].PutAsync` | PUT | `/clusters/{cluster_id}/certificates/{certificate_id}` |
| `Clusters[{cluster_id}].Certificates[{certificate_id}].DeleteAsync` | DELETE | `/clusters/{cluster_id}/certificates/{certificate_id}` |

### Cluster

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Clusters.GetAsync` | GET | `/clusters` |
| `Clusters.PostAsync` | POST | `/clusters` |
| `Clusters[{cluster_id}].GetAsync` | GET | `/clusters/{cluster_id}` |
| `Clusters[{cluster_id}].PutAsync` | PUT | `/clusters/{cluster_id}` |
| `Clusters[{cluster_id}].DeleteAsync` | DELETE | `/clusters/{cluster_id}` |
| `Clusters[{cluster_id}].Load_balancer.GetAsync` | GET | `/clusters/{cluster_id}/load_balancer` |
| `Clusters[{cluster_id}].Load_balancer.PatchAsync` | PATCH | `/clusters/{cluster_id}/load_balancer` |
| `Clusters[{cluster_id}].Asg.GetAsync` | GET | `/clusters/{cluster_id}/asg` |
| `Clusters[{cluster_id}].Asg.PostAsync` | POST | `/clusters/{cluster_id}/asg` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].GetAsync` | GET | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].DeleteAsync` | DELETE | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Load_balancers.GetAsync` | GET | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/load_balancers` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Load_balancers.PostAsync` | POST | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/load_balancers` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Load_balancers[{load_balancer_id}].GetAsync` | GET | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/load_balancers/{load_balancer_id}` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Load_balancers[{load_balancer_id}].DeleteAsync` | DELETE | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/load_balancers/{load_balancer_id}` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Load_balancers[{load_balancer_id}].Load_balancer_nodes.GetAsync` | GET | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/load_balancers/{load_balancer_id}/load_balancer_nodes` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Load_balancers[{load_balancer_id}].Load_balancer_nodes[{load_balancer_node_id}].GetAsync` | GET | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/load_balancers/{load_balancer_id}/load_balancer_nodes/{load_balancer_node_id}` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Worker_nodes.GetAsync` | GET | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/worker_nodes` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Worker_nodes[{worker_node_id}].GetAsync` | GET | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/worker_nodes/{worker_node_id}` |
| `Clusters[{cluster_id}].Asg[{auto_scaling_group_id}].Worker_nodes[{worker_node_id}].Draining.PutAsync` | PUT | `/clusters/{cluster_id}/asg/{auto_scaling_group_id}/worker_nodes/{worker_node_id}/draining` |

### Service Class

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Service_classes.Lb.GetAsync` | GET | `/service_classes/lb` |
| `Service_classes.Worker.GetAsync` | GET | `/service_classes/worker` |

### Limit

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Limits.GetAsync` | GET | `/limits` |

## 使用方法

本 SDK を使用して AppRun専有型 API を呼び出す方法について記載します。

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

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`AppRunDedicatedClient` のインスタンスを作成します。

    ```csharp
    AppRunDedicatedClient client = new AppRunDedicatedClient(adapter, loggerFactory);
    ```

    ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。

    ```csharp
    AppRunDedicatedClient client = new AppRunDedicatedClient(adapter);
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

AppRunDedicatedClient client = new AppRunDedicatedClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレット、証明書秘密鍵、レジストリパスワードなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

#### アプリケーションの一覧取得

```csharp
ListApplicationsResponse listApplicationsResponse = await client.Applications.GetAsync(config =>
{
    config.QueryParameters.MaxItems = 20;
});
```

#### アプリケーションの詳細取得

```csharp
ReadApplicationResponse getApplicationResponse = await client.Applications[new Guid("<YOUR_APPLICATION_ID>")].GetAsync();
```

#### アプリケーションの作成

```csharp
CreateApplicationResponse createApplicationResponse = await client.Applications.PostAsync(new CreateApplicationRequest
{
    Name = "example-app",
    ClusterID = new Guid("<YOUR_CLUSTER_ID>")
});
```

#### アプリケーション情報の更新

```csharp
await client.Applications[new Guid("<YOUR_APPLICATION_ID>")].PutAsync(new UpdateApplicationRequest
{
    ActiveVersion = 1
});
```

#### アプリケーションの削除

```csharp
await client.Applications[new Guid("<YOUR_APPLICATION_ID>")].DeleteAsync();
```

#### アプリケーションバージョンの作成

```csharp
CreateApplicationVersionResponse createApplicationVersionResponse = await client.Applications[new Guid("<YOUR_APPLICATION_ID>")].Versions.PostAsync(new CreateApplicationVersionRequest
{
    ScalingMode = ScalingMode.Manual,
    FixedScale = 1,
    RegistryPasswordAction = RegistryPasswordAction.New,
    RegistryUsername = "<YOUR_CONTAINER_REGISTRY_USERNAME>",
    RegistryPassword = "<YOUR_CONTAINER_REGISTRY_PASSWORD>",
    Cpu = 100,
    Memory = 128,
    Image = "<YOUR_CONTAINER_IMAGE>",
    ExposedPorts = new List<ExposedPort>
    {
        new ExposedPort
        {
            TargetPort = 8080,
            LoadBalancerPort = null,
            UseLetsEncrypt = false,
            HealthCheck = null
        }
    },
    Env = new List<CreateEnvironmentVariable>()
    {
        new CreateEnvironmentVariable
        {
            Key = "EXAMPLE_ENV_SECRET",
            Value = "example_secret_value",
            Secret = true
        }
    }
});
```

## エラー処理とページングの簡易ガイド

### 例外の扱い

AppRun専有型 API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、`Sacloud.Sdk.AppRunDedicated.Models.Error` がスローされます。
例外にはステータスコードやエラーメッセージが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.AppRunDedicated.Models;

try
{
    ListClustersResponse clusters = await client.Clusters.GetAsync();  
    // 取得した結果を処理
}
catch (Error apiError)
{
    Console.WriteLine($"API error: status={apiError.Status}, title={apiError.Title}");
}
catch (ApiException apiEx)
{
    Console.WriteLine($"API error: {apiEx.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

### ページングと大量データ取得

`/clusters` や `/applications` など一部の一覧系 API にはページングのためのクエリパラメータが定義されています。
大量件数を扱う場合は、これらのパラメータを指定して少量ずつ取得することを推奨します。

具体的なパラメータ名や制約値は、AppRun専有型 API リファレンスおよび OpenAPI 定義を参照してください。
Kiota で生成されたクライアントでは、一覧取得メソッドに対してクエリパラメータを指定できるようになっています。
アプリケーションの規模や想定件数に応じて、ページサイズやカーソルを適切に設定してください。

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キーを使用している場合**、次の点を確認してください。

- AppRun専有型用の API キーを発行していること
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
