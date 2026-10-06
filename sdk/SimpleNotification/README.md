# SimpleNotification API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する SimpleNotification (シンプル通知) API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの SimpleNotification を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本 README の内容を読めば、通知先・通知先グループ・通知ルーティングの基本操作と、通知送信・履歴参照まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `SimpleNotification` (シンプル通知)
- サービス概要:
  - メールや Webhook を利用して、任意のタイミングで通知を送信するためのサービスです。
  - 通知先、通知先グループ、通知ルーティングの管理に加えて、メッセージ送信や通知履歴の参照を API 経由で行えます。
- 本 SDK の目的:
  - さくらのクラウド「SimpleNotification」向けの C# API クライアントライブラリ `Sacloud.Sdk.SimpleNotification` を提供します。
  - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
  - C# 7.3 以降
  - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
  - ファイルパス: `sdk/SimpleNotification/openapi-spec/1.0.0/openapi.json`
  - OpenAPI バージョン: `3.0.1`
- SimpleNotification API バージョン:
  - `info.version`: `1.0.0`
- ベース URL:
  - `https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1`
  - 本 SDK からのすべての API 呼び出しは、上記ベース URL を基点として実行されます。
- 認証方式（概要）:
  - HTTP Basic 認証
    - ユーザー名: API キーの「アクセストークン (UUID)」
    - パスワード: API キーの「アクセストークンシークレット」
    - ヘッダー形式: `Authorization: Basic <base64(user:password)>`
    - OpenAPI の `components.securitySchemes` には定義がありませんが、`info.description` に記載された仕様に基づいて Basic 認証として扱います。
  - Bearer 認証
    - アクセストークン： サービスプリンシパルキーを使用し発行したアクセストークン
    - ヘッダー形式： `Authorization: Bearer <AccessToken>`

## 事前セットアップ

### 認証情報の準備

SimpleNotification SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### API キーを使用する

認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。  

- [API キー | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/api/apikey.html#api)

#### サービスプリンシパルキーを使用して発行したアクセストークンを使用する

認証にサービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従い アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
  - .NET Standard 2.0 でサポートされているバージョンであること
  - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```bash
dotnet add package Sacloud.Sdk.SimpleNotification
```

## クイックスタート

ここでは、最小限のコードで SimpleNotification API にアクセスし、リソース一覧を取得するまでの流れを示します。  
認証には API キー を使用します。  

1. 任意のディレクトリでコンソールアプリケーションを作成します。

```bash
dotnet new console -n SimpleNotificationQuickStart
cd SimpleNotificationQuickStart
dotnet add package Sacloud.Sdk.SimpleNotification
```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

```csharp
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.SimpleNotification;
using Sacloud.Sdk.SimpleNotification.Models;

string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";

BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);

using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

SimpleNotificationClient client = new SimpleNotificationClient(adapter, loggerFactory);

try
{
    ListCommonServiceItemsResponse result = await client.Commonserviceitem.GetAsync();
    Console.WriteLine("CommonServiceItems:");

    if (result?.CommonServiceItems is not null)
    {
        foreach (CommonServiceItem item in result.CommonServiceItems)
        {
            Console.WriteLine($"- {item.ID}: {item.Name} ({item.Provider?.Class})");
        }
    }
    else
    {
        Console.WriteLine("No resources found.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error while calling SimpleNotification API: {ex.Message}");
}
```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

```bash
dotnet run
```

より詳細な認証手順や、通知先・通知先グループ・通知ルーティングの作成 / 更新 / 削除、通知送信、履歴参照については、後述の「使用方法」セクションを参照してください。

より多くの操作例や対話メニュー形式での実行例については、リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/SimpleNotification/samples/`

このディレクトリには、実際の SimpleNotification リソースに対して SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/SimpleNotification/samples/SimpleNotification.Samples.csproj
```

実行前に、`Program.cs` 内の以下のプレースホルダーを適切な値に置き換えてください。

- `<YOUR_API_KEY_ACCESS_TOKEN>`
- `<YOUR_API_KEY_ACCESS_TOKEN_SECRET>`
- `<YOUR_JSON_WEB_TOKEN>`（サービスプリンシパル認証を使用する場合）

## SDK のメソッドと API エンドポイントの対応表

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Commonserviceitem.GetAsync` | GET | `/commonserviceitem` |
| `Commonserviceitem.PostAsync` | POST | `/commonserviceitem` |
| `Commonserviceitem[{id}].GetAsync` | GET | `/commonserviceitem/{id}` |
| `Commonserviceitem[{id}].PutAsync` | PUT | `/commonserviceitem/{id}` |
| `Commonserviceitem[{id}].DeleteAsync` | DELETE | `/commonserviceitem/{id}` |
| `Commonserviceitem[{id}].Simplenotification.Status.GetAsync` | GET | `/commonserviceitem/{id}/simplenotification/status` |
| `Commonserviceitem[{id}].Simplenotification.Message.PostAsync` | POST | `/commonserviceitem/{id}/simplenotification/message` |
| `Commonserviceitem.Simplenotification.History.GetAsync` | GET | `/commonserviceitem/simplenotification/history` |
| `Commonserviceitem.Simplenotification.History[{request_id}].GetAsync` | GET | `/commonserviceitem/simplenotification/history/{request_id}` |
| `Commonserviceitem.Simplenotification.Sources.GetAsync` | GET | `/commonserviceitem/simplenotification/sources` |
| `Commonserviceitem.Simplenotification.Routing.Reorder.PutAsync` | PUT | `/commonserviceitem/simplenotification/routing/reorder` |

## 使用方法

本 SDK を使用して SimpleNotification API を呼び出す方法について記載します。

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

string jwt = "<YOUR_JSON_WEB_TOKEN>";

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

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`SimpleNotificationClient` のインスタンスを作成します。

```csharp
SimpleNotificationClient client = new SimpleNotificationClient(adapter, loggerFactory);
```

ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。

```csharp
SimpleNotificationClient client = new SimpleNotificationClient(adapter);
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

SimpleNotificationClient client = new SimpleNotificationClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン / シークレットなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### リソース種別の考え方

SimpleNotification では、主に次の 3 種類のリソースを `CommonServiceItem` として扱います。

- 通知先
  - `Provider.Class`: `saknoticedestination`
  - `ServiceClass`: `cloud/saknoticedestination/2`
  - `DestinationSettings.Type` は `email` または `webhook` を指定します。
- 通知先グループ
  - `Provider.Class`: `saknoticegroup`
  - `ServiceClass`: `cloud/saknoticegroup/2`
  - 既存の通知先 ID 一覧を `GroupSettings.Destinations` に指定します。
- 通知ルーティング
  - `Provider.Class`: `saknoticerouting`
  - `ServiceClass`: `cloud/saknoticerouting/2`
  - 通知元 ID、通知先グループ ID、ラベル条件などを `RoutingSettings` に指定します。

### 使用例

#### リソース一覧の取得

```csharp
ListCommonServiceItemsResponse result = await client.Commonserviceitem.GetAsync();
```

#### 通知先だけを絞り込んで取得

SimpleNotification API の一覧取得では、`Provider.Class` を JSON 形式のクエリ文字列で渡して絞り込みます。
この SDK では `WithUrl` を用いてフィルタ付き URL を指定できます。

```csharp
string filter = Uri.EscapeDataString("{\"Filter\":{\"Provider.Class\":\"saknoticedestination\"}}");
ListCommonServiceItemsResponse destinations = await client.Commonserviceitem
    .WithUrl($"{adapter.BaseUrl}/commonserviceitem?{filter}")
    .GetAsync();
```

#### 通知先の作成

```csharp
using Sacloud.Sdk.SimpleNotification.Models;

PostCommonServiceItemRequest body = new PostCommonServiceItemRequest
{
    CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
    {
        Name = "sample-destination",
        Description = "SimpleNotification sample destination",
        Provider = new CommonServiceItemProvider
        {
            Class = CommonServiceItemProvider_Class.Saknoticedestination,
            Name = "SimpleNotification"
        },
        ServiceClass = CommonServiceItemServiceClass.CloudSaknoticedestination2,
        Settings = new CommonServiceItemSettings
        {
            DestinationSettings = new DestinationSettings
            {
                Disabled = false,
                Type = DestinationSettings_Type.Email,
                Value = "user@example.com"
            }
        },
        Tags = new List<string> { "sample" }
    }
};

CommonserviceitemPostResponse created = await client.Commonserviceitem.PostAsync(body);
```

#### 通知先グループの作成

```csharp
PostCommonServiceItemRequest body = new PostCommonServiceItemRequest
{
    CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
    {
        Name = "sample-group",
        Description = "SimpleNotification sample group",
        Provider = new CommonServiceItemProvider
        {
            Class = CommonServiceItemProvider_Class.Saknoticegroup,
            Name = "SimpleNotification"
        },
        ServiceClass = CommonServiceItemServiceClass.CloudSaknoticegroup2,
        Settings = new CommonServiceItemSettings
        {
            GroupSettings = new GroupSettings
            {
                Disabled = false,
                Destinations = new List<string>
                {
                    "<DESTINATION_ID_1>",
                    "<DESTINATION_ID_2>"
                }
            }
        },
        Tags = new List<string> { "sample" }
    }
};

CommonserviceitemPostResponse created = await client.Commonserviceitem.PostAsync(body);
```

#### 通知ルーティングの作成

```csharp
PostCommonServiceItemRequest body = new PostCommonServiceItemRequest
{
    CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
    {
        Name = "sample-routing",
        Description = "SimpleNotification sample routing",
        Provider = new CommonServiceItemProvider
        {
            Class = CommonServiceItemProvider_Class.Saknoticerouting,
            Name = "SimpleNotification"
        },
        ServiceClass = CommonServiceItemServiceClass.CloudSaknoticerouting2,
        Settings = new CommonServiceItemSettings
        {
            RoutingSettings = new RoutingSettings
            {
                SourceID = "<SOURCE_ID>",
                TargetGroupID = "<GROUP_ID>",
                MatchLabels = new List<RoutingSettings_MatchLabels>
                {
                    new RoutingSettings_MatchLabels
                    {
                        Name = "environment",
                        Value = "production"
                    }
                }
            }
        },
        Tags = new List<string> { "sample" }
    }
};

CommonserviceitemPostResponse created = await client.Commonserviceitem.PostAsync(body);
```

#### リソース詳細の取得

```csharp
CommonserviceitemGetResponse detail = await client.Commonserviceitem["<RESOURCE_ID>"].GetAsync();
```

#### リソースの更新

```csharp
PutCommonServiceItemRequest body = new PutCommonServiceItemRequest
{
    CommonServiceItem = new PutCommonServiceItemRequest_CommonServiceItem
    {
        Name = "sample-destination-updated",
        Description = "Updated destination",
        Settings = new CommonServiceItemSettings
        {
            DestinationSettings = new DestinationSettings
            {
                Disabled = false,
                Type = DestinationSettings_Type.Email,
                Value = "updated@example.com"
            }
        },
        Tags = new List<string> { "sample", "updated" }
    }
};

CommonserviceitemPutResponse updated = await client.Commonserviceitem["<RESOURCE_ID>"].PutAsync(body);
```

#### リソースの削除

```csharp
CommonserviceitemDeleteResponse deleted = await client.Commonserviceitem["<RESOURCE_ID>"].DeleteAsync();
```

#### 通知先ステータスの取得

```csharp
GetCommonServiceItemStatusResponse status = await client.Commonserviceitem["<DESTINATION_ID>"]
.Simplenotification
.Status
.GetAsync();
```

#### 通知メッセージの送信

指定先は通知先グループ ID です。

```csharp
SendNotificationMessageRequest body = new SendNotificationMessageRequest
{
    Message = "SimpleNotification sample message"
};

SendNotificationMessageResponse response = await client.Commonserviceitem["<GROUP_ID>"]
.Simplenotification
.Message
.PostAsync(body);
```

#### 通知履歴一覧の取得

直近 30 日以内の通知送信履歴を最大 100 件まで取得できます。

```csharp
ListSimpleNotificationHistoriesResponse histories = await client.Commonserviceitem
.Simplenotification
.History
.GetAsync();
```

#### 通知履歴詳細の取得

```csharp
GetSimpleNotificationHistoryResponse history = await client.Commonserviceitem
.Simplenotification
.History["<REQUEST_ID>"]
.GetAsync();
```

#### 通知元一覧の取得

```csharp
ListSourcesResponse sources = await client.Commonserviceitem
.Simplenotification
.Sources
.GetAsync();
```

#### 通知ルーティング優先順位の並び替え

`PriorityRank` は他の通知ルーティングと重複しないように指定してください。

```csharp
PutCommonServiceItemRoutingReorderRequest body = new PutCommonServiceItemRoutingReorderRequest
{
    Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
    {
        new PutCommonServiceItemRoutingReorderRequest_Orders
        {
            RoutingID = "<ROUTING_ID_1>",
            PriorityRank = 1
        },
        new PutCommonServiceItemRoutingReorderRequest_Orders
        {
            RoutingID = "<ROUTING_ID_2>",
            PriorityRank = 2
        }
    }
};

ReorderPutResponse result = await client.Commonserviceitem
.Simplenotification
.Routing
.Reorder
.PutAsync(body);
```

## エラー処理とページングの簡易ガイド

### 例外の扱い

SimpleNotification API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;

try
{
    ListCommonServiceItemsResponse result = await client.Commonserviceitem.GetAsync();
}
catch (ApiException apiEx)
{
    Console.WriteLine($"API error: {apiEx.Message}");

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

### 共通エラー型(`ApiError`)と詳細情報の確認

SimpleNotification SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、`ResponseBody` プロパティにエラーレスポンスボディの内容（フィールド名と値のディクショナリ）を保持します。
- SimpleNotification 固有のエラー情報も、この `ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから `ResponseBody` を参照することで、API ごとの詳細なエラー情報を確認できます。

### ページングと大量データ取得

`/commonserviceitem` の一覧取得結果には、`Count` / `From` / `Total` が含まれます。
大量件数を扱う場合は、これらの値を確認しながら取得状況を管理することを推奨します。

具体的なパラメータ名や制約値は、SimpleNotification API リファレンスおよび OpenAPI 定義を参照してください。

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キーを使用している場合**、次の点を確認してください。

- SimpleNotification を利用可能な API キーを使用していること
- アクセストークン (UUID) とアクセストークンシークレットを正しくコピーしていること
- `"<ACCESS_TOKEN>:<ACCESS_TOKEN_SECRET>"` の形式で文字列連結し、UTF-8 で Base64 エンコードしていること
- `Authorization` ヘッダーが `Basic <base64>` 形式で送信されていること

**サービスプリンシパルキーを使用して発行したアクセストークンを使用している場合**、次の点を確認してください。

- アクセストークンの有効期限が切れていないこと（`ExpiresIn` または `TokenExpiredAt` で確認）
- `Authorization` ヘッダーが `Bearer <ACCESS_TOKEN>` 形式で送信されていること
- JWT の署名に使用したサービスプリンシパルキーが有効であること

それでも解消しない場合は、`ApiError` の内容やログを確認し、レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。

### Q. リソース一覧を通知先や通知ルーティングだけに絞り込めますか

可能です。
一覧 API は `Provider.Class` を JSON 形式のクエリとして受け取るため、`WithUrl` を用いてフィルタ付き URL を指定してください。

- 通知先: `saknoticedestination`
- 通知先グループ: `saknoticegroup`
- 通知ルーティング: `saknoticerouting`

### Q. タイムアウトや接続エラーが発生します

まず、開発環境から `https://secure.sakura.ad.jp` への通信経路（プロキシ設定やファイアウォール）に問題がないか確認してください。
ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、`HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか

本 SDK は .NET Standard 2.0 対応のライブラリです。
そのため、.NET 6 以降を含む .NET Standard 2.0 互換ランタイムで利用できます。
