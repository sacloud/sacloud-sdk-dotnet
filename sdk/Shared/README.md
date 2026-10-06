# 共通ライブラリプロジェクト

## 基本情報

さくらのクラウドの各サービス向け C# SDK で共通して利用される処理をまとめた C# ライブラリです。

> [!IMPORTANT]
> `Sacloud.Sdk.Shared` は各 SDK の内部実装から参照されることを目的としたライブラリです。  
> アプリケーション開発者が本パッケージを単体で直接インストールして利用することは想定していません。  
> さくらのクラウドの各サービスを利用する場合は、対応するサービス別 SDK（例: `Sacloud.Sdk.AppRunDedicated`）をご利用ください。

:warning:  v1.0に達するまでは互換性のない形で変更される可能性がありますのでご注意ください。

### パッケージ仕様

- パッケージ名: `Sacloud.Sdk.Shared`
- 目的:
    - 各サービス SDK に共通する処理を一元管理します。
    - 個々のサービス SDK はこのライブラリを内部依存として利用します。
- 対応言語 / ランタイム:
    - C# 7.3 以降
    - .NET Standard 2.0 対応

## 提供機能

本ライブラリが提供する主な機能は以下のとおりです。

### バリデーション属性 (`sdk/Shared/src/Attributes/`)

リクエストパラメータのバリデーションに使用するカスタム属性を提供します。

| クラス名 | 説明 |
|---|---|
| `LocalizedRequiredAttribute` | 必須項目のバリデーション |
| `LocalizedRangeAttribute` | `int` 整数値の範囲バリデーション |
| `LocalizedLongRangeAttribute` | `long` 整数値の範囲バリデーション |
| `LocalizedMinimumAttribute` / `LocalizedMaximumAttribute` | 数値の最小値・最大値バリデーション |
| `LocalizedLengthAttribute` | 特定の文字列長バリデーション |
| `LocalizedLengthRangeAttribute` | 文字列の最小・最大長バリデーション |
| `LocalizedMinLengthAttribute` / `LocalizedMaxLengthAttribute` | 文字列の最小・最大長バリデーション |
| `LocalizedRegularExpressionAttribute` | 正規表現バリデーション |
| `LocalizedEnumDataTypeAttribute` | 列挙型バリデーション |
| `LocalizedDeniedValuesAttribute` | 指定値の禁止バリデーション |
| `LocalizedAllowedValuesAttribute` | 指定値の許可バリデーション |
| `LocalizedIpv4AddressAttribute` | IPv4 アドレス形式バリデーション |
| `LocalizedArrayElementCountAttribute` | 配列の最大要素数バリデーション |
| `LocalizedArrayElementSpecificCountAttribute` | 配列の特定要素数バリデーション |
| `LocalizedArrayElementRangeAttribute` | 配列要素の数値範囲バリデーション |
| `LocalizedArrayElementLengthRangeAttribute` | 配列要素（文字列）の最小・最大長バリデーション |
| `LocalizedArrayElementMaxLengthAttribute` | 配列要素（文字列）の最大長バリデーション |
| `LocalizedArrayElementRegularExpressionAttribute` | 配列要素の正規表現バリデーション |
| `LocalizedArrayElementEnumDataTypeAttribute` | 配列要素の列挙型バリデーション |
| `LocalizedNotEmptyArrayAttribute` | 空配列禁止バリデーション |
| `LocalizedUniqueArrayAttribute` | 配列要素の重複禁止バリデーション |
| `LocalizedAtLeastOneAttribute` | 1 プロパティ以上指定バリデーション |
| `LocalizedExactlyOneAttribute` | 1 プロパティのみ指定バリデーション |
| `LocalizedMutualExclusivityAttribute` | プロパティ相互排他バリデーション |

### リクエストパラメータバリデーション (`sdk/Shared/src/Validation/`)

SDK がリクエストを送信する前にパスパラメータ、クエリパラメータ、リクエストボディを検証する仕組みを提供します。

| クラス名 | 説明 |
|---|---|
| `RequestParameterValidator` | リクエストパラメータの検証ロジック |
| `PathParameterValidationOptions` | パスパラメータの検証オプション |

### エラーハンドリング (`sdk/Shared/src/Models/`, `sdk/Shared/src/Helpers/`)

API エラーのモデルとログ出力ヘルパーを提供します。

| クラス名 | 説明 |
|---|---|
| `ApiError` | API エラー情報を保持するモデルクラス |
| `ApiErrorLogger` | API エラーをログ出力するヘルパークラス |

### サービスプリンシパル認証 (`sdk/Shared/src/Helpers/`, `sdk/Shared/src/Models/`)

各サービス SDK がサービスプリンシパルキーによる認証を行うためのヘルパーとモデルを提供します。  
IAM SDK への依存を持たず、IAM API エンドポイントを `HttpClient` で直接呼び出す実装になっています。

| クラス名 | 説明 |
|---|---|
| `AccessTokenHelper` | JWT を使用してアクセストークンを取得するヘルパークラス |
| `ServicePrincipalOAuth2AccessToken` | アクセストークン取得 API のレスポンスを表すモデルクラス |

#### 使用例

```csharp
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

// JWT はサービスプリンシパルキーの秘密鍵で署名して事前に生成しておく
string jwt = /* 生成済み JWT */;

// アクセストークンを取得する
ServicePrincipalOAuth2AccessToken result = await AccessTokenHelper.GetAccessTokenAsync(jwt);
string accessToken = result.AccessToken;
```

引数の詳細：

| 引数 | 型 | 必須 | 説明 |
|---|---|---|---|
| `jwt` | `string` | ✓ | サービスプリンシパルキーで署名した JWT |
| `httpClient` | `HttpClient` | - | 省略時は既定の静的インスタンスを使用 |
| `loggerFactory` | `ILoggerFactory` | - | 省略時はログ出力なし |
| `cancellationToken` | `CancellationToken` | - | キャンセル用トークン |

### シリアライゼーション拡張 (`sdk/Shared/src/Extensions/`)

Kiota の `ISerializationWriter` に対する拡張メソッドを提供します。

| クラス名 | 説明 |
|---|---|
| `ISerializationWriterExtension` | シリアライゼーション処理の拡張メソッド群 |
