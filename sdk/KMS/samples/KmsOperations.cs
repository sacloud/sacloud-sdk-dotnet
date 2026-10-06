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
        var response = await _client.Kms.Keys.GetAsync();
        Console.WriteLine("キー一覧:");
        if (response?.Keys is null || response.Keys.Count == 0)
        {
            Console.WriteLine("キーが見つかりませんでした。");
            return;
        }

        foreach (var key in response.Keys)
        {
            Console.WriteLine($"- {key.ID}: {key.Name} ({key.Status})");
        }
    }

    private async Task CreateKeyAsync()
    {
        var body = new WrappedCreateKey
        {
            Key = new CreateKey
            {
                Name = "<YOUR_KEY_NAME>",
                Description = "KMS sample key",
                Tags = new List<string> { "sample", "kms" }
            }
        };

        var response = await _client.Kms.Keys.PostAsync(body);
        Console.WriteLine("作成したキー:");
        Console.WriteLine($"ID: {response?.Key?.ID}");
        Console.WriteLine($"Name: {response?.Key?.Name}");
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

        current.Key.Description = $"Updated by sample at {DateTimeOffset.UtcNow:O}";
        current.Key.Tags = new List<string> { "sample", "kms", "updated" };

        var updated = await _client.Kms.Keys[keyId].PutAsync(current);
        Console.WriteLine("更新したキー:");
        Console.WriteLine($"ID: {updated?.Key?.ID}");
        Console.WriteLine($"Description: {updated?.Key?.Description}");
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
        var plainText = "Hello KMS";
        var plainBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

        var body = new WrappedKeyPlain
        {
            Key = new KeyPlain
            {
                Algo = KeyEncryptAlgoEnum.Aes256Gcm,
                Plain = plainBase64
            }
        };

        var encrypted = await _client.Kms.Keys[keyId].Encrypt.PostAsync(body);
        Console.WriteLine("暗号化結果(Base64):");
        Console.WriteLine(encrypted?.Key?.Cipher);
    }

    private async Task DecryptWithKeyAsync()
    {
        var keyId = ReadRequiredInput("復号に利用するキーIDを入力してください");
        var cipherText = ReadRequiredInput("復号する暗号文(Base64)を入力してください");

        var body = new WrappedKeyCipher
        {
            Key = new KeyCipher
            {
                Cipher = cipherText
            }
        };

        var decrypted = await _client.Kms.Keys[keyId].Decrypt.PostAsync(body);
        var decoded = decrypted?.Key?.Plain is null
            ? ""
            : Encoding.UTF8.GetString(Convert.FromBase64String(decrypted.Key.Plain));

        Console.WriteLine("復号結果:");
        Console.WriteLine(decoded);
    }

    private async Task RotateKeyMaterialAsync()
    {
        var keyId = ReadRequiredInput("キーマテリアルを更新するキーIDを入力してください");
        var response = await _client.Kms.Keys[keyId].Rotate.PostAsync();
        Console.WriteLine("キーマテリアルを更新しました。");
        Console.WriteLine($"LatestVersion: {response?.Key?.LatestVersion}");
    }

    private async Task ChangeKeyStatusAsync()
    {
        var keyId = ReadRequiredInput("ステータスを変更するキーIDを入力してください");
        Console.WriteLine("変更後ステータスを選択してください: 1.Active 2.Restricted 3.Suspended");
        Console.WriteLine("選択 (1 ~ 3 から入力): ");
        var statusChoice = Console.ReadLine();

        var nextStatus = statusChoice switch
        {
            "1" => ChangeKeyStatus_Status.Active,
            "2" => ChangeKeyStatus_Status.Restricted,
            "3" => ChangeKeyStatus_Status.Suspended,
            _ => throw new InvalidOperationException("無効なステータスが選択されました。")
        };

        var body = new WrappedChangeKeyStatus
        {
            Key = new ChangeKeyStatus
            {
                Status = nextStatus
            }
        };

        await _client.Kms.Keys[keyId].Status.PostAsync(body);
        Console.WriteLine("キーステータスを更新しました。");
    }

    private async Task ScheduleKeyDestructionAsync()
    {
        var keyId = ReadRequiredInput("削除予約するキーIDを入力してください");
        Console.WriteLine("猶予日数を入力してください (7 ~ 14): ");
        var pendingDaysText = Console.ReadLine();
        if (!int.TryParse(pendingDaysText, out var pendingDays) || pendingDays < 7 || pendingDays > 14)
        {
            throw new InvalidOperationException("猶予日数は7から14で指定してください。");
        }

        var body = new WrappedScheduleDestructionKey
        {
            Key = new ScheduleDestructionKey
            {
                PendingDays = pendingDays
            }
        };

        await _client.Kms.Keys[keyId].ScheduleDestruction.PostAsync(body);
        Console.WriteLine("キーの削除予約を登録しました。");
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
