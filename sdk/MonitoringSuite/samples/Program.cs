using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.MonitoringSuite;
using Sacloud.Sdk.MonitoringSuite.Samples;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

// APIキーのアクセストークン
var accessToken = "<YOUR_API_KEY_ACCESS_TOKEN>";
// APIキーのアクセストークンシークレット
var accessTokenSecret = "<YOUR_API_KEY_ACCESS_TOKEN_SECRET>";

// ロガーファクトリーの設定
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Information);
});

// APIキー認証を利用する
BasicAuthenticationProvider authProvider = new(accessToken, accessTokenSecret);

// サービスプリンシパル認証を利用する場合は、上記のAPIキー認証のコードの代わりに以下のコメントアウトされたコードを使用する
//string jwt = "<YOUR_JSON_WEB_TOKEN>";

//ServicePrincipalOAuth2AccessToken servicePrincipalOAuth2AccessToken = await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory);
//ApiKeyAuthenticationProvider authProvider = new(
//    $"Bearer {servicePrincipalOAuth2AccessToken.AccessToken}",
//    "Authorization",
//    ApiKeyAuthenticationProvider.KeyLocation.Header);

// 認証プロバイダーを使用してアダプターを初期化
using var adapter = new HttpClientRequestAdapter(authProvider);
// MonitoringSuiteクライアントの初期化
MonitoringSuiteClient client = new(adapter, loggerFactory);

// サンプルコードの実行
try
{
    await new PublishersSample(client).RunAsync();

    /*
    sdk/MonitoringSuite/samples 配下に API ドキュメントの各カテゴリに対応したサンプルコードクラスがあります。
    動作を確認したいAPIのサンプルコードクラスを呼び出してください。

    var logStorageId = <YOUR_LOG_STORAGE_ID>;
    var metricsStorageId = <YOUR_METRICS_STORAGE_ID>;

    await new PublishersSample(client).RunAsync();
    await new AlertsSample(client).RunAsync(logStorageId, metricsStorageId);
    await new DashboardsSample(client).RunAsync();
    await new LogsSample(client).RunAsync();
    await new MetricsSample(client).RunAsync();
    await new TracesSample(client).RunAsync();
    await new ManagementSample(client).RunAsync();
    */
}
catch (ApiError ex)
{
    Console.WriteLine("API Error が発生しました");
    Console.WriteLine($"  - ステータスコード: {ex.ResponseStatusCode}");

    if (ex.ResponseBody != null)
    {
        foreach (var kv in ex.ResponseBody)
        {
            Console.WriteLine($"  - {kv.Key}: {kv.Value}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Exceptionが発生しました");
    Console.WriteLine($"  - {ex.Message}");
}
