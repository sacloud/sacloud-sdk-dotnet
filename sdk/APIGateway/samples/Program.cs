using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.APIGateway;
using Sacloud.Sdk.APIGateway.Samples.Samples;
using Sacloud.Sdk.Shared.Models;

// ロガーファクトリーの設定
using var loggerFactory = LoggerFactory.Create(builder =>
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

// 認証プロバイダーを使用してアダプターを初期化
using HttpClientRequestAdapter adapter = new(authProvider);

// APIGatewayクライアントの初期化
APIGatewayClient client = new(adapter, loggerFactory);

// 操作を実行
List<ISample> samples =
[
    new ServiceSample(client),
    new UserSample(client),
    new GroupSample(client),
    new DomainSample(client),
    new CertificateSample(client),
    new PlanSample(client),
    new SubscriptionSample(client),
    new OidcSample(client),
];

while (true)
{
    Console.WriteLine("\n実行する操作を選択してください");
    for (int i = 0; i < samples.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {samples[i].Description}");
    }
    Console.WriteLine($"-------------------------");
    Console.Write($"1 ～ {samples.Count} を入力して選択（ exit で終了）: ");

    var input = Console.ReadLine();

    if (input == "exit") break;
    if (!int.TryParse(input, out int choice)
        || choice < 1
        || choice > samples.Count)
    {
        Console.WriteLine("\n無効な番号が選択されました。");
        continue;
    }

    try
    {
        await samples[choice - 1].RunAsync();
    }
    catch (ApiError ex)
    {
        Console.WriteLine("ApiErrorが発生しました:");
        Console.WriteLine($"  - StatusCode: {ex.ResponseStatusCode}");
        Console.WriteLine($"  - ResponseBody:");
        foreach (var error in ex.ResponseBody)
        {
            Console.WriteLine($"    - {error.Key}: {error.Value}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nエラーが発生しました:");
        Console.WriteLine($"  - {ex.Message}");
    }
}
