using System.Text;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Samples;

/// <summary>
/// KMSの各種操作を実装するクラス
/// </summary>
public class KmsOperations
{
    private readonly KMSClient _client;

    public KmsOperations(KMSClient client)
    {
        _client = client;
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. キー一覧を取得");
        Console.WriteLine("2. キーを作成");
        Console.WriteLine("3. キー詳細を取得");
        Console.WriteLine("4. キー情報を更新");
        Console.WriteLine("5. キーを削除");
        Console.WriteLine("6. キーで暗号化");
        Console.WriteLine("7. キーで復号");
        Console.WriteLine("8. キーマテリアルを更新");
        Console.WriteLine("9. キーステータスを変更");
        Console.WriteLine("10. キー削除を予約");
        Console.WriteLine("選択 (1 ~ 10 から入力): ");
        var choice = Console.ReadLine();

        await ExecuteOperationAsync(choice);
    }

    private async Task ExecuteOperationAsync(string? choice)
    {
        switch (choice)
        {
            case "1":
                await ListKeysAsync();
                break;
            case "2":
                await CreateKeyAsync();
                break;
            case "3":
                await GetKeyDetailsAsync();
                break;
            case "4":
                await UpdateKeyAsync();
                break;
            case "5":
                await DeleteKeyAsync();
                break;
            case "6":
                await EncryptWithKeyAsync();
                break;
            case "7":
                await DecryptWithKeyAsync();
                break;
            case "8":
                await RotateKeyMaterialAsync();
                break;
            case "9":
                await ChangeKeyStatusAsync();
                break;
            case "10":
                await ScheduleKeyDestructionAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }
    }

    private async Task ListKeysAsync()
    {
        var count = ReadOptionalInteger("1ページあたりの件数 (未入力でAPI既定値)");
        var from = ReadOptionalInteger("取得開始位置 (未入力でAPI既定値)");
        var response = await _client.Kms.Keys.GetAsync(configuration =>
        {
            configuration.QueryParameters.Count = count;
            configuration.QueryParameters.From = from;
        });

        Console.WriteLine("キー一覧:");
        Console.WriteLine($"IsOk: {response?.IsOk?.ToString() ?? "(未返却)"}");
        Console.WriteLine($"Count: {response?.Count?.ToString() ?? "(未返却)"}");
        Console.WriteLine($"From: {response?.From?.ToString() ?? "(未返却)"}");
        Console.WriteLine($"Total: {response?.Total?.ToString() ?? "(未返却)"}");
        if (response?.Keys is null || response.Keys.Count == 0)
        {
            Console.WriteLine("キーが見つかりませんでした。");
            return;
        }

        foreach (var key in response.Keys)
        {
            Console.WriteLine($"- {key.ID ?? "(ID未返却)"}: {key.Name ?? "(名前未設定)"} ({key.Status?.ToString() ?? "状態未返却"})");
        }
    }

    private async Task CreateKeyAsync()
    {
        var body = new WrappedCreateKeyRequest
        {
            Key = new CreateKeyRequest
            {
                Name = ReadRequiredInput("作成するキー名を入力してください"),
                Description = "KMS sample key",
                Tags = new List<string> { "sample", "kms" }
            }
        };

        var response = await _client.Kms.Keys.PostAsync(body);
        DisplayKey("作成したキー:", response?.Key);
    }

    private async Task GetKeyDetailsAsync()
    {
        var keyId = ReadRequiredInput("詳細を取得するキーIDを入力してください");
        var response = await _client.Kms.Keys[keyId].GetAsync();
        Console.WriteLine("キー詳細:");
        Console.WriteLine($"ID: {response?.Key?.ID}");
        Console.WriteLine($"Name: {response?.Key?.Name}");
        Console.WriteLine($"Description: {response?.Key?.Description}");
        Console.WriteLine($"LatestVersion: {response?.Key?.LatestVersion}");
        Console.WriteLine($"Status: {response?.Key?.Status}");
    }

    private async Task UpdateKeyAsync()
    {
        var keyId = ReadRequiredInput("更新するキーIDを入力してください");
        var current = await _client.Kms.Keys[keyId].GetAsync();
        if (current?.Key is null)
        {
            Console.WriteLine("更新対象のキーが見つかりませんでした。");
            return;
        }

        var body = new WrappedKeyRequest
        {
            Key = new KeyRequest
            {
                Name = current.Key.Name ?? throw new InvalidOperationException("更新対象のキー名が応答にありません。"),
                Description = $"Updated by sample at {DateTimeOffset.UtcNow:O}",
                Tags = new List<string> { "sample", "kms", "updated" }
            }
        };

        var updated = await _client.Kms.Keys[keyId].PutAsync(body);
        DisplayKey("更新したキー:", updated?.Key);
    }

    private async Task DeleteKeyAsync()
    {
        var keyId = ReadRequiredInput("削除するキーIDを入力してください");
        await _client.Kms.Keys[keyId].DeleteAsync();
        Console.WriteLine("キーを削除しました。");
    }

    private async Task EncryptWithKeyAsync()
    {
        var keyId = ReadRequiredInput("暗号化に利用するキーIDを入力してください");
        var plainText = ReadRequiredInput("暗号化する平文を入力してください");
        var plainBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

        var body = new WrappedEncryptionRequest
        {
            Key = new EncryptionRequest
            {
                Algo = EncryptionRequest_Algo.Aes256Gcm,
                Plain = plainBase64
            }
        };

        var encrypted = await _client.Kms.Keys[keyId].Encrypt.PostAsync(body);
        var cipherText = encrypted?.Key?.Cipher;
        if (string.IsNullOrWhiteSpace(cipherText))
        {
            throw new InvalidOperationException("暗号化応答に暗号文がありません。");
        }

        try
        {
            _ = Convert.FromBase64String(cipherText);
        }
        catch (FormatException exception)
        {
            throw new InvalidOperationException("暗号化応答の暗号文がBase64形式ではありません。", exception);
        }

        Console.WriteLine("暗号化結果(Base64):");
        Console.WriteLine(cipherText);
    }

    private async Task DecryptWithKeyAsync()
    {
        var keyId = ReadRequiredInput("復号に利用するキーIDを入力してください");
        var cipherText = ReadRequiredInput("復号する暗号文(Base64)を入力してください");

        var body = new WrappedDecryptionRequest
        {
            Key = new DecryptionRequest
            {
                Cipher = cipherText
            }
        };

        var decrypted = await _client.Kms.Keys[keyId].Decrypt.PostAsync(body);
        var plainBase64 = decrypted?.Key?.Plain;
        if (plainBase64 is null)
        {
            throw new InvalidOperationException("復号応答に平文がありません。");
        }

        string decoded;
        try
        {
            decoded = Encoding.UTF8.GetString(Convert.FromBase64String(plainBase64));
        }
        catch (FormatException exception)
        {
            throw new InvalidOperationException("復号応答の平文がBase64形式ではありません。", exception);
        }

        Console.WriteLine("復号結果:");
        Console.WriteLine(decoded);
    }

    private async Task RotateKeyMaterialAsync()
    {
        var keyId = ReadRequiredInput("キーマテリアルを更新するキーIDを入力してください");
        var response = await _client.Kms.Keys[keyId].Rotate.PostAsync();
        DisplayKey("rotate応答:", response?.Key);
        Console.WriteLine("キーマテリアルを更新しました。");
    }

    private async Task ChangeKeyStatusAsync()
    {
        var keyId = ReadRequiredInput("ステータスを変更するキーIDを入力してください");
        Console.WriteLine("変更後ステータスを選択してください: 1.Active 2.Restricted 3.Suspended");
        Console.WriteLine("選択 (1 ~ 3 から入力): ");
        var statusChoice = Console.ReadLine();

        var nextStatus = statusChoice switch
        {
            "1" => ChangeKeyStateRequest_Status.Active,
            "2" => ChangeKeyStateRequest_Status.Restricted,
            "3" => ChangeKeyStateRequest_Status.Suspended,
            _ => throw new InvalidOperationException("無効なステータスが選択されました。")
        };

        var body = new WrappedChangeKeyStateRequest
        {
            Key = new ChangeKeyStateRequest
            {
                Status = nextStatus
            }
        };

        var response = await _client.Kms.Keys[keyId].Status.PostAsync(body);
        var updatedStatus = response?.Key?.Status
            ?? throw new InvalidOperationException("ステータス変更応答に状態がありません。");

        Console.WriteLine("キーステータスを更新しました。");
        Console.WriteLine($"Status: {updatedStatus}");
    }

    private async Task ScheduleKeyDestructionAsync()
    {
        var keyId = ReadRequiredInput("削除予約するキーIDを入力してください");
        Console.WriteLine("猶予日数を入力してください (7 ~ 90): ");
        var pendingDaysText = Console.ReadLine();
        if (!int.TryParse(pendingDaysText, out var pendingDays) || pendingDays < 7 || pendingDays > 90)
        {
            throw new InvalidOperationException("猶予日数は7から90で指定してください。");
        }

        var body = new WrappedScheduleDestructionKeyRequest
        {
            Key = new ScheduleDestructionKeyRequest
            {
                PendingDays = pendingDays
            }
        };

        var response = await _client.Kms.Keys[keyId].ScheduleDestruction.PostAsync(body);
        var scheduledKey = response?.Key
            ?? throw new InvalidOperationException("削除予約応答にキー情報がありません。");
        var scheduledStatus = scheduledKey.Status
            ?? throw new InvalidOperationException("削除予約応答に状態がありません。");
        var deletionScheduledAfter = scheduledKey.DeletionScheduledAfter
            ?? throw new InvalidOperationException("削除予約応答に削除予定日時がありません。");

        Console.WriteLine("キーの削除予約を登録しました。");
        Console.WriteLine($"Status: {scheduledStatus}");
        Console.WriteLine($"DeletionScheduledAfter: {deletionScheduledAfter:O}");
    }

    private static void DisplayKey(string title, Key? key)
    {
        if (key is null)
        {
            throw new InvalidOperationException($"{title}応答にキー情報がありません。");
        }

        DisplayKeyDetails(
            title,
            key.ID,
            key.Name,
            key.Description,
            key.Tags,
            key.KeyOrigin?.ToString(),
            key.ServiceClass?.ToString(),
            key.Status?.ToString(),
            key.LatestVersion,
            key.CreatedAt,
            key.ModifiedAt,
            key.DeletionScheduledAfter);
    }

    private static void DisplayKey(string title, CreateKeyResponse? key)
    {
        if (key is null)
        {
            throw new InvalidOperationException($"{title}応答にキー情報がありません。");
        }

        DisplayKeyDetails(
            title,
            key.ID,
            key.Name,
            key.Description,
            key.Tags,
            key.KeyOrigin?.ToString(),
            key.ServiceClass?.ToString(),
            key.Status?.ToString(),
            key.LatestVersion,
            key.CreatedAt,
            key.ModifiedAt,
            key.DeletionScheduledAfter);
    }

    private static void DisplayKeyDetails(
        string title,
        string? id,
        string? name,
        string? description,
        List<string>? tags,
        string? keyOrigin,
        string? serviceClass,
        string? status,
        int? latestVersion,
        DateTimeOffset? createdAt,
        DateTimeOffset? modifiedAt,
        DateTimeOffset? deletionScheduledAfter)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new InvalidOperationException($"{title}応答にキーIDがありません。");
        }

        Console.WriteLine(title);
        Console.WriteLine($"ID: {id}");
        Console.WriteLine($"Name: {name ?? "(未設定)"}");
        Console.WriteLine($"Description: {description ?? "(未設定)"}");
        Console.WriteLine($"Tags: {(tags is null ? "(未設定)" : string.Join(", ", tags))}");
        Console.WriteLine($"KeyOrigin: {keyOrigin ?? "(未返却)"}");
        Console.WriteLine($"ServiceClass: {serviceClass ?? "(未返却)"}");
        Console.WriteLine($"Status: {status ?? "(未返却)"}");
        Console.WriteLine($"LatestVersion: {latestVersion?.ToString() ?? "(未返却)"}");
        Console.WriteLine($"CreatedAt: {createdAt?.ToString("O") ?? "(未返却)"}");
        Console.WriteLine($"ModifiedAt: {modifiedAt?.ToString("O") ?? "(未返却)"}");
        Console.WriteLine($"DeletionScheduledAfter: {deletionScheduledAfter?.ToString("O") ?? "(未設定)"}");
    }

    private static int? ReadOptionalInteger(string prompt)
    {
        Console.WriteLine($"{prompt}: ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        if (!int.TryParse(input, out var value))
        {
            throw new InvalidOperationException($"{prompt}は整数で指定してください。");
        }

        return value;
    }

    private static string ReadRequiredInput(string prompt)
    {
        Console.WriteLine($"{prompt}: ");
        var value = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{prompt}が未入力です。");
        }

        return value;
    }
}
