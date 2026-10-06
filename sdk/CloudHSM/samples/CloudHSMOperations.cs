using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Samples;

/// <summary>
/// CloudHSM本体の操作と対話メニューを実装するクラス
/// </summary>
public class CloudHSMOperations
{
    private readonly CloudHSMClient _client;
    private readonly CloudHSMClientsOperations _clientsOperations;
    private readonly CloudHSMPeersOperations _peersOperations;
    private readonly SoftwareLicensesOperations _softwareLicensesOperations;

    public CloudHSMOperations(CloudHSMClient client)
    {
        _client = client;
        _clientsOperations = new CloudHSMClientsOperations(client);
        _peersOperations = new CloudHSMPeersOperations(client);
        _softwareLicensesOperations = new SoftwareLicensesOperations(client);
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. CloudHSM一覧を取得");
        Console.WriteLine("2. CloudHSMを作成");
        Console.WriteLine("3. CloudHSM詳細を取得");
        Console.WriteLine("4. CloudHSMを更新");
        Console.WriteLine("5. CloudHSMを削除");
        Console.WriteLine("6. CloudHSMクライアント一覧を取得");
        Console.WriteLine("7. CloudHSMクライアントを作成");
        Console.WriteLine("8. CloudHSMクライアント詳細を取得");
        Console.WriteLine("9. CloudHSMクライアントを更新");
        Console.WriteLine("10. CloudHSMクライアントを削除");
        Console.WriteLine("11. CloudHSMピア一覧を取得");
        Console.WriteLine("12. CloudHSMピアを作成");
        Console.WriteLine("13. CloudHSMピアを削除");
        Console.WriteLine("14. ソフトウェアライセンス一覧を取得");
        Console.WriteLine("15. ソフトウェアライセンスを作成");
        Console.WriteLine("16. ソフトウェアライセンス詳細を取得");
        Console.WriteLine("17. ソフトウェアライセンスを更新");
        Console.WriteLine("18. ソフトウェアライセンスを削除");
        Console.Write("選択 (1 ~ 18 から入力): ");

        await ExecuteOperationAsync(Console.ReadLine());
    }

    private async Task ExecuteOperationAsync(string? choice)
    {
        switch (choice)
        {
            case "1": await ListAsync(); break;
            case "2": await CreateAsync(); break;
            case "3": await GetDetailsAsync(); break;
            case "4": await UpdateAsync(); break;
            case "5": await DeleteAsync(); break;
            case "6": await _clientsOperations.ListAsync(); break;
            case "7": await _clientsOperations.CreateAsync(); break;
            case "8": await _clientsOperations.GetDetailsAsync(); break;
            case "9": await _clientsOperations.UpdateAsync(); break;
            case "10": await _clientsOperations.DeleteAsync(); break;
            case "11": await _peersOperations.ListAsync(); break;
            case "12": await _peersOperations.CreateAsync(); break;
            case "13": await _peersOperations.DeleteAsync(); break;
            case "14": await _softwareLicensesOperations.ListAsync(); break;
            case "15": await _softwareLicensesOperations.CreateAsync(); break;
            case "16": await _softwareLicensesOperations.GetDetailsAsync(); break;
            case "17": await _softwareLicensesOperations.UpdateAsync(); break;
            case "18": await _softwareLicensesOperations.DeleteAsync(); break;
            default: Console.WriteLine("無効な番号が選択されました。"); break;
        }
    }

    public async Task ListAsync()
    {
        var response = await _client.Cloudhsm.Cloudhsms.GetAsync();
        Console.WriteLine("CloudHSM一覧:");
        if (response?.CloudHSMs is null || response.CloudHSMs.Count == 0)
        {
            Console.WriteLine("CloudHSMがありません。");
            return;
        }

        foreach (var cloudHsm in response.CloudHSMs)
        {
            Console.WriteLine($"- {cloudHsm.ID}: {cloudHsm.Name}");
        }
    }

    public async Task CreateAsync()
    {
        var body = new WrappedCreateCloudHSM
        {
            CloudHSM = new CreateCloudHSM
            {
                Name = ReadRequiredInput("CloudHSM名を入力してください"),
                Description = ReadOptionalInput("説明を入力してください"),
                IPv4NetworkAddress = ReadRequiredInput("IPv4ネットワークアドレスを入力してください"),
                IPv4PrefixLength = ReadRequiredIntInput("IPv4プレフィックス長を入力してください"),
                Tags = new List<string> { "sample" }
            }
        };

        var response = await _client.Cloudhsm.Cloudhsms.PostAsync(body);
        Console.WriteLine("作成したCloudHSM:");
        Console.WriteLine($"- {response?.CloudHSM?.ID}: {response?.CloudHSM?.Name}");
    }

    public async Task GetDetailsAsync()
    {
        var cloudHsmId = ReadRequiredInput("詳細を取得するCloudHSM IDを入力してください");
        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].GetAsync();
        Console.WriteLine("CloudHSM詳細:");
        Console.WriteLine($"ID: {response?.CloudHSM?.ID}");
        Console.WriteLine($"Name: {response?.CloudHSM?.Name}");
        Console.WriteLine($"Description: {response?.CloudHSM?.Description}");
        Console.WriteLine($"IPv4Address: {response?.CloudHSM?.IPv4Address}");
    }

    public async Task UpdateAsync()
    {
        var cloudHsmId = ReadRequiredInput("更新するCloudHSM IDを入力してください");
        var current = await _client.Cloudhsm.Cloudhsms[cloudHsmId].GetAsync();
        var cloudHsm = current?.CloudHSM
            ?? throw new InvalidOperationException("更新対象のCloudHSMが見つかりませんでした。");

        cloudHsm.Name = ReadRequiredInput("更新後のCloudHSM名を入力してください");
        cloudHsm.Description = ReadOptionalInput("更新後の説明を入力してください");
        cloudHsm.Tags ??= new List<string> { "sample" };

        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].PutAsync(new WrappedCloudHSM
        {
            CloudHSM = cloudHsm
        });
        Console.WriteLine("更新したCloudHSM:");
        Console.WriteLine($"- {response?.CloudHSM?.ID}: {response?.CloudHSM?.Name}");
    }

    public async Task DeleteAsync()
    {
        var cloudHsmId = ReadRequiredInput("削除するCloudHSM IDを入力してください");
        await _client.Cloudhsm.Cloudhsms[cloudHsmId].DeleteAsync();
        Console.WriteLine("CloudHSMを削除しました。");
    }

    private static string ReadRequiredInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        var value = Console.ReadLine();
        return value ?? string.Empty;
    }

    private static string? ReadOptionalInput(string prompt)
    {
        Console.Write($"{prompt}（省略可）: ");
        return Console.ReadLine();
    }

    private static int ReadRequiredIntInput(string prompt)
    {
        var value = ReadRequiredInput(prompt);
        if (!int.TryParse(value, out var result))
        {
            throw new InvalidOperationException($"{prompt}は整数で入力してください。");
        }

        return result;
    }
}