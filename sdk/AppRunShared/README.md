# AppRun共用型 API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する AppRun共用型 API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの AppRun共用型 を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本READMEの内容を読めば、簡単なアプリケーション操作まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `AppRunShared`（AppRun共用型）
- サービス概要:
	- コンテナ化されたアプリケーションを簡単にデプロイできる PaaS 型サービスです。
	- トラフィックに応じた自動スケーリングやインフラ管理の抽象化により、アプリケーション開発に集中できます。
- 本 SDK の目的:
	- さくらのクラウド「AppRun共用型」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.AppRunShared` を提供します。
    - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、
      通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
	- C# 7.3 以降
	- .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
    - ファイルパス: `sdk/AppRunShared/openapi-spec/1.5.0/apprun-shared-api.yaml`
    - OpenAPI バージョン: 3.0.0
- AppRun共用型 API バージョン:
    - `info.version`: `1.5.0`
- ベース URL:
    - `https://secure.sakura.ad.jp/cloud/api/apprun/1.0/apprun/api`
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

AppRun共用型 SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### 1-A. API キーを使用する
認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。

- [APIリファレンス | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/api/cloud/portal/?api=apprun-shared-api)

#### 1-B. サービスプリンシパルキーを使用して発行したアクセストークンを使用する
認証に サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従い アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
    - .NET Standard 2.0 でサポートされているバージョンであること
    - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。
```
dotnet add package Sacloud.Sdk.AppRunShared
```

## クイックスタート

ここでは、最小限のコードで AppRun共用型 API にアクセスし、アプリケーション一覧を取得するまでの流れを示します。
認証には API キー を使用します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

    ```bash
    dotnet new console -n AppRunSharedQuickStart
    cd AppRunSharedQuickStart
    dotnet add package Sacloud.Sdk.AppRunShared
    ```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

    ```csharp
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Kiota.Abstractions.Authentication;
    using Microsoft.Kiota.Http.HttpClientLibrary;
    using Sacloud.Sdk.AppRunShared.Models;

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

    Sacloud.Sdk.AppRunShared.AppRunSharedClient client = new Sacloud.Sdk.AppRunShared.AppRunSharedClient(adapter, loggerFactory);

    try
    {
        // 3. アプリケーションの一覧を取得
        ListApplications applications = await client.Applications.GetAsync();

        Console.WriteLine("Applications:");
        if (applications?.Data is not null)
        {
            foreach (ListApplications_data app in applications.Data)
            {
                Console.WriteLine($"- {app.Id}: {app.Name}");
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
        Console.WriteLine($"Error while calling AppRunShared API: {ex.Message}");
    }
    ```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

    ```bash
    dotnet run
    ```

より詳細な認証手順や、アプリケーションの追加・更新・削除などの操作については、
後述の「使用方法」セクションを参照してください。

より多くの操作例や対話的なメニュー形式での実行例については、
リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/AppRunShared/samples/`

このディレクトリには、実際の AppRun共用型 アプリケーションリソースに対して
SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、
サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/AppRunShared/samples/AppRunShared.Samples.csproj
```

実行前に、`Program.cs` 内の `<YOUR_API_KEY_ACCESS_TOKEN>` と
`<YOUR_API_KEY_ACCESS_TOKEN_SECRET>` を適切な値に置き換えてください。

## SDK のメソッドと API エンドポイントの対応表

### アプリケーション

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Applications.GetAsync` | GET | `/applications` |
| `Applications.PostAsync` | POST | `/applications` |
| `Applications[{id}].GetAsync` | GET | `/applications/{id}` |
| `Applications[{id}].PatchAsync` | PATCH | `/applications/{id}` |
| `Applications[{id}].DeleteAsync` | DELETE | `/applications/{id}` |
| `Applications[{id}].Status.GetAsync` | GET | `/applications/{id}/status` |

### ユーザー

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `User.GetAsync` | GET | `/user` |
| `User.PostAsync` | POST | `/user` |

### トラフィック

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Applications[{id}].Traffics.GetAsync` | GET | `/applications/{id}/traffics` |
| `Applications[{id}].Traffics.PutAsync` | PUT | `/applications/{id}/traffics` |

### バージョン

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Applications[{id}].Versions.GetAsync` | GET | `/applications/{id}/versions` |
| `Applications[{id}].Versions[{version_id}].GetAsync` | GET | `/applications/{id}/versions/{version_id}` |
| `Applications[{id}].Versions[{version_id}].DeleteAsync` | DELETE | `/applications/{id}/versions/{version_id}` |
| `Applications[{id}].Versions[{version_id}].Status.GetAsync` | GET | `/applications/{id}/versions/{version_id}/status` |

### パケットフィルタ

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Applications[{id}].Packet_filter.GetAsync` | GET | `/applications/{id}/packet_filter` |
| `Applications[{id}].Packet_filter.PatchAsync` | PATCH | `/applications/{id}/packet_filter` |


## 使用方法

本 SDK を使用して AppRun共用型 API を呼び出す方法について記載します。

### 認証

#### API キーを使用する場合

1. 「事前セットアップ」で発行した API キーの「アクセストークン (UUID)」と「アクセストークンシークレット」を文字列として用意します。
    ```csharp
    string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
    string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";
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
    using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
        // 必要に応じてログレベルや出力先などの設定を行う
    });
    ```
2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`AppRunSharedClient` のインスタンスを作成します。
    ```csharp
    AppRunSharedClient client = new AppRunSharedClient(adapter, loggerFactory);
    ```
    ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。
    ```csharp
    AppRunSharedClient client = new AppRunSharedClient(adapter);
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

AppRunSharedClient client = new AppRunSharedClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレット、パスワードなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

アプリケーションに対する各種操作の API 呼び出し例を示します。

#### 一覧取得

```csharp
ListApplications applications = await client.Applications.GetAsync();
```

#### 詳細取得

```csharp
ReadApplication application = await client.Applications["<YOUR_APPLICATION_ID>"].GetAsync();
```

#### 追加

```csharp
CreateApplicationBody body = new CreateApplicationBody
{
    Name = "<YOUR_APPLICATION_NAME>",
    TimeoutSeconds = 60,
    Port = <YOUR_APPLICATION_PORT>,
    MinScale = 0,
    MaxScale = 10,
    Components = new List<CreateApplicationBody_components>
    {
        new CreateApplicationBody_components
        {
            Name = "<YOUR_COMPONENT_NAME>",
            MaxCpu = CreateApplicationBody_components_max_cpu.ZeroFive,
            MaxMemory = CreateApplicationBody_components_max_memory.OneGi,
            DeploySource = new CreateApplicationBody_components_deploy_source
            {
                ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry
                {
                    Image = "<YOUR_REGISTRY_IMAGE>",
                    Username = "<YOUR_REGISTRY_USERNAME>",
                    Password = "<YOUR_REGISTRY_PASSWORD>"
                }
            }
        }
    }
};

CreateApplication createdApplication = await client.Applications.PostAsync(body);
```

#### 更新

```csharp
PatchApplicationBody body = new PatchApplicationBody
{
    TimeoutSeconds = <YOUR_NEW_APPLICATION_TIMEOUT_SECONDS>
};
PatchApplication updatedApplication = await client.Applications["<YOUR_APPLICATION_ID>"].PatchAsync(body);
```

#### 削除

```csharp
await client.Applications["<YOUR_APPLICATION_ID>"].DeleteAsync();
```

## エラー処理とページングの簡易ガイド

### 例外の扱い

AppRun共用型 API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;

try
{
    var applications = await client.Applications.GetAsync();
    // 取得した結果を処理
}
catch (ApiException apiEx)
{
    Console.WriteLine($"API error: {apiEx.Message}");

    // 共通エラー型(ApiError)として返却されている場合は、レスポンスボディの詳細を参照できます。
    if (apiEx is ApiError apiError)
    {
        foreach (var kv in apiError.ResponseBody)
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

AppRun共用型 SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型
`Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、
    `ResponseBody` プロパティにエラーレスポンスボディの内容 (フィールド名と値のディクショナリ) を保持します。
- AppRun共用型固有のエラー形式（`defaultError` や `cloudctrlError` 相当の構造）も、
    この `ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから
`ResponseBody` を参照することで、APIごとの詳細なエラー情報を確認できます。

### ページングと大量データ取得

`/applications` など一部の一覧系 API には、ページングやソートのためのクエリパラメータが定義されています。
大量件数を扱う場合は、これらのパラメータを指定して少量ずつ取得することを推奨します。

具体的なパラメータ名や制約値は、AppRun共用型 API リファレンスおよび OpenAPI 定義を参照してください。
Kiota で生成されたクライアントでは、一覧取得メソッドに対してクエリパラメータを指定できるようになっています。
アプリケーションの規模や想定件数に応じて、ページサイズやソート順を適切に設定してください。

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キーを使用している場合**、次の点を確認してください。

- AppRun共用型用の API キーを発行していること
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

### Q. 他のサービス(AppRun専有型やEventBusなど)でも同じように使えますか？

本 README は AppRun共用型 (`Sacloud.Sdk.AppRunShared`) 向けの説明です。
同一リポジトリ内には AppRun専有型 や EventBus、Workflows など他サービス向けの SDK も含まれていますが、
それぞれ `sdk/<ServiceName>/` 配下に独自の README とサンプルが用意される想定です。
基本的な考え方（.NET プロジェクトへのパッケージ追加、認証、クライアント初期化、ログ設定など）は共通ですが、
サービスごとに利用する API やユースケースが異なるため、対象サービスの README も併せて参照してください。

AppRun共用型 SDK を利用するだけであれば、通常は本パッケージ `Sacloud.Sdk.AppRunShared` を参照すれば十分であり、
共通ライブラリプロジェクト (`sdk/Shared`) を意識する必要はありません。
