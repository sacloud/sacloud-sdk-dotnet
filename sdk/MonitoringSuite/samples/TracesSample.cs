using Sacloud.Sdk.MonitoringSuite.Traces.Storages;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Samples;

class TracesSample(MonitoringSuiteClient client)
{
    private readonly MonitoringSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private const string suffix = "-ms-s";

    public async Task RunAsync()
    {
        Console.WriteLine("トレースのサンプルを実行します");

        // -------------------------------------------------------
        // POST トレースストレージを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /traces/storages | トレースストレージを作成する");

        var createStorageResponse = await client.Traces.Storages.PostAsync(new TraceStorageCreateRequest
        {
            Name = "sdk-sample-trace-storage" + suffix,
            Description = "SDKから作成したサンプルトレースストレージ",
        });
        var storageId = createStorageResponse?.ResourceId
            ?? throw new InvalidOperationException("トレースストレージのResourceIdがNullです");
        Console.WriteLine($"  - 作成したトレースストレージID: {storageId}");
        Console.WriteLine($"    - Name: {createStorageResponse.Name}");

        // -------------------------------------------------------
        // GET トレースストレージ一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /traces/storages | トレースストレージ一覧を取得する");
        var listStoragesResponse = await client.Traces.Storages.GetAsync();
        Console.WriteLine($"  - 取得したトレースストレージ数: {listStoragesResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET トレースストレージ一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /traces/storages?account_id=<YOUR_ACCOUNT_ID>&log_storage__bucket__classification=shared&resource_id={storageId}&count=10&from=0 | トレースストレージ一覧をクエリパラメータ付きで取得する");
        var listStoragesWithQueryResponse = await client.Traces.Storages.GetAsync(config =>
        {
            config.QueryParameters.AccountId = "<YOUR_ACCOUNT_ID>";
            config.QueryParameters.LogStorageBucketClassification = GetLog_storage__bucket__classificationQueryParameterType.Shared;
            config.QueryParameters.ResourceId = storageId;
            config.QueryParameters.Count = 10;
            config.QueryParameters.From = 0;
        });
        Console.WriteLine($"  - 取得したトレースストレージ数 (account_id, classification=shared, resource_id={storageId}, count=10, from=0): {listStoragesWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET トレースストレージ詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /traces/storages/{storageId} | トレースストレージ詳細を取得する");
        var getStorageResponse = await client.Traces.Storages[storageId].GetAsync();
        Console.WriteLine($"  - 取得したトレースストレージID: {getStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {getStorageResponse?.Name}");

        // -------------------------------------------------------
        // PATCH トレースストレージを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /traces/storages/{storageId} | トレースストレージを更新する");
        var patchStorageResponse = await client.Traces.Storages[storageId].PatchAsync(new PatchedTraceStorageRequest
        {
            Name = "sdk-updated-trace-storage" + suffix,
            Description = "SDKから更新したサンプルトレースストレージ",
        });
        Console.WriteLine($"  - 更新したトレースストレージID: {patchStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {patchStorageResponse?.Name}");

        // -------------------------------------------------------
        // PUT トレースストレージを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /traces/storages/{storageId} | トレースストレージを更新する");
        var putStorageResponse = await client.Traces.Storages[storageId].PutAsync(new TraceStorageRequest
        {
            Name = "sdk-put-trace-storage" + suffix,
            Description = "SDKからPUT更新したサンプルトレースストレージ",
            ServicePrincipalId = patchStorageResponse?.ServicePrincipalId,
        });
        Console.WriteLine($"  - PUT更新したトレースストレージID: {putStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {putStorageResponse?.Name}");

        // -------------------------------------------------------
        // POST トレースストレージの保存期間を設定する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /traces/storages/{storageId}/set-expire | トレースストレージの保存期間を設定する");
        var setExpireResponse = await client.Traces.Storages[storageId].SetExpire.PostAsync(new SetTraceStorageExpireDayRequest
        {
            Days = 30,
        });
        Console.WriteLine($"  - 設定後の保存期間(日): {setExpireResponse?.RetentionPeriodDays}");

        // -------------------------------------------------------
        // GET トレースストレージ日次利用状況を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /traces/storages/{storageId}/stats/daily | トレースストレージ日次利用状況を取得する");
        var dailyUsageResponse = await client.Traces.Storages[storageId].Stats.Daily.GetAsync();
        Console.WriteLine($"  - 取得した日次利用状況数: {dailyUsageResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // GET トレースストレージ日次利用状況をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /traces/storages/{storageId}/stats/daily?start_date=2024-01-01&end_date=2024-01-31 | トレースストレージ日次利用状況をクエリパラメータ付きで取得する");
        var dailyUsageWithQueryResponse = await client.Traces.Storages[storageId].Stats.Daily.GetAsync(config =>
        {
            config.QueryParameters.StartDate = new Microsoft.Kiota.Abstractions.Date(2024, 1, 1);
            config.QueryParameters.EndDate = new Microsoft.Kiota.Abstractions.Date(2024, 1, 31);
        });
        Console.WriteLine($"  - 取得した日次利用状況数 (start_date=2024-01-01, end_date=2024-01-31): {dailyUsageWithQueryResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // GET トレースストレージ月次利用状況を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /traces/storages/{storageId}/stats/monthly?year=2024 | トレースストレージ月次利用状況を取得する");
        var monthlyUsageResponse = await client.Traces.Storages[storageId].Stats.Monthly.GetAsync(config =>
        {
            config.QueryParameters.Year = 2024;
        });
        Console.WriteLine($"  - 取得した月次利用状況数: {monthlyUsageResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // POST トレースストレージアクセスキーを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /traces/storages/{storageId}/keys | トレースストレージアクセスキーを作成する");
        var createKeyResponse = await client.Traces.Storages[storageId].Keys.PostAsync(new TraceStorageAccessKeyRequest
        {
            Description = "SDKから作成したトレースストレージアクセスキー",
        });
        var keyUid = createKeyResponse?.Uid
            ?? throw new InvalidOperationException("トレースストレージアクセスキーUIDがNullです");
        Console.WriteLine($"  - 作成したトレースストレージアクセスキーUID: {keyUid}");
        Console.WriteLine("    - Token: <REDACTED>");

        // -------------------------------------------------------
        // GET トレースストレージアクセスキー一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /traces/storages/{storageId}/keys | トレースストレージアクセスキー一覧を取得する");
        var listKeysResponse = await client.Traces.Storages[storageId].Keys.GetAsync();
        Console.WriteLine($"  - 取得したトレースストレージアクセスキー数: {listKeysResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET トレースストレージアクセスキー詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /traces/storages/{storageId}/keys/{keyUid} | トレースストレージアクセスキー詳細を取得する");
        var getKeyResponse = await client.Traces.Storages[storageId].Keys[keyUid].GetAsync();
        Console.WriteLine($"  - 取得したトレースストレージアクセスキーUID: {getKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {getKeyResponse?.Description}");

        // -------------------------------------------------------
        // PUT トレースストレージアクセスキーを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /traces/storages/{storageId}/keys/{keyUid} | トレースストレージアクセスキーを更新する");
        var putKeyResponse = await client.Traces.Storages[storageId].Keys[keyUid].PutAsync(new TraceStorageAccessKeyRequest
        {
            Description = "SDKからPUT更新したトレースストレージアクセスキー",
        });
        Console.WriteLine($"  - PUT更新したトレースストレージアクセスキーUID: {putKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {putKeyResponse?.Description}");

        // -------------------------------------------------------
        // PATCH トレースストレージアクセスキーを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /traces/storages/{storageId}/keys/{keyUid} | トレースストレージアクセスキーを更新する");
        var patchKeyResponse = await client.Traces.Storages[storageId].Keys[keyUid].PatchAsync(new PatchedTraceStorageAccessKeyRequest
        {
            Description = "SDKからPATCH更新したトレースストレージアクセスキー",
        });
        Console.WriteLine($"  - PATCH更新したトレースストレージアクセスキーUID: {patchKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {patchKeyResponse?.Description}");

        // -------------------------------------------------------
        // DELETE トレースストレージアクセスキーを削除する（クリーンアップ）
        /*
        await client.Traces.Storages[storageId].Keys[keyUid].DeleteAsync();
        Console.WriteLine("  - トレースストレージアクセスキーの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE トレースストレージを削除する（クリーンアップ）
        /*
        await client.Traces.Storages[storageId].DeleteAsync();
        Console.WriteLine("  - トレースストレージの削除が完了しました (レスポンスボディ無し)");
        */

        Console.WriteLine("トレースのサンプルの実行が完了しました");
    }
}
