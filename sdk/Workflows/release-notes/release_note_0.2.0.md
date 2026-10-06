# Workflows リリースノート

## v0.2.0

このリリースでは、OpenAPI スペック 1.0.1 への追従により、ワークフローの**並行実行モード（ConcurrencyMode）** の設定・取得が可能になりました。また、実行履歴のキャンセル状態の追加、実行のステップ数の取得、および数値型プロパティの型修正が含まれます。数値型の型変更は既存コードに影響する場合があります。

## 重要: 互換性影響のある変更

### 数値型プロパティの型が double から int に変更

複数のモデルで数値型プロパティの型が `double?` から `int?` に変更されました。

対象プロパティ（影響範囲）:

- `CreateSubscription.PlanId`
- `CurrentPlan.PlanId`
- `ListPlansResponse.TaxRate`
- `MonthAppliedPlan.BasePrice`, `IncludedSteps`, `OveragePricePerUnit`, `OverageStepUnit`, `PlanGrade`, `PlanId`
- `Plan.BasePrice`, `Grade`, `Id`, `IncludedSteps`, `OveragePricePerUnit`, `OverageStepUnit`
- `Suggest.Count`

```cs
// 変更前（コンパイルエラーになる可能性）
double? planId = createSubscription.PlanId;

// 変更後
int? planId = createSubscription.PlanId;

// 変更前
double? planBasePrice = plan.BasePrice;

// 変更後
int? planBasePrice = plan.BasePrice;
```

影響:

- `double?` 型の変数に受け取っていた場合、コンパイルエラーが発生します。
- `double` 型との算術演算や比較を行っている場合、型の変換が必要になります。
- `Plan.Id` を `double?` 型として扱っていた場合も同様に修正が必要です。

移行の考え方:

- 上記プロパティを受け取っている変数の型を `double?` から `int?` または `int` に変更してください。
- `(double)plan.Id` のようにキャストしていた箇所は不要になります。
- `double` として計算していた箇所は、必要に応じて `int` に置き換えるか明示的なキャストを追加してください。

### MinLength バリデーションの追加

以下プロパティに MinLength(1) のバリデーションが追加されました。空文字を渡した場合にバリデーションエラーが発生します。

- `CreateExecution.Args`
- `CreateExecution.Name`
- `UpdateWorkflow.Name`

影響:

- 上記プロパティに空文字 (`""`) を渡していたコードは、バリデーションエラーが返されるようになります。
- `null` は引き続き許容されます（省略可能なプロパティの場合）。

移行の考え方:

- 空文字を渡していた箇所は、`null` に変更するか、1 文字以上の値を設定するよう修正してください。

## 追加・改善

### ワークフローの並行実行モード（ConcurrencyMode）を設定・取得可能に

ワークフローの並行実行時の動作を制御する ConcurrencyMode プロパティが追加されました。ワークフローの作成・更新・取得のすべてで利用できます。

設定できる値:

| 値 | 説明 |
|---|---|
| `Parallel` | 同時並行実行を許可 |
| `Lock` | 実行中は追加の実行をロック |
| `Queue` | 実行中は追加の実行をキュー待機 |

```cs
// ワークフロー作成時に並行実行モードを指定
CreateWorkflow createWorkflow = new CreateWorkflow
{
    Name = "my-workflow",
    Logging = true,
    Publish = true,
    Runbook = "...",
    ConcurrencyMode = CreateWorkflow_ConcurrencyMode.Queue,
};

// ワークフロー更新時に並行実行モードを変更
UpdateWorkflow updateWorkflow = new UpdateWorkflow
{
    ConcurrencyMode = UpdateWorkflow_ConcurrencyMode.Lock,
};

// ワークフロー取得時に並行実行モードを確認
Workflow workflow = ...; // GetWorkflow 等で取得
Workflow_ConcurrencyMode? mode = workflow.ConcurrencyMode;
```

### 実行のステップ数（StepCount）を取得可能に

`Execution` モデルに `StepCount`（`int?`）プロパティが追加されました。実行が処理したステップ数を取得できます。

```cs
Execution execution = ...; // GetExecution 等で取得
int? stepCount = execution.StepCount;
```

### 履歴タイプに「ワークフローキャンセル」が追加

`Historie_Type` enum に `WorkflowDidCanceled`（`workflowDidCanceled`）が追加されました。履歴一覧取得時に、ワークフローがキャンセルされた履歴がこのタイプで返されます。

影響:

- `Historie_Type` の値を switch 文等で処理している場合、`WorkflowDidCanceled` が `default` に流れます。ワークフローキャンセル時の処理が必要な場合は、明示的に case を追加してください。

## 仕様追随

- OpenAPI スペック 1.0.0 → 1.0.1 に追従
- SDK バージョン 0.1.1 → 0.2.0 に更新

## 利用者向け確認ポイント

1. 数値型プロパティ（`PlanId`, `BasePrice`, `TaxRate`, `Grade`, `IncludedSteps` など）を `double?` として受け取っているコードがないか確認し、`int?` に変更してください。
2. `CreateExecution.Args`, `CreateExecution.Name`, `UpdateWorkflow.Name` に空文字 (`""`) を渡している箇所がないか確認し、`null` または 1 文字以上の値に修正してください。
3. `Historie_Type` を switch 等で分岐処理している場合、`WorkflowDidCanceled` の処理を追加するか、`default` 処理が意図通りかを確認してください。
4. `Workflow`, `CreateWorkflow`, `UpdateWorkflow` に `ConcurrencyMode` プロパティが追加されました。必要に応じて並行実行モードを設定・確認してください。
5. `Execution.StepCount` でステップ数が取得できるようになりました。必要に応じて利用してください。
