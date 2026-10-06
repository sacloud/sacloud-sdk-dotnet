# ワークフロー API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する ワークフロー API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの ワークフロー を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本 README の内容を読めば、簡単なワークフロー操作まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `ワークフロー`（さくらのクラウド ワークフロー）
- サービス概要:
	- YAML 形式の Runbook を定義してワークフローを作成・実行できる PaaS 型のワークフロー自動化サービスです。
	- ワークフローの作成・管理・実行・リビジョン管理、および料金プランの設定を提供します。
- 本 SDK の目的:
	- さくらのクラウド「ワークフロー」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.Workflows` を提供します。
	- 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、
	  通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
	- C# 7.3 以降
	- .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
    - ファイルパス: `sdk/Workflows/openapi-spec/1.0.1/openapi.kiota.json`
    - OpenAPI バージョン: 3.0.0
- ワークフロー API バージョン:
    - `info.version`: `1.0.1`
- ベース URL:
    - `https://secure.sakura.ad.jp/cloud/zone/tk1b/api/workflow/1.0`
    - 本 SDK からのすべての API 呼び出しは、上記ベース URL を基点として実行されます。
- 認証方式（概要）:
    - HTTP Basic 認証
        - ユーザー名: API キーの「アクセストークン (UUID)」
        - パスワード: API キーの「アクセストークンシークレット」
        - ヘッダー形式: `Authorization: Basic <base64(user:password)>`
    - Bearer 認証
        - アクセストークン: サービスプリンシパルキーを使用して発行したアクセストークン
        - ヘッダー形式: `Authorization: Bearer <AccessToken>`

## 事前セットアップ

### 認証情報の準備

ワークフロー SDK は、認証に API キー または サービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
以下の手順に従い、事前に認証に必要なリソースを作成してください。  

#### API キーを使用する

認証に API キーを使用する場合は、下記ドキュメントに従い、さくらのクラウドの API キーを発行します。  

- [API リファレンス | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/api/cloud/workflows/)

#### サービスプリンシパルキーを使用して発行したアクセストークンを使用する

認証にサービスプリンシパルキーを使用して発行したアクセストークンを使用する場合は、下記ドキュメントに従い アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
    - .NET Standard 2.0 でサポートされているバージョンであること
    - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。
```
dotnet add package Sacloud.Sdk.Workflows
```

## クイックスタート

ここでは、最小限のコードで ワークフロー API にアクセスし、ワークフローの一覧を取得するまでの流れを示します。  
認証には API キー を使用します。  

1. 任意のディレクトリでコンソールアプリケーションを作成します。

    ```bash
    dotnet new console -n WorkflowsQuickStart
    cd WorkflowsQuickStart
    dotnet add package Sacloud.Sdk.Workflows
    ```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

    ```csharp
    using Microsoft.Extensions.Logging;
    using Microsoft.Kiota.Abstractions.Authentication;
    using Microsoft.Kiota.Http.HttpClientLibrary;
    using Sacloud.Sdk.Workflows;
    using Sacloud.Sdk.Workflows.Models;

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

    WorkflowsClient client = new WorkflowsClient(adapter, loggerFactory);

    try
    {
        // 3. ワークフローの一覧を取得
        ListWorkflowResponse workflows = await client.Workflows.GetAsync();

        Console.WriteLine("Workflows:");
        if (workflows?.Workflows is not null)
        {
            foreach (Workflow workflow in workflows.Workflows)
            {
                Console.WriteLine($"- {workflow.Id}: {workflow.Name}");
            }
        }
        else
        {
            Console.WriteLine("No workflows found.");
        }
    }
    catch (Exception ex)
    {
        // 簡易的なエラー処理
        Console.WriteLine($"Error while calling Workflows API: {ex.Message}");
    }
    ```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

    ```bash
    dotnet run
    ```

より詳細な認証手順や、ワークフローの追加・更新・削除・実行などの操作については、
後述の「使用方法」セクションを参照してください。

より多くの操作例や対話的なメニュー形式での実行例については、
リポジトリ内のサンプルプロジェクトを参照してください。

- サンプルプロジェクト: `sdk/Workflows/samples/`

このディレクトリには、実際の ワークフロー リソースに対して
SDK を介して各種操作を行うコンソールアプリケーションが含まれています。
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、
サンプルをビルド・実行できます。

```bash
dotnet run --project sdk/Workflows/samples/Workflows.Samples.csproj
```

実行前に、`Program.cs` 内の以下のプレースホルダーを適切な値に置き換えてください。

- `<YOUR_API_KEY_ACCESS_TOKEN>`
- `<YOUR_API_KEY_ACCESS_TOKEN_SECRET>`
- `<YOUR_JSON_WEB_TOKEN>`（サービスプリンシパル認証を使用する場合）

## SDK のメソッドと API エンドポイントの対応表

### ワークフローの管理

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Workflows.PostAsync` | POST | `/workflows` |
| `Workflows.GetAsync` | GET | `/workflows` |
| `Workflows.Suggest.GetAsync` | GET | `/workflows/suggest` |
| `Workflows[{id}].GetAsync` | GET | `/workflows/{id}` |
| `Workflows[{id}].PatchAsync` | PATCH | `/workflows/{id}` |
| `Workflows[{id}].DeleteAsync` | DELETE | `/workflows/{id}` |
| `Workflows[{id}].Revisions.PostAsync` | POST | `/workflows/{id}/revisions` |
| `Workflows[{id}].Revisions.GetAsync` | GET | `/workflows/{id}/revisions` |
| `Workflows[{id}].Revisions[{revisionId}].GetAsync` | GET | `/workflows/{id}/revisions/{revisionId}` |
| `Workflows[{id}].Revisions[{revisionId}].Revision_alias.PutAsync` | PUT | `/workflows/{id}/revisions/{revisionId}/revision_alias` |
| `Workflows[{id}].Revisions[{revisionId}].Revision_alias.DeleteAsync` | DELETE | `/workflows/{id}/revisions/{revisionId}/revision_alias` |

### ワークフローの実行

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Workflows[{id}].Executions.PostAsync` | POST | `/workflows/{id}/executions` |
| `Workflows[{id}].Executions.GetAsync` | GET | `/workflows/{id}/executions` |
| `Workflows[{id}].Executions[{executionId}].Cancel.PostAsync` | POST | `/workflows/{id}/executions/{executionId}/cancel` |
| `Workflows[{id}].Executions[{executionId}].GetAsync` | GET | `/workflows/{id}/executions/{executionId}` |
| `Workflows[{id}].Executions[{executionId}].DeleteAsync` | DELETE | `/workflows/{id}/executions/{executionId}` |
| `Workflows[{id}].Executions[{executionId}].Exec_history.GetAsync` | GET | `/workflows/{id}/executions/{executionId}/exec_history` |

### プラン

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Plans.GetAsync` | GET | `/plans` |

### サブスクリプション

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Subscriptions.GetAsync` | GET | `/subscriptions` |
| `Subscriptions.PostAsync` | POST | `/subscriptions` |
| `Subscriptions.DeleteAsync` | DELETE | `/subscriptions` |

## 使用方法

本 SDK を使用して ワークフロー API を呼び出す方法について記載します。

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
    using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
        // 必要に応じてログレベルや出力先などの設定を行う
    });
    ```
2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`WorkflowsClient` のインスタンスを作成します。
    ```csharp
    WorkflowsClient client = new WorkflowsClient(adapter, loggerFactory);
    ```
    ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。
    ```csharp
    WorkflowsClient client = new WorkflowsClient(adapter);
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

WorkflowsClient client = new WorkflowsClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークン／シークレットなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

#### ワークフロー一覧取得

```csharp
ListWorkflowResponse workflows = await client.Workflows.GetAsync();
```

#### ワークフロー作成

```csharp
CreateWorkflow body = new CreateWorkflow
{
    Name = "<YOUR_WORKFLOW_NAME>",
    Runbook = "<YOUR_RUNBOOK_YAML>",
    Description = "<YOUR_WORKFLOW_DESCRIPTION>",
    Publish = true,
    Logging = false
};

CreateWorkflowResponse result = await client.Workflows.PostAsync(body);
```

Runbook には YAML 形式の文字列を指定します。
Runbook の構文については [構文リファレンス](https://manual.sakura.ad.jp/cloud/appliance/workflows/reference-syntax.html) を参照してください。

#### ワークフロー詳細取得

```csharp
GetWorkflowResponse result = await client.Workflows["<YOUR_WORKFLOW_ID>"].GetAsync();
```

#### ワークフロー更新

```csharp
UpdateWorkflow body = new UpdateWorkflow
{
    Name = "<YOUR_NEW_WORKFLOW_NAME>"
};

UpdateWorkflowResponse result = await client.Workflows["<YOUR_WORKFLOW_ID>"].PatchAsync(body);
```

#### ワークフロー削除

```csharp
DeleteWorkflowResponse result = await client.Workflows["<YOUR_WORKFLOW_ID>"].DeleteAsync();
```

#### ワークフロー実行

```csharp
CreateExecution body = new CreateExecution();
// 特定リビジョンで実行する場合は RevisionId を指定する (省略時は最新リビジョン)
// body.RevisionId = <YOUR_REVISION_ID>;

CreateExecutionResponse result = await client.Workflows["<YOUR_WORKFLOW_ID>"].Executions.PostAsync(body);
Console.WriteLine($"Execution ID: {result.Execution.ExecutionId}");
Console.WriteLine($"Status: {result.Execution.Status}");
```

#### ワークフロー実行のキャンセル

```csharp
CancelExecutionResponse result = await client.Workflows["<YOUR_WORKFLOW_ID>"].Executions["<YOUR_EXECUTION_ID>"].Cancel.PostAsync();
```

#### ワークフロー実行履歴の取得

```csharp
ListExecutionHistoryResponse history = await client.Workflows["<YOUR_WORKFLOW_ID>"].Executions["<YOUR_EXECUTION_ID>"].Exec_history.GetAsync();
```

#### ワークフローリビジョン追加

```csharp
CreateWorkflowRevision body = new CreateWorkflowRevision
{
    Runbook = "<YOUR_NEW_RUNBOOK_YAML>",
    RevisionAlias = "<YOUR_REVISION_ALIAS>" // 省略可
};

CreateWorkflowRevisionResponse result = await client.Workflows["<YOUR_WORKFLOW_ID>"].Revisions.PostAsync(body);
```

#### ワークフローリビジョンエイリアス更新

```csharp
UpdateWorkflowRevisionAlias body = new UpdateWorkflowRevisionAlias
{
    RevisionAlias = "<YOUR_NEW_ALIAS>"
};

UpdateWorkflowRevisionAliasResponse result = await client.Workflows["<YOUR_WORKFLOW_ID>"].Revisions[<YOUR_REVISION_ID>].Revision_alias.PutAsync(body);
```

#### 料金プラン一覧取得

```csharp
ListPlansResponse plans = await client.Plans.GetAsync();
```

#### 課金プランの設定

```csharp
CreateSubscription body = new CreateSubscription
{
    PlanId = <YOUR_PLAN_ID>
};

await client.Subscriptions.PostAsync(body);
```

#### 課金プランの削除

```csharp
await client.Subscriptions.DeleteAsync();
```

## エラー処理とページングの簡易ガイド

### 例外の扱い

ワークフロー API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;

try
{
    ListWorkflowResponse workflows = await client.Workflows.GetAsync();
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

ワークフロー SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型
`Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、
    `ResponseBody` プロパティにエラーレスポンスボディの内容 (フィールド名と値のディクショナリ) を保持します。
- ワークフロー固有のエラー形式でも、`Code`、`Message`、`Cause` などの値を
    `ResponseBody` から参照できます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから
`ResponseBody` を参照することで、API ごとの詳細なエラー情報を確認できます。

### ページングと大量データ取得

`/workflows` や `/workflows/{id}/executions` など一部の一覧系 API には、
ページングやソートのためのクエリパラメータが定義されています。
大量件数を扱う場合は、これらのパラメータを指定して少量ずつ取得することを推奨します。

具体的なパラメータ名や制約値は、Workflows API リファレンスおよび OpenAPI 定義を参照してください。
Kiota で生成されたクライアントでは、一覧取得メソッドに対してクエリパラメータを指定できるようになっています。
ワークフローの件数や実行履歴の件数に応じて、ページサイズやソート順を適切に設定してください。

```csharp
// ページサイズとページ番号を指定した一覧取得の例
ListWorkflowResponse workflows = await client.Workflows.GetAsync(config =>
{
    config.QueryParameters.Page = 1;
    config.QueryParameters.PageLimit = 20;
});
```

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

**API キー認証を使用している場合**、次の点を確認してください。

- ワークフロー用の API キーを発行していること
- アクセストークン (UUID) とアクセストークンシークレットを正しくコピーしていること
- `BasicAuthenticationProvider` の引数順序が `(accessToken, accessTokenSecret)` であること

**サービスプリンシパル認証を使用している場合**、次の点を確認してください。

- サービスプリンシパルキーを正しく発行していること
- アクセストークンの有効期限が切れていないこと（`ServicePrincipalOAuth2AccessToken.ExpiresIn` または `ServicePrincipalOAuth2AccessToken.TokenExpiredAt` で確認できます）
- `ApiKeyAuthenticationProvider` の `Authorization` ヘッダー値が `Bearer <AccessToken>` 形式になっていること
- JWT の署名に使用したサービスプリンシパルキーが有効であること

それでも解消しない場合は、`ApiError` の内容やログを確認し、
レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。

### Q. ワークフローを実行するにはどうすればよいですか？

ワークフローを実行するには、事前にプランを設定する必要があります。
さくらのクラウド ホームから、ワークフローにアクセスし、料金プランを選択してサブスクリプションを有効化してください。
SDKから`Plans.GetAsync` で利用可能なプラン一覧を取得し、`Subscriptions.PostAsync` で料金プランを選択することも可能です。
その後、`Workflows[{id}].Executions.PostAsync` でワークフローを実行できます。

### Q. Runbook とは何ですか？

Runbook はワークフローの処理内容を記述した YAML 形式の定義ファイルです。
Runbook の構文については [構文リファレンス](https://manual.sakura.ad.jp/cloud/appliance/workflows/reference-syntax.html) を参照してください。

### Q. タイムアウトや接続エラーが発生します

まず、開発環境から `https://secure.sakura.ad.jp` への通信経路（プロキシ設定やファイアウォール）に問題がないか確認してください。
ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、
`HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか？

本 SDK は .NET Standard 2.0 対応のライブラリです。
そのため、.NET 6 以降を含む .NET Standard 2.0 互換のランタイムで利用できます。

### Q. 他のサービス（AppRun や IAM など）でも同じように使えますか？

本 README は ワークフロー (`Sacloud.Sdk.Workflows`) 向けの説明です。
同一リポジトリ内には AppRun共用型・AppRun専有型・IAM・EventBus など他サービス向けの SDK も含まれていますが、
それぞれ `sdk/<ServiceName>/` 配下に独自の README とサンプルが用意されています。
基本的な考え方（.NET プロジェクトへのパッケージ追加、認証、クライアント初期化、ログ設定など）は共通ですが、
サービスごとに利用する API やユースケースが異なるため、対象サービスの README も併せて参照してください。
