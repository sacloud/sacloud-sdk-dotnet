using System.Globalization;
using Microsoft.Kiota.Abstractions.Serialization;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.ObjectStorage.Samples.Samples;

namespace Sacloud.Sdk.ObjectStorage.Samples;

/// <summary>
/// ObjectStorage の各種操作を実装するクラス
/// </summary>
public class ObjectStorageOperations
{
    private readonly ObjectStorageClient _client;
    private readonly ClusterSample _clusterSample;
    private readonly BucketSample _bucketSample;
    private readonly AccountSample _accountSample;
    private readonly PermissionsSample _permissionsSample;
    private readonly StatusSample _statusSample;
    private readonly PlansSample _plansSample;
    private readonly QuotaSample _quotaSample;
    private readonly MeteringBucketSample _meteringBucketSample;

    private readonly Dictionary<string, OperationDetail> _operations;

    public ObjectStorageOperations(ObjectStorageClient client)
    {
        _client = client;

        _clusterSample = new ClusterSample(_client);
        _bucketSample = new BucketSample(_client);
        _accountSample = new AccountSample(_client);
        _permissionsSample = new PermissionsSample(_client);
        _statusSample = new StatusSample(_client);
        _plansSample = new PlansSample(_client);
        _quotaSample = new QuotaSample(_client);
        _meteringBucketSample = new MeteringBucketSample(_client);

        _operations = new Dictionary<string, OperationDetail>
        {
            { "1-1", new OperationDetail() { Description = "サイト一覧の取得", Operation = _clusterSample.GetClusterAsync } },
            { "1-2", new OperationDetail () { Description = "サイトの取得", Operation = _clusterSample.GetClusterItemAsync } },
            { "2-1", new OperationDetail { Description = "バケット一覧の取得", Operation = _bucketSample.GetBucketsAsync } },
            { "2-2", new OperationDetail { Description = "バケットの作成", Operation = _bucketSample.PutBucketAsync } },
            { "2-3", new OperationDetail { Description = "バケットの削除", Operation = _bucketSample.DeleteBucketAsync } },
            { "2-4", new OperationDetail { Description = "バケットのレプリケーション設定の取得", Operation = _bucketSample.GetReplicationAsync } },
            { "2-5", new OperationDetail { Description = "バケットのレプリケーション設定の作成", Operation = _bucketSample.PostReplicationAsync } },
            { "2-6", new OperationDetail { Description = "レプリケーション設定の削除", Operation = _bucketSample.DeleteReplicationAsync } },
            { "2-7", new OperationDetail { Description = "バケットのレプリケーション設定可能なディスティネーションバケットを取得", Operation = _bucketSample.GetBucketReplicableTargetsAsync } },
            { "2-8", new OperationDetail { Description = "バケット暗号化設定の取得", Operation = _bucketSample.GetBucketEncryptionAsync } },
            { "2-9", new OperationDetail { Description = "バケット暗号化設定の作成（KMSキー指定）", Operation = _bucketSample.PutBucketEncryptionAsync } },
            { "2-10", new OperationDetail { Description = "バケット暗号化設定の削除（無効化）", Operation = _bucketSample.DeleteBucketEncryptionAsync } },
            { "2-11", new OperationDetail { Description = "現在のプランを取得", Operation = _bucketSample.GetBucketPlanAsync } },
            { "2-12", new OperationDetail { Description = "プラン変更を実行", Operation = _bucketSample.PutBucketPlanAsync } },
            { "2-13", new OperationDetail { Description = "バケットのペナルティ状況を取得", Operation = _bucketSample.GetBucketPenaltyAsync } },
            { "2-14", new OperationDetail { Description = "バケットの使用量を取得", Operation = _bucketSample.GetBucketUsageAsync } },
            { "2-15", new OperationDetail { Description = "バケットの制限値を取得", Operation = _bucketSample.GetBucketQuotaAsync } },
            { "3-1", new OperationDetail { Description = "サイトアカウントの取得", Operation = _accountSample.GetAccountAsync } },
            { "3-2", new OperationDetail { Description = "サイトアカウントの作成", Operation = _accountSample.PostAccountAsync } },
            { "3-3", new OperationDetail { Description = "サイトアカウントの削除", Operation = _accountSample.DeleteAccountAsync } },
            { "3-4", new OperationDetail { Description = "サイトアカウントのアクセスキー一覧の取得", Operation = _accountSample.GetAccountKeysAsync } },
            { "3-5", new OperationDetail { Description = "サイトアカウントのアクセスキーの発行", Operation = _accountSample.PostAccountKeyAsync } },
            { "3-6", new OperationDetail { Description = "サイトアカウントのアクセスキーの取得", Operation = _accountSample.GetAccountKeyAsync } },
            { "3-7", new OperationDetail { Description = "サイトアカウントのアクセスキーの削除", Operation = _accountSample.DeleteAccountKeyAsync } },
            { "4-1", new OperationDetail { Description = "パーミッション一覧の取得", Operation = _permissionsSample.GetPermissionsAsync } },
            { "4-2", new OperationDetail { Description = "パーミッションの作成", Operation = _permissionsSample.CreatePermissionAsync } },
            { "4-3", new OperationDetail { Description = "パーミッションの取得", Operation = _permissionsSample.GetPermissionAsync } },
            { "4-4", new OperationDetail { Description = "パーミッションの更新", Operation = _permissionsSample.UpdatePermissionAsync } },
            { "4-5", new OperationDetail { Description = "パーミッションの削除", Operation = _permissionsSample.DeletePermissionAsync } },
            { "4-6", new OperationDetail { Description = "パーミッションが保有するアクセスキー一覧の取得", Operation = _permissionsSample.GetPermissionKeysAsync } },
            { "4-7", new OperationDetail { Description = "パーミッションのアクセスキーの発行", Operation = _permissionsSample.CreatePermissionKeyAsync } },
            { "4-8", new OperationDetail { Description = "パーミッションが保有するアクセスキーの取得", Operation = _permissionsSample.GetPermissionKeyAsync } },
            { "4-9", new OperationDetail { Description = "パーミッションが保有するアクセスキーの削除", Operation = _permissionsSample.DeletePermissionKeyAsync } },
            { "5-1", new OperationDetail { Description = "サイトのステータスの取得", Operation = _statusSample.GetStatusAsync } },
            { "6-1", new OperationDetail { Description = "利用可能なプランの一覧取得", Operation = _plansSample.GetPlansAsync } },
            { "7-1", new OperationDetail { Description = "全体制限値の取得", Operation = _quotaSample.GetQuotaAsync } },
            { "8-1", new OperationDetail { Description = "バケット使用量の取得", Operation = _meteringBucketSample.GetBucketMeteringBucketAsync } },
        };
    }

    private class OperationDetail
    {
        public string Description { get; set; } = string.Empty;
        public Func<Task> Operation { get; set; } = () => Task.CompletedTask;
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\n実行するサンプルを選択してください:");
            foreach (var operation in _operations)
            {
                Console.WriteLine($"{operation.Key}. {operation.Value.Description}");
            }

            Console.WriteLine("0.    終了");
            Console.WriteLine("-------------------------");
            Console.Write("選択 (例: 1-1, 4-2, 0): ");

            var choice = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(choice))
            {
                Console.WriteLine("番号が未入力です。再入力してください。");
                continue;
            }

            if (choice == "0")
            {
                break;
            }

            if (!_operations.TryGetValue(choice, out var operationDetail))
            {
                Console.WriteLine("無効な番号が選択されました。");
                continue;
            }

            try
            {
                await operationDetail.Operation();
            }

            catch (ApiError ex)
            {
                Console.Error.WriteLine($"{operationDetail.Description}: APIエラーが発生しました。");
                Console.Error.WriteLine($"  - ステータスコード: {ex.ResponseStatusCode}");
                if (ex.ResponseBody is null || !ex.ResponseBody.Any())
                {
                    Console.Error.WriteLine("  - エラーレスポンス本文: なし");
                }
                else
                {
                    Console.Error.WriteLine("  - エラーレスポンス本文:");
                    foreach (var entry in ex.ResponseBody)
                    {
                        PrintApiErrorValue(entry.Key, entry.Value, "    ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{operationDetail.Description}: {ex.Message}");
            }
        }
    }

    private static void PrintApiErrorValue(string key, object? value, string indent)
    {
        switch (value)
        {
            case UntypedObject untypedObject:
                Console.Error.WriteLine($"{indent}- {key}:");
                var objectValue = untypedObject.GetValue();
                if (objectValue is null || !objectValue.Any())
                {
                    Console.Error.WriteLine($"{indent}  (空)");
                    return;
                }

                foreach (var entry in objectValue)
                {
                    PrintApiErrorValue(entry.Key, entry.Value, $"{indent}  ");
                }

                return;

            case IDictionary<string, object> dictionary:
                Console.Error.WriteLine($"{indent}- {key}:");
                if (!dictionary.Any())
                {
                    Console.Error.WriteLine($"{indent}  (空)");
                    return;
                }

                foreach (var entry in dictionary)
                {
                    PrintApiErrorValue(entry.Key, entry.Value, $"{indent}  ");
                }

                return;

            case UntypedArray untypedArray:
                Console.Error.WriteLine($"{indent}- {key}:");
                var arrayValue = untypedArray.GetValue();
                if (arrayValue is null || !arrayValue.Any())
                {
                    Console.Error.WriteLine($"{indent}  (空)");
                    return;
                }

                var index = 0;
                foreach (var item in arrayValue)
                {
                    PrintApiErrorValue($"[{index}]", item, $"{indent}  ");
                    index++;
                }

                return;

            case IEnumerable<object> collection:
                Console.Error.WriteLine($"{indent}- {key}:");
                var collectionIndex = 0;
                foreach (var item in collection)
                {
                    PrintApiErrorValue($"[{collectionIndex}]", item, $"{indent}  ");
                    collectionIndex++;
                }

                return;

            default:
                var formattedValue = value switch
                {
                    null => "null",
                    UntypedString untypedString => untypedString.GetValue() ?? "null",
                    UntypedInteger untypedInteger => untypedInteger.GetValue().ToString(CultureInfo.InvariantCulture),
                    UntypedLong untypedLong => untypedLong.GetValue().ToString(CultureInfo.InvariantCulture),
                    UntypedDouble untypedDouble => untypedDouble.GetValue().ToString(CultureInfo.InvariantCulture),
                    UntypedFloat untypedFloat => untypedFloat.GetValue().ToString(CultureInfo.InvariantCulture),
                    UntypedBoolean untypedBoolean => untypedBoolean.GetValue().ToString(CultureInfo.InvariantCulture),
                    UntypedNull => "null",
                    _ => value.ToString() ?? "null"
                };

                Console.Error.WriteLine($"{indent}- {key}: {formattedValue}");
                return;
        }
    }
}
