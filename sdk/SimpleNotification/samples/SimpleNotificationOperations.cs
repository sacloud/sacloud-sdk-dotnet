// さくらのクラウドに作成したSimpleNotificationリソースに対してSDKを介して各種操作を実行する
// 対話メニューを用いて通知先・通知先グループ・通知ルーティング・履歴などの操作を確認するサンプル
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Samples;

/// <summary>
/// SimpleNotificationの各種操作を実装するクラス
/// </summary>
public class SimpleNotificationOperations
{
    private readonly SimpleNotificationClient _client;
    private readonly string _baseUrl;

    /// <summary>
    /// <see cref="SimpleNotificationOperations"/> の新しいインスタンスを初期化する
    /// </summary>
    /// <param name="client">SimpleNotificationクライアント</param>
    /// <param name="baseUrl">APIのベースURL</param>
    public SimpleNotificationOperations(SimpleNotificationClient client, string baseUrl)
    {
        _client = client;
        _baseUrl = baseUrl.TrimEnd('/');
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. 全リソース一覧を取得");
        Console.WriteLine("2. 通知先一覧を取得");
        Console.WriteLine("3. 通知先グループ一覧を取得");
        Console.WriteLine("4. 通知ルーティング一覧を取得");
        Console.WriteLine("5. 通知先を作成");
        Console.WriteLine("6. 通知先グループを作成");
        Console.WriteLine("7. 通知ルーティングを作成");
        Console.WriteLine("8. リソース詳細を取得");
        Console.WriteLine("9. 通知先を更新");
        Console.WriteLine("10. 通知先グループを更新");
        Console.WriteLine("11. 通知ルーティングを更新");
        Console.WriteLine("12. リソースを削除");
        Console.WriteLine("13. 通知先ステータスを取得");
        Console.WriteLine("14. 通知メッセージを送信");
        Console.WriteLine("15. 通知履歴一覧を取得");
        Console.WriteLine("16. 通知履歴詳細を取得");
        Console.WriteLine("17. 通知元一覧を取得");
        Console.WriteLine("18. 通知ルーティング優先順位を変更");
        Console.Write("選択 (1 ~ 18 から入力): ");
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
                await ListAllCommonServiceItemsAsync();
                break;
            case "2":
                await ListDestinationsAsync();
                break;
            case "3":
                await ListGroupsAsync();
                break;
            case "4":
                await ListRoutingsAsync();
                break;
            case "5":
                await CreateDestinationAsync();
                break;
            case "6":
                await CreateGroupAsync();
                break;
            case "7":
                await CreateRoutingAsync();
                break;
            case "8":
                await GetCommonServiceItemDetailsAsync();
                break;
            case "9":
                await UpdateDestinationAsync();
                break;
            case "10":
                await UpdateGroupAsync();
                break;
            case "11":
                await UpdateRoutingAsync();
                break;
            case "12":
                await DeleteCommonServiceItemAsync();
                break;
            case "13":
                await GetDestinationStatusAsync();
                break;
            case "14":
                await SendMessageAsync();
                break;
            case "15":
                await ListHistoriesAsync();
                break;
            case "16":
                await GetHistoryDetailsAsync();
                break;
            case "17":
                await ListSourcesAsync();
                break;
            case "18":
                await ReorderRoutingAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }
    }

    /// <summary>
    /// 全てのCommonServiceItemを取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task ListAllCommonServiceItemsAsync()
    {
        var items = await GetCommonServiceItemsAsync();
        Console.WriteLine("全リソース一覧:");
        PrintCommonServiceItems(items);
    }

    /// <summary>
    /// 通知先一覧を取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task ListDestinationsAsync()
    {
        var items = await GetCommonServiceItemsAsync(CommonServiceItemProvider_Class.Saknoticedestination);
        Console.WriteLine("通知先一覧:");
        PrintCommonServiceItems(items);
    }

    /// <summary>
    /// 通知先グループ一覧を取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task ListGroupsAsync()
    {
        var items = await GetCommonServiceItemsAsync(CommonServiceItemProvider_Class.Saknoticegroup);
        Console.WriteLine("通知先グループ一覧:");
        PrintCommonServiceItems(items);
    }

    /// <summary>
    /// 通知ルーティング一覧を取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task ListRoutingsAsync()
    {
        var items = await GetCommonServiceItemsAsync(CommonServiceItemProvider_Class.Saknoticerouting);
        Console.WriteLine("通知ルーティング一覧:");
        PrintCommonServiceItems(items);
    }

    /// <summary>
    /// 通知先を作成する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task CreateDestinationAsync()
    {
        var body = new PostCommonServiceItemRequest
        {
            CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
            {
                Name = "<YOUR_DESTINATION_NAME>",
                Description = "SimpleNotification sample destination",
                Provider = new CommonServiceItemProvider
                {
                    Class = CommonServiceItemProvider_Class.Saknoticedestination,
                    Name = "SimpleNotification"
                },
                ServiceClass = CommonServiceItemServiceClass.CloudSaknoticedestination2,
                Settings = new CommonServiceItemSettings
                {
                    DestinationSettings = new DestinationSettings
                    {
                        Disabled = false,
                        Type = DestinationSettings_Type.Email,
                        Value = "<YOUR_EMAIL_ADDRESS>"
                    }
                },
                Tags = new List<string> { "sample" }
            }
        };

        var response = await _client.Commonserviceitem.PostAsync(body);
        Console.WriteLine("作成された通知先:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 通知先グループを作成する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task CreateGroupAsync()
    {
        var destinationIds = ReadCommaSeparatedValues("通知先グループに含める通知先IDをカンマ区切りで入力してください: ");
        var body = new PostCommonServiceItemRequest
        {
            CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
            {
                Name = "<YOUR_GROUP_NAME>",
                Description = "SimpleNotification sample group",
                Provider = new CommonServiceItemProvider
                {
                    Class = CommonServiceItemProvider_Class.Saknoticegroup,
                    Name = "SimpleNotification"
                },
                ServiceClass = CommonServiceItemServiceClass.CloudSaknoticegroup2,
                Settings = new CommonServiceItemSettings
                {
                    GroupSettings = new GroupSettings
                    {
                        Disabled = false,
                        Destinations = destinationIds
                    }
                },
                Tags = new List<string> { "sample" }
            }
        };

        var response = await _client.Commonserviceitem.PostAsync(body);
        Console.WriteLine("作成された通知先グループ:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 通知ルーティングを作成する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task CreateRoutingAsync()
    {
        var body = new PostCommonServiceItemRequest
        {
            CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
            {
                Name = "<YOUR_ROUTING_NAME>",
                Description = "SimpleNotification sample routing",
                Provider = new CommonServiceItemProvider
                {
                    Class = CommonServiceItemProvider_Class.Saknoticerouting,
                    Name = "SimpleNotification"
                },
                ServiceClass = CommonServiceItemServiceClass.CloudSaknoticerouting2,
                Settings = new CommonServiceItemSettings
                {
                    RoutingSettings = new RoutingSettings
                    {
                        SourceID = "<YOUR_SOURCE_ID>",
                        TargetGroupID = "<YOUR_GROUP_ID>",
                        MatchLabels = new List<RoutingSettings_MatchLabels>
                        {
                            new RoutingSettings_MatchLabels
                            {
                                Name = "environment",
                                Value = "production"
                            }
                        }
                    }
                },
                Tags = new List<string> { "sample" }
            }
        };

        var response = await _client.Commonserviceitem.PostAsync(body);
        Console.WriteLine("作成された通知ルーティング:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 指定したリソースの詳細を取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task GetCommonServiceItemDetailsAsync()
    {
        var id = ReadRequiredValue("詳細を取得するリソースIDを入力してください: ");
        if (id == null)
        {
            return;
        }

        var response = await _client.Commonserviceitem[id].GetAsync();
        Console.WriteLine("リソース詳細:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 通知先を更新する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task UpdateDestinationAsync()
    {
        var id = ReadRequiredValue("更新する通知先IDを入力してください: ");
        if (id == null)
        {
            return;
        }

        var body = new PutCommonServiceItemRequest
        {
            CommonServiceItem = new PutCommonServiceItemRequest_CommonServiceItem
            {
                Name = "<YOUR_UPDATED_DESTINATION_NAME>",
                Description = "Updated destination from sample",
                Settings = new CommonServiceItemSettings
                {
                    DestinationSettings = new DestinationSettings
                    {
                        Disabled = false,
                        Type = DestinationSettings_Type.Email,
                        Value = "<YOUR_UPDATED_EMAIL_ADDRESS>"
                    }
                },
                Tags = new List<string> { "sample", "updated" }
            }
        };

        var response = await _client.Commonserviceitem[id].PutAsync(body);
        Console.WriteLine("更新した通知先:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 通知先グループを更新する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task UpdateGroupAsync()
    {
        var id = ReadRequiredValue("更新する通知先グループIDを入力してください: ");
        if (id == null)
        {
            return;
        }

        var body = new PutCommonServiceItemRequest
        {
            CommonServiceItem = new PutCommonServiceItemRequest_CommonServiceItem
            {
                Name = "<YOUR_UPDATED_GROUP_NAME>",
                Description = "Updated group from sample",
                Tags = new List<string> { "sample", "updated" }
            }
        };

        var response = await _client.Commonserviceitem[id].PutAsync(body);
        Console.WriteLine("更新した通知先グループ:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 通知ルーティングを更新する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task UpdateRoutingAsync()
    {
        var id = ReadRequiredValue("更新する通知ルーティングIDを入力してください: ");
        if (id == null)
        {
            return;
        }

        var body = new PutCommonServiceItemRequest
        {
            CommonServiceItem = new PutCommonServiceItemRequest_CommonServiceItem
            {
                Name = "<YOUR_UPDATED_ROUTING_NAME>",
                Description = "Updated routing from sample",
                Tags = new List<string> { "sample", "updated" }
            }
        };

        var response = await _client.Commonserviceitem[id].PutAsync(body);
        Console.WriteLine("更新した通知ルーティング:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 指定したリソースを削除する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task DeleteCommonServiceItemAsync()
    {
        var id = ReadRequiredValue("削除するリソースIDを入力してください: ");
        if (id == null)
        {
            return;
        }

        var response = await _client.Commonserviceitem[id].DeleteAsync();
        Console.WriteLine("削除したリソース:");
        PrintCommonServiceItem(response?.CommonServiceItem);
    }

    /// <summary>
    /// 通知先のステータスを取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task GetDestinationStatusAsync()
    {
        var id = ReadRequiredValue("ステータスを取得する通知先IDを入力してください: ");
        if (id == null)
        {
            return;
        }

        var response = await _client.Commonserviceitem[id].Simplenotification.Status.GetAsync();
        var status = response?.NotificationStatus;
        if (status == null)
        {
            Console.WriteLine("ステータスが見つかりませんでした。");
            return;
        }

        Console.WriteLine("通知先ステータス:");
        Console.WriteLine($"IsValid: {status.IsValid}");
        Console.WriteLine($"ModifiedAt: {status.ModifiedAt}");
    }

    /// <summary>
    /// 通知先グループにメッセージを送信する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task SendMessageAsync()
    {
        var groupId = ReadRequiredValue("メッセージを送信する通知先グループIDを入力してください: ");
        if (groupId == null)
        {
            return;
        }

        var body = new SendNotificationMessageRequest
        {
            Message = "SimpleNotification sample message"
        };

        var response = await _client.Commonserviceitem[groupId].Simplenotification.Message.PostAsync(body);
        Console.WriteLine($"送信結果: {response?.IsOk}");
    }

    /// <summary>
    /// 通知履歴一覧を取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task ListHistoriesAsync()
    {
        var response = await _client.Commonserviceitem.Simplenotification.History.GetAsync();
        Console.WriteLine("通知履歴一覧:");

        var histories = response?.NotificationHistories;
        if (histories == null || histories.Count == 0)
        {
            Console.WriteLine("通知履歴がありません。");
            return;
        }

        foreach (var history in histories)
        {
            Console.WriteLine($"- RequestId: {history.RequestId}, SourceId: {history.SourceId}, ReceivedAt: {history.ReceivedAt}");
        }
    }

    /// <summary>
    /// 指定した通知履歴の詳細を取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task GetHistoryDetailsAsync()
    {
        var requestId = ReadRequiredValue("詳細を取得する通知履歴のRequest IDを入力してください: ");
        if (requestId == null)
        {
            return;
        }

        var response = await _client.Commonserviceitem.Simplenotification.History[requestId].GetAsync();
        var history = response?.NotificationHistory;
        if (history == null)
        {
            Console.WriteLine("通知履歴が見つかりませんでした。");
            return;
        }

        Console.WriteLine("通知履歴詳細:");
        Console.WriteLine($"RequestId: {history.RequestId}");
        Console.WriteLine($"SourceId: {history.SourceId}");
        Console.WriteLine($"ReceivedAt: {history.ReceivedAt}");
        Console.WriteLine($"Title: {history.Message?.Title}");
        Console.WriteLine($"Body: {history.Message?.Body}");

        if (history.Statuses == null || history.Statuses.Count == 0)
        {
            Console.WriteLine("通知ステータスがありません。");
            return;
        }

        Console.WriteLine("Statuses:");
        foreach (var status in history.Statuses)
        {
            Console.WriteLine($"- DestinationId: {status.DestinationId}, Status: {FormatNotificationStatus(status.Status)}, ErrorInfo: {status.ErrorInfo}");
        }
    }

    /// <summary>
    /// 通知元一覧を取得して表示する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task ListSourcesAsync()
    {
        var response = await _client.Commonserviceitem.Simplenotification.Sources.GetAsync();
        Console.WriteLine("通知元一覧:");

        var sources = response?.Sources;
        if (sources == null || sources.Count == 0)
        {
            Console.WriteLine("通知元がありません。");
            return;
        }

        foreach (var source in sources)
        {
            Console.WriteLine($"- {source.Id}: {source.Name}");
        }
    }

    /// <summary>
    /// 通知ルーティングの優先順位を変更する
    /// </summary>
    /// <returns>非同期処理を表すタスク</returns>
    private async Task ReorderRoutingAsync()
    {
        var firstRoutingId = ReadRequiredValue("優先順位1にする通知ルーティングIDを入力してください: ");
        if (firstRoutingId == null)
        {
            return;
        }

        var secondRoutingId = ReadRequiredValue("優先順位2にする通知ルーティングIDを入力してください: ");
        if (secondRoutingId == null)
        {
            return;
        }

        var body = new PutCommonServiceItemRoutingReorderRequest
        {
            Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
            {
                new PutCommonServiceItemRoutingReorderRequest_Orders
                {
                    RoutingID = firstRoutingId,
                    PriorityRank = 1
                },
                new PutCommonServiceItemRoutingReorderRequest_Orders
                {
                    RoutingID = secondRoutingId,
                    PriorityRank = 2
                }
            }
        };

        var response = await _client.Commonserviceitem.Simplenotification.Routing.Reorder.PutAsync(body);
        Console.WriteLine($"並び替え結果: {response?.IsOk}");
    }

    /// <summary>
    /// 条件に応じてCommonServiceItem一覧を取得する
    /// </summary>
    /// <param name="providerClass">取得対象のProvider.Class。未指定の場合は全件取得</param>
    /// <returns>取得したCommonServiceItem一覧</returns>
    private async Task<List<CommonServiceItem>> GetCommonServiceItemsAsync(CommonServiceItemProvider_Class? providerClass = null)
    {
        var response = providerClass.HasValue
            ? await _client.Commonserviceitem.GetAsync(config =>
            {
                config.QueryParameters.ProviderClass = providerClass;
            })
            : await _client.Commonserviceitem.GetAsync();

        return response?.CommonServiceItems ?? new List<CommonServiceItem>();
    }

    /// <summary>
    /// CommonServiceItem一覧をコンソールに表示する
    /// </summary>
    /// <param name="items">表示対象のCommonServiceItem一覧</param>
    private static void PrintCommonServiceItems(List<CommonServiceItem> items)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("リソースがありません。");
            return;
        }

        foreach (var item in items)
        {
            Console.WriteLine($"- {item.ID}: {item.Name} ({item.Provider?.Class})");
        }
    }

    /// <summary>
    /// CommonServiceItemの詳細をコンソールに表示する
    /// </summary>
    /// <param name="item">表示対象のCommonServiceItem</param>
    private static void PrintCommonServiceItem(CommonServiceItem? item)
    {
        if (item == null)
        {
            Console.WriteLine("リソースが見つかりませんでした。");
            return;
        }

        Console.WriteLine($"ID: {item.ID}");
        Console.WriteLine($"Name: {item.Name}");
        Console.WriteLine($"Description: {item.Description}");
        Console.WriteLine($"Provider.Class: {item.Provider?.Class}");
        Console.WriteLine($"Tags: {string.Join(", ", item.Tags ?? new List<string>())}");

        var destinationSettings = item.Settings?.DestinationSettings;
        if (destinationSettings != null)
        {
            Console.WriteLine("Destination Settings:");
            Console.WriteLine($"  Type: {destinationSettings.Type}");
            Console.WriteLine($"  Value: {destinationSettings.Value}");
            Console.WriteLine($"  Disabled: {destinationSettings.Disabled}");
            return;
        }

        var groupSettings = item.Settings?.GroupSettings;
        if (groupSettings != null)
        {
            Console.WriteLine("Group Settings:");
            Console.WriteLine($"  Disabled: {groupSettings.Disabled}");
            Console.WriteLine($"  Destinations: {string.Join(", ", groupSettings.Destinations ?? new List<string>())}");
            return;
        }

        var routingSettings = item.Settings?.RoutingSettings;
        if (routingSettings != null)
        {
            Console.WriteLine("Routing Settings:");
            Console.WriteLine($"  SourceID: {routingSettings.SourceID}");
            Console.WriteLine($"  TargetGroupID: {routingSettings.TargetGroupID}");
            Console.WriteLine($"  PriorityRank: {routingSettings.PriorityRank}");

            if (routingSettings.MatchLabels == null || routingSettings.MatchLabels.Count == 0)
            {
                Console.WriteLine("  MatchLabels: なし");
            }
            else
            {
                Console.WriteLine("  MatchLabels:");
                foreach (var label in routingSettings.MatchLabels)
                {
                    Console.WriteLine($"    - {label.Name}: {label.Value}");
                }
            }
        }
    }

    /// <summary>
    /// カンマ区切り入力を文字列一覧に変換する
    /// </summary>
    /// <param name="prompt">入力を促すメッセージ</param>
    /// <returns>入力された文字列一覧</returns>
    private static List<string> ReadCommaSeparatedValues(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<string> { "<YOUR_ID>" };
        }

        return input
            .Split(',')
            .Select(value => value.Trim())
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToList();
    }

    /// <summary>
    /// 必須入力値を受け取る
    /// </summary>
    /// <param name="prompt">入力を促すメッセージ</param>
    /// <returns>入力値。未入力の場合は <see langword="null"/></returns>
    private static string? ReadRequiredValue(string prompt)
    {
        Console.Write(prompt);
        var value = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value))
        {
            Console.WriteLine("値が入力されていません。");
            return null;
        }

        return value;
    }

    /// <summary>
    /// 通知ステータスコードを表示用文字列に変換する
    /// </summary>
    /// <param name="status">通知ステータスコード</param>
    /// <returns>表示用の通知ステータス文字列</returns>
    private static string FormatNotificationStatus(int? status)
    {
        switch (status)
        {
            case 0:
                return "未送信";
            case 1:
                return "送信中";
            case 2:
                return "送信済";
            case 3:
                return "スキップ";
            case 9:
                return "送信失敗";
            default:
                return status?.ToString() ?? "不明";
        }
    }
}
