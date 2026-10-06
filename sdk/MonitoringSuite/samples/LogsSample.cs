using Sacloud.Sdk.MonitoringSuite.Logs.Storages;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Samples;

class LogsSample(MonitoringSuiteClient client)
{
    private readonly MonitoringSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private const string suffix = "-ms-s";

    public async Task RunAsync()
    {
        Console.WriteLine("ログのサンプルを実行します");

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
        // POST ログストレージを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /logs/storages | ログストレージを作成する");

        var createStorageResponse = await client.Logs.Storages.PostAsync(new LogStorageCreateRequest
        {
            Name = "sdk-sample-log-storage" + suffix,
            Description = "SDKから作成したサンプルログストレージ",
            IsSystem = false,
        });
        var storageId = createStorageResponse?.ResourceId
            ?? throw new InvalidOperationException("ログストレージのResourceIdがNullです");
        Console.WriteLine($"  - 作成したログストレージID: {storageId}");
        Console.WriteLine($"    - Name: {createStorageResponse.Name}");

        // -------------------------------------------------------
        // GET ログストレージ一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /logs/storages | ログストレージ一覧を取得する");
        var listStoragesResponse = await client.Logs.Storages.GetAsync();
        Console.WriteLine($"  - 取得したログストレージ数: {listStoragesResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ログストレージ一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/storages?is_system=false&bucket__classification=shared&status=assigned&resource_id={storageId} | ログストレージ一覧をクエリパラメータ付きで取得する");
        var listStoragesWithQueryResponse = await client.Logs.Storages.GetAsync(config =>
        {
            config.QueryParameters.IsSystem = false;
            config.QueryParameters.BucketClassification = GetBucket__classificationQueryParameterType.Shared;
            config.QueryParameters.Status = GetStatusQueryParameterType.Assigned;
            config.QueryParameters.ResourceId = storageId;
        });
        Console.WriteLine($"  - 取得したログストレージ数 (is_system=false, classification=shared, status=assigned, resource_id={storageId}): {listStoragesWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ログストレージ詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/storages/{storageId} | ログストレージ詳細を取得する");
        var getStorageResponse = await client.Logs.Storages[storageId].GetAsync();
        Console.WriteLine($"  - 取得したログストレージID: {getStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {getStorageResponse?.Name}");

        // -------------------------------------------------------
        // PATCH ログストレージを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /logs/storages/{storageId} | ログストレージを更新する");
        var patchStorageResponse = await client.Logs.Storages[storageId].PatchAsync(new PatchedLogStorageRequest
        {
            Name = "sdk-updated-log-storage" + suffix,
            Description = "SDKから更新したサンプルログストレージ",
        });
        Console.WriteLine($"  - 更新したログストレージID: {patchStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {patchStorageResponse?.Name}");

        // -------------------------------------------------------
        // PUT ログストレージを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /logs/storages/{storageId} | ログストレージを更新する");
        var putStorageResponse = await client.Logs.Storages[storageId].PutAsync(new LogStorageRequest
        {
            Name = "sdk-put-log-storage" + suffix,
            Description = "SDKからPUT更新したサンプルログストレージ",
            ServicePrincipalId = patchStorageResponse?.ServicePrincipalId,
        });
        Console.WriteLine($"  - PUT更新したログストレージID: {putStorageResponse?.Id}");
        Console.WriteLine($"    - Name: {putStorageResponse?.Name}");

        // -------------------------------------------------------
        // POST ログストレージの保存期間を設定する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /logs/storages/{storageId}/set-expire | ログストレージの保存期間を設定する");
        var setExpireResponse = await client.Logs.Storages[storageId].SetExpire.PostAsync(new SetLogStorageExpireDayRequest
        {
            Days = 30,
        });
        Console.WriteLine($"  - 設定後の保存期間(日): {setExpireResponse?.ExpireDay}");

        // -------------------------------------------------------
        // GET ログストレージ日次利用状況を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/storages/{storageId}/stats/daily | ログストレージ日次利用状況を取得する");
        var dailyUsageResponse = await client.Logs.Storages[storageId].Stats.Daily.GetAsync();
        Console.WriteLine($"  - 取得した日次利用状況数: {dailyUsageResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // GET ログストレージ日次利用状況をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/storages/{storageId}/stats/daily?start_date=2024-01-01&end_date=2024-01-31 | ログストレージ日次利用状況をクエリパラメータ付きで取得する");
        var dailyUsageWithQueryResponse = await client.Logs.Storages[storageId].Stats.Daily.GetAsync(config =>
        {
            config.QueryParameters.StartDate = new Microsoft.Kiota.Abstractions.Date(2024, 1, 1);
            config.QueryParameters.EndDate = new Microsoft.Kiota.Abstractions.Date(2024, 1, 31);
        });
        Console.WriteLine($"  - 取得した日次利用状況数 (start_date=2024-01-01, end_date=2024-01-31): {dailyUsageWithQueryResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // GET ログストレージ月次利用状況を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/storages/{storageId}/stats/monthly?year=2024 | ログストレージ月次利用状況を取得する");
        var monthlyUsageResponse = await client.Logs.Storages[storageId].Stats.Monthly.GetAsync(config =>
        {
            config.QueryParameters.Year = 2024;
        });
        Console.WriteLine($"  - 取得した月次利用状況数: {monthlyUsageResponse?.Usages?.Count}");

        // -------------------------------------------------------
        // POST ログストレージアクセスキーを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"POST /logs/storages/{storageId}/keys | ログストレージアクセスキーを作成する");
        var createKeyResponse = await client.Logs.Storages[storageId].Keys.PostAsync(new LogStorageAccessKeyRequest
        {
            Description = "SDKから作成したログストレージアクセスキー",
        });
        var keyUid = createKeyResponse?.Uid
            ?? throw new InvalidOperationException("ログストレージアクセスキーUIDがNullです");
        Console.WriteLine($"  - 作成したログストレージアクセスキーUID: {keyUid}");
        Console.WriteLine("    - Token: <REDACTED>");

        // -------------------------------------------------------
        // GET ログストレージアクセスキー一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/storages/{storageId}/keys | ログストレージアクセスキー一覧を取得する");
        var listKeysResponse = await client.Logs.Storages[storageId].Keys.GetAsync();
        Console.WriteLine($"  - 取得したログストレージアクセスキー数: {listKeysResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ログストレージアクセスキー詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/storages/{storageId}/keys/{keyUid} | ログストレージアクセスキー詳細を取得する");
        var getKeyResponse = await client.Logs.Storages[storageId].Keys[keyUid].GetAsync();
        Console.WriteLine($"  - 取得したログストレージアクセスキーUID: {getKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {getKeyResponse?.Description}");

        // -------------------------------------------------------
        // PUT ログストレージアクセスキーを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /logs/storages/{storageId}/keys/{keyUid} | ログストレージアクセスキーを更新する");
        var putKeyResponse = await client.Logs.Storages[storageId].Keys[keyUid].PutAsync(new LogStorageAccessKeyRequest
        {
            Description = "SDKからPUT更新したログストレージアクセスキー",
        });
        Console.WriteLine($"  - PUT更新したログストレージアクセスキーUID: {putKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {putKeyResponse?.Description}");

        // -------------------------------------------------------
        // PATCH ログストレージアクセスキーを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /logs/storages/{storageId}/keys/{keyUid} | ログストレージアクセスキーを更新する");
        var patchKeyResponse = await client.Logs.Storages[storageId].Keys[keyUid].PatchAsync(new PatchedLogStorageAccessKeyRequest
        {
            Description = "SDKからPATCH更新したログストレージアクセスキー",
        });
        Console.WriteLine($"  - PATCH更新したログストレージアクセスキーUID: {patchKeyResponse?.Uid}");
        Console.WriteLine($"    - Description: {patchKeyResponse?.Description}");

        // -------------------------------------------------------
        // POST ログルーティングを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /logs/routings | ログルーティングを作成する");
        var createRoutingResponse = await client.Logs.Routings.PostAsync(new LogRoutingRequest
        {
            PublisherCode = publisherCode,
            Variant = variant,
            LogStorageId = storageId,
        });
        var routingUid = createRoutingResponse?.Uid
            ?? throw new InvalidOperationException("ログルーティングUIDがNullです");
        Console.WriteLine($"  - 作成したログルーティングUID: {routingUid}");
        Console.WriteLine($"    - Variant: {createRoutingResponse?.Variant}");

        // -------------------------------------------------------
        // GET ログルーティング一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /logs/routings | ログルーティング一覧を取得する");
        var listRoutingsResponse = await client.Logs.Routings.GetAsync();
        Console.WriteLine($"  - 取得したログルーティング数: {listRoutingsResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ログルーティング一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/routings?publisher__code={publisherCode}&resource_id={storageId}&variant={variant}&count=10&from=0 | ログルーティング一覧をクエリパラメータ付きで取得する");
        var listRoutingsWithQueryResponse = await client.Logs.Routings.GetAsync(config =>
        {
            config.QueryParameters.PublisherCode = publisherCode;
            config.QueryParameters.ResourceId = storageId;
            config.QueryParameters.Variant = variant;
            config.QueryParameters.Count = 10;
            config.QueryParameters.From = 0;
        });
        Console.WriteLine($"  - 取得したログルーティング数 (publisher__code, resource_id={storageId}, variant, count=10, from=0): {listRoutingsWithQueryResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET ログルーティング詳細を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /logs/routings/{routingUid} | ログルーティング詳細を取得する");
        var getRoutingResponse = await client.Logs.Routings[routingUid].GetAsync();
        Console.WriteLine($"  - 取得したログルーティングUID: {getRoutingResponse?.Uid}");
        Console.WriteLine($"    - Variant: {getRoutingResponse?.Variant}");
        Console.WriteLine($"    - IsOk: {getRoutingResponse?.IsOk}");

        // -------------------------------------------------------
        // PUT ログルーティングを更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /logs/routings/{routingUid} | ログルーティングを更新する");
        var putRoutingResponse = await client.Logs.Routings[routingUid].PutAsync(new LogRoutingRequest
        {
            PublisherCode = publisherCode,
            Variant = variant,
            LogStorageId = storageId,
        });
        Console.WriteLine($"  - 更新したログルーティングUID: {putRoutingResponse?.Uid}");
        Console.WriteLine($"    - Variant: {putRoutingResponse?.Variant}");

        // -------------------------------------------------------
        // PATCH ログルーティングを部分更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PATCH /logs/routings/{routingUid} | ログルーティングを部分更新する");
        var patchRoutingResponse = await client.Logs.Routings[routingUid].PatchAsync(new PatchedLogRoutingRequest
        {
            LogStorageId = storageId,
        });
        Console.WriteLine($"  - 部分更新したログルーティングUID: {patchRoutingResponse?.Uid}");
        Console.WriteLine($"    - Variant: {patchRoutingResponse?.Variant}");

        // -------------------------------------------------------
        // DELETE ログルーティングを削除する（クリーンアップ）
        /*
        await client.Logs.Routings[routingUid].DeleteAsync();
        Console.WriteLine("  - ログルーティングの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE ログストレージアクセスキーを削除する（クリーンアップ）
        /*
        await client.Logs.Storages[storageId].Keys[keyUid].DeleteAsync();
        Console.WriteLine("  - ログストレージアクセスキーの削除が完了しました (レスポンスボディ無し)");
        */

        // -------------------------------------------------------
        // DELETE ログストレージを削除する（クリーンアップ）
        /*
        await client.Logs.Storages[storageId].DeleteAsync();
        Console.WriteLine("  - ログストレージの削除が完了しました (レスポンスボディ無し)");
        */

        Console.WriteLine("ログのサンプルの実行が完了しました");
    }
}
