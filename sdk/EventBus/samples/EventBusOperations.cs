using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Samples;

/// <summary>
/// EventBusの各種操作を実装するクラス
/// </summary>
public class EventBusOperations
{
    private readonly EventBusClient _client;

    public EventBusOperations(EventBusClient client)
    {
        _client = client;
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. 実行設定・スケジュール・トリガーの一覧取得");
        Console.WriteLine("2. 実行設定・スケジュール・トリガーの作成");
        Console.WriteLine("3. 実行設定・スケジュール・トリガーの取得");
        Console.WriteLine("4. 実行設定・スケジュール・トリガーの更新");
        Console.WriteLine("5. 実行設定・スケジュール・トリガーの削除");
        Console.WriteLine("6. 実行設定のSecret設定");
        Console.Write("選択 (1 ~ 6 から入力): ");
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
                await ListResourcesAsync();
                break;
            case "2":
                await CreateResourceAsync();
                break;
            case "3":
                await GetResourceDetailsAsync();
                break;
            case "4":
                await UpdateResourceAsync();
                break;
            case "5":
                await DeleteResourceAsync();
                break;
            case "6":
                await ConfigProcessConfigurationSecretAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }
    }

    private async Task ListResourcesAsync()
    {
        Console.WriteLine("\n一覧取得するリソースのProvider.Classを選択してください: ");
        Console.WriteLine("1. 実行設定（eventbusprocessconfiguration）");
        Console.WriteLine("2. スケジュール（eventbusschedule）");
        Console.WriteLine("3. トリガー（eventbustrigger）");
        Console.Write("選択 (1 ~ 3 から入力): ");
        var choice = Console.ReadLine();

        Provider_Class providerClass;
        switch (choice)
        {
            case "1":
                providerClass = Provider_Class.Eventbusprocessconfiguration;
                break;
            case "2":
                providerClass = Provider_Class.Eventbusschedule;
                break;
            case "3":
                providerClass = Provider_Class.Eventbustrigger;
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                return;
        }

        var resources = await _client.Commonserviceitem.GetAsync(config =>
        {
            config.QueryParameters.ProviderClass = providerClass;
        });
        Console.WriteLine("リソース一覧:");
        foreach (var resource in resources.CommonServiceItems)
        {
            Console.WriteLine($"- {resource.ID}: {resource.Name}");
        }
    }

    private async Task CreateResourceAsync()
    {
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "sample-resource-name",
                Description = "SDKサンプルコードから作成",
                Settings = new SettingsRequest
                {
                    ProcessConfigurationSettings = new ProcessConfigurationSettings
                    {
                        Destination = ProcessConfigurationSettings_Destination.Simplemq,
                        Parameters = "{\"queue_name\": \"sample-queue\", \"content\": \"sampleContent\"}"
                    }
                },
                Provider = new Provider
                {
                    Class = Provider_Class.Eventbusprocessconfiguration
                }
            }
        };
        var result = await _client.Commonserviceitem.PostAsync(body);
        Console.WriteLine("作成されたリソース:");
        Console.WriteLine($"- {result.CommonServiceItem.ID}: {result.CommonServiceItem.Name}");
    }

    private async Task GetResourceDetailsAsync()
    {
        Console.Write("詳細を取得するリソースIDを入力してください: ");
        var id = Console.ReadLine();
        var result = await _client.Commonserviceitem[id].GetAsync();
        Console.WriteLine("リソース詳細:");
        Console.WriteLine($"ID: {result.CommonServiceItem.ID}");
        Console.WriteLine($"Name: {result.CommonServiceItem.Name}");
        Console.WriteLine($"Description: {result.CommonServiceItem.Description}");
    }

    private async Task UpdateResourceAsync()
    {
        Console.Write("更新するリソースIDを入力してください: ");
        var id = Console.ReadLine();
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "sample-resource-name-updated"
            }
        };
        var result = await _client.Commonserviceitem[id].PutAsync(body);
        Console.WriteLine("更新されたリソース:");
        Console.WriteLine($"ID: {result.CommonServiceItem.ID}");
        Console.WriteLine($"Name: {result.CommonServiceItem.Name}");
        Console.WriteLine($"Description: {result.CommonServiceItem.Description}");
    }

    private async Task DeleteResourceAsync()
    {
        Console.Write("削除するリソースIDを入力してください: ");
        var id = Console.ReadLine();
        var result = await _client.Commonserviceitem[id].DeleteAsync();
        Console.WriteLine("削除されたリソース:");
        Console.WriteLine($"ID: {result.CommonServiceItem.ID}");
        Console.WriteLine($"Name: {result.CommonServiceItem.Name}");
    }

    private async Task ConfigProcessConfigurationSecretAsync()
    {
        Console.Write("シークレットを設定する実行設定のリソースIDを入力してください: ");
        var id = Console.ReadLine();
        var body = new SetSecretRequest
        {
            Secret = new SetSecretRequest.SetSecretRequest_Secret
            {
                SimpleMQSecret = new SimpleMQSecret
                {
                    APIKey = "<YOUR_SIMPLE_MQ_API_KEY>"
                }
            }
        };
        var result = await _client.Commonserviceitem[id].Eventbus.Processconfiguration.SetSecret.PutAsync(body);
        Console.WriteLine($"実行結果: {result.Process.Result}");
    }
}
