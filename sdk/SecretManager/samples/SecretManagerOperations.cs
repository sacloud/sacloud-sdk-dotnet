using Sacloud.Sdk.SecretManager;
using Sacloud.Sdk.SecretManager.Models;

/// <summary>
/// SecretManagerの各種操作を実装するクラス
/// </summary>
public class SecretManagerOperations
{
    private readonly SecretManagerClient _client;

    public SecretManagerOperations(SecretManagerClient client)
    {
        _client = client;
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. Vaultを作成");
        Console.WriteLine("2. Vault一覧を取得");
        Console.WriteLine("3. Vault詳細を取得");
        Console.WriteLine("4. Vaultを更新");
        Console.WriteLine("5. Vaultを削除");
        Console.WriteLine("6. Secretを作成");
        Console.WriteLine("7. Secret一覧を取得");
        Console.WriteLine("8. Secretを削除");
        Console.WriteLine("9. Secretをアンベール");
        Console.Write("選択 (1 ~ 9 から入力): ");
        var choice = Console.ReadLine();

        await ExecuteOperationAsync(choice ?? string.Empty);
    }

    /// <summary>
    /// 選択された操作を実行する
    /// </summary>
    private async Task ExecuteOperationAsync(string choice)
    {
        switch (choice)
        {
            case "1":
                await CreateVaultAsync();
                break;
            case "2":
                await ListVaultsAsync();
                break;
            case "3":
                await GetVaultDetailsAsync();
                break;
            case "4":
                await UpdateVaultAsync();
                break;
            case "5":
                await DeleteVaultAsync();
                break;
            case "6":
                await CreateSecretAsync();
                break;
            case "7":
                await ListSecretsAsync();
                break;
            case "8":
                await DeleteSecretAsync();
                break;
            case "9":
                await UnveilSecretAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }
    }

    /// <summary>
    /// Vault一覧を取得する
    /// </summary>
    private async Task ListVaultsAsync()
    {
        var vaults = await _client.Secretmanager.Vaults.GetAsync();
        Console.WriteLine("Vault一覧:");
        if (vaults?.Vaults == null || vaults.Vaults.Count == 0)
        {
            Console.WriteLine("Vaultがありません。");
            return;
        }

        foreach (var vault in vaults.Vaults)
        {
            Console.WriteLine($"- {vault.ID}: {vault.Name}");
        }
    }

    /// <summary>
    /// Vaultを作成する
    /// </summary>
    private async Task CreateVaultAsync()
    {
        var body = new WrappedCreateVault
        {
            Vault = new CreateVault
            {
                Name = "<YOUR_VAULT_NAME>",
                Description = "<YOUR_VAULT_DESCRIPTION>",
                KmsKeyID = "<YOUR_KMS_KEY_ID>",
                Tags = new List<string> { "sample" }
            }
        };
        var vault = await _client.Secretmanager.Vaults.PostAsync(body);
        Console.WriteLine("作成されたVault:");
        Console.WriteLine($"- {vault?.Vault?.ID}: {vault?.Vault?.Name}");
    }

    /// <summary>
    /// Vaultの詳細を取得する
    /// </summary>
    private async Task GetVaultDetailsAsync()
    {
        Console.Write("詳細を取得するVault IDを入力してください: ");
        var vaultId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vaultId))
        {
            Console.WriteLine("Vault IDが入力されていません。");
            return;
        }

        var vault = await _client.Secretmanager.Vaults[vaultId].GetAsync();
        Console.WriteLine("Vault詳細:");
        Console.WriteLine($"ID: {vault?.Vault?.ID}");
        Console.WriteLine($"Name: {vault?.Vault?.Name}");
        Console.WriteLine($"Description: {vault?.Vault?.Description}");
    }

    /// <summary>
    /// Vaultを更新する
    /// </summary>
    private async Task UpdateVaultAsync()
    {
        Console.Write("更新するVault IDを入力してください: ");
        var vaultId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vaultId))
        {
            Console.WriteLine("Vault IDが入力されていません。");
            return;
        }

        var body = new WrappedVault
        {
            Vault = new Vault
            {
                Name = "<YOUR_UPDATED_VAULT_NAME>",
                Description = "<YOUR_UPDATED_VAULT_DESCRIPTION>",
                Tags = new List<string> { "updated" }
            }
        };
        var vault = await _client.Secretmanager.Vaults[vaultId].PutAsync(body);
        Console.WriteLine("更新したVault:");
        Console.WriteLine($"- {vault?.Vault?.ID}: {vault?.Vault?.Name}");
    }

    /// <summary>
    /// Vaultを削除する
    /// </summary>
    private async Task DeleteVaultAsync()
    {
        Console.Write("削除するVault IDを入力してください: ");
        var vaultId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vaultId))
        {
            Console.WriteLine("Vault IDが入力されていません。");
            return;
        }

        await _client.Secretmanager.Vaults[vaultId].DeleteAsync();
    }

    /// <summary>
    /// Secret一覧を取得する
    /// </summary>
    private async Task ListSecretsAsync()
    {
        Console.Write("Secret一覧を取得するVault IDを入力してください: ");
        var vaultId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vaultId))
        {
            Console.WriteLine("Vault IDが入力されていません。");
            return;
        }

        var secrets = await _client.Secretmanager.Vaults[vaultId].Secrets.GetAsync();
        Console.WriteLine("Secret一覧:");
        if (secrets?.Secrets == null || secrets.Secrets.Count == 0)
        {
            Console.WriteLine("Secretがありません。");
            return;
        }

        foreach (var secret in secrets.Secrets)
        {
            Console.WriteLine($"- {secret.Name}: {secret.LatestVersion}");
        }
    }

    /// <summary>
    /// Secretを作成する
    /// </summary>
    private async Task CreateSecretAsync()
    {
        Console.Write("Secretを作成するVault IDを入力してください: ");
        var vaultId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vaultId))
        {
            Console.WriteLine("Vault IDが入力されていません。");
            return;
        }

        var body = new WrappedCreateSecret
        {
            Secret = new CreateSecret
            {
                Name = "<YOUR_SECRET_NAME>",
                Value = "<YOUR_SECRET_VALUE>"
            }
        };
        var secret = await _client.Secretmanager.Vaults[vaultId].Secrets.PostAsync(body);
        Console.WriteLine("作成されたSecret:");
        Console.WriteLine($"- {secret?.Secret?.Name}: {secret?.Secret?.LatestVersion}");
    }

    /// <summary>
    /// Secretを削除する
    /// </summary>
    private async Task DeleteSecretAsync()
    {
        Console.Write("Secretを削除するVault IDを入力してください: ");
        var vaultId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vaultId))
        {
            Console.WriteLine("Vault IDが入力されていません。");
            return;
        }

        Console.Write("削除するSecret名を入力してください: ");
        var secretName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(secretName))
        {
            Console.WriteLine("Secret名が入力されていません。");
            return;
        }

        var body = new WrappedDeleteSecret
        {
            Secret = new DeleteSecret
            {
                Name = secretName
            }
        };
        await _client.Secretmanager.Vaults[vaultId].Secrets.DeleteAsync(body);
    }

    /// <summary>
    /// Secretをアンベールする
    /// </summary>
    private async Task UnveilSecretAsync()
    {
        Console.Write("SecretをアンベールするVault IDを入力してください: ");
        var vaultId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vaultId))
        {
            Console.WriteLine("Vault IDが入力されていません。");
            return;
        }

        Console.Write("アンベールするSecret名を入力してください: ");
        var secretName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(secretName))
        {
            Console.WriteLine("Secret名が入力されていません。");
            return;
        }

        Console.Write("バージョンを入力してください (省略可): ");
        var versionInput = Console.ReadLine();
        int? version = null;
        if (int.TryParse(versionInput, out var parsedVersion))
        {
            version = parsedVersion;
        }

        var body = new WrappedUnvail
        {
            Secret = new Unvail
            {
                Name = secretName,
                Version = version
            }
        };
        var unveiled = await _client.Secretmanager.Vaults[vaultId].Secrets.Unveil.PostAsync(body);
        Console.WriteLine("アンベールしたSecret:");
        Console.WriteLine($"- {unveiled?.Secret?.Name}: {unveiled?.Secret?.Value}");
    }
}
