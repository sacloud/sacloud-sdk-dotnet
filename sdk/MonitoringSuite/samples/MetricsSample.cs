using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Samples;

class MetricsSample(MonitoringSuiteClient client)
{
    private readonly MonitoringSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private const string suffix = "-ms-s";

    public async Task RunAsync()
    {
        Console.WriteLine("メトリクスのサンプルを実行します");

        var listPublishersResponse = await client.Publishers.GetAsync();
        Console.WriteLine($"  - 取得した連携サービス数: {listPublishersResponse?.Results?.Count}");

        var publisher = listPublishersResponse?.Results?.FirstOrDefault()
            ?? throw new InvalidOperationException("利用可能な連携サービスが見つかりませんでした");
        var publisherCode = publisher.Code
            ?? throw new InvalidOperationException("連携サービスコードがNullです");
        var publisherVariant = publisher.Variants?.FirstOrDefault()
            ?? throw new InvalidOperationException($"連携サービス {publisherCode} に利用可能な variant が見つかりませんでした");
        var variant = publisherVariant.Name
            ?? throw new InvalidOperationException($"連携サービス {publisherCode} の variant 名がNullです");

        Console.WriteLine($"  - 使用する連携サービスコード: {publisherCode}");
        Console.WriteLine($"  - 使用する variant: {variant}");

        // -------------------------------------------------------
        // POST メトリクスストレージを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /metrics/storages | メトリクスストレージを作成する");

        var createStorageResponse = await client.Metrics.Storages.PostAsync(new MetricsStorageCreateRequest
        {
            Name = "sdk-sample-metrics-storage" + suffix,
            Description = "SDKから作成したサンプルメトリクスストレージ",
            IsSystem = false,
        });
        var storageId = createStorageResponse?.ResourceId
            ?? throw new InvalidOperationException("メトリクスストレージのResourceIdがNullです");
        Console.WriteLine($"  - 作成したメトリクスストレージID: {storageId}");
        Console.WriteLine($"    - Name: {createStorageResponse.Name}");

        // -------------------------------------------------------
        // GET メトリクスストレージ一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /metrics/storages | メトリクスストレージ一覧を取得する");
        var listStoragesResponse = await client.Metrics.Storages.GetAsync();
        Console.WriteLine($"  - 取得したメトリクスストレージ数: {listStoragesResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET メトリクスストレージ一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/storages?account_id=<YOUR_ACCOUNT_ID>&resource_id={storageId}&is_system=false&count=10&from=0 | メトリクスストレージ一覧をクエリパラメータ付きで取得する");
        var listStoragesWithQueryResponse = await client.Metrics.Storages.GetAsync(config =>
        {
            config.QueryParameters.AccountId = "<YOUR_ACCOUNT_ID>";
            config.QueryParameters.IsSystem = false;
            config.QueryParameters.ResourceId = storageId;
            config.QueryParameters.Count = 10;
            config.QueryParameters.From = 0;
        });
        Console.WriteLine($"  - 取得したメトリクスストレージ数 (account_id, is_system=false, resource_id={storageId}, count=10, from=0): {listStoragesWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET メトリクスストレージ詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/storages/{storageId} | メトリクスストレージ詳細を取得する");
        var getStorageResponse = await client.Metrics.Storages[storageId].GetAsync();
        Console.WriteLine($"  - 取得したメトリクスストレージID: {getStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {getStorageResponse?.Name}");

        // -------------------------------------------------------
        // PATCH メトリクスストレージを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /metrics/storages/{storageId} | メトリクスストレージを更新する");
        var patchStorageResponse = await client.Metrics.Storages[storageId].PatchAsync(new PatchedMetricsStorageRequest
        {
            Name = "sdk-updated-metrics-storage" + suffix,
            Description = "SDKから更新したサンプルメトリクスストレージ",
        });
        Console.WriteLine($"  - 更新したメトリクスストレージID: {patchStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {patchStorageResponse?.Name}");

        // -------------------------------------------------------
        // PUT メトリクスストレージを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /metrics/storages/{storageId} | メトリクスストレージを更新する");
        var putStorageResponse = await client.Metrics.Storages[storageId].PutAsync(new MetricsStorageRequest
        {
            Name = "sdk-put-metrics-storage" + suffix,
            Description = "SDKからPUT更新したサンプルメトリクスストレージ",
        });
        Console.WriteLine($"  - PUT更新したメトリクスストレージID: {putStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {putStorageResponse?.Name}");

        // -------------------------------------------------------
        // GET メトリクスストレージ日次利用状況を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/storages/{storageId}/stats/daily | メトリクスストレージ日次利用状況を取得する");
        var dailyUsageResponse = await client.Metrics.Storages[storageId].Stats.Daily.GetAsync();
        Console.WriteLine($"  - 取得した日次利用状況数: {dailyUsageResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // GET メトリクスストレージ日次利用状況をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/storages/{storageId}/stats/daily?start_date=2024-01-01&end_date=2024-01-31 | メトリクスストレージ日次利用状況をクエリパラメータ付きで取得する");
        var dailyUsageWithQueryResponse = await client.Metrics.Storages[storageId].Stats.Daily.GetAsync(config =>
        {
            config.QueryParameters.StartDate = new Microsoft.Kiota.Abstractions.Date(2024, 1, 1);
            config.QueryParameters.EndDate = new Microsoft.Kiota.Abstractions.Date(2024, 1, 31);
        });
        Console.WriteLine($"  - 取得した日次利用状況数 (start_date=2024-01-01, end_date=2024-01-31): {dailyUsageWithQueryResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // GET メトリクスストレージ月次利用状況を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/storages/{storageId}/stats/monthly?year=2024 | メトリクスストレージ月次利用状況を取得する");
        var monthlyUsageResponse = await client.Metrics.Storages[storageId].Stats.Monthly.GetAsync(config =>
        {
            config.QueryParameters.Year = 2024;
        });
        Console.WriteLine($"  - 取得した月次利用状況数: {monthlyUsageResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // POST メトリクスストレージアクセスキーを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /metrics/storages/{storageId}/keys | メトリクスストレージアクセスキーを作成する");
        var createKeyResponse = await client.Metrics.Storages[storageId].Keys.PostAsync(new MetricsStorageAccessKeyRequest
        {
            Description = "SDKから作成したメトリクスストレージアクセスキー",
        });
        var keyUid = createKeyResponse?.Uid
            ?? throw new InvalidOperationException("メトリクスストレージアクセスキーUIDがNullです");
        Console.WriteLine($"  - 作成したメトリクスストレージアクセスキーUID: {keyUid}");
        Console.WriteLine("    - Token: <REDACTED>");

        // -------------------------------------------------------
        // GET メトリクスストレージアクセスキー一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/storages/{storageId}/keys | メトリクスストレージアクセスキー一覧を取得する");
        var listKeysResponse = await client.Metrics.Storages[storageId].Keys.GetAsync();
        Console.WriteLine($"  - 取得したメトリクスストレージアクセスキー数: {listKeysResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET メトリクスストレージアクセスキー詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/storages/{storageId}/keys/{keyUid} | メトリクスストレージアクセスキー詳細を取得する");
        var getKeyResponse = await client.Metrics.Storages[storageId].Keys[keyUid].GetAsync();
        Console.WriteLine($"  - 取得したメトリクスストレージアクセスキーUID: {getKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {getKeyResponse?.Description}");

        // -------------------------------------------------------
        // PUT メトリクスストレージアクセスキーを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /metrics/storages/{storageId}/keys/{keyUid} | メトリクスストレージアクセスキーを更新する");
        var putKeyResponse = await client.Metrics.Storages[storageId].Keys[keyUid].PutAsync(new MetricsStorageAccessKeyRequest
        {
            Description = "SDKからPUT更新したメトリクスストレージアクセスキー",
        });
        Console.WriteLine($"  - PUT更新したメトリクスストレージアクセスキーUID: {putKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {putKeyResponse?.Description}");

        // -------------------------------------------------------
        // PATCH メトリクスストレージアクセスキーを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /metrics/storages/{storageId}/keys/{keyUid} | メトリクスストレージアクセスキーを更新する");
        var patchKeyResponse = await client.Metrics.Storages[storageId].Keys[keyUid].PatchAsync(new PatchedMetricsStorageAccessKeyRequest
        {
            Description = "SDKからPATCH更新したメトリクスストレージアクセスキー",
        });
        Console.WriteLine($"  - PATCH更新したメトリクスストレージアクセスキーUID: {patchKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {patchKeyResponse?.Description}");

        // -------------------------------------------------------
        // POST メトリクスルーティングを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /metrics/routings | メトリクスルーティングを作成する");
        var createRoutingResponse = await client.Metrics.Routings.PostAsync(new MetricsRoutingRequest
        {
            PublisherCode = publisherCode,
            Variant = variant,
            MetricsStorageId = storageId,
        });
        var routingUid = createRoutingResponse?.Uid
            ?? throw new InvalidOperationException("メトリクスルーティングUIDがNullです");
        Console.WriteLine($"  - 作成したメトリクスルーティングUID: {routingUid}");
        Console.WriteLine($"    - Variant: {createRoutingResponse?.Variant}");

        // -------------------------------------------------------
        // GET メトリクスルーティング一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /metrics/routings | メトリクスルーティング一覧を取得する");
        var listRoutingsResponse = await client.Metrics.Routings.GetAsync();
        Console.WriteLine($"  - 取得したメトリクスルーティング数: {listRoutingsResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET メトリクスルーティング一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/routings?publisher__code={publisherCode}&resource_id={storageId}&variant={variant}&count=10&from=0 | メトリクスルーティング一覧をクエリパラメータ付きで取得する");
        var listRoutingsWithQueryResponse = await client.Metrics.Routings.GetAsync(config =>
        {
            config.QueryParameters.PublisherCode = publisherCode;
            config.QueryParameters.ResourceId = storageId;
            config.QueryParameters.Variant = variant;
            config.QueryParameters.Count = 10;
            config.QueryParameters.From = 0;
        });
        Console.WriteLine($"  - 取得したメトリクスルーティング数 (publisher__code, resource_id={storageId}, variant, count=10, from=0): {listRoutingsWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET メトリクスルーティング詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /metrics/routings/{routingUid} | メトリクスルーティング詳細を取得する");
        var getRoutingResponse = await client.Metrics.Routings[routingUid].GetAsync();
        Console.WriteLine($"  - 取得したメトリクスルーティングUID: {getRoutingResponse?.Uid}");
        Console.WriteLine($"    - Variant: {getRoutingResponse?.Variant}");
        Console.WriteLine($"    - IsOk: {getRoutingResponse?.IsOk}");

        // -------------------------------------------------------
        // PUT メトリクスルーティングを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /metrics/routings/{routingUid} | メトリクスルーティングを更新する");
        var putRoutingResponse = await client.Metrics.Routings[routingUid].PutAsync(new MetricsRoutingRequest
        {
            PublisherCode = publisherCode,
            Variant = variant,
            MetricsStorageId = storageId,
        });
        Console.WriteLine($"  - 更新したメトリクスルーティングUID: {putRoutingResponse?.Uid}");
        Console.WriteLine($"    - Variant: {putRoutingResponse?.Variant}");

        // -------------------------------------------------------
        // PATCH メトリクスルーティングを部分更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /metrics/routings/{routingUid} | メトリクスルーティングを部分更新する");
        var patchRoutingResponse = await client.Metrics.Routings[routingUid].PatchAsync(new PatchedMetricsRoutingRequest
        {
            MetricsStorageId = storageId,
        });
        Console.WriteLine($"  - 部分更新したメトリクスルーティングUID: {patchRoutingResponse?.Uid}");
        Console.WriteLine($"    - Variant: {patchRoutingResponse?.Variant}");

        // -------------------------------------------------------
        // DELETE メトリクスルーティングを削除する（クリーンアップ）
        /*
        await client.Metrics.Routings[routingUid].DeleteAsync();
        Console.WriteLine("  - メトリクスルーティングの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE メトリクスストレージアクセスキーを削除する（クリーンアップ）
        /*
        await client.Metrics.Storages[storageId].Keys[keyUid].DeleteAsync();
        Console.WriteLine("  - メトリクスストレージアクセスキーの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE メトリクスストレージを削除する（クリーンアップ）
        /*
        await client.Metrics.Storages[storageId].DeleteAsync();
        Console.WriteLine("  - メトリクスストレージの削除が完了しました (レスポンスボディ無し)");
        */

        Console.WriteLine("メトリクスのサンプルの実行が完了しました");
    }
}
