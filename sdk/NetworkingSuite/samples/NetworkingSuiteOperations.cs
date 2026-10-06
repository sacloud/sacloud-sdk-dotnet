using System.ComponentModel.DataAnnotations;
using Sacloud.Sdk.NetworkingSuite.Samples.Samples;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.NetworkingSuite.Samples;

public sealed class NetworkingSuiteOperations
{
    private readonly Dictionary<string, OperationDetail> operations;

    public NetworkingSuiteOperations(NetworkingSuiteClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        SubnetGroupsSample subnetGroupsSample = new(client);
        SubnetsSample subnetsSample = new(client);
        AddressesSample addressesSample = new(client);
        InterfaceConnectionsSample interfaceConnectionsSample = new(client);

        operations = new Dictionary<string, OperationDetail>
        {
            { "1-1", new("サブネットグループ一覧の取得", subnetGroupsSample.ListSubnetGroupsAsync) },
            { "1-2", new("サブネットグループの取得", subnetGroupsSample.GetSubnetGroupAsync) },
            { "1-3", new("サブネットグループの作成", subnetGroupsSample.CreateSubnetGroupAsync) },
            { "1-4", new("サブネットグループの更新", subnetGroupsSample.UpdateSubnetGroupAsync) },
            { "1-5", new("サブネットグループの削除", subnetGroupsSample.DeleteSubnetGroupAsync) },
            { "2-1", new("サブネット一覧の取得", subnetsSample.ListSubnetsAsync) },
            { "2-2", new("サブネットの取得", subnetsSample.GetSubnetAsync) },
            { "2-3", new("サブネットの作成", subnetsSample.CreateSubnetAsync) },
            { "2-4", new("サブネットの更新", subnetsSample.UpdateSubnetAsync) },
            { "2-5", new("サブネットの削除", subnetsSample.DeleteSubnetAsync) },
            { "3-1", new("アドレス一覧の取得", addressesSample.ListAddressesAsync) },
            { "3-2", new("アドレスの取得", addressesSample.GetAddressAsync) },
            { "4-1", new("NIC のサブネット接続", interfaceConnectionsSample.CreateInterfaceConnectionAsync) },
            { "4-2", new("NIC のサブネット切断", interfaceConnectionsSample.DeleteInterfaceConnectionAsync) },
        };
    }

    public async Task RunInteractiveMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\n実行する操作を選択してください:");
            foreach (KeyValuePair<string, OperationDetail> operation in operations)
            {
                Console.WriteLine($"{operation.Key}. {operation.Value.Description}");
            }

            Console.WriteLine("0. 終了");
            Console.WriteLine("-------------------------");
            Console.Write("選択 (例: 1-1, 2-1, 0): ");

            string? choice = Console.ReadLine()?.Trim();
            if (choice == "0")
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(choice))
            {
                Console.WriteLine("番号が未入力です。再入力してください。");
                continue;
            }
            if (!operations.TryGetValue(choice, out OperationDetail? operationDetail))
            {
                Console.WriteLine("無効な番号が選択されました。");
                continue;
            }

            try
            {
                await operationDetail.ExecuteAsync();
            }
            catch (ValidationException)
            {
                Console.WriteLine();
                Console.Error.WriteLine($"{operationDetail.Description}: バリデーションエラーが発生しました。");
            }
            catch (ApiError ex)
            {
                Console.WriteLine();
                Console.Error.WriteLine($"{operationDetail.Description}: APIエラーが発生しました。");
                Console.Error.WriteLine($"  - ステータスコード: {ex.ResponseStatusCode}");

                if (ex.ResponseBody is null || !ex.ResponseBody.Any())
                {
                    Console.Error.WriteLine("  - エラーレスポンス本文: なし");
                    continue;
                }

                Console.Error.WriteLine("  - エラーレスポンス本文:");
                foreach (KeyValuePair<string, object> entry in ex.ResponseBody)
                {
                    Console.Error.WriteLine($"    - {entry.Key}: {entry.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.Error.WriteLine($"{operationDetail.Description}: エラーが発生しました。");
                Console.Error.WriteLine($"  - {ex.Message}");
            }
        }
    }

    private sealed record OperationDetail(string Description, Func<Task> ExecuteAsync);
}
