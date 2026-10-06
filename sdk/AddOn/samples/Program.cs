using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.AddOn;
using Sacloud.Sdk.AddOn.Samples.Samples;
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

// AddOnクライアントの初期化
var addOnClient = new AddOnClient(adapter, loggerFactory);

// サンプルコードの実行
await new AiSample(addOnClient).RunAsync();
/*
Samples/ 配下に API ドキュメントの各カテゴリに対応したサンプルコードクラスがあります。
動作を確認したいAPIのサンプルコードクラスを呼び出してください。

await new CdnSample(addOnClient).RunAsync();
await new DdosSample(addOnClient).RunAsync();
await new WafSample(addOnClient).RunAsync();
await new VulnerabilitySample(addOnClient).RunAsync();
await new DataLakeSample(addOnClient).RunAsync();
await new DwhSample(addOnClient).RunAsync();
await new EtlSample(addOnClient).RunAsync();
await new QuerySample(addOnClient).RunAsync();
await new SearchSample(addOnClient).RunAsync();
await new StreamingSample(addOnClient).RunAsync();
*/
