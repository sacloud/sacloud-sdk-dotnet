# IAM API クライアントプロジェクト

## 基本情報

さくらのクラウドが提供する IAM API を利用するための C# SDK です。

本ドキュメントは、さくらのクラウドの IAM を .NET/C# から利用したいアプリケーション開発者を主な読者として想定し、
SDK の導入手順と、最初に押さえておくべき基本的な使い方を説明します。
本READMEの内容を読めば、簡単なリソース操作まで一通り試せる構成になっています。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### 該当サービスの基本情報

- 対象サービス名: `IAM`（さくらのクラウド IAM）
- サービス概要:
	- さくらのクラウド上のリソースに対するアクセス制御を管理するサービスです。
	- 組織・プロジェクト・フォルダの階層管理、IAMポリシー・IDポリシーの設定、サービスプリンシパルやグループの管理、SSO連携などを提供します。
- 本 SDK の目的:
	- さくらのクラウド「IAM」サービス向けの C# API クライアントライブラリ `Sacloud.Sdk.IAM` を提供します。
    - 内部的には OpenAPI 定義から Kiota で生成された `RequestBuilder` / `Models` を土台としていますが、
      通常の SDK 利用時にこれらの実装詳細を意識する必要はありません。
- 対応言語 / ランタイム:
	- C# 7.3 以降
	- .NET Standard 2.0 対応

### 本 SDK で対応している API バージョン情報

- OpenAPI 定義:
    - ファイルパス: `sdk/IAM/openapi-spec/2.1.0/iam-api.yaml`
    - OpenAPI バージョン: 3.0.3
- IAM API バージョン:
    - `info.version`: `2.1.0`
- ベース URL:
    - `https://secure.sakura.ad.jp/cloud/api/iam/1.0`
    - 本 SDK からのすべての API 呼び出しは、上記ベース URL を基点として実行されます。
- 認証方式（概要）:
    - Bearer認証
    - アクセストークン： サービスプリンシパルキーを使用して発行したアクセストークン
    - ヘッダー形式： `Authorization: Bearer <AccessToken>`

## 事前セットアップ

### 認証情報の準備

IAM SDK は、認証にサービスプリンシパルキーを使用して発行したアクセストークンを使用します。  
下記ドキュメントに従い、アクセストークンを発行します。  

- [サービスプリンシパル | さくらのクラウド マニュアル](https://manual.sakura.ad.jp/cloud/controlpanel/service-principal.html#service-principal-key)

### 必要なツール

- 次の条件をすべて満たす .NET SDK
    - .NET Standard 2.0 でサポートされているバージョンであること
    - C# 7.3 以降をサポートしていること

### パッケージのインストール

本 SDK を使用する .NET プロジェクトに対して、パッケージをインストールする下記コマンドを実行します。
```
dotnet add package Sacloud.Sdk.IAM
```

## クイックスタート

ここでは、最小限のコードで IAM API にアクセスし、
プロジェクトの一覧を取得するまでの流れを示します。

1. 任意のディレクトリでコンソールアプリケーションを作成します。

    ```bash
    dotnet new console -n IAMQuickStart
    cd IAMQuickStart
    dotnet add package Sacloud.Sdk.IAM
    ```

2. `Program.cs` に、以下のコード例を保存します。（C# のトップレベルステートメント構文を使用しています）

    ```csharp
    using Microsoft.Extensions.Logging;
    using Microsoft.Kiota.Abstractions.Authentication;
    using Microsoft.Kiota.Http.HttpClientLibrary;
    using Sacloud.Sdk.IAM;
    using Sacloud.Sdk.IAM.Projects;
    using Sacloud.Sdk.IAM.Models;

    // 1. 認証情報を設定
    string accessToken = "<YOUR_ACCESS_TOKEN>";

    ApiKeyAuthenticationProvider authProvider = new ApiKeyAuthenticationProvider(
        $"Bearer {accessToken}",
        "Authorization",
        ApiKeyAuthenticationProvider.KeyLocation.Header);

    using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(authProvider);

    // 2. ロガーとクライアントを初期化
    using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    IAMClient client = new IAMClient(adapter, loggerFactory);

    try
    {
        // 3. プロジェクトの一覧を取得
        ProjectsGetResponse projects = await client.Projects.GetAsync();

        Console.WriteLine("Projects:");
        if (projects?.Items is not null)
        {
            foreach (Project project in projects.Items)
            {
                Console.WriteLine($"- {project.Id}: {project.Name} ({project.Code})");
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
        Console.WriteLine($"Error while calling IAM API: {ex.Message}");
    }
    ```

3. 以下のコマンドで実行し、一覧が取得できることを確認します。

    ```bash
    dotnet run
    ```

より詳細な認証手順や、リソースの追加・更新・削除などの操作については、後述の「使用方法」セクションを参照してください。  
より多くの操作例や実行例については、リポジトリ内のサンプルプロジェクトを参照してください。  

- サンプルプロジェクト: `sdk/IAM/samples/`  

このディレクトリには、実際の IAM リソースに対してSDK を介して各種操作を行うコンソールアプリケーションが含まれています。  
プロジェクトルート（`Sacloud.Sdk.slnx` があるディレクトリ）で次のコマンドを実行すると、サンプルをビルド・実行できます。  

```bash
dotnet run --project sdk/IAM/samples/IAM.Samples.csproj
```

サンプルプロジェクトには、JWT生成を行う簡易的なヘルパークラスが含まれています。  
JWTを用意済みの場合は、SDK本体の `Sacloud.Sdk.IAM.Helpers.AccessTokenHelper` を利用してアクセストークンを取得できます。  
実行前に、認証に使用するサービスプリンシパルのリソースID、kid、秘密鍵を適切な値に置き換えてください。  

## SDK のメソッドと API エンドポイントの対応表

### 認証

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `OrganizationPasswordPolicy.GetAsync` | GET | `/organization-password-policy` |
| `OrganizationPasswordPolicy.PutAsync` | PUT | `/organization-password-policy` |
| `OrganizationAuthConditions.GetAsync` | GET | `/organization-auth-conditions` |
| `OrganizationAuthConditions.PutAsync` | PUT | `/organization-auth-conditions` |
| `Auth.Context.GetAsync` | GET | `/auth/context` |

### ユーザ

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Compat.Users.GetAsync` | GET | `/compat/users` |
| `Compat.Users.PostAsync` | POST | `/compat/users` |
| `Compat.Users[{user_id}].GetAsync` | GET | `/compat/users/{user_id}` |
| `Compat.Users[{user_id}].PutAsync` | PUT | `/compat/users/{user_id}` |
| `Compat.Users[{user_id}].DeleteAsync` | DELETE | `/compat/users/{user_id}` |
| `Compat.Users[{user_id}].RegisterEmail.PostAsync` | POST | `/compat/users/{user_id}/register-email` |
| `Compat.Users[{user_id}].UnregisterEmail.PostAsync` | POST | `/compat/users/{user_id}/unregister-email` |

### ユーザ2要素認証

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Compat.Users[{user_id}].DeactivateOtp.PostAsync` | POST | `/compat/users/{user_id}/deactivate-otp` |
| `Compat.Users[{user_id}].TrustedDevices.GetAsync` | GET | `/compat/users/{user_id}/trusted-devices` |
| `Compat.Users[{user_id}].TrustedDevices[{trusted_device_id}].DeleteAsync` | DELETE | `/compat/users/{user_id}/trusted-devices/{trusted_device_id}` |
| `Compat.Users[{user_id}].ClearTrustedDevices.PostAsync` | POST | `/compat/users/{user_id}/clear-trusted-devices` |
| `Compat.Users[{user_id}].SecurityKeys.GetAsync` | GET | `/compat/users/{user_id}/security-keys` |

### グループ

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Groups.GetAsync` | GET | `/groups` |
| `Groups.PostAsync` | POST | `/groups` |
| `Groups[{group_id}].GetAsync` | GET | `/groups/{group_id}` |
| `Groups[{group_id}].PutAsync` | PUT | `/groups/{group_id}` |
| `Groups[{group_id}].DeleteAsync` | DELETE | `/groups/{group_id}` |
| `Groups[{group_id}].Memberships.GetAsync` | GET | `/groups/{group_id}/memberships` |
| `Groups[{group_id}].Memberships.PutAsync` | PUT | `/groups/{group_id}/memberships` |

### IAMロール

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `IamRoles.GetAsync` | GET | `/iam-roles` |
| `IamRoles[{iam_role_id}].GetAsync` | GET | `/iam-roles/{iam_role_id}` |

### IDロール

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `IdRoles.GetAsync` | GET | `/id-roles` |
| `IdRoles[{id_role_id}].GetAsync` | GET | `/id-roles/{id_role_id}` |

### プロジェクト

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Projects.GetAsync` | GET | `/projects` |
| `Projects.PostAsync` | POST | `/projects` |
| `MoveProjects.PostAsync` | POST | `/move-projects` |
| `Projects[{project_id}].GetAsync` | GET | `/projects/{project_id}` |
| `Projects[{project_id}].PutAsync` | PUT | `/projects/{project_id}` |
| `Projects[{project_id}].DeleteAsync` | DELETE | `/projects/{project_id}` |

### フォルダ

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Folders.PostAsync` | POST | `/folders` |
| `Folders.GetAsync` | GET | `/folders` |
| `Folders[{folder_id}].PutAsync` | PUT | `/folders/{folder_id}` |
| `Folders[{folder_id}].DeleteAsync` | DELETE | `/folders/{folder_id}` |
| `Folders[{folder_id}].GetAsync` | GET | `/folders/{folder_id}` |
| `MoveFolders.PostAsync` | POST | `/move-folders` |

### IAMポリシー

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Projects[{project_id}].IamPolicy.GetAsync` | GET | `/projects/{project_id}/iam-policy` |
| `Projects[{project_id}].IamPolicy.PutAsync` | PUT | `/projects/{project_id}/iam-policy` |
| `Folders[{folder_id}].IamPolicy.GetAsync` | GET | `/folders/{folder_id}/iam-policy` |
| `Folders[{folder_id}].IamPolicy.PutAsync` | PUT | `/folders/{folder_id}/iam-policy` |
| `OrganizationIamPolicy.GetAsync` | GET | `/organization-iam-policy` |
| `OrganizationIamPolicy.PutAsync` | PUT | `/organization-iam-policy` |

### IDポリシー

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `OrganizationIdPolicy.GetAsync` | GET | `/organization-id-policy` |
| `OrganizationIdPolicy.PutAsync` | PUT | `/organization-id-policy` |

### サービスプリンシパル

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `ServicePrincipals.GetAsync` | GET | `/service-principals` |
| `ServicePrincipals.PostAsync` | POST | `/service-principals` |
| `ServicePrincipals[{service_principal_id}].GetAsync` | GET | `/service-principals/{service_principal_id}` |
| `ServicePrincipals[{service_principal_id}].PutAsync` | PUT | `/service-principals/{service_principal_id}` |
| `ServicePrincipals[{service_principal_id}].DeleteAsync` | DELETE | `/service-principals/{service_principal_id}` |
| `ServicePrincipals.Oauth2.Token.PostAsync` | POST | `/service-principals/oauth2/token` |
| `ServicePrincipals[{service_principal_id}].Keys.GetAsync` | GET | `/service-principals/{service_principal_id}/keys` |
| `ServicePrincipals[{service_principal_id}].UploadKey.PostAsync` | POST | `/service-principals/{service_principal_id}/upload-key` |
| `ServicePrincipals[{service_principal_id}].Keys[{service_principal_key_id}].Enable.PostAsync` | POST | `/service-principals/{service_principal_id}/keys/{service_principal_key_id}/enable` |
| `ServicePrincipals[{service_principal_id}].Keys[{service_principal_key_id}].Disable.PostAsync` | POST | `/service-principals/{service_principal_id}/keys/{service_principal_key_id}/disable` |
| `ServicePrincipals[{service_principal_id}].Keys[{service_principal_key_id}].DeleteAsync` | DELETE | `/service-principals/{service_principal_id}/keys/{service_principal_key_id}` |

### プロジェクトAPIキー

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Compat.ApiKeys.GetAsync` | GET | `/compat/api-keys` |
| `Compat.ApiKeys.PostAsync` | POST | `/compat/api-keys` |
| `Compat.ApiKeys[{apikey_id}].GetAsync` | GET | `/compat/api-keys/{apikey_id}` |
| `Compat.ApiKeys[{apikey_id}].PutAsync` | PUT | `/compat/api-keys/{apikey_id}` |
| `Compat.ApiKeys[{apikey_id}].DeleteAsync` | DELETE | `/compat/api-keys/{apikey_id}` |

### SSO連携

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `SsoProfiles.GetAsync` | GET | `/sso-profiles` |
| `SsoProfiles.PostAsync` | POST | `/sso-profiles` |
| `SsoProfiles[{sso_profile_id}].GetAsync` | GET | `/sso-profiles/{sso_profile_id}` |
| `SsoProfiles[{sso_profile_id}].PutAsync` | PUT | `/sso-profiles/{sso_profile_id}` |
| `SsoProfiles[{sso_profile_id}].DeleteAsync` | DELETE | `/sso-profiles/{sso_profile_id}` |
| `SsoProfiles[{sso_profile_id}].Assign.PostAsync` | POST | `/sso-profiles/{sso_profile_id}/assign` |
| `SsoProfiles[{sso_profile_id}].Unassign.PostAsync` | POST | `/sso-profiles/{sso_profile_id}/unassign` |

### 組織

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `Organization.GetAsync` | GET | `/organization` |
| `Organization.PutAsync` | PUT | `/organization` |
| `OrganizationServicePolicy.PutAsync` | PUT | `/organization-service-policy` |
| `OrganizationServicePolicy.GetAsync` | GET | `/organization-service-policy` |

### サービスポリシー

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `EnableServicePolicy.PostAsync` | POST | `/enable-service-policy` |
| `DisableServicePolicy.PostAsync` | POST | `/disable-service-policy` |
| `ServicePolicyStatus.GetAsync` | GET | `/service-policy-status` |
| `ServicePolicyRuleTemplates.GetAsync` | GET | `/service-policy-rule-templates` |

### ユーザープロビジョニング

| SDK メソッド | HTTP メソッド | API エンドポイント |
|---|---|---|
| `ScimConfigurations.GetAsync` | GET | `/scim-configurations` |
| `ScimConfigurations.PostAsync` | POST | `/scim-configurations` |
| `ScimConfigurations[{id}].GetAsync` | GET | `/scim-configurations/{id}` |
| `ScimConfigurations[{id}].PutAsync` | PUT | `/scim-configurations/{id}` |
| `ScimConfigurations[{id}].DeleteAsync` | DELETE | `/scim-configurations/{id}` |
| `ScimConfigurations[{id}].RegenerateToken.PostAsync` | POST | `/scim-configurations/{id}/regenerate-token` |


## 使用方法

本 SDK を使用して IAM API を呼び出す方法について記載します。

### 認証

#### 認証プロバイダのセットアップ

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

アクセストークンの取得には、`ServicePrincipals.Oauth2.Token.PostAsync` の呼び出しを簡略化した `Sacloud.Sdk.IAM.Helpers.AccessTokenHelper` を利用できます。  
事前に生成した JWT を用いて、以下のコード例のようにアクセストークンを取得できます。  

```csharp
using Microsoft.Extensions.Logging;
using Sacloud.Sdk.IAM.Helpers;

string jwt = "<YOUR_JWT>";

// ログの利用は任意のため、必要ない場合はILoggerFactoryのインスタンス生成は不要です。
// APIクライアントと同じILoggerFactoryのインスタンスを使用することを推奨します。
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

string accessToken = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory);
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
2. 「認証」セクションで作成した `HttpClientRequestAdapter` と `ILoggerFactory` インスタンスを使用して、`IAMClient` のインスタンスを作成します。
    ```csharp
    IAMClient client = new IAMClient(adapter, loggerFactory);
    ```
    ログを受け取る必要がない場合は、`ILoggerFactory` 引数を省略してインスタンスを作成できます。
    ```csharp
    IAMClient client = new IAMClient(adapter);
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

IAMClient client = new IAMClient(adapter, loggerFactory);
```

ログには次のような情報が含まれます。

- API 呼び出しの開始・終了、HTTP ステータスコード
- クライアント側バリデーション違反の内容
- HTTP 4xx/5xx 応答時のエラー概要

一方で、`Authorization` ヘッダー値やアクセストークンなどの秘密情報はログに出力されないよう実装されています。
アプリケーション側で独自にログを出力する場合も、これらの情報を記録しないよう注意してください。

### 使用例

プロジェクトに対する各種操作の API 呼び出し例を示します。

#### 一覧取得

```csharp
ProjectsGetResponse projects = await client.Projects.GetAsync();
```

#### 詳細取得

```csharp
Project project = await client.Projects[<YOUR_PROJECT_ID>].GetAsync();
```

#### 追加

```csharp
ProjectsPostRequestBody body = new ProjectsPostRequestBody
{
    Name = "<YOUR_PROJECT_NAME>",
    Code = "<YOUR_PROJECT_CODE>",
    Description = "<YOUR_PROJECT_DESCRIPTION>"
};

Project createdProject = await client.Projects.PostAsync(body);
```

#### 更新

```csharp
WithProject_PutRequestBody body = new WithProject_PutRequestBody
{
    Name = "<YOUR_NEW_PROJECT_NAME>",
    Description = "<YOUR_NEW_PROJECT_DESCRIPTION>"
};

Project updatedProject = await client.Projects[<YOUR_PROJECT_ID>].PutAsync(body);
```

#### 削除

```csharp
await client.Projects[<YOUR_PROJECT_ID>].DeleteAsync();
```

## SDK使用時の注意点

### IAMポリシー・IDポリシーの更新

ポリシーの更新操作は、現在のポリシーを上書きします。  
SDKからポリシーの追加や更新を行う場合、更新前のポリシーを取得してから、必要な変更を加えて更新することが推奨されます。  

例）
```csharp
using Sacloud.Sdk.IAM.OrganizationIamPolicy;
using Sacloud.Sdk.IAM.Models;

// 事前に認証プロバイダとクライアントをセットアップしている前提

// 既存のIAMポリシーを取得
OrganizationIamPolicyGetResponse currentIamPolicy = await client.OrganizationIamPolicy.GetAsync();

// IAMポリシーの追加
currentIamPolicy.Bindings.Add(new IamPolicy()
{
    Role = new IamPolicy_role()
    {
        Id = "owner",
        Type = IamPolicy_role_type.Preset
    },
    Principals = new List<Principal>
    {
        new Principal()
        {
            Id = 1234567890,
            Type = "service-principal"
        }
    }
});

// 更新用のリクエストボディを作成
var newIamPolicy = new OrganizationIamPolicyPutRequestBody()
{
    Bindings = currentIamPolicy.Bindings
};

// IAMポリシーを更新
await client.OrganizationIamPolicy.PutAsync(newIamPolicy);
```

## エラー処理とページングの簡易ガイド

### 例外の扱い

IAM API の呼び出し中に HTTP エラーやバリデーションエラーが発生した場合、
Kiota によって `Microsoft.Kiota.Abstractions.ApiException` を継承した例外がスローされます。
特に HTTP 4xx/5xx のエラー応答時には、共通エラー型 `Sacloud.Sdk.Shared.Models.ApiError` がスローされます。
例外にはステータスコードやエラーレスポンスボディが含まれるため、少なくとも `try-catch` でハンドリングしておくことを推奨します。

```csharp
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;

try
{
    var projects = await client.Projects.GetAsync();
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

IAM SDK では、HTTP 4xx/5xx のエラー応答を共通エラー型
`Sacloud.Sdk.Shared.Models.ApiError` として扱います。

- `ApiError` は `Microsoft.Kiota.Abstractions.ApiException` を継承しており、
    `ResponseBody` プロパティにエラーレスポンスボディの内容 (フィールド名と値のディクショナリ) を保持します。
- IAM 固有のエラー形式も、この `ResponseBody` の中にキーと値として展開されます。

そのため、まずは `ApiException` を catch し、`ApiError` にキャストしてから
`ResponseBody` を参照することで、APIごとの詳細なエラー情報を確認できます。

### ページングと大量データ取得

一覧系 API には、ページングやソートのためのクエリパラメータが定義されています。
大量件数を扱う場合は、これらのパラメータを指定して少量ずつ取得することを推奨します。

具体的なパラメータ名や制約値は、IAM API リファレンスおよび OpenAPI 定義を参照してください。
Kiota で生成されたクライアントでは、一覧取得メソッドに対してクエリパラメータを指定できるようになっています。
アプリケーションの規模や想定件数に応じて、ページサイズやソート順を適切に設定してください。

## よくある質問

### Q. `401 Unauthorized` や `403 Forbidden` が返ってきます

次の点を確認してください。

- アクセストークンを発行したサービスプリンシパルに、適切な権限を付与していること。
- JWT の署名に使用したサービスプリンシパルキーが有効であること。
- アクセストークンを正しくコピーしていること。
- `Authorization` ヘッダーが `Bearer <AccessToken>` 形式で送信されていること。
- アクセストークンの有効期限が切れていないこと（`ExpiresIn` または `TokenExpiredAt` で確認）。

それでも解消しない場合は、`ApiError` の内容やログを確認し、
レスポンスボディに含まれるエラーコード・メッセージから原因を切り分けてください。

### Q. タイムアウトや接続エラーが発生します

まず、開発環境から `https://secure.sakura.ad.jp` への通信経路（プロキシ設定やファイアウォール）に問題がないか確認してください。
ネットワーク経路に問題がない場合は、アプリケーション側で再試行ロジックを追加したり、
`HttpClient` のタイムアウト設定を見直すことを検討してください。

### Q. どの .NET ランタイムで利用できますか？

本 SDK は .NET Standard 2.0 対応のライブラリです。
そのため、.NET 6 以降を含む .NET Standard 2.0 互換のランタイムで利用できます。
