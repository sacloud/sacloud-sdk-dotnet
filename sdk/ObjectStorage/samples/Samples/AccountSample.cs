using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class AccountSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET サイトアカウントの取得
    public async Task GetAccountAsync()
    {
        Console.WriteLine("\nGET /account | サイトアカウントの取得\n");

        var response = await client.Account.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var account = response?.Data
            ?? throw new InvalidOperationException("アカウント情報(data)が null です。");
        Console.WriteLine($"ResourceID: {account.ResourceId}");
        Console.WriteLine($"Code: {account.Code}");
        Console.WriteLine($"CreatedAt: {account.CreatedAt}");
    }

    // POST サイトアカウントの作成
    public async Task PostAccountAsync()
    {
        Console.WriteLine("\nPOST /account | サイトアカウントの作成\n");

        var response = await client.Account.PostAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var account = response?.Data
            ?? throw new InvalidOperationException("アカウント情報(data)が null です。");
        Console.WriteLine($"ResourceID: {account.ResourceId}");
        Console.WriteLine($"Code: {account.Code}");
        Console.WriteLine($"CreatedAt: {account.CreatedAt}");
    }

    // DELETE サイトアカウントの削除
    public async Task DeleteAccountAsync()
    {
        Console.WriteLine("\nDELETE /account | サイトアカウントの削除\n");

        try
        {
            await client.Account.DeleteAsync();

            await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

            Console.WriteLine("アカウントを削除しました。（レスポンスボディ無し）");
        }
        catch (ApiError ex) when (ex.ResponseStatusCode == 409)
        {
            Console.Error.WriteLine("Account Delete が 409 Conflict で失敗しました。削除対象サイトにリソースが残っています。");
            Console.Error.WriteLine("先に以下を削除してから再実行してください:");
            Console.Error.WriteLine("- 2-1/2-3 バケット一覧の取得・バケットの削除でバケットを削除");
            Console.Error.WriteLine("- 3-4〜3-7 サイトアカウントのアクセスキー操作で不要なアカウントキーを削除");
            Console.Error.WriteLine("- 4-1〜4-5 パーミッション操作で不要なパーミッションを削除");
        }
    }

    // GET サイトアカウントのアクセスキーの取得
    public async Task GetAccountKeysAsync()
    {
        Console.WriteLine("\nGET /account/keys | サイトアカウントのアクセスキー一覧の取得\n");

        var response = await client.Account.Keys.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var keys = response?.Data
            ?? throw new InvalidOperationException("アカウントキー情報(data)が null です。");
        foreach (var key in keys)
        {
            Console.WriteLine($"- ID: {key.Id}");
            Console.WriteLine($"  CreatedAt: {key.CreatedAt}");
        }
    }

    // POST サイトアカウントのアクセスキーの発行
    public async Task PostAccountKeyAsync()
    {
        Console.WriteLine("\nPOST /account/keys | サイトアカウントのアクセスキーの発行\n");

        var response = await client.Account.Keys.PostAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var key = response?.Data
            ?? throw new InvalidOperationException("アカウントキー情報(data)が null です。");
        Console.WriteLine($"ID: {key.Id}");
        Console.WriteLine($"Secret: {key.Secret}");
        Console.WriteLine($"CreatedAt: {key.CreatedAt}");
    }

    // GET サイトアカウントのアクセスキーの取得
    public async Task GetAccountKeyAsync()
    {
        Console.WriteLine("\nGET /account/keys/{keyId} | サイトアカウントのアクセスキーの取得\n");

        var keyId = ReadRequiredValue("KeyId");
        if (keyId is null) return;

        var response = await client.Account.Keys[keyId].GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var key = response?.Data
            ?? throw new InvalidOperationException("アカウントキー情報(data)が null です。");
        Console.WriteLine($"ID: {key.Id}");
        Console.WriteLine($"CreatedAt: {key.CreatedAt}");
    }

    // DELETE サイトアカウントのアクセスキーの削除
    public async Task DeleteAccountKeyAsync()
    {
        Console.WriteLine("\nDELETE /account/keys/{keyId} | サイトアカウントのアクセスキーの削除\n");

        var keyId = ReadRequiredValue("KeyId");
        if (keyId is null) return;

        await client.Account.Keys[keyId].DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        Console.WriteLine($"アカウントキーを削除しました。（レスポンスボディ無し）");
    }
}
