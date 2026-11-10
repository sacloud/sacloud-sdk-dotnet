# APIGateway リリースノート

## v0.2.0

API 仕様 2.1.0 に追従しました。ACME 証明書の発行・再発行・更新に対応し、証明書一覧で状態やタイプなどの情報を取得できるようになりました。あわせて、ルートおよび IP 制限のプロトコル指定から HTTP 系の値が削除されているため、該当する値を使っている場合はコードの見直しが必要です。

## 重要: 互換性影響のある変更

### プロトコル指定から HTTP 系の値を削除

ルートおよび IP 制限のプロトコル指定で選択できる値が `https` のみになりました。

- ルートのプロトコル
  - 変更前: `http,https` / `http` / `https` を指定可能
  - 変更後: `https` のみ指定可能
- IP 制限のプロトコル
  - 変更前: `http,https` / `http` / `https` を指定可能
  - 変更後: `https` のみ指定可能

```cs
// 変更前
// Protocols = Route_protocols.Http;

// 変更後
Protocols = Route_protocols.Https;
```

影響:

- `Http` または `HttpHttps` を参照しているコードは、更新後にコンパイルできなくなります。

移行の考え方:

- `Https` に置き換えてください。HTTP での公開を前提としていた構成は、運用面でも見直してください。

### 証明書一覧レスポンスの型変更

証明書一覧取得の結果が、より多くの情報を持つ型に変わりました。

- 証明書一覧取得
  - 変更前: 戻り値の `Apigw` は `CertificateDTO` 型
  - 変更後: 戻り値の `Apigw` は `GetCertificates` 型

```cs
// 変更前
// CertificateDTO? dto = response?.Apigw;

// 変更後
GetCertificates? result = response?.Apigw;
```

影響:

- `Apigw` を旧型の変数に代入している、または旧型を引数に取るメソッドへ渡しているコードは、見直しが必要です。

移行の考え方:

- 変数の型を `GetCertificates` に変更してください。

## 追加・改善

### ACME 証明書の発行・再発行・更新

- ACME 証明書を新規発行する操作が追加されました。
- 既存の証明書に対して、識別名や SAN（Subject Alternative Name）のドメインを更新して再発行する操作が追加されました。
- 既存の証明書を更新する操作が追加されました。
- ACME 証明書を利用する場合に確認してください。

### 証明書一覧で取得できる情報の追加

- 証明書ごとに SAN、状態（`valid` / `expired` / `issuing` / `renewing` / `issuance_failed` / `renewal_failed`）、状態メッセージ、タイプ（`auto` / `manual`）を取得できます。
- 自動発行可能な証明書の上限数を取得できます。
- 証明書の状態を判定するコードを運用している場合に確認してください。

## 仕様追随

- OpenAPI 仕様を 2.0.1 から 2.1.0 に更新しました。
- SDK バージョン: `0.1.1` → `0.2.0`

## 利用者向け確認ポイント

1. **ルートおよび IP 制限のプロトコル設定の修正（ビルドエラーへの対処）**
   - ルート設定（`Route_protocols`）または IP 制限設定（`IpRestrictionConfig_protocols`）で `Http` や `HttpHttps` を指定しているコードがある場合、更新後にコンパイルエラーになります。
   - 指定を `Https` に置き換え、HTTP 前提の構成やアクセス制御を行っていた場合は運用の見直しを行ってください。
2. **証明書一覧取得レスポンスの型定義の更新**
   - 証明書一覧取得（`client.Certificates.GetAsync()`）の結果である `Apigw` プロパティの型が `CertificateDTO` から `GetCertificates` に変更されました。
   - レスポンスの受け取りや引数で `CertificateDTO` を明示的に型指定している箇所があれば、`GetCertificates` に置き換えてください。
3. **証明書ステータス・タイプ情報の活用**
   - 証明書一覧から SAN（`Sans`）、状態（`Status`）、状態メッセージ（`StatusMessage`）、タイプ（`Type`）、自動発行上限（`AutoIssueLimit`）が取得できるようになりました。
   - 証明書の状態（発行中・更新中・エラー等）を監視・判定する処理を実装している場合は、新設されたプロパティの利用をご検討ください。
4. **ACME 証明書関連 API の利用**
   - ACME 証明書の新規発行（`client.Certificates.Acme`）、再発行（`client.Certificates[certificateId].Acme.Reissue`）、手動更新（`client.Certificates[certificateId].Acme.Renew`）の API が追加されました。
   - ACME による証明書の自動発行や更新運用を行う際にご活用ください。
