using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Samples;

class DashboardsSample(MonitoringSuiteClient client)
{
    private readonly MonitoringSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private const string suffix = "-ms-s";

    public async Task RunAsync()
    {
        Console.WriteLine("ダッシュボードのサンプルを実行します");

        // -------------------------------------------------------
        // POST ダッシュボードプロジェクトを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /dashboards/projects | ダッシュボードプロジェクトを作成する");

        var createResponse = await client.Dashboards.Projects.PostAsync(new DashboardProjectCreateRequest
        {
            Name = "sdk-sample-dashboard-project" + suffix,
            Description = "SDKから作成したサンプルダッシュボードプロジェクト",
        });
        var resourceId = createResponse?.ResourceId
            ?? throw new InvalidOperationException("ダッシュボードプロジェクトのResourceIdがNullです");
        Console.WriteLine($"  - 作成したダッシュボードプロジェクトID: {createResponse.Id}");
        Console.WriteLine($"    - ResourceId: {resourceId}");
        Console.WriteLine($"    - Name: {createResponse.Name}");

        // -------------------------------------------------------
        // GET ダッシュボードプロジェクト一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /dashboards/projects | ダッシュボードプロジェクト一覧を取得する");
        var listResponse = await client.Dashboards.Projects.GetAsync();
        Console.WriteLine($"  - 取得したダッシュボードプロジェクト数: {listResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ダッシュボードプロジェクト詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /dashboards/projects/{resourceId} | ダッシュボードプロジェクト詳細を取得する");
        var getResponse = await client.Dashboards.Projects[resourceId].GetAsync();
        Console.WriteLine($"  - 取得したダッシュボードプロジェクトID: {getResponse?.Id}");
        Console.WriteLine($"    - Name: {getResponse?.Name}");
        Console.WriteLine($"    - IsOk: {getResponse?.IsOk}");

        // -------------------------------------------------------
        // PATCH ダッシュボードプロジェクトを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /dashboards/projects/{resourceId} | ダッシュボードプロジェクトを更新する");
        var patchResponse = await client.Dashboards.Projects[resourceId].PatchAsync(new PatchedDashboardProjectRequest
        {
            Name = "sdk-updated-dashboard-project" + suffix,
            Description = "SDKから更新したサンプルダッシュボードプロジェクト",
        });
        Console.WriteLine($"  - 更新したダッシュボードプロジェクトID: {patchResponse?.Id}");
        Console.WriteLine($"    - Name: {patchResponse?.Name}");

        // -------------------------------------------------------
        // PUT ダッシュボードプロジェクトを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /dashboards/projects/{resourceId} | ダッシュボードプロジェクトを更新する");
        var putResponse = await client.Dashboards.Projects[resourceId].PutAsync(new DashboardProjectRequest
        {
            Name = "sdk-put-dashboard-project" + suffix,
            Description = "SDKからPUT更新したサンプルダッシュボードプロジェクト",
        });
        Console.WriteLine($"  - PUT更新したダッシュボードプロジェクトID: {putResponse?.Id}");
        Console.WriteLine($"    - Name: {putResponse?.Name}");

        // -------------------------------------------------------
        // DELETE ダッシュボードプロジェクトを削除する（クリーンアップ）
        /*
        await client.Dashboards.Projects[resourceId].DeleteAsync();
        Console.WriteLine("  - ダッシュボードプロジェクトの削除が完了しました (レスポンスボディ無し)");
        */
        Console.WriteLine("ダッシュボードのサンプルの実行が完了しました");
    }
}
