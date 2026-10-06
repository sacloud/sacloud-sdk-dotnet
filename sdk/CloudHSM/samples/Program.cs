// さくらのクラウドCloudHSMリソースに対してSDKを介してリクエストを送信する
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.CloudHSM;
using Sacloud.Sdk.CloudHSM.Samples;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

// ロガーファクトリーの設定
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Information);
});

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
using HttpClientRequestAdapter adapter = new(authProvider);
adapter.BaseUrl = "http://localhost:18092/is1b/api/cloud/1.1"; // sakumock-cloudhsmのベースURLに上書きする

// CloudHSMクライアントの初期化
CloudHSMClient client = new(adapter, loggerFactory);

try
{
    // 操作を実行
    var operations = new CloudHSMOperations(client);
    await operations.RunInteractiveMenuAsync();
}
catch (ApiError ex)
{
    Console.WriteLine("-----[APIエラーが発生しました]-----");
    Console.WriteLine($"ステータスコード: {ex.ResponseStatusCode}");
}
catch (Exception ex)
{
    Console.WriteLine("-----[エラーが発生しました]-----");
    Console.WriteLine(ex.Message);
}