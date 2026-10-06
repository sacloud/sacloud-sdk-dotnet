using Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.Histories;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Samples;

class AlertsSample(MonitoringSuiteClient client)
{
    private readonly MonitoringSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private const string suffix = "-ms-s";
    private const string notificationUrl = "https://example.com/monitoring-suite/alerts";

    public async Task RunAsync(long logStorageId, long metricsStorageId)
    {
        Console.WriteLine("アラートのサンプルを実行します");

        // -------------------------------------------------------
        // POST アラートプロジェクトを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /alerts/projects | アラートプロジェクトを作成する");

        var createResponse = await client.Alerts.Projects.PostAsync(new AlertProjectCreateRequest
        {
            Name = "sdk-sample-alert-project" + suffix,
            Description = "SDKから作成したサンプルアラートプロジェクト",
        });
        var projectId = createResponse?.ResourceId
            ?? throw new InvalidOperationException("アラートプロジェクトのResourceIdがNullです");
        Console.WriteLine($"  - 作成したアラートプロジェクトID: {projectId}");
        Console.WriteLine($"    - Name: {createResponse.Name}");
        Console.WriteLine($"    - Description: {createResponse.Description}");

        // -------------------------------------------------------
        // GET アラートプロジェクト一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /alerts/projects | アラートプロジェクト一覧を取得する");
        var listResponse = await client.Alerts.Projects.GetAsync();
        Console.WriteLine($"  - 取得したアラートプロジェクト数: {listResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET アラートプロジェクト詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId} | アラートプロジェクト詳細を取得する");
        var getResponse = await client.Alerts.Projects[projectId].GetAsync();
        Console.WriteLine($"  - 取得したアラートプロジェクトID: {getResponse?.Id}");
        Console.WriteLine($"    - Name: {getResponse?.Name}");

        // -------------------------------------------------------
        // PUT アラートプロジェクトを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /alerts/projects/{projectId} | アラートプロジェクトを更新する");
        var putResponse = await client.Alerts.Projects[projectId].PutAsync(new AlertProjectRequest
        {
            Name = "sdk-updated-alert-project" + suffix,
            Description = "SDKから更新したサンプルアラートプロジェクト",
        });
        Console.WriteLine($"  - 更新したアラートプロジェクトID: {putResponse?.Id}");
        Console.WriteLine($"    - Name: {putResponse?.Name}");

        // -------------------------------------------------------
        // PATCH アラートプロジェクトを部分更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /alerts/projects/{projectId} | アラートプロジェクトを部分更新する");
        var patchResponse = await client.Alerts.Projects[projectId].PatchAsync(new PatchedAlertProjectRequest
        {
            Description = "SDKから部分更新したサンプルアラートプロジェクト",
        });
        Console.WriteLine($"  - 部分更新したアラートプロジェクトID: {patchResponse?.Id}");
        Console.WriteLine($"    - Description: {patchResponse?.Description}");

        // -------------------------------------------------------
        // POST アラートルールを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /alerts/projects/{projectId}/rules | アラートルールを作成する");
        var createRuleResponse = await client.Alerts.Projects[projectId].Rules.PostAsync(new AlertRuleRequest
        {
            Name = "sdk-sample-alert-rule" + suffix,
            MetricsStorageId = metricsStorageId,
            Query = "up",
            EnabledWarning = true,
            ThresholdWarning = "< 1",
            ThresholdDurationWarning = 300,
        });
        var ruleUid = createRuleResponse?.Uid
            ?? throw new InvalidOperationException("アラートルールUIDがNullです");
        Console.WriteLine($"  - 作成したアラートルールUID: {ruleUid}");
        Console.WriteLine($"    - Name: {createRuleResponse.Name}");

        // -------------------------------------------------------
        // GET アラートルール一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/rules | アラートルール一覧を取得する");
        var listRulesResponse = await client.Alerts.Projects[projectId].Rules.GetAsync();
        Console.WriteLine($"  - 取得したアラートルール数: {listRulesResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET アラートルール詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/rules/{ruleUid} | アラートルール詳細を取得する");
        var getRuleResponse = await client.Alerts.Projects[projectId].Rules[ruleUid].GetAsync();
        Console.WriteLine($"  - 取得したアラートルールUID: {getRuleResponse?.Uid}");

        // -------------------------------------------------------
        // PUT アラートルールを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /alerts/projects/{projectId}/rules/{ruleUid} | アラートルールを更新する");
        var currentMetricsStorageId = createRuleResponse?.MetricsStorageId
            ?? throw new InvalidOperationException("アラートルールのMetricsStorageIdがNullです");
        var query = createRuleResponse?.Query
            ?? throw new InvalidOperationException("アラートルールのQueryがNullです");
        var putRuleResponse = await client.Alerts.Projects[projectId].Rules[ruleUid].PutAsync(new AlertRuleRequest
        {
            Name = "sdk-updated-alert-rule" + suffix,
            MetricsStorageId = currentMetricsStorageId,
            Query = query,
            EnabledWarning = createRuleResponse.EnabledWarning,
            EnabledCritical = createRuleResponse.EnabledCritical,
            Format = createRuleResponse.Format,
            Template = createRuleResponse.Template,
            ThresholdWarning = createRuleResponse.ThresholdWarning,
            ThresholdCritical = createRuleResponse.ThresholdCritical,
            ThresholdDurationWarning = createRuleResponse.ThresholdDurationWarning,
            ThresholdDurationCritical = createRuleResponse.ThresholdDurationCritical,
        });
        Console.WriteLine($"  - 更新したアラートルールUID: {putRuleResponse?.Uid}");
        Console.WriteLine($"    - Name: {putRuleResponse?.Name}");

        // -------------------------------------------------------
        // PATCH アラートルールを部分更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /alerts/projects/{projectId}/rules/{ruleUid} | アラートルールを部分更新する");
        var patchRuleResponse = await client.Alerts.Projects[projectId].Rules[ruleUid].PatchAsync(new PatchedAlertRuleRequest
        {
            Template = "sdk-alert-template" + suffix,
            ThresholdWarning = "< 1",
        });
        Console.WriteLine($"  - 部分更新したアラートルールUID: {patchRuleResponse?.Uid}");
        Console.WriteLine($"    - Template: {patchRuleResponse?.Template}");

        // -------------------------------------------------------
        // GET アラート履歴一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/histories | アラート履歴一覧を取得する");
        var listHistoriesResponse = await client.Alerts.Projects[projectId].Histories.GetAsync();
        Console.WriteLine($"  - 取得したアラート履歴数: {listHistoriesResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET アラート履歴一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/histories?severity=warning&open=true | アラート履歴一覧をクエリパラメータ付きで取得する");
        var listHistoriesWithQueryResponse = await client.Alerts.Projects[projectId].Histories.GetAsync(config =>
        {
            config.QueryParameters.Severity = GetSeverityQueryParameterType.Warning;
            config.QueryParameters.Open = true;
        });
        Console.WriteLine($"  - 取得したアラート履歴数 (severity=warning, open=true): {listHistoriesWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET アラート履歴詳細を取得する
        if (listHistoriesResponse?.Results != null && listHistoriesResponse.Results.Count > 0)
        {
            var firstHistoryUid = listHistoriesResponse.Results[0].Uid
                ?? throw new InvalidOperationException("アラート履歴UIDがNullです");
            Console.WriteLine("-----------------------");
            Console.WriteLine($"GET /alerts/projects/{projectId}/histories/{firstHistoryUid} | アラート履歴詳細を取得する");
            var getHistoryResponse = await client.Alerts.Projects[projectId].Histories[firstHistoryUid].GetAsync();
            Console.WriteLine($"  - 取得したアラート履歴UID: {getHistoryResponse?.Uid}");
            Console.WriteLine($"    - Severity: {getHistoryResponse?.Severity}");
            Console.WriteLine($"    - Open: {getHistoryResponse?.Open}");
        }

        // -------------------------------------------------------
        // GET アラートルール履歴一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/rules/{ruleUid}/histories | アラートルール履歴一覧を取得する");
        var listRuleHistoriesResponse = await client.Alerts.Projects[projectId].Rules[ruleUid].Histories.GetAsync();
        Console.WriteLine($"  - 取得したアラートルール履歴数: {listRuleHistoriesResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET アラートルール履歴一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/rules/{ruleUid}/histories?severity=warning&open=true | アラートルール履歴一覧をクエリパラメータ付きで取得する");
        var listRuleHistoriesWithQueryResponse = await client.Alerts.Projects[projectId].Rules[ruleUid].Histories.GetAsync(config =>
        {
            config.QueryParameters.Severity = Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.Rules.Item.Histories.GetSeverityQueryParameterType.Warning;
            config.QueryParameters.Open = true;
        });
        Console.WriteLine($"  - 取得したアラートルール履歴数 (severity=warning, open=true): {listRuleHistoriesWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET アラートルール履歴詳細を取得する
        if (listRuleHistoriesResponse?.Results != null && listRuleHistoriesResponse.Results.Count > 0)
        {
            var firstRuleHistoryUid = listRuleHistoriesResponse.Results[0].Uid
                ?? throw new InvalidOperationException("アラートルール履歴UIDがNullです");
            Console.WriteLine("-----------------------");
            Console.WriteLine($"GET /alerts/projects/{projectId}/rules/{ruleUid}/histories/{firstRuleHistoryUid} | アラートルール履歴詳細を取得する");
            var getRuleHistoryResponse = await client.Alerts.Projects[projectId].Rules[ruleUid].Histories[firstRuleHistoryUid].GetAsync();
            Console.WriteLine($"  - 取得したアラートルール履歴UID: {getRuleHistoryResponse?.Uid}");
            Console.WriteLine($"    - Severity: {getRuleHistoryResponse?.Severity}");
            Console.WriteLine($"    - Open: {getRuleHistoryResponse?.Open}");
        }

        // -------------------------------------------------------
        // POST 通知対象を作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /alerts/projects/{projectId}/notification-targets | 通知対象を作成する");
        var createTargetResponse = await client.Alerts.Projects[projectId].NotificationTargets.PostAsync(new NotificationTargetRequest
        {
            Description = "SDKから作成した通知対象",
            ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE,
            Url = notificationUrl,
        });
        var targetUid = createTargetResponse?.Uid
            ?? throw new InvalidOperationException("通知対象UIDがNullです");
        Console.WriteLine($"  - 作成した通知対象UID: {targetUid}");
        Console.WriteLine($"    - Url: {createTargetResponse?.Url}");

        // -------------------------------------------------------
        // GET 通知対象一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/notification-targets | 通知対象一覧を取得する");
        var listTargetsResponse = await client.Alerts.Projects[projectId].NotificationTargets.GetAsync();
        Console.WriteLine($"  - 取得した通知対象数: {listTargetsResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET 通知対象一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/notification-targets?service_type=SAKURA_SIMPLE_NOTICE | 通知対象一覧をクエリパラメータ付きで取得する");
        var listTargetsWithQueryResponse = await client.Alerts.Projects[projectId].NotificationTargets.GetAsync(config =>
        {
            config.QueryParameters.ServiceType = Sacloud.Sdk.MonitoringSuite.Alerts.Projects.Item.NotificationTargets.GetService_typeQueryParameterType.SAKURA_SIMPLE_NOTICE;
        });
        Console.WriteLine($"  - 取得した通知対象数 (service_type=SAKURA_SIMPLE_NOTICE): {listTargetsWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET 通知対象詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/notification-targets/{targetUid} | 通知対象詳細を取得する");
        var getTargetResponse = await client.Alerts.Projects[projectId].NotificationTargets[targetUid].GetAsync();
        Console.WriteLine($"  - 取得した通知対象UID: {getTargetResponse?.Uid}");
        Console.WriteLine($"    - Description: {getTargetResponse?.Description}");

        // -------------------------------------------------------
        // PUT 通知対象を更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /alerts/projects/{projectId}/notification-targets/{targetUid} | 通知対象を更新する");
        var putTargetResponse = await client.Alerts.Projects[projectId].NotificationTargets[targetUid].PutAsync(new NotificationTargetRequest
        {
            Description = "SDKから更新した通知対象",
            ServiceType = NotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE,
            Url = notificationUrl + "/put",
        });
        Console.WriteLine($"  - 更新した通知対象UID: {putTargetResponse?.Uid}");
        Console.WriteLine($"    - Url: {putTargetResponse?.Url}");

        // -------------------------------------------------------
        // PATCH 通知対象を部分更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /alerts/projects/{projectId}/notification-targets/{targetUid} | 通知対象を部分更新する");
        var patchTargetResponse = await client.Alerts.Projects[projectId].NotificationTargets[targetUid].PatchAsync(new PatchedNotificationTargetRequest
        {
            Description = "SDKから部分更新した通知対象",
            ServiceType = PatchedNotificationTargetRequest_service_type.SAKURA_SIMPLE_NOTICE,
            Url = notificationUrl + "/patch",
        });
        Console.WriteLine($"  - 部分更新した通知対象UID: {patchTargetResponse?.Uid}");
        Console.WriteLine($"    - Description: {patchTargetResponse?.Description}");

        // -------------------------------------------------------
        // POST 通知ルーティングを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /alerts/projects/{projectId}/notification-routings | 通知ルーティングを作成する");
        var createRoutingResponse = await client.Alerts.Projects[projectId].NotificationRoutings.PostAsync(new NotificationRoutingRequest
        {
            MatchLabels =
            [
                new MatchLabelsItemRequest
                {
                    Name = "severity",
                    Value = "warning",
                },
            ],
            NotificationTargetUid = targetUid,
            ResendIntervalMinutes = 10,
        });
        var routingUid = createRoutingResponse?.Uid
            ?? throw new InvalidOperationException("通知ルーティングUIDがNullです");
        Console.WriteLine($"  - 作成した通知ルーティングUID: {routingUid}");
        Console.WriteLine($"    - ResendIntervalMinutes: {createRoutingResponse?.ResendIntervalMinutes}");

        // -------------------------------------------------------
        // GET 通知ルーティング一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/notification-routings | 通知ルーティング一覧を取得する");
        var listRoutingsResponse = await client.Alerts.Projects[projectId].NotificationRoutings.GetAsync();
        Console.WriteLine($"  - 取得した通知ルーティング数: {listRoutingsResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET 通知ルーティング詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/notification-routings/{routingUid} | 通知ルーティング詳細を取得する");
        var getRoutingResponse = await client.Alerts.Projects[projectId].NotificationRoutings[routingUid].GetAsync();
        Console.WriteLine($"  - 取得した通知ルーティングUID: {getRoutingResponse?.Uid}");
        Console.WriteLine($"    - Order: {getRoutingResponse?.Order}");

        // -------------------------------------------------------
        // PUT 通知ルーティングを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /alerts/projects/{projectId}/notification-routings/{routingUid} | 通知ルーティングを更新する");
        var putRoutingResponse = await client.Alerts.Projects[projectId].NotificationRoutings[routingUid].PutAsync(new NotificationRoutingRequest
        {
            MatchLabels =
            [
                new MatchLabelsItemRequest
                {
                    Name = "severity",
                    Value = "critical",
                },
            ],
            NotificationTargetUid = targetUid,
            ResendIntervalMinutes = 20,
        });
        Console.WriteLine($"  - 更新した通知ルーティングUID: {putRoutingResponse?.Uid}");
        Console.WriteLine($"    - ResendIntervalMinutes: {putRoutingResponse?.ResendIntervalMinutes}");

        // -------------------------------------------------------
        // PATCH 通知ルーティングを部分更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /alerts/projects/{projectId}/notification-routings/{routingUid} | 通知ルーティングを部分更新する");
        var patchRoutingResponse = await client.Alerts.Projects[projectId].NotificationRoutings[routingUid].PatchAsync(new PatchedNotificationRoutingRequest
        {
            MatchLabels =
            [
                new MatchLabelsItemRequest
                {
                    Name = "severity",
                    Value = "warning",
                },
            ],
            NotificationTargetUid = targetUid,
            ResendIntervalMinutes = 30,
        });
        Console.WriteLine($"  - 部分更新した通知ルーティングUID: {patchRoutingResponse?.Uid}");
        Console.WriteLine($"    - ResendIntervalMinutes: {patchRoutingResponse?.ResendIntervalMinutes}");

        // -------------------------------------------------------
        // PUT 通知ルーティングの順序を更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /alerts/projects/{projectId}/notification-routings/reorder | 通知ルーティングの順序を更新する");
        await client.Alerts.Projects[projectId].NotificationRoutings.Reorder.PutAsync(
        [
            new NotificationRoutingOrderRequest
            {
                NotificationRoutingUid = routingUid,
                Order = 1,
            },
        ]);
        Console.WriteLine("  - 通知ルーティングの順序更新が完了しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // POST ログ計測ルールを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /alerts/projects/{projectId}/log-measure-rules | ログ計測ルールを作成する");
        var createLogMeasureRuleResponse = await client.Alerts.Projects[projectId].LogMeasureRules.PostAsync(new LogMeasureRuleRequest
        {
            Name = "sdk_sample_log_measure_rule" + suffix,
            Description = "SDKから作成したログ計測ルール",
            LogStorageId = logStorageId,
            MetricsStorageId = metricsStorageId,
            Rule = new LogMeasureRuleModelRequest
            {
                Version = LogMeasureRuleVersionEnum.V1,
                Query = new LogMeasureRuleV1
                {
                    Matchers = [],
                },
            },
        });
        var logMeasureRuleUid = createLogMeasureRuleResponse?.Uid
            ?? throw new InvalidOperationException("ログ計測ルールUIDがNullです");
        Console.WriteLine($"  - 作成したログ計測ルールUID: {logMeasureRuleUid}");
        Console.WriteLine($"    - Name: {createLogMeasureRuleResponse?.Name}");

        // -------------------------------------------------------
        // GET ログ計測ルール一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/log-measure-rules | ログ計測ルール一覧を取得する");
        var listLogMeasureRulesResponse = await client.Alerts.Projects[projectId].LogMeasureRules.GetAsync();
        Console.WriteLine($"  - 取得したログ計測ルール数: {listLogMeasureRulesResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ログ計測ルール一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/log-measure-rules?metrics_storage_id=0 | ログ計測ルール一覧をクエリパラメータ付きで取得する");
        var listLogMeasureRulesWithQueryResponse = await client.Alerts.Projects[projectId].LogMeasureRules.GetAsync(config =>
        {
            config.QueryParameters.MetricsStorageId = 0;
        });
        Console.WriteLine($"  - 取得したログ計測ルール数 (metrics_storage_id=0): {listLogMeasureRulesWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ログ計測ルール詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /alerts/projects/{projectId}/log-measure-rules/{logMeasureRuleUid} | ログ計測ルール詳細を取得する");
        var getLogMeasureRuleResponse = await client.Alerts.Projects[projectId].LogMeasureRules[logMeasureRuleUid].GetAsync();
        Console.WriteLine($"  - 取得したログ計測ルールUID: {getLogMeasureRuleResponse?.Uid}");
        Console.WriteLine($"    - Name: {getLogMeasureRuleResponse?.Name}");

        // -------------------------------------------------------
        // PUT ログ計測ルールを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /alerts/projects/{projectId}/log-measure-rules/{logMeasureRuleUid} | ログ計測ルールを更新する");
        var putLogMeasureRuleResponse = await client.Alerts.Projects[projectId].LogMeasureRules[logMeasureRuleUid].PutAsync(new LogMeasureRuleRequest
        {
            Name = "sdk_updated_log_measure_rule" + suffix,
            Description = "SDKから更新したログ計測ルール",
            LogStorageId = logStorageId,
            MetricsStorageId = metricsStorageId,
            Rule = new LogMeasureRuleModelRequest
            {
                Version = LogMeasureRuleVersionEnum.V1,
                Query = new LogMeasureRuleV1
                {
                    Matchers = [],
                },
            },
        });
        Console.WriteLine($"  - 更新したログ計測ルールUID: {putLogMeasureRuleResponse?.Uid}");
        Console.WriteLine($"    - Name: {putLogMeasureRuleResponse?.Name}");

        // -------------------------------------------------------
        // PATCH ログ計測ルールを部分更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /alerts/projects/{projectId}/log-measure-rules/{logMeasureRuleUid} | ログ計測ルールを部分更新する");
        var patchLogMeasureRuleResponse = await client.Alerts.Projects[projectId].LogMeasureRules[logMeasureRuleUid].PatchAsync(new PatchedLogMeasureRuleRequest
        {
            Description = "SDKから部分更新したログ計測ルール",
        });
        Console.WriteLine($"  - 部分更新したログ計測ルールUID: {patchLogMeasureRuleResponse?.Uid}");
        Console.WriteLine($"    - Description: {patchLogMeasureRuleResponse?.Description}");

        // -------------------------------------------------------
        // DELETE アラートルールを削除する（クリーンアップ）
        /*
        await client.Alerts.Projects[projectId].Rules[ruleUid].DeleteAsync();
        Console.WriteLine("  - アラートルールの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE ログ計測ルールを削除する（クリーンアップ）
        /*
        await client.Alerts.Projects[projectId].LogMeasureRules[logMeasureRuleUid].DeleteAsync();
        Console.WriteLine("  - ログ計測ルールの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE 通知ルーティングを削除する（クリーンアップ）
        /*
        await client.Alerts.Projects[projectId].NotificationRoutings[routingUid].DeleteAsync();
        Console.WriteLine("  - 通知ルーティングの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE 通知対象を削除する（クリーンアップ）
        /*
        await client.Alerts.Projects[projectId].NotificationTargets[targetUid].DeleteAsync();
        Console.WriteLine("  - 通知対象の削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE アラートプロジェクトを削除する（クリーンアップ）
        /*
        await client.Alerts.Projects[projectId].DeleteAsync();
        Console.WriteLine("  - アラートプロジェクトの削除が完了しました (レスポンスボディ無し)");
        */

        Console.WriteLine("アラートのサンプルの実行が完了しました");
    }
}
