using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Samples;

class ManagementSample(MonitoringSuiteClient client)
{
    private readonly MonitoringSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private static void PrintLimit(string label, ResourceItemLimits? limit)
    {
        Console.WriteLine($"  - {label} (MaxUserCount): {limit?.MaxUserCount}");
        Console.WriteLine($"  - {label} (MaxUserDedicatedCount): {limit?.MaxUserDedicatedCount}");
    }

    public async Task RunAsync()
    {
        Console.WriteLine("管理のサンプルを実行します");

        // -------------------------------------------------------
        // GET リソース上限を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /management/limits | リソース上限を取得する");
        var limitsResponse = await client.Management.Limits.GetAsync();
        PrintLimit("ログストレージ上限", limitsResponse?.Logs);
        PrintLimit("メトリクスストレージ上限", limitsResponse?.Metrics);
        PrintLimit("アラートプロジェクト上限", limitsResponse?.Alerts);
        PrintLimit("ダッシュボード上限", limitsResponse?.Dashboards);
        PrintLimit("トレースストレージ上限", limitsResponse?.Traces);

        // -------------------------------------------------------
        // GET プロビジョニング状態を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /management/provisioning/state | プロビジョニング状態を取得する");
        var stateResponse = await client.Management.Provisioning.State.GetAsync();
        Console.WriteLine($"  - ログストレージ (system_exist): {stateResponse?.Logs?.SystemExist}");
        Console.WriteLine($"  - ログストレージ (user_exist): {stateResponse?.Logs?.UserExist}");
        Console.WriteLine($"  - メトリクスストレージ (system_exist): {stateResponse?.Metrics?.SystemExist}");
        Console.WriteLine($"  - メトリクスストレージ (user_exist): {stateResponse?.Metrics?.UserExist}");

        // -------------------------------------------------------
        // POST プロビジョニングを初期化する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /management/provisioning/initialize | プロビジョニングを初期化する");
        var initResponse = await client.Management.Provisioning.Initialize.PostAsync(new ProvisioningCreateRequest
        {
            Logs = new ProvisioningExistRequest
            {
                SystemExist = true,
                UserExist = true,
            },
            Metrics = new ProvisioningExistRequest
            {
                SystemExist = true,
                UserExist = true,
            },
        });
        Console.WriteLine($"  - ログストレージ (system_exist): {initResponse?.Logs?.SystemExist}");
        Console.WriteLine($"  - ログストレージ (user_exist): {initResponse?.Logs?.UserExist}");
        Console.WriteLine($"  - メトリクスストレージ (system_exist): {initResponse?.Metrics?.SystemExist}");
        Console.WriteLine($"  - メトリクスストレージ (user_exist): {initResponse?.Metrics?.UserExist}");

        Console.WriteLine("管理のサンプルの実行が完了しました");
    }
}
