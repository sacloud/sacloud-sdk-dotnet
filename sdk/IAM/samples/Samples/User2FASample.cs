namespace Sacloud.Sdk.IAM.Samples.Samples;

class User2FASample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(long userId)
    {
        Console.WriteLine("ユーザ2要素認証のサンプルを実行します");

        // -------------------------------------------------------
        // POST ユーザのOTPを無効化する
        Console.WriteLine($"POST /compat/users/{userId}/deactivate-otp | ユーザのOTPを無効化する");
        await client.Compat.Users[userId].DeactivateOtp.PostAsync();
        Console.WriteLine("  - ユーザのOTPを無効化しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // GET ユーザの信頼済みディバイス一覧を取得する
        Console.WriteLine($"GET /compat/users/{userId}/trusted-devices | ユーザの信頼済みディバイス一覧を取得する");
        var trustedDevices = await client.Compat.Users[userId].TrustedDevices.GetAsync();
        Console.WriteLine($"  - 取得した信頼済みディバイス数: {trustedDevices.Items?.Count ?? 0}");
        var trustedDeviceId = trustedDevices.Items?.FirstOrDefault()?.Id;

        // -------------------------------------------------------
        // DELETE ユーザの信頼済みディバイスを削除する
        Console.WriteLine($"DELETE /compat/users/{userId}/trusted-devices/{trustedDeviceId} | ユーザの信頼済みディバイスを削除する");
        if (trustedDeviceId is long deviceId)
        {
            await client.Compat.Users[userId].TrustedDevices[deviceId].DeleteAsync();
            Console.WriteLine("  - 指定した信頼済みディバイスを削除しました (レスポンスボディ無し)");
        }
        else
        {
            Console.WriteLine("  - 信頼済みディバイスが存在しないためスキップします");
        }

        // -------------------------------------------------------
        // POST ユーザの全信頼済みディバイスを削除する
        Console.WriteLine($"POST /compat/users/{userId}/clear-trusted-devices | ユーザの全信頼済みディバイスを削除する");
        await client.Compat.Users[userId].ClearTrustedDevices.PostAsync();
        Console.WriteLine("  - ユーザの全信頼済みディバイスを削除しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // GET ユーザのセキュリティキー一覧を取得する
        Console.WriteLine($"GET /compat/users/{userId}/security-keys | ユーザのセキュリティキー一覧を取得する");
        var listSecurityKeys = await client.Compat.Users[userId].SecurityKeys.GetAsync();
        Console.WriteLine($"  - 取得したセキュリティキー数: {listSecurityKeys.Items?.Count ?? 0}");

        Console.WriteLine("ユーザ2要素認証のサンプルの実行が完了しました");
    }
}
