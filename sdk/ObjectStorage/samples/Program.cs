using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.ObjectStorage;
using Sacloud.Sdk.ObjectStorage.Samples;
using Sacloud.Sdk.Shared.Helpers;
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

var siteIds = new List<string>
{
    "isk01",
    "tky01",
    "arc02",
};

try
{
    while (true)
    {
        var selectedSiteId = SelectSiteId(siteIds);
        if (selectedSiteId is null)
        {
            break;
        }

        // 認証プロバイダーを使用してアダプターを初期化
        using var adapter = new HttpClientRequestAdapter(authProvider);

        // ObjectStorageクライアントの初期化
        var client = new ObjectStorageClient(adapter, selectedSiteId, loggerFactory);

        // 操作を実行
        var operations = new ObjectStorageOperations(client);
        await operations.RunInteractiveMenuAsync();
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"エラーが発生しました: {ex.Message}");
}

/// <summary>
/// 利用する site を選択または追加します。
/// </summary>
/// <param name="siteIds">選択対象の site 一覧です。</param>
/// <returns>選択した site。終了時は null。</returns>
static string? SelectSiteId(IList<string> siteIds)
{
    ArgumentNullException.ThrowIfNull(siteIds);

    while (true)
    {
        Console.WriteLine("\n接続先 site を選択してください:");

        for (var index = 0; index < siteIds.Count; index++)
        {
            Console.WriteLine($"{index + 1}. {siteIds[index]}");
        }

        var addOptionNumber = siteIds.Count + 1;
        Console.WriteLine($"{addOptionNumber}. site を追加");
        Console.WriteLine("0. 終了");
        Console.Write("選択: ");

        var input = Console.ReadLine()?.Trim();
        if (input == "0")
        {
            return null;
        }

        if (!int.TryParse(input, out var selectedNumber))
        {
            Console.WriteLine("番号で入力してください。");
            continue;
        }

        if (selectedNumber >= 1 && selectedNumber <= siteIds.Count)
        {
            return siteIds[selectedNumber - 1];
        }

        if (selectedNumber == addOptionNumber)
        {
            var addedSiteId = PromptNewSiteId(siteIds);
            if (addedSiteId is not null)
            {
                return addedSiteId;
            }

            continue;
        }

        Console.WriteLine("無効な番号です。");
    }
}

/// <summary>
/// 新しい site を一覧に追加します。
/// </summary>
/// <param name="siteIds">追加先の site 一覧です。</param>
/// <returns>追加した site。未入力時は null。</returns>
static string? PromptNewSiteId(IList<string> siteIds)
{
    ArgumentNullException.ThrowIfNull(siteIds);

    Console.Write("追加する site ID を入力してください: ");
    var inputSiteId = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(inputSiteId))
    {
        Console.WriteLine("site ID が未入力です。");
        return null;
    }

    var existingSiteId = siteIds.FirstOrDefault(siteId => string.Equals(siteId, inputSiteId, StringComparison.OrdinalIgnoreCase));
    if (existingSiteId is not null)
    {
        Console.WriteLine($"{existingSiteId} はすでに登録されています。");
        return existingSiteId;
    }

    siteIds.Add(inputSiteId);
    return inputSiteId;
}
