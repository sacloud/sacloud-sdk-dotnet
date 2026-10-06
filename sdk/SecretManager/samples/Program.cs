// さくらのクラウドに作成したSecretManagerに対してSDKを介してリクエストを送信する
// 対話メニューを用いてVault/Secretの基本操作を確認するサンプル
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.SecretManager;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;


// ロガーファクトリーの設定
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Information);
});

// 認証プロバイダの初期化
// 使用する認証方法に応じて、認証プロバイダを選択してください。
// 本サンプルコードでは、APIキー認証を例として使用します。
// サービスプリンシパル認証を利用する場合は、コメントアウトされているコードを参考にしてください。

// APIキー認証を利用する
string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";
BasicAuthenticationProvider basicAuthProvider = new BasicAuthenticationProvider(accessToken, accessTokenSecret);

// サービスプリンシパル認証を利用する
/*
string jwt = "<YOUR_JSON_WEB_TOKEN>";

ServicePrincipalOAuth2AccessToken servicePrincipalOAuth2AccessToken = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory);
ApiKeyAuthenticationProvider servicePrincipalAuthProvider = new ApiKeyAuthenticationProvider(
    $"Bearer {servicePrincipalOAuth2AccessToken.AccessToken}",
    "Authorization",
    ApiKeyAuthenticationProvider.KeyLocation.Header);
*/

// 認証プロバイダの選択
// 設定した認証プロバイダを使用し、HttpClientRequestAdapterを初期化します。
// 使用する認証プロバイダに応じて、HttpClientRequestAdapterの引数を変更してください。
using HttpClientRequestAdapter adapter = new HttpClientRequestAdapter(basicAuthProvider);

// SecretManagerクライアントの初期化
var zone = SelectZone();
SecretManagerClient client = new SecretManagerClient(adapter, zone, loggerFactory);

try
{
    // 操作を実行
    var operations = new SecretManagerOperations(client);
    await operations.RunInteractiveMenuAsync();
}
catch (ApiError ex)
{
    Console.WriteLine($"API Error が発生しました");
    Console.WriteLine($"  - {ex.ResponseBody}");
}
catch (Exception ex)
{
    Console.WriteLine($"Exceptionが発生しました");
    Console.WriteLine($"  - {ex.Message}");
}


/// <summary>
/// 利用するゾーンを対話的に選択する
/// </summary>
static SecretManagerClient.SakuraCloudZone SelectZone()
{
    Console.WriteLine("\n使用するゾーンを選択してください:");
    Console.WriteLine("1. Is1a (石狩第1ゾーン)");
    Console.WriteLine("2. Is1b (石狩第2ゾーン)");
    Console.WriteLine("3. Tk1a (東京第1ゾーン)");
    Console.WriteLine("4. Tk1b (東京第2ゾーン)");
    Console.Write("選択 (1 ~ 4 から入力): ");

    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            return SecretManagerClient.SakuraCloudZone.Is1a;
        case "2":
            return SecretManagerClient.SakuraCloudZone.Is1b;
        case "3":
            return SecretManagerClient.SakuraCloudZone.Tk1a;
        case "4":
            return SecretManagerClient.SakuraCloudZone.Tk1b;
        default:
            Console.WriteLine("無効な番号のため Is1a を使用します。");
            return SecretManagerClient.SakuraCloudZone.Is1a;
    }
}
