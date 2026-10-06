# MonitoringSuite API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する MonitoringSuite API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドのMonitoringSuite を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。本READMEの内容を読めば、ログやメトリクス情報の取得、
アラートプロジェクトの管理など、一通りの操作が試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `MonitoringSuite`
- サービス概要:
  - ログ / メトリクス / トレースのストレージ管理、ルーティング、アラート、ダッシュボードを API で操作できる監視サービスです。
- 本 SDK の目的:
  - さくらのクラウド「MonitoringSuite」向けの C# API クライアントライブラリ `Sacloud.Sdk.MonitoringSuite` を提供します。
  - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、通常の SDK利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
  - C# 7.3 以降
  - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義バージョン: 
    - ファイルパス: `sdk/MonitoringSuite/openapi-spec/1.3.0/monitoring-suite-api.yaml`
  - OpenAPI バージョン:  3.0.3
- MonitoringSuite API バージョン:
    - `info.version`: `1.3.0`
- ベース URL:
  - `https://secure.sakura.ad.jp/cloud/zone/is1a/api/monitoring/1.0`
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

MonitoringSuite SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。
以下の手順に従い、事前に認証に必要なリソースを作成してください。

#### 1-A. APIキーを使用する
認証にAPIキーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。
- API キー: [API キー | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/api/apikey.html#api)

#### 1-B. サービスプリンシパルキーを使用して発行したアクセストークンを使用する
認証にサービスプリンシパルキーを使用して発行したアクセストークン使用する場合は、下記ドキュメントに従いアクセストークンを発行します。
- サービスプリンシパル: [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
  - .NET Standard 2.0 でサポートされているバージョンであること
  - C# 7.3 以降をサポートしていること

### パッケージのインストール
本 SDK を使用する.NETプロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```bash
dotnet add package Sacloud.Sdk.MonitoringSuite
```

## クイックスタート

ここでは、最小限のコードで MonitoringSuite API にアクセスし、アラートプロジェクト一覧を取得する例を示します。認証には API キーを使用します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

```bash
dotnet new console -n MonitoringSuiteQuickStart
cd MonitoringSuiteQuickStart
dotnet add package Sacloud.Sdk.MonitoringSuite
```

2. `Program.cs` に以下のコードを保存します。(C# のトップレベルステートメント構文を使用しています)

```csharp
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.MonitoringSuite;
using Sacloud.Sdk.MonitoringSuite.Models;

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

MonitoringSuiteClient client = new MonitoringSuiteClient(adapter, loggerFactory);

try
{
    // 3. アラートプロジェクトの一覧を取得
    PaginatedAlertProjectList result = await client.Alerts.Projects.GetAsync();

    Console.WriteLine("Alert Projects:");
    if (result?.Results is not null)
    {
        foreach (AlertProject project in result.Results)
        {
            Console.WriteLine($"- {project.ResourceId}: {project.Name}");
        }
    }
    else
    {
        Console.WriteLine("No projects found.");
    }
}
catch (Exception ex)
{
    // 簡易的なエラー処理
    Console.WriteLine($"Error while calling MonitoringSuite API: {ex.Message}");
}
```

3. 以下のコマンドを実行し、一覧が取得できることを確認します。

```bash
dotnet run
```

より詳細な認証手順や、各一覧の取得・更新・削除などの操作については、後述の「使用方法」セクションを参照してください。
より多くの操作例や対話的なメニュー形式での実行例については、リポジトリ内のサンプルプロジェクトを参照してください。
- サンプルプロジェクト: `sdk/MonitoringSuite/samples/`

このディレクトリには、実際の MonitoringSuite アプリケーションリソースに対して SDK を介して各種操作を行うコンソールアプリケーションが
含まれています。プロジェクトルート( `Sacloud.Sdk.slnx` があるディレクトリ)で次のコマンドを実行すると、サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/MonitoringSuite/samples/MonitoringSuite.Samples.csproj
```
実行前に、`Program.cs` 内の `<YOUR_API_KEY_ACCESS_TOKEN>` と `<YOUR_API_KEY_ACCESS_TOKEN_SECRET>` を適切な値に置き換えてください。

## SDK のメソッドと API エンドポイントの対応表

### アラート
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Alerts.Projects.GetAsync` | GET | `/alerts/projects/` |
| `Alerts.Projects.PostAsync` | POST | `/alerts/projects/` |
| `Alerts.Projects[{projectResourceId}].Histories.GetAsync` | GET | `/alerts/projects/{projectResourceId}/histories/` |
| `Alerts.Projects[{projectResourceId}].Histories[{uid}].GetAsync` | GET | `/alerts/projects/{projectResourceId}/histories/{uid}/` |
| `Alerts.Projects[{projectResourceId}].LogMeasureRules.GetAsync` | GET | `/alerts/projects/{projectResourceId}/log-measure-rules/` |
| `Alerts.Projects[{projectResourceId}].LogMeasureRules.PostAsync` | POST | `/alerts/projects/{projectResourceId}/log-measure-rules/` |
| `Alerts.Projects[{projectResourceId}].LogMeasureRules[{uid}].GetAsync` | GET | `/alerts/projects/{projectResourceId}/log-measure-rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].LogMeasureRules[{uid}].PutAsync` | PUT | `/alerts/projects/{projectResourceId}/log-measure-rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].LogMeasureRules[{uid}].PatchAsync` | PATCH | `/alerts/projects/{projectResourceId}/log-measure-rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].LogMeasureRules[{uid}].DeleteAsync` | DELETE | `/alerts/projects/{projectResourceId}/log-measure-rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationRoutings.GetAsync` | GET | `/alerts/projects/{projectResourceId}/notification-routings/` |
| `Alerts.Projects[{projectResourceId}].NotificationRoutings.PostAsync` | POST | `/alerts/projects/{projectResourceId}/notification-routings/` |
| `Alerts.Projects[{projectResourceId}].NotificationRoutings[{uid}].GetAsync` | GET | `/alerts/projects/{projectResourceId}/notification-routings/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationRoutings[{uid}].PutAsync` | PUT | `/alerts/projects/{projectResourceId}/notification-routings/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationRoutings[{uid}].PatchAsync` | PATCH | `/alerts/projects/{projectResourceId}/notification-routings/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationRoutings[{uid}].DeleteAsync` | DELETE | `/alerts/projects/{projectResourceId}/notification-routings/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationRoutings.Reorder.PutAsync` | PUT | `/alerts/projects/{projectResourceId}/notification-routings/reorder/` |
| `Alerts.Projects[{projectResourceId}].NotificationTargets.GetAsync` | GET | `/alerts/projects/{projectResourceId}/notification-targets/` |
| `Alerts.Projects[{projectResourceId}].NotificationTargets.PostAsync` | POST | `/alerts/projects/{projectResourceId}/notification-targets/` |
| `Alerts.Projects[{projectResourceId}].NotificationTargets[{uid}].GetAsync` | GET | `/alerts/projects/{projectResourceId}/notification-targets/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationTargets[{uid}].PutAsync` | PUT | `/alerts/projects/{projectResourceId}/notification-targets/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationTargets[{uid}].PatchAsync` | PATCH | `/alerts/projects/{projectResourceId}/notification-targets/{uid}/` |
| `Alerts.Projects[{projectResourceId}].NotificationTargets[{uid}].DeleteAsync` | DELETE | `/alerts/projects/{projectResourceId}/notification-targets/{uid}/` |
| `Alerts.Projects[{projectResourceId}].Rules.GetAsync` | GET | `/alerts/projects/{projectResourceId}/rules/` |
| `Alerts.Projects[{projectResourceId}].Rules.PostAsync` | POST | `/alerts/projects/{projectResourceId}/rules/` |
| `Alerts.Projects[{projectResourceId}].Rules[{ruleUid}].Histories.GetAsync` | GET | `/alerts/projects/{projectResourceId}/rules/{ruleUid}/histories/` |
| `Alerts.Projects[{projectResourceId}].Rules[{ruleUid}].Histories[{uid}].GetAsync` | GET | `/alerts/projects/{projectResourceId}/rules/{ruleUid}/histories/{uid}/` |
| `Alerts.Projects[{projectResourceId}].Rules[{uid}].GetAsync` | GET | `/alerts/projects/{projectResourceId}/rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].Rules[{uid}].PutAsync` | PUT | `/alerts/projects/{projectResourceId}/rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].Rules[{uid}].PatchAsync` | PATCH | `/alerts/projects/{projectResourceId}/rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].Rules[{uid}].DeleteAsync` | DELETE | `/alerts/projects/{projectResourceId}/rules/{uid}/` |
| `Alerts.Projects[{projectResourceId}].GetAsync` | GET | `/alerts/projects/{resourceId}/` |
| `Alerts.Projects[{projectResourceId}].PutAsync` | PUT | `/alerts/projects/{resourceId}/` |
| `Alerts.Projects[{projectResourceId}].PatchAsync` | PATCH | `/alerts/projects/{resourceId}/` |
| `Alerts.Projects[{projectResourceId}].DeleteAsync` | DELETE | `/alerts/projects/{resourceId}/` |

### ダッシュボード
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Dashboards.Projects.GetAsync` | GET | `/dashboards/projects/` |
| `Dashboards.Projects.PostAsync` | POST | `/dashboards/projects/` |
| `Dashboards.Projects[{resourceId}].GetAsync` | GET | `/dashboards/projects/{resourceId}/` |
| `Dashboards.Projects[{resourceId}].PutAsync` | PUT | `/dashboards/projects/{resourceId}/` |
| `Dashboards.Projects[{resourceId}].PatchAsync` | PATCH | `/dashboards/projects/{resourceId}/` |
| `Dashboards.Projects[{resourceId}].DeleteAsync` | DELETE | `/dashboards/projects/{resourceId}/` |

### ログルーティング
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Logs.Routings.GetAsync` | GET | `/logs/routings/` |
| `Logs.Routings.PostAsync` | POST | `/logs/routings/` |
| `Logs.Routings[{uid}].GetAsync` | GET | `/logs/routings/{uid}/` |
| `Logs.Routings[{uid}].PutAsync` | PUT | `/logs/routings/{uid}/` |
| `Logs.Routings[{uid}].PatchAsync` | PATCH | `/logs/routings/{uid}/` |
| `Logs.Routings[{uid}].DeleteAsync` | DELETE | `/logs/routings/{uid}/` |

### ログストレージ
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Logs.Storages.GetAsync` | GET | `/logs/storages/` |
| `Logs.Storages.PostAsync` | POST | `/logs/storages/` |
| `Logs.Storages[{logResourceId}].Keys.GetAsync` | GET | `/logs/storages/{logResourceId}/keys/` |
| `Logs.Storages[{logResourceId}].Keys.PostAsync` | POST | `/logs/storages/{logResourceId}/keys/` |
| `Logs.Storages[{logResourceId}].Keys[{uid}].GetAsync` | GET | `/logs/storages/{logResourceId}/keys/{uid}/` |
| `Logs.Storages[{logResourceId}].Keys[{uid}].PutAsync` | PUT | `/logs/storages/{logResourceId}/keys/{uid}/` |
| `Logs.Storages[{logResourceId}].Keys[{uid}].PatchAsync` | PATCH | `/logs/storages/{logResourceId}/keys/{uid}/` |
| `Logs.Storages[{logResourceId}].Keys[{uid}].DeleteAsync` | DELETE | `/logs/storages/{logResourceId}/keys/{uid}/` |
| `Logs.Storages[{logResourceId}].GetAsync` | GET | `/logs/storages/{resourceId}/` |
| `Logs.Storages[{logResourceId}].PutAsync` | PUT | `/logs/storages/{resourceId}/` |
| `Logs.Storages[{logResourceId}].PatchAsync` | PATCH | `/logs/storages/{resourceId}/` |
| `Logs.Storages[{logResourceId}].DeleteAsync` | DELETE | `/logs/storages/{resourceId}/` |
| `Logs.Storages[{logResourceId}].SetExpire.PostAsync` | POST | `/logs/storages/{resourceId}/set-expire/` |
| `Logs.Storages[{logResourceId}].Stats.Daily.GetAsync` | GET | `/logs/storages/{resourceId}/stats/daily/` |
| `Logs.Storages[{logResourceId}].Stats.Monthly.GetAsync` | GET | `/logs/storages/{resourceId}/stats/monthly/` |

### 管理
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Management.Limits.GetAsync` | GET | `/management/limits/` |
| `Management.Provisioning.Initialize.PostAsync` | POST | `/management/provisioning/initialize/` |
| `Management.Provisioning.State.GetAsync` | GET | `/management/provisioning/state/` |

### メトリクスルーティング
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Metrics.Routings.GetAsync` | GET | `/metrics/routings/` |
| `Metrics.Routings.PostAsync` | POST | `/metrics/routings/` |
| `Metrics.Routings[{uid}].GetAsync` | GET | `/metrics/routings/{uid}/` |
| `Metrics.Routings[{uid}].PutAsync` | PUT | `/metrics/routings/{uid}/` |
| `Metrics.Routings[{uid}].PatchAsync` | PATCH | `/metrics/routings/{uid}/` |
| `Metrics.Routings[{uid}].DeleteAsync` | DELETE | `/metrics/routings/{uid}/` |

### メトリクスストレージ
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Metrics.Storages.GetAsync` | GET | `/metrics/storages/` |
| `Metrics.Storages.PostAsync` | POST | `/metrics/storages/` |
| `Metrics.Storages[{metricsResourceId}].Keys.GetAsync` | GET | `/metrics/storages/{metricsResourceId}/keys/` |
| `Metrics.Storages[{metricsResourceId}].Keys.PostAsync` | POST | `/metrics/storages/{metricsResourceId}/keys/` |
| `Metrics.Storages[{metricsResourceId}].Keys[{uid}].GetAsync` | GET | `/metrics/storages/{metricsResourceId}/keys/{uid}/` |
| `Metrics.Storages[{metricsResourceId}].Keys[{uid}].PutAsync` | PUT | `/metrics/storages/{metricsResourceId}/keys/{uid}/` |
| `Metrics.Storages[{metricsResourceId}].Keys[{uid}].PatchAsync` | PATCH | `/metrics/storages/{metricsResourceId}/keys/{uid}/` |
| `Metrics.Storages[{metricsResourceId}].Keys[{uid}].DeleteAsync` | DELETE | `/metrics/storages/{metricsResourceId}/keys/{uid}/` |
| `Metrics.Storages[{metricsResourceId}].GetAsync` | GET | `/metrics/storages/{resourceId}/` |
| `Metrics.Storages[{metricsResourceId}].PutAsync` | PUT | `/metrics/storages/{resourceId}/` |
| `Metrics.Storages[{metricsResourceId}].PatchAsync` | PATCH | `/metrics/storages/{resourceId}/` |
| `Metrics.Storages[{metricsResourceId}].DeleteAsync` | DELETE | `/metrics/storages/{resourceId}/` |
| `Metrics.Storages[{metricsResourceId}].Stats.Daily.GetAsync` | GET | `/metrics/storages/{resourceId}/stats/daily/` |
| `Metrics.Storages[{metricsResourceId}].Stats.Monthly.GetAsync` | GET | `/metrics/storages/{resourceId}/stats/monthly/` |

### 連携サービス
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Publishers.GetAsync` | GET | `/publishers/` |
| `Publishers[{code}].GetAsync` | GET | `/publishers/{code}/` |

### トレースストレージ
| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Traces.Storages.GetAsync` | GET | `/traces/storages/` |
| `Traces.Storages.PostAsync` | POST | `/traces/storages/` |
| `Traces.Storages[{resourceId}].GetAsync` | GET | `/traces/storages/{resourceId}/` |
| `Traces.Storages[{resourceId}].PutAsync` | PUT | `/traces/storages/{resourceId}/` |
| `Traces.Storages[{resourceId}].PatchAsync` | PATCH | `/traces/storages/{resourceId}/` |
| `Traces.Storages[{resourceId}].DeleteAsync` | DELETE | `/traces/storages/{resourceId}/` |
| `Traces.Storages[{resourceId}].SetExpire.PostAsync` | POST | `/traces/storages/{resourceId}/set-expire/` |
| `Traces.Storages[{resourceId}].Stats.Daily.GetAsync` | GET | `/traces/storages/{resourceId}/stats/daily/` |
| `Traces.Storages[{resourceId}].Stats.Monthly.GetAsync` | GET | `/traces/storages/{resourceId}/stats/monthly/` |
| `Traces.Storages[{resourceId}].Keys.GetAsync` | GET | `/traces/storages/{traceResourceId}/keys/` |
| `Traces.Storages[{resourceId}].Keys.PostAsync` | POST | `/traces/storages/{traceResourceId}/keys/` |
| `Traces.Storages[{resourceId}].Keys[{uid}].GetAsync` | GET | `/traces/storages/{traceResourceId}/keys/{uid}/` |
| `Traces.Storages[{resourceId}].Keys[{uid}].PutAsync` | PUT | `/traces/storages/{traceResourceId}/keys/{uid}/` |
| `Traces.Storages[{resourceId}].Keys[{uid}].PatchAsync` | PATCH | `/traces/storages/{traceResourceId}/keys/{uid}/` |
| `Traces.Storages[{resourceId}].Keys[{uid}].DeleteAsync` | DELETE | `/traces/storages/{traceResourceId}/keys/{uid}/` |

## 使用方法
本 SDK を使用して MonitoringSuite API を呼び出す方法について記載します。
### 認証

#### API キーを使用する場合

1.「事前セットアップ」で発行した APIキー の「アクセストークン (UUID)」と「アクセストークンシークレット」を文字列として用意します。

```csharp
string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";
```

2.Kiota が提供する `BasicAuthenticationProvider` を使用して、 Basic 認証用の認証プロバイダを作成します。 Base64 エンコードや `Authorization` ヘッダーの組み立ては内部で自動的に行われます。
```csharp
BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);
```
3.認証プロバイダを用いて、 `HttpClientRequestAdapter` のインスタンスを作成します。
```csharp
using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);

```
#### サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合

1.「事前セットアップ」で発行したアクセストークンを文字列として用意します。
```csharp
string accessToken = "<YOUR_ACCESS_TOKEN>";
```

2.Kiota が提供する `ApiKeyAuthenticationProvider` を使用して、 Bearer 認証用の認証プロバイダを作成します。

```csharp
ApiKeyAuthenticationProvider authProvider = new ApiKeyAuthenticationProvider(
    $"Bearer {accessToken}",
    "Authorization",
    ApiKeyAuthenticationProvider.KeyLocation.Header);
```

3.認証プロバイダを用いて、 `HttpClientRequestAdapter` のインスタンスを作成します。
```csharp
using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
```

#### コードベースでアクセストークンを取得する場合
サービスプリンシパルキーを使用して発行したアクセストークンを使用する場合、コードからアクセストークンを取得することが可能です。
各サービス SDK の共通ライブラリである `Sacloud.Sdk.Shared` プロジェクトにある `Sacloud.Sdk.Shared.Helpers.AccessTokenHelper` クラスを利用することができます。

```csharp
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

string jwt = "<YOUR_JWT>";

// ログの利用は任意のため、必要ない場合はILoggerFactoryのインスタンス生成は不要です。
// APIクライアントと同じILoggerFactoryのインスタンスを使用することを推奨します。
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

ServicePrincipalOAuth2AccessToken token = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory);
string accessToken = token.AccessToken;
```

JWT の生成方法は以下を参照してください。

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#id14)

なお、取得したアクセストークンは有効期限があります。
期限切れの場合は `GetAccessTokenAsync` を再度呼び出してアクセストークンを取得し直してください。
有効期限は `ServicePrincipalOAuth2AccessToken.ExpiresIn` (秒)で確認できます。

### クライアントのセットアップ
1.本 SDK で出力するログを受け取るための `ILoggerFactory` インスタンスを作成します。
    ログを受け取る必要がない場合はこの手順をスキップしてください。

```csharp
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
    // 必要に応じてログレベルや出力先などの設定を行う
});
```
2.「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、 `MonitoringSuiteClient` のインスタンスを作成します。
```csharp
MonitoringSuiteClient client = new MonitoringSuiteClient(adapter, loggerFactory);
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

MonitoringSuiteClient client = new MonitoringSuiteClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレットなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。


### 使用例

#### アラートプロジェクト一覧の取得

```csharp
PaginatedAlertProjectList projects = await client.Alerts.Projects.GetAsync();
```

#### ログストレージの一覧取得

```csharp
PaginatedLogStorageList storages = await client.Logs.Storages.GetAsync();
```

#### メトリクスストレージの作成

```csharp
MetricsStorageCreateRequest request = new MetricsStorageCreateRequest
{
    Name = "example-metrics-storage"
};

MetricsStorage created = await client.Metrics.Storages.PostAsync(request);
```

#### 初期化プロビジョニングの実行

```csharp
ProvisioningCreateRequest request = new ProvisioningCreateRequest
{
    Logs = new ProvisioningExistRequest { SystemExist = true, UserExist = true },
    Metrics = new ProvisioningExistRequest { SystemExist = true, UserExist = true }
};

Provisioning provisioning = await client.Management.Provisioning.Initialize.PostAsync(request);
```

## エラー処理とページングの簡易ガイド

### 例外の扱い

MonitoringSuite API 呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using System.Collections.Generic;
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;

try
{
    PaginatedAlertProjectList projects = await client.Alerts.Projects.GetAsync();
    // 取得した結果を処理
}
catch (ApiException apiEx)
{
    Console.WriteLine($"API error: {apiEx.Message}");

    // 共通エラー型(ApiError)として返却されている場合は、レスポンスボディの詳細を参照できます。
    if (apiEx is ApiError apiError)
    {
        foreach (KeyValuePair<string, object> kv in apiError.ResponseBody)
        {
            Console.WriteLine($"{kv.Key}: {kv.Value}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

### 共通エラー型 (`ApiError`) と詳細情報の確認

MonitoringSuite SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、`ResponseBody` プロパティにエラーレスポンスボディの内容（フィールド名と値のディクショナリ）を保持します。
- MonitoringSuite 固有のエラー情報も、`ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから `ResponseBody` を参照することで、API ごとの詳細なエラー情報を確認できます。

### ページングと大量データ取得

MonitoringSuite の一覧取得 API では、クエリパラメータとして `count` / `from` を利用できます。
大量件数を扱う場合は、これらの値を指定して取得範囲を分割し、段階的にデータを取得することを推奨します。

具体的なパラメータ名や制約値は、MonitoringSuite API リファレンスおよび OpenAPI 定義を参照してください。
Kiota で生成されたクライアントでは、一覧取得メソッドに対してクエリパラメータを指定できます。
想定件数や処理負荷に応じて `count` と `from` を調整してください。

```csharp
// count/from を指定した一覧取得の例
PaginatedAlertProjectList projects = await client.Alerts.Projects.GetAsync(config =>
{
    config.QueryParameters.Count = 100;
    config.QueryParameters.From = 0;
});
```

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キー認証を使用している場合**、次の点を確認してください。

- MonitoringSuite を利用可能な API キーを使用していること
- アクセストークン (UUID) とアクセストークンシークレットを正しくコピーしていること
- `BasicAuthenticationProvider` の引数順序が `(accessToken, accessTokenSecret)` であること

**サービスプリンシパル認証を使用している場合**、次の点を確認してください。

- アクセストークンの有効期限が切れていないこと（`ServicePrincipalOAuth2AccessToken.ExpiresIn` または `ServicePrincipalOAuth2AccessToken.TokenExpiredAt` で確認できます）
- `Authorization` ヘッダーが `Bearer <ACCESS_TOKEN>` 形式で送信されていること
- JWT の署名に使用したサービスプリンシパルキーが有効であること

それでも解消しない場合は、`ApiError` の内容やログを確認し、レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。

### Q. タイムアウトや接続エラーが発生します

まず、開発環境から `https://secure.sakura.ad.jp` への通信経路（プロキシ設定やファイアウォール）に問題がないか確認してください。
ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、`HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか？

本 SDK は .NET Standard 2.0 対応のライブラリです。
そのため、.NET 6 以降を含む .NET Standard 2.0 互換のランタイムで利用できます。

### Q. `resource_id` と `uid` のどちらを使えばよいですか？

MonitoringSuite API では、操作対象によって識別子が異なります。

- プロジェクトやストレージなどの親リソースは、`resource_id`（または `log_resource_id` / `metrics_resource_id` / `trace_resource_id`）を使用します。
- ルール、通知先、API キーなど親配下の個別設定は、`uid` を使用します。

一覧 API のレスポンスで取得した識別子を、詳細取得・更新・削除 API にそのまま渡す運用を推奨します。

### Q. `management/provisioning/initialize` はいつ実行すべきですか？

`Management.Provisioning.Initialize.PostAsync` は、MonitoringSuite 利用開始時の初期化処理です。
通常は初回セットアップ時に実行し、状態確認は `Management.Provisioning.State.GetAsync` で行います。

運用中に毎回実行する API ではないため、初期化済みかを確認して必要な場合のみ呼び出してください。