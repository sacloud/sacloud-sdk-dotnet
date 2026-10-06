using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.NetworkingSuite;
using Sacloud.Sdk.NetworkingSuite.Samples;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

List<string> siteIds =
[
    "is1c"
];

// ロガーファクトリーの設定
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Information);
});

// APIキー認証を利用する
string accessToken = "YOUR_ACCESS_TOKEN";
string accessTokenSecret = "YOUR_ACCESS_TOKEN_SECRET";

BasicAuthenticationProvider authProvider = new(accessToken, accessTokenSecret);

// サービスプリンシパル認証を利用する場合は、上記のAPIキー認証のコードの代わりに以下のコメントアウトされたコードを使用する
/*
string jwt = "<YOUR_JSON_WEB_TOKEN>";

ServicePrincipalOAuth2AccessToken servicePrincipalOAuth2AccessToken = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory);
ApiKeyAuthenticationProvider authProvider = new(
    $"Bearer {servicePrincipalOAuth2AccessToken.AccessToken}",
    "Authorization",
    ApiKeyAuthenticationProvider.KeyLocation.Header);
*/

try
{
    while (true)
    {
        // ゾーン選択
        Console.WriteLine("接続先のゾーンを選択してください");
        for (int index = 1; index <= siteIds.Count; index++)
        {
            Console.WriteLine($"{index}. {siteIds[index - 1]}");
        }
        Console.WriteLine($"0. 終了");
        Console.WriteLine("--------------");
        Console.Write("選択: ");
        string? inputZone = Console.ReadLine()?.Trim();
        if (inputZone == "0")
        {
            break;
        }
        if (!int.TryParse(inputZone, out int selectedIndex) || selectedIndex < 1 || selectedIndex > siteIds.Count)
        {
            Console.WriteLine("\n無効な入力です\n");
            continue;
        }
        string zone = siteIds[selectedIndex - 1];

        // クライアント初期化
        using HttpClientRequestAdapter adapter = new(authProvider);
        NetworkingSuiteClient client = new(adapter, zone, loggerFactory);

        // 操作
        NetworkingSuiteOperations operations = new(client);
        await operations.RunInteractiveMenuAsync();
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"エラーが発生しました: {ex.Message}");
}
