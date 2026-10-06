// さくらのクラウドに作成したSimpleNotificationリソースに対してSDKを介してリクエストを送信する
// 対話メニューを用いて通知先・通知先グループ・通知ルーティングの基本操作を確認するサンプル
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.SimpleNotification;
using Sacloud.Sdk.SimpleNotification.Samples;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Shared.Helpers;

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

BasicAuthenticationProvider basicAuthProvider = new(accessToken, accessTokenSecret);

// サービスプリンシパル認証を利用する
/*
string jwt = "<YOUR_JSON_WEB_TOKEN>";

ServicePrincipalOAuth2AccessToken servicePrincipalOAuth2AccessToken = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory);
ApiKeyAuthenticationProvider servicePrincipalAuthProvider = new(
    $"Bearer {servicePrincipalOAuth2AccessToken.AccessToken}",
    "Authorization",
    ApiKeyAuthenticationProvider.KeyLocation.Header);
*/

// 認証プロバイダの選択
// 設定した認証プロバイダを使用し、HttpClientRequestAdapterを初期化します。
// 使用する認証プロバイダに応じて、HttpClientRequestAdapterの引数を変更してください。
using var adapter = new HttpClientRequestAdapter(basicAuthProvider);

// SimpleNotificationクライアントの初期化
var simpleNotificationClient = new SimpleNotificationClient(adapter, loggerFactory);

try
{
    // 操作を実行
    var operations = new SimpleNotificationOperations(simpleNotificationClient, adapter.BaseUrl ?? string.Empty);
    await operations.RunInteractiveMenuAsync();
}
catch (ApiError ex)
{
    Console.WriteLine("-----[APIエラーが発生しました]-----");
    foreach (KeyValuePair<string, object> kv in ex.ResponseBody)
    {
        Console.WriteLine($"{kv.Key}: {kv.Value}");
    }
}
catch (Exception ex)
{
    Console.WriteLine("-----[エラーが発生しました]-----");
    Console.WriteLine(ex.Message);
}
