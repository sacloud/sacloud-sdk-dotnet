using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Samples;

/// <summary>
/// CloudHSMクライアントの操作を実装するクラス
/// </summary>
public class CloudHSMClientsOperations
{
    private readonly CloudHSMClient _client;

    public CloudHSMClientsOperations(CloudHSMClient client)
    {
        _client = client;
    }

    public async Task ListAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].Clients.GetAsync();
        Console.WriteLine("CloudHSMクライアント一覧:");
        if (response?.Clients is null || response.Clients.Count == 0)
        {
            Console.WriteLine("CloudHSMクライアントがありません。");
            return;
        }

        foreach (var client in response.Clients)
        {
            Console.WriteLine($"- {client.ID}: {client.Name}");
        }
    }

    public async Task CreateAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var body = new WrappedCreateCloudHSMClient
        {
            Client = new CreateCloudHSMClient
            {
                Name = ReadRequiredInput("CloudHSMクライアント名を入力してください"),
                Certificate = ReadRequiredInput("PEM形式の証明書を入力してください")
            }
        };

        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].Clients.PostAsync(body);
        Console.WriteLine("作成したCloudHSMクライアント:");
        Console.WriteLine($"- {response?.Client?.ID}: {response?.Client?.Name}");
    }

    public async Task GetDetailsAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var clientId = ReadRequiredInput("CloudHSMクライアント IDを入力してください");
        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].Clients[clientId].GetAsync();
        Console.WriteLine("CloudHSMクライアント詳細:");
        Console.WriteLine($"ID: {response?.Client?.ID}");
        Console.WriteLine($"Name: {response?.Client?.Name}");
    }

    public async Task UpdateAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var clientId = ReadRequiredInput("更新するCloudHSMクライアント IDを入力してください");
        var current = await _client.Cloudhsm.Cloudhsms[cloudHsmId].Clients[clientId].GetAsync();
        var client = current?.Client
            ?? throw new InvalidOperationException("更新対象のCloudHSMクライアントが見つかりませんでした。");

        client.Name = ReadRequiredInput("更新後のCloudHSMクライアント名を入力してください");
        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].Clients[clientId].PutAsync(new WrappedCloudHSMClient
        {
            Client = client
        });
        Console.WriteLine("更新したCloudHSMクライアント:");
        Console.WriteLine($"- {response?.Client?.ID}: {response?.Client?.Name}");
    }

    public async Task DeleteAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var clientId = ReadRequiredInput("削除するCloudHSMクライアント IDを入力してください");
        await _client.Cloudhsm.Cloudhsms[cloudHsmId].Clients[clientId].DeleteAsync();
        Console.WriteLine("CloudHSMクライアントを削除しました。");
    }

    private static string ReadRequiredInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        var value = Console.ReadLine();
        return value ?? string.Empty;
    }
}
