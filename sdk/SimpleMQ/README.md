# SimpleMQ API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する SimpleMQ API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの SimpleMQ を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本 README の内容を読めば、メッセージの送信・受信・タイムアウト延長・削除まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `SimpleMQ`（シンプルMQ）
- サービス概要:
	- アプリケーション間で非同期にデータを受け渡すための、マネージド型メッセージキューサービスです。
	- キューを使って producer（送信側）と consumer（受信側）を疎結合にし、処理の平準化や再試行制御を実現できます。
- 本 SDK の目的:
	- さくらのクラウド「SimpleMQ」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.SimpleMQ` を提供します。
	- 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、
      通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
	- C# 7.3 以降
	- .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
    - ファイルパス: `sdk/SimpleMQ/openapi-spec/1.1.0/openapi.kiota.json`
    - OpenAPI バージョン: 3.1.0
- SimpleMQ API バージョン:
    - `info.version`: `1.1.0`
- ベース URL:
    - `https://simplemq.tk1b.api.sacloud.jp/v1`
    - 本 SDK からのすべての API 呼び出しは、上記ベース URL を基点として実行されます。
    - API ドキュメントでは各エンドポイントに `/v1` が含まれて記載されていますが、本 SDK のベース URL はあらかじめ `/v1` を含んでいます。
      そのため、SDK 利用時に `/v1` を重ねて指定する必要はありません。
- 認証方式（概要）:
    - HTTP Bearer 認証
    - 認証情報: キュー作成時に発行される API キー
    - ヘッダー形式: `Authorization: Bearer <API_KEY>`

## 事前セットアップ

下記の API マニュアルにしたがって、SimpleMQ のキューを作成し、API キーを取得します。

- [APIキー | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/api/apikey.html)

### 必要なツール

- 次の条件を満たす .NET SDK
    - .NET Standard 2.0 でサポートされているバージョンであること
    - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。

```bash
dotnet add package Sacloud.Sdk.SimpleMQ
```

## クイックスタート

ここでは、最小限のコードで SimpleMQ API にアクセスし、キューへのメッセージ送信を行う手順を示します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

```bash
dotnet new console -n SimpleMQQuickStart
cd SimpleMQQuickStart
dotnet add package Sacloud.Sdk.SimpleMQ
```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

```csharp
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.SimpleMQ.Models;

// 1. 認証情報を設定（キュー作成時に発行された API キー）
string queueApiKey = "<YOUR_QUEUE_API_KEY>";
string authHeaderValue = $"Bearer {queueApiKey}";

ApiKeyAuthenticationProvider authProvider = new ApiKeyAuthenticationProvider(
    authHeaderValue,
    "Authorization",
    ApiKeyAuthenticationProvider.KeyLocation.Header);

using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);

// 2. ロガーとクライアントを初期化
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

Sacloud.Sdk.SimpleMQ.SimpleMQClient client = new Sacloud.Sdk.SimpleMQ.SimpleMQClient(adapter, loggerFactory);

// 3. 操作対象のキュー名を指定
string queueName = "<YOUR_QUEUE_NAME>";

try
{
    // 4. メッセージを送信
    SendRequest body = new SendRequest
    {
        Content = "U2ltcGxlTVEtUXVpY2tTdGFydA==" // Base64 文字列
    };

    Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.MessagesPostResponse response = await client.Queues[queueName].Messages.PostAsync(body);

    Console.WriteLine($"Send result: {response?.Result}");
    Console.WriteLine($"Message ID: {response?.Message?.Id}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error while calling SimpleMQ API: {ex.Message}");
}
```

3. 以下のコマンドで実行し、送信結果を確認します。

```bash
dotnet run
```

より詳細な認証手順や、メッセージ送信・受信・タイムアウト延長・削除などの操作については、
後述の「使用方法」セクションを参照してください。

より多くの操作例や対話的な実行例については、リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/SimpleMQ/samples/`

このディレクトリには、実際の SimpleMQ アプリケーションリソースに対して
SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（Sacloud.Sdk.slnx があるディレクトリ）で次のコマンドを実行すると、
サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/SimpleMQ/samples/SimpleMQ.Samples.csproj
```

実行前に、`Program.cs` 内の `<YOUR_QUEUE_API_KEY>` と `SimpleMQOperations.cs` 内の `<YOUR_QUEUE_NAME>` を適切な値に置き換えてください。

## SDK のメソッドと API エンドポイントの対応表

### メッセージ

| SDK メソッド | HTTP メソッド | API エンドポイント |
|--------------|----------------|--------------------|
| `Queues[{queueName}].Messages.PostAsync` | POST | `/queues/{queueName}/messages` |
| `Queues[{queueName}].Messages.GetAsync` | GET | `/queues/{queueName}/messages` |
| `Queues[{queueName}].Messages[{messageId}].PutAsync` | PUT | `/queues/{queueName}/messages/{messageId}` |
| `Queues[{queueName}].Messages[{messageId}].DeleteAsync` | DELETE | `/queues/{queueName}/messages/{messageId}` |

## 使用方法

本 SDK を使用して SimpleMQ API を呼び出す方法について記載します。

### 認証

1. キュー作成時に発行した API キーを文字列として用意します。

```csharp
string queueApiKey = "<YOUR_QUEUE_API_KEY>";
```

2. `Authorization` ヘッダーに `Bearer` 形式で設定します。

```csharp
string authHeaderValue = $"Bearer {queueApiKey}";
ApiKeyAuthenticationProvider authProvider = new ApiKeyAuthenticationProvider(
    authHeaderValue,
    "Authorization",
    ApiKeyAuthenticationProvider.KeyLocation.Header);
```

3. 認証プロバイダを使って `HttpClientRequestAdapter` を作成します。

```csharp
using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);
```

### クライアントのセットアップ

1. 本 SDK で出力するログを受け取るための `ILoggerFactory` インスタンスを作成します。  
   ログを受け取る必要がない場合はこの手順をスキップしてください。

```csharp
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    // 必要に応じてログレベルや出力先などの設定を行う
});
```

2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`SimpleMQClient` のインスタンスを作成します。

```csharp
Sacloud.Sdk.SimpleMQ.SimpleMQClient client = new Sacloud.Sdk.SimpleMQ.SimpleMQClient(adapter, loggerFactory);
```

ログを受け取る必要がない場合は `ILoggerFactory` 引数を省略してインスタンスを作成できます。

```csharp
Sacloud.Sdk.SimpleMQ.SimpleMQClient client = new Sacloud.Sdk.SimpleMQ.SimpleMQClient(adapter);
```

### ログ出力と運用上の注意

本 SDK は `Microsoft.Extensions.Logging` を利用して API 呼び出しの開始・終了やエラー情報などをログとして出力します。
運用環境では、適切なログレベルと出力先を設定することで、トラブルシュートや監視に役立てることができます。

```csharp
using Microsoft.Extensions.Logging;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.SetMinimumLevel(LogLevel.Information);
    builder.AddConsole();
});

Sacloud.Sdk.SimpleMQ.SimpleMQClient client = new Sacloud.Sdk.SimpleMQ.SimpleMQClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値や API キーなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

#### メッセージ送信 (enqueue)

```csharp
SendRequest body = new SendRequest
{
    Content = "U2FtcGxlQ29udGVudA=="
};

Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.MessagesPostResponse sendResult = await client.Queues["<YOUR_QUEUE_NAME>"].Messages.PostAsync(body);
Console.WriteLine(sendResult?.Message?.Id);
```

#### メッセージ受信 (dequeue)

```csharp
using Sacloud.Sdk.SimpleMQ.Models;

Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.MessagesGetResponse receiveResult = await client.Queues["<YOUR_QUEUE_NAME>"].Messages.GetAsync();
if (receiveResult?.Messages is not null)
{
    foreach (Message message in receiveResult.Messages)
    {
        Console.WriteLine($"{message.Id}: {message.Content}");
    }
}
```

#### メッセージタイムアウト延長

```csharp
Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.Item.WithMessagePutResponse extendResult = await client
    .Queues["<YOUR_QUEUE_NAME>"]
    .Messages["<YOUR_MESSAGE_ID>"]
    .PutAsync();

Console.WriteLine(extendResult?.Result);
```

#### メッセージ削除 (ack)

```csharp
Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.Item.WithMessageDeleteResponse deleteResult = await client
    .Queues["<YOUR_QUEUE_NAME>"]
    .Messages["<YOUR_MESSAGE_ID>"]
    .DeleteAsync();

Console.WriteLine(deleteResult?.Result);
```

## エラー処理の簡易ガイド

### 例外の扱い

SimpleMQ API 呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特にHTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using System.Collections.Generic;
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;

try
{
    Sacloud.Sdk.SimpleMQ.Queues.Item.Messages.MessagesGetResponse result = await client.Queues["<YOUR_QUEUE_NAME>"].Messages.GetAsync();
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

### 共通エラー型(ApiError)と詳細情報の確認

SimpleMQ SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型
`Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、
  `ResponseBody` プロパティにエラーレスポンスボディの内容（フィールド名と値のディクショナリ）を保持します。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから
`ResponseBody` を参照することで、APIごとの詳細なエラー情報を確認できます。

## よくある質問

### Q. `401 Unauthorized` が返ってきます

次の点を確認してください。

- キュー作成時に発行された API キーを使用していること
- `Authorization` ヘッダーが `Bearer <API_KEY>` 形式になっていること
- API キーの前後に不要な空白や改行が含まれていないこと

### Q. `404 Not Found` が返ってきます

`PUT /queues/{queueName}/messages/{messageId}` または `DELETE /queues/{queueName}/messages/{messageId}` では、指定した `messageId` が存在しない場合に 404 が返ることがあります。
`messageId` は受信済みメッセージの ID を指定してください。

### Q. タイムアウトや接続エラーが発生します

まず、開発環境から `https://simplemq.tk1b.api.sacloud.jp` への通信経路（プロキシ・ファイアウォール）に問題がないか確認してください。
ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、
`HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか？

本 SDK は .NET Standard 2.0 対応のライブラリです。
そのため、.NET 6 以降を含む .NET Standard 2.0 互換のランタイムで利用できます。
