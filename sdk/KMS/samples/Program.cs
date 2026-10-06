// さくらのクラウドKMSリソースに対してSDKを介してリクエストを送信する
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.KMS;
using Sacloud.Sdk.KMS.Samples;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

// ロガーファクトリーの設定
ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Information);
});

HttpClientRequestAdapter? adapter = null;

try
{
    // APIキー認証を利用する
    string accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
    string accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";

    BasicAuthenticationProvider authProvider = new(accessToken, accessTokenSecret);

    // サービスプリンシパル認証を利用する場合は、上記のAPIキー認証のコードの代わりに以下のコメントアウトされたコードを使用する
    //string jwt = "<YOUR_JSON_WEB_TOKEN>";

    //ServicePrincipalOAuth2AccessToken servicePrincipalOAuth2AccessToken = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory);
    //ApiKeyAuthenticationProvider authProvider = new(
    //    $"Bearer {servicePrincipalOAuth2AccessToken.AccessToken}",
    //    "Authorization",
    //    ApiKeyAuthenticationProvider.KeyLocation.Header);

    // 認証プロバイダーを使用してアダプターを初期化
    adapter = new(authProvider);

    // KMSクライアントの初期化
    var zone = SelectZone();
    var kmsClient = new KMSClient(adapter, zone, loggerFactory);
    
    // 操作を実行
    var operations = new KmsOperations(kmsClient);
    await operations.RunInteractiveMenuAsync();
}
catch (Exception ex)
{
    Console.WriteLine("-----[エラーが発生しました]-----");
    Console.WriteLine(ex.Message);
}
finally
{
    if (adapter is not null)
    {
        adapter.Dispose();
    }

    loggerFactory.Dispose();
}

/// <summary>
/// 利用するゾーンを対話的に選択する
/// </summary>
static KMSClient.SakuraCloudZone SelectZone()
{
    Console.WriteLine("\n使用するゾーンを選択してください:");
    Console.WriteLine("1. Is1a (石狩第1ゾーン)");
    Console.WriteLine("2. Is1b (石狩第2ゾーン)");
    Console.WriteLine("3. Tk1a (東京第1ゾーン)");
    Console.WriteLine("4. Tk1b (東京第2ゾーン)");
    Console.WriteLine("選択 (1 ~ 4 から入力): ");

    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            return KMSClient.SakuraCloudZone.Is1a;
        case "2":
            return KMSClient.SakuraCloudZone.Is1b;
        case "3":
            return KMSClient.SakuraCloudZone.Tk1a;
        case "4":
            return KMSClient.SakuraCloudZone.Tk1b;
        default:
            Console.WriteLine("無効な番号のため Is1a を使用します。");
            return KMSClient.SakuraCloudZone.Is1a;
    }
}
