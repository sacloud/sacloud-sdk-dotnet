# AppRunShared リリースノート

## v0.2.0

OpenAPI 仕様 1.3.0 への追従に伴う更新です。ユーザー情報取得・作成 API の戻り値が構造化されたモデルオブジェクトとして返されるようになりました。また、コンテナレジストリを使うアプリ更新時に認証情報の引き継ぎ方法を指定できる `Action` プロパティが追加されました。既存コードへの影響箇所があるため、下記の互換性変更を確認してください。

## 重要: 互換性影響のある変更

### ユーザー情報取得 API の戻り値変更

ログイン中のユーザー情報を取得する `GetAsync()` の戻り値が変わりました。以前は生のバイトストリームが返されていましたが、今回からユーザー情報を表すオブジェクトが直接返されます。

- `UserRequestBuilder.GetAsync()`
  - 変更前: `Task<Stream>`
  - 変更後: `Task<GetUser>`

```cs
// 変更前: Stream を受け取って自前でデシリアライズする必要があった
Stream stream = await client.User.GetAsync();
// stream を読み込んでデシリアライズする処理 ...

// 変更後: GetUser オブジェクトがそのまま返される
GetUser user = await client.User.GetAsync();
int? limit = user?.Limit?.ApplicationCount;
```

影響:

- 既存コードで `Stream` 型の変数に戻り値を受け取っていた場合、コンパイルエラーになります
- `Stream` を読み込んで JSON デシリアライズするコードがある場合は削除し、`GetUser` のプロパティへのアクセスに置き換えてください

移行の考え方:

- 戻り値の型を `GetUser` に変更してください
- ユーザー情報の各フィールドは `GetUser` のプロパティ経由でアクセスできます（例: `user.Limit.ApplicationCount`）
- 独自のデシリアライズ処理は不要になります

### ユーザー作成 API の戻り値変更

ユーザーを作成する `PostAsync()` の戻り値が変わりました。以前は戻り値がなく（void）、作成後の情報を取得する手段がありませんでしたが、今回から作成されたユーザー情報を表すオブジェクトが返されます。

- `UserRequestBuilder.PostAsync()`
  - 変更前: `Task` (戻り値なし)
  - 変更後: `Task<PostUser>`

```cs
// 変更前: 戻り値なし
await client.User.PostAsync();

// 変更後: PostUser オブジェクトが返される（受け取らなくてもコンパイルエラーにはならない）
PostUser user = await client.User.PostAsync();
int? limit = user?.Limit?.ApplicationCount;
```

影響:

- `await client.User.PostAsync()` のように戻り値を破棄していた既存コードはそのまま動作します
- 戻り値を明示的に受け取っていたコードはありませんが、戻り値を利用して作成後のユーザー情報を取得できるようになりました

移行の考え方:

- 既存コードが戻り値を無視している場合は変更不要です
- 作成後のユーザー情報（例: `Limit.ApplicationCount`）を利用したい場合は、戻り値を `PostUser` 型の変数で受け取ってください

## 追加・改善

### コンテナレジストリ認証情報の制御方法指定（アプリ更新時）

アプリケーション更新リクエスト（`PatchApplicationBody` の `components[].deploy_source.container_registry`）に `Action` プロパティが追加されました。この設定を使うことで、アプリ更新時にコンテナレジストリの認証情報をどのように扱うかを指定できます。

- 追加プロパティ: `PatchApplicationBody_components_deploy_source_container_registry.Action`
  - 型: `ContainerRegistryAction?`（列挙型: `New` / `Keep`）
  - デフォルト値: `ContainerRegistryAction.New`（指定した認証情報を使用）

| 値 | 意味 |
|---|---|
| `New` | リクエストで指定した認証情報を使用する |
| `Keep` | 最新バージョンで保存された認証情報を引き継ぐ |

```cs
// 例: 既存の認証情報をそのまま引き継ぐ場合
PatchApplicationBody_components_deploy_source_container_registry registry
    = new PatchApplicationBody_components_deploy_source_container_registry
    {
        Action = ContainerRegistryAction.Keep,
        Image = "your-image:tag"
        // Server / Username / Password は省略可能
    };
```

影響:

- デフォルト値が `ContainerRegistryAction.New` であるため、既存コードで `Action` を設定せずにリクエストを送ると `action: "new"` が API へ送信されます
- API 側の旧動作と異なる場合、認証情報が再設定されることがあります。API 側の仕様と既存のリクエスト内容を確認してください

移行の考え方:

- 認証情報を毎回指定してアプリを更新していた場合は `Action = ContainerRegistryAction.New`（デフォルト）を使用してください
- 認証情報を省略してアプリを更新していた場合（既存の認証情報を引き継ぎたい場合）は `Action = ContainerRegistryAction.Keep` を明示的に設定してください

### コンテナレジストリ認証情報フィールドのバリデーション強化

アプリ作成・更新リクエストのコンテナレジストリ設定において、`Server`・`Username`・`Password` フィールドに空文字を設定した場合にバリデーションエラーが発生するようになりました。

- 対象モデル:
  - `PostApplicationBody_components_deploy_source_container_registry`
  - `PatchApplicationBody_components_deploy_source_container_registry`
- 変更: `Server`・`Username`・`Password` に `LocalizedMinLength(1)` バリデーションを追加

影響:

- 各フィールドに空文字（`""`）を明示的に渡していた場合、バリデーションエラーが発生するようになります
- 認証情報を省略する場合は `null` を設定するか、該当フィールドに値を設定しないようにしてください

## 仕様追随

- OpenAPI 仕様バージョン: 1.2.0 → 1.3.0
- SDK バージョン: 0.1.2 → 0.2.0
- コンテナレジストリ関連プロパティの説明文が仕様に合わせて修正されました（コードの動作変更なし）

## 利用者向け確認ポイント

1. `client.User.GetAsync()` の戻り値を `Stream` 型で受け取っているコードがあれば、`GetUser` 型への変更とデシリアライズ処理の削除を行ってください
2. アプリケーション更新リクエストでコンテナレジストリ認証情報を設定している場合、`Action` プロパティのデフォルト値（`ContainerRegistryAction.New`）が意図した動作かを確認し、必要に応じて `ContainerRegistryAction.Keep` を明示的に指定してください
3. コンテナレジストリの `Server`・`Username`・`Password` に空文字を渡しているコードがあれば、`null` に変更するか値の設定を省略するよう修正してください
4. `client.User.PostAsync()` の戻り値を破棄している既存コードは変更不要ですが、作成後のユーザー情報（例: アプリケーション数の上限）を利用する場合は戻り値を `PostUser` 型で受け取ってください
