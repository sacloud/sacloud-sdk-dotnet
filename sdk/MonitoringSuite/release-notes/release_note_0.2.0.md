# MonitoringSuite リリースノート

## v0.2.0

MonitoringSuite SDK を MonitoringSuite API の OpenAPI 定義 1.3.0 に追随させました。SDK のバージョンは `0.2.0` です。通常のクライアント操作を利用している場合、利用コードへの影響は限定的ですが、生成された型を直接参照している場合は型名を確認してください。

## 重要: 互換性影響のある変更

### 生成型名の見直し

OpenAPI 定義に含まれるリソース名の反映方法が更新され、一部の生成型名とパスパラメーターキーが変更されています。

| 変更前 | 変更後 |
|---|---|
| `Project_resource_ItemRequestBuilder` | `ProjectResourceItemRequestBuilder` |
| `Rule_uItemRequestBuilder` | `RuleUItemRequestBuilder` |
| `WithResource_ItemRequestBuilder` | `WithResourceItemRequestBuilder` |
| `Log_resource_ItemRequestBuilder` | `LogResourceItemRequestBuilder` |
| `Metrics_resource_ItemRequestBuilder` | `MetricsResourceItemRequestBuilder` |
| `Resource_ItemRequestBuilder` | `ResourceItemRequestBuilder` |

パスパラメーターキーも次のように変更されています。

| 変更前 | 変更後 |
|---|---|
| `project_resource_id` | `projectResourceId` |
| `rule_uid` | `ruleUid` |
| `log_resource_id` | `logResourceId` |
| `metrics_resource_id` | `metricsResourceId` |
| `resource_id` | `resourceId` |

影響:

- RequestBuilder などの生成型をアプリケーションコードで直接参照している場合、コンパイルエラーになる可能性があります。
- RequestBuilder を `Dictionary<string, object>` のパスパラメーター付きで直接生成している場合、旧キーではパラメーターを解決できない可能性があります。
- `MonitoringSuiteClient` から通常の API 操作を呼び出しているコードは、更新後の SDK でビルドと主要操作を確認してください。

移行の考え方:

- 直接参照している生成型がある場合は、更新後にコンパイルエラーとなった型名を新しい型名へ置き換えてください。
- RequestBuilder を直接生成している場合は、パスパラメーターのキーを新しい camelCase 名へ置き換えてください。
- 生成型を直接参照せず、クライアントから API 操作を呼び出している場合は、既存コードを維持したままビルド結果と利用する API の応答を確認してください。

### リソース ID の範囲バリデーション追加

一部のプロジェクト、ログ、メトリクス、トレースのリソース ID に、クライアント側の数値範囲バリデーションが追加されています。

- 最小値: `0`
- 最大値: `999999999999`

影響:

- 対象のリソース ID に範囲外の値を渡していた場合、API 送信前にバリデーションエラーになる可能性があります。

移行の考え方:

- リソース ID には `0` 以上 `999999999999` 以下の値を指定してください。
- ID を文字列や外部入力から変換している場合は、API 呼び出し前に値の範囲を確認してください。

## 仕様追随

- MonitoringSuite API の OpenAPI 定義を 1.2.0 から 1.3.0 に更新しました。
- SDK のバージョンを `0.1.0` から `0.2.0` に更新しました。
- API 定義の更新に伴い、Alerts、Dashboards、Logs、Management、Metrics、Publishers、Traces のリクエスト構築コードを更新しました。

## 利用者向け確認ポイント

1. SDK を `0.2.0` に更新して、アプリケーションがビルドできることを確認してください。
2. 生成された RequestBuilder 型を直接参照している場合は、6種類の型名変更を確認してください。
3. RequestBuilder を直接生成している場合は、パスパラメーターキーを camelCase 名へ更新してください。
4. リソース ID に範囲外の値を渡していないことを確認してください。
5. 利用している API 操作について、リクエスト送信とレスポンス処理が期待どおりであることを確認してください。
