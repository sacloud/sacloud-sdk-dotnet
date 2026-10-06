using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Samples;

/// <summary>
/// Workflowsの各種操作を実装するクラス
/// </summary>
public class WorkflowsOperations
{
    private readonly WorkflowsClient _client;

    public WorkflowsOperations(WorkflowsClient client)
    {
        _client = client;
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. ワークフロー一覧を取得");
        Console.WriteLine("2. ワークフローを作成");
        Console.WriteLine("3. ワークフロー詳細を取得");
        Console.WriteLine("4. ワークフローを更新");
        Console.WriteLine("5. ワークフローを削除");
        Console.WriteLine("6. ワークフローサジェストを取得");
        Console.WriteLine("7. 料金プラン一覧を取得");
        Console.WriteLine("8. 課金プランを取得");
        Console.WriteLine("9. 課金プランを設定");
        Console.WriteLine("10. 課金プランを削除");
        Console.WriteLine("11. ワークフロー実行一覧を取得");
        Console.WriteLine("12. ワークフローを実行");
        Console.WriteLine("13. ワークフロー実行を取得");
        Console.WriteLine("14. ワークフロー実行をキャンセル");
        Console.WriteLine("15. ワークフロー実行履歴を取得");
        Console.WriteLine("16. ワークフローリビジョン一覧を取得");
        Console.WriteLine("17. ワークフローリビジョンを追加");
        Console.WriteLine("18. ワークフローリビジョンを取得");
        Console.WriteLine("19. ワークフローリビジョンエイリアスを更新");
        Console.WriteLine("20. ワークフローリビジョンエイリアスを削除");
        Console.WriteLine("21. ワークフロー実行を削除");
        Console.Write("選択 (1 ~ 21 から入力): ");
        var choice = Console.ReadLine();

        await ExecuteOperationAsync(choice);
    }

    /// <summary>
    /// 選択された操作を実行する
    /// </summary>
    private async Task ExecuteOperationAsync(string? choice)
    {
        switch (choice)
        {
            case "1":
                await ListWorkflowsAsync();
                break;
            case "2":
                await CreateWorkflowAsync();
                break;
            case "3":
                await GetWorkflowDetailsAsync();
                break;
            case "4":
                await UpdateWorkflowAsync();
                break;
            case "5":
                await DeleteWorkflowAsync();
                break;
            case "6":
                await GetWorkflowSuggestAsync();
                break;
            case "7":
                await ListPlansAsync();
                break;
            case "8":
                await GetSubscriptionAsync();
                break;
            case "9":
                await CreateSubscriptionAsync();
                break;
            case "10":
                await DeleteSubscriptionAsync();
                break;
            case "11":
                await ListExecutionsAsync();
                break;
            case "12":
                await ExecuteWorkflowAsync();
                break;
            case "13":
                await GetExecutionAsync();
                break;
            case "14":
                await CancelExecutionAsync();
                break;
            case "15":
                await GetExecutionHistoryAsync();
                break;
            case "16":
                await ListRevisionsAsync();
                break;
            case "17":
                await CreateRevisionAsync();
                break;
            case "18":
                await GetRevisionAsync();
                break;
            case "19":
                await UpdateRevisionAliasAsync();
                break;
            case "20":
                await DeleteRevisionAliasAsync();
                break;
            case "21":
                await DeleteExecutionAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }
    }

    private async Task ListWorkflowsAsync()
    {
        var workflows = await _client.Workflows.GetAsync();
        Console.WriteLine("ワークフロー一覧:");
        foreach (var workflow in workflows.Workflows)
        {
            Console.WriteLine($"- {workflow.Id}: {workflow.Name}");
        }
    }

    private async Task CreateWorkflowAsync()
    {
        var body = new CreateWorkflow
        {
            Name = "sample-workflow", // ワークフロー名を指定
            ConcurrencyMode = CreateWorkflow_ConcurrencyMode.Parallel, // 並列実行モード (Parallel / Lock / Queue)
            Runbook = """
                meta:
                  description: サンプルワークフロー
                args:
                  sample:
                    type: number
                    description: サンプル引数
                steps:
                  result:
                    return: ${args.sample * 2}
            """, // Runbook YAMLを指定
            Description = "サンプルワークフロー",
            Publish = true,
            Logging = false
        };
        var result = await _client.Workflows.PostAsync(body);
        Console.WriteLine("作成されたワークフロー:");
        Console.WriteLine($"- {result.Workflow.Id}: {result.Workflow.Name}");
    }

    private async Task GetWorkflowDetailsAsync()
    {
        Console.Write("詳細を取得するワークフローIDを入力してください: ");
        var id = Console.ReadLine();
        var result = await _client.Workflows[id].GetAsync();
        Console.WriteLine("ワークフロー詳細:");
        Console.WriteLine($"ID: {result.Workflow.Id}");
        Console.WriteLine($"Name: {result.Workflow.Name}");
        Console.WriteLine($"Description: {result.Workflow.Description}");
    }

    private async Task UpdateWorkflowAsync()
    {
        Console.Write("更新するワークフローIDを入力してください: ");
        var id = Console.ReadLine();
        var body = new UpdateWorkflow
        {
            Name = "renamed-workflow", // 更新するワークフロー名を指定
            ConcurrencyMode = UpdateWorkflow_ConcurrencyMode.Lock // 同時実行制御モード (Parallel / Lock / Queue)
        };
        var result = await _client.Workflows[id].PatchAsync(body);
        Console.WriteLine("更新されたワークフロー:");
        Console.WriteLine($"ID: {result.Workflow.Id}");
        Console.WriteLine($"Name: {result.Workflow.Name}");
    }

    private async Task DeleteWorkflowAsync()
    {
        Console.Write("削除するワークフローIDを入力してください: ");
        var id = Console.ReadLine();
        await _client.Workflows[id].DeleteAsync();
        Console.WriteLine("ワークフローを削除しました。");
    }

    private async Task GetWorkflowSuggestAsync()
    {
        Console.Write("サジェストを取得するワークフロー名を入力してください: ");
        var name = Console.ReadLine();
        var result = await _client.Workflows.Suggest.GetAsync(config =>
        {
            config.QueryParameters.Name = name;
        });
        Console.WriteLine("ワークフローサジェスト:");
        foreach (var suggest in result.Suggests)
        {
            Console.WriteLine($"- {suggest.Name} (Count: {suggest.Count})");
        }
    }

    private async Task ListPlansAsync()
    {
        var plans = await _client.Plans.GetAsync();
        Console.WriteLine("料金プラン一覧:");
        foreach (var plan in plans.Plans)
        {
            Console.WriteLine($"- {plan.Id}: {plan.Name}");
        }
    }

    private async Task GetSubscriptionAsync()
    {
        var subscription = await _client.Subscriptions.GetAsync();
        Console.WriteLine("課金プラン:");
        Console.WriteLine($"CurrentPlan ID: {subscription.CurrentPlan?.Id}");
        Console.WriteLine($"CurrentPlan PlanID: {subscription.CurrentPlan?.PlanId}");
        Console.WriteLine($"CurrentPlan PlanName: {subscription.CurrentPlan?.PlanName}");
    }

    private async Task CreateSubscriptionAsync()
    {
        Console.Write("設定する料金プランIDを入力してください (数値): ");
        var planIdStr = Console.ReadLine();
        if (int.TryParse(planIdStr, out var planId))
        {
            var body = new CreateSubscription
            {
                PlanId = planId
            };
            await _client.Subscriptions.PostAsync(body);
            Console.WriteLine("課金プランを設定しました。");
        }
        else
        {
            Console.WriteLine("無効なプランIDです。");
        }
    }

    private async Task DeleteSubscriptionAsync()
    {
        await _client.Subscriptions.DeleteAsync();
        Console.WriteLine("課金プランを削除しました。");
    }

    private async Task ListExecutionsAsync()
    {
        Console.Write("実行一覧を取得するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        var executions = await _client.Workflows[workflowId].Executions.GetAsync();
        Console.WriteLine("ワークフロー実行一覧:");
        foreach (var execution in executions.Executions)
        {
            Console.WriteLine($"- {execution.ExecutionId}: {execution.Status}");
        }
    }

    private async Task ExecuteWorkflowAsync()
    {
        Console.Write("実行するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("リビジョンIDを入力してください (数値、空欄の場合は最新): ");
        var revisionIdStr = Console.ReadLine();
        var body = new CreateExecution();
        if (!string.IsNullOrEmpty(revisionIdStr) && int.TryParse(revisionIdStr, out var revisionId))
        {
            body.RevisionId = revisionId;
        }
        var result = await _client.Workflows[workflowId].Executions.PostAsync(body);
        Console.WriteLine("ワークフロー実行:");
        Console.WriteLine($"Execution ID: {result.Execution.ExecutionId}");
        Console.WriteLine($"Status: {result.Execution.Status}");
    }

    private async Task GetExecutionAsync()
    {
        Console.Write("実行を取得するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("取得する実行IDを入力してください: ");
        var executionId = Console.ReadLine();
        var result = await _client.Workflows[workflowId].Executions[executionId].GetAsync();
        Console.WriteLine("ワークフロー実行:");
        Console.WriteLine($"ID: {result.Execution.ExecutionId}");
        Console.WriteLine($"Status: {result.Execution.Status}");
    }

    private async Task CancelExecutionAsync()
    {
        Console.Write("キャンセルするワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("キャンセルする実行IDを入力してください: ");
        var executionId = Console.ReadLine();
        await _client.Workflows[workflowId].Executions[executionId].Cancel.PostAsync();
        Console.WriteLine("ワークフロー実行をキャンセルしました。");
    }

    private async Task GetExecutionHistoryAsync()
    {
        Console.Write("実行履歴を取得するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("実行履歴を取得する実行IDを入力してください: ");
        var executionId = Console.ReadLine();
        var history = await _client.Workflows[workflowId].Executions[executionId].Exec_history.GetAsync();
        Console.WriteLine("ワークフロー実行履歴:");
        foreach (var item in history.Histories)
        {
            Console.WriteLine($"- JobId: {item.JobId}, Type: {item.Type}, CreatedAt: {item.CreatedAt}");
        }
    }

    private async Task ListRevisionsAsync()
    {
        Console.Write("リビジョン一覧を取得するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        var revisions = await _client.Workflows[workflowId].Revisions.GetAsync();
        Console.WriteLine("ワークフローリビジョン一覧:");
        foreach (var revision in revisions.Revisions)
        {
            Console.WriteLine($"- {revision.RevisionId}: {revision.RevisionAlias}");
        }
    }

    private async Task CreateRevisionAsync()
    {
        Console.Write("リビジョンを追加するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("リビジョンエイリアスを入力してください (空欄可): ");
        var revisionAlias = Console.ReadLine();
        
        var body = new CreateWorkflowRevision
        {
            Runbook = """
                meta:
                  description: 新しいサンプルワークフロー
                args:
                  sample:
                    type: number
                    description: サンプル引数
                steps:
                  result:
                    return: ${args.sample * 3}
            """
        };
        
        if (!string.IsNullOrEmpty(revisionAlias))
        {
            body.RevisionAlias = revisionAlias;
        }
        
        var result = await _client.Workflows[workflowId].Revisions.PostAsync(body);
        Console.WriteLine("リビジョンを追加しました:");
        Console.WriteLine($"Revision ID: {result.Revision.RevisionId}");
        Console.WriteLine($"Alias: {result.Revision.RevisionAlias}");
    }

    private async Task GetRevisionAsync()
    {
        Console.Write("リビジョンを取得するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("取得するリビジョンIDを入力してください (数値): ");
        var revisionIdStr = Console.ReadLine();
        if (int.TryParse(revisionIdStr, out var revisionId))
        {
            var result = await _client.Workflows[workflowId].Revisions[revisionId].GetAsync();
            Console.WriteLine("ワークフローリビジョン:");
            Console.WriteLine($"ID: {result.Revision.RevisionId}");
            Console.WriteLine($"Alias: {result.Revision.RevisionAlias}");
            Console.WriteLine($"CreatedAt: {result.Revision.CreatedAt}");
        }
        else
        {
            Console.WriteLine("無効なリビジョンIDです。");
        }
    }

    private async Task UpdateRevisionAliasAsync()
    {
        Console.Write("リビジョンエイリアスを更新するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("リビジョンIDを入力してください (数値): ");
        var revisionIdStr = Console.ReadLine();
        if (int.TryParse(revisionIdStr, out var revisionId))
        {
            Console.Write("新しいエイリアス名を入力してください: ");
            var aliasName = Console.ReadLine();
            var body = new UpdateWorkflowRevisionAlias
            {
                RevisionAlias = aliasName
            };
            var result = await _client.Workflows[workflowId].Revisions[revisionId].Revision_alias.PutAsync(body);
            Console.WriteLine("リビジョンエイリアスを更新しました:");
            Console.WriteLine($"Revision ID: {result.Revision.RevisionId}");
            Console.WriteLine($"Alias: {result.Revision.RevisionAlias}");
        }
        else
        {
            Console.WriteLine("無効なリビジョンIDです。");
        }
    }

    private async Task DeleteRevisionAliasAsync()
    {
        Console.Write("リビジョンエイリアスを削除するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("リビジョンIDを入力してください (数値): ");
        var revisionIdStr = Console.ReadLine();
        if (int.TryParse(revisionIdStr, out var revisionId))
        {
            await _client.Workflows[workflowId].Revisions[revisionId].Revision_alias.DeleteAsync();
            Console.WriteLine("リビジョンエイリアスを削除しました。");
        }
        else
        {
            Console.WriteLine("無効なリビジョンIDです。");
        }
    }

    private async Task DeleteExecutionAsync()
    {
        Console.Write("実行を削除するワークフローIDを入力してください: ");
        var workflowId = Console.ReadLine();
        Console.Write("削除する実行IDを入力してください: ");
        var executionId = Console.ReadLine();
        await _client.Workflows[workflowId].Executions[executionId].DeleteAsync();
        Console.WriteLine("ワークフロー実行を削除しました。");
    }
}
