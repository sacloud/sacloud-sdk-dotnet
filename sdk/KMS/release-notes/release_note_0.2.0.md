# KMS リリースノート

## v0.2.0

KMS API 1.2.0 の仕様に追従し、キー操作のリクエストとレスポンスの型を更新しました。一部の公開型や戻り値が変更されているため、既存コードの修正が必要です。キー一覧では取得件数と開始位置を指定できるようになりました。

## 重要: 互換性影響のある変更

### キー操作の入出力型

キー作成、キー更新、暗号化、復号、状態変更、破棄予約で使用するリクエスト型が変更されました。キー作成・状態変更・破棄予約では、応答も以前の型や `Stream` ではなく専用のレスポンスモデルで返されます。

- キー作成
    - 変更前: `WrappedCreateKey` を渡し、`WrappedCreateKey` を受け取る
    - 変更後: `WrappedCreateKeyRequest` を渡し、`WrappedCreateKeyResponse` を受け取る
- キー更新
    - 変更前: `WrappedKey` を渡す
    - 変更後: `WrappedKeyRequest` を渡す
- キーの状態変更
    - 変更前: `WrappedChangeKeyStatus` を渡し、`Stream` を受け取る
    - 変更後: `WrappedChangeKeyStateRequest` を渡し、`WrappedChangeKeyState` を受け取る
- 暗号化・復号
    - 変更前: `WrappedKeyPlain` または `WrappedKeyCipher` をリクエストに使用
    - 変更後: `WrappedEncryptionRequest` または `WrappedDecryptionRequest` をリクエストに使用
- キーの破棄予約
    - 変更前: `WrappedScheduleDestructionKey` を渡し、`Stream` を受け取る
    - 変更後: `WrappedScheduleDestructionKeyRequest` を渡し、`WrappedKeyScheduledDestruction` を受け取る

影響:

- 上記操作を呼び出しているコードは、リクエストの生成箇所を新しい型に置き換える必要があります。
- `Stream` として応答を処理していたコードは、新しいレスポンスモデルを使うように変更してください。
- キー状態・キー種別・暗号化アルゴリズムなどの enum 名も更新されています。旧型名を参照している場合はコンパイルエラーになります。

移行の考え方:

- 新しい SDK の型定義に合わせ、各操作のリクエスト生成と応答処理を更新してください。
- キー作成結果、状態変更結果、破棄予約結果から値を取得する処理は、レスポンスモデルのプロパティを確認して置き換えてください。
- enum 型をコードで参照している場合は、対象プロパティごとに次のように置き換えてください。
    - キー情報の `Status`
        - 変更前: `Key.Status` の型は `KeyStatusEnum`
        - 変更後: `Key.Status` の型は `Key_Status`
    - キー情報の `KeyOrigin`
        - 変更前: `Key.KeyOrigin` の型は `KeyOriginEnum`
        - 変更後: `Key.KeyOrigin` の型は `Key_KeyOrigin`
    - キー情報の `ServiceClass`
        - 変更前: `Key.ServiceClass` の型は `KeyServiceClassEnum`
        - 変更後: `Key.ServiceClass` の型は `Key_ServiceClass`
    - 状態変更リクエストの `Key.Status`
        - 変更前: `WrappedChangeKeyStatus.Key.Status` の型は `ChangeKeyStatus_Status`
        - 変更後: `WrappedChangeKeyStateRequest.Key.Status` の型は `ChangeKeyStateRequest_Status`
    - 状態変更レスポンスの `Key.Status`
        - 変更前: 戻り値は `Stream`（`Status` プロパティなし）
        - 変更後: `WrappedChangeKeyState.Key.Status` の型は `ChangeKeyState_Status`
    - キー作成レスポンスの `Key.KeyOrigin`
        - 変更前: `WrappedCreateKey.Key.KeyOrigin` の型は `KeyOriginEnum`
        - 変更後: `WrappedCreateKeyResponse.Key.KeyOrigin` の型は `CreateKeyResponse_KeyOrigin`
    - キー作成レスポンスの `Key.Status`
        - 変更前: `WrappedCreateKey.Key` に `Status` プロパティなし
        - 変更後: `WrappedCreateKeyResponse.Key.Status` の型は `CreateKeyResponse_Status`（新規）
    - キー作成レスポンスの `Key.ServiceClass`
        - 変更前: `WrappedCreateKey.Key` に `ServiceClass` プロパティなし
        - 変更後: `WrappedCreateKeyResponse.Key.ServiceClass` の型は `CreateKeyResponse_ServiceClass`（新規）
    - 暗号化リクエストの `Algo`
        - 変更前: `KeyPlain.Algo` の型は `KeyEncryptAlgoEnum`
        - 変更後: `EncryptionRequest.Algo` の型は `EncryptionRequest_Algo`

## 追加・改善

### キー一覧のページング

キー一覧の取得で、今回新たに `Count`（取得件数）と `From`（取得開始位置）を SDK から指定できるようになりました。ページングを利用する場合は、API のページング仕様に合わせて各値を設定してください。

### キー破棄情報の応答

キー情報に破棄予定日時などが含まれるようになり、破棄予約の応答もモデルで受け取れるようになりました。これらの情報を運用判断に使用する場合は、応答値の取り扱いを確認してください。

### ゾーン指定

ゾーン指定に石狩第3ゾーン `Is1c` が追加されました。同ゾーンを利用する場合は、ゾーン指定にこの値を使用してください。

## 仕様追随

- KMS API 1.2.0 の定義に追従しました。リクエスト/レスポンスのモデルや一部の操作結果が更新されています。
- SDK バージョンは `0.2.0` です。

## 利用者向け確認ポイント

1. キー操作で旧リクエスト型や旧 enum 型を参照していないか確認してください。
2. 状態変更・破棄予約などで、戻り値を `Stream` として扱うコードを新しいレスポンスモデルに合わせてください。
3. キー一覧のページング値が意図した取得範囲になっているか確認してください。
4. 石狩第3ゾーンを利用する場合は、ゾーン指定に `Is1c` を使用してください。
