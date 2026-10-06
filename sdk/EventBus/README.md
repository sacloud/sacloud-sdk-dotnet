# EventBus API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する EventBus API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの EventBus を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本READMEの内容を読めば、簡単な実行設定の操作まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `EventBus`
- サービス概要:
  - EventBus は、さくらのクラウド上でイベント駆動型の処理を構築するためのサービスです。
  - 実行設定・スケジュール・トリガーなどのリソースを API で操作できます。
- 本 SDK の目的:
  - さくらのクラウド「EventBus」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.EventBus` を提供します。
  - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、
    通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
  - C# 7.3 以降
  - .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
  - ファイルパス: `sdk/EventBus/openapi-spec/1.0.0/openapi.json`
  - OpenAPI バージョン: `3.1.0`
- EventBus API バージョン:
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

EventBus SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### 1-A. API キーを使用する
認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。

- [APIリファレンス | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/api/cloud/eventbus/)

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
dotnet add package Sacloud.Sdk.EventBus
```

## クイックスタート

ここでは、最小限のコードで EventBus API にアクセスし、リソース一覧を取得するまでの流れを示します。
認証には API キー を使用します。

1. 任意のディレクトリでコンソールアプリを作成します。

```bash
dotnet new console -n EventBusQuickStart
cd EventBusQuickStart
dotnet add package Sacloud.Sdk.EventBus
```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

```csharp
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.EventBus;
using Sacloud.Sdk.EventBus.Commonserviceitem;
using Sacloud.Sdk.EventBus.Models;

// 1. 認証情報を設定
string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";

BasicAuthenticationProvider authProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);

using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);

// 2. ロガーとクライアントを初期化
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

EventBusClient client = new EventBusClient(adapter, loggerFactory);

try
{
    // 3. リソース一覧を取得
    CommonserviceitemGetResponse result = await client.Commonserviceitem.GetAsync();
    Console.WriteLine("CommonServiceItems:");

    if (result?.CommonServiceItems != null)
    {
        foreach (CommonServiceItem item in result.CommonServiceItems)
        {
            Console.WriteLine($"- {item.ID}: {item.Name}");
        }
    }
}
catch (Exception ex)
{
    // 簡易的なエラー処理
    Console.WriteLine($"Error while calling EventBus API: {ex.Message}");
}
```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

```bash
dotnet run
```

より詳細な認証手順や、実行設定・スケジュール・トリガーの作成・更新・削除などの操作については、後述の「使用方法」セクションを参照してください。

より多くの操作例や対話的なメニュー形式での実行例については、リポジトリ内のサンプルプロジェクトを参照してください。

- サンプル: `sdk/EventBus/samples/`

このディレクトリには、実際の EventBus リソースに対して
SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、
サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/EventBus/samples/EventBus.Samples.csproj
```

実行前に、`Program.cs` 内の `<YOUR_API_KEY_ACCESS_TOKEN>` と
`<YOUR_API_KEY_ACCESS_TOKEN_SECRET>` を適切な値に置き換えてください。

## SDK のメソッドと API エンドポイントの対応表

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Commonserviceitem.GetAsync` | GET | `/commonserviceitem` |
| `Commonserviceitem.PostAsync` | POST | `/commonserviceitem` |
| `Commonserviceitem[{id}].GetAsync` | GET | `/commonserviceitem/{id}` |
| `Commonserviceitem[{id}].PutAsync` | PUT | `/commonserviceitem/{id}` |
| `Commonserviceitem[{id}].DeleteAsync` | DELETE | `/commonserviceitem/{id}` |
| `Commonserviceitem[{id}].Eventbus.Processconfiguration.SetSecret.PutAsync` | PUT | `/commonserviceitem/{id}/eventbus/processconfiguration/set-secret` |

## 使用方法

本 SDK を使用して EventBus API を呼び出す方法について記載します。

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
using Microsoft.Extensions.Logging;
using Sacloud.Sdk.EventBus;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    // 必要に応じてログレベルや出力先などの設定を行う
});

EventBusClient client = new EventBusClient(adapter, loggerFactory);
```

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`EventBusClient` のインスタンスを作成します。

```csharp
EventBusClient client = new EventBusClient(adapter, loggerFactory);
```

ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。

```csharp
EventBusClient client = new EventBusClient(adapter);
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

EventBusClient client = new EventBusClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレット、パスワードなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

#### 一覧取得

```csharp
CommonserviceitemGetResponse result = await client.Commonserviceitem.GetAsync();
```

#### 作成

```csharp
using Sacloud.Sdk.EventBus.Models;

CreateCommonServiceItemRequest body = new CreateCommonServiceItemRequest
{
    CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
    {
        Name = "sample-resource-name",
        Description = "SDK sample",
        Settings = new SettingsRequest
        {
            ProcessConfigurationSettings = new ProcessConfigurationSettings
            {
                Destination = ProcessConfigurationSettings_Destination.Simplemq,
                Parameters = "{\"queue_name\": \"sample-queue\", \"content\": \"sampleContent\"}"
            }
        },
        Provider = new Provider
        {
            Class = Provider_Class.Eventbusprocessconfiguration
        }
    }
};

CommonserviceitemPostResponse created = await client.Commonserviceitem.PostAsync(body);
```

#### 詳細取得

```csharp
CommonserviceitemGetResponse detail = await client.Commonserviceitem["<RESOURCE_ID>"].GetAsync();
```

#### 更新

```csharp
UpdateCommonServiceItemRequest updateBody = new UpdateCommonServiceItemRequest
{
    CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
    {
        Name = "sample-resource-name-updated"
    }
};

CommonserviceitemPutResponse updated = await client.Commonserviceitem["<RESOURCE_ID>"].PutAsync(updateBody);
```

#### 削除

```csharp
CommonserviceitemDeleteResponse deleted = await client.Commonserviceitem["<RESOURCE_ID>"].DeleteAsync();
```

#### 実行設定の Secret 設定

```csharp
SetSecretRequest secretBody = new SetSecretRequest
{
    Secret = new SetSecretRequest.SetSecretRequest_Secret
    {
        SimpleMQSecret = new SimpleMQSecret
        {
            APIKey = "<YOUR_SIMPLE_MQ_API_KEY>"
        }
    }
};

SetSecretPutResponse secretResult = await client.Commonserviceitem["<RESOURCE_ID>"]
    .Eventbus
    .Processconfiguration
    .SetSecret
    .PutAsync(secretBody);
```

## エラー処理の簡易ガイド

### 例外の扱い

EventBus API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using System.Collections.Generic;
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.EventBus.Commonserviceitem;
using Sacloud.Sdk.Shared.Models;

try
{
    CommonserviceitemGetResponse result = await client.Commonserviceitem.GetAsync();
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

EventBus SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型
`Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、
  `ResponseBody` プロパティにエラーレスポンスボディの内容（フィールド名と値のディクショナリ）を保持します。
- EventBus 固有のエラー情報も、この `ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから
`ResponseBody` を参照することで、APIごとの詳細なエラー情報を確認できます。

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キーを使用している場合**、次の点を確認してください。

- EventBus 用の API キーを発行していること
- アクセストークン（UUID）とアクセストークンシークレットを正しくコピーしていること
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
