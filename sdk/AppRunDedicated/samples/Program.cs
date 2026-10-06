using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.AppRunDedicated;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.AppRunDedicated.Samples;
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
string accessToken = "<YOUR_ACCESS_TOKEN>";
string accessTokenSecret = "<YOUR_ACCESS_TOKEN_SECRET>";
string servicePrincipalID = "<YOUR_SERVICE_PRINCIPAL_ID>";
string letsEncryptEmail = "<YOUR_LETS_ENCRYPT_EMAIL>";

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
HttpClientRequestAdapter adapter = new(basicAuthProvider);
AppRunDedicatedClient client = new(adapter, loggerFactory);

try
{
    // clusterサンプルの実行
    await ClusterSample.RunSampleAsync(client, servicePrincipalID, letsEncryptEmail);

    // applicationサンプルの実行
    await ApplicationSample.RunSampleAsync(client, servicePrincipalID);

    // application_versionサンプルの実行
    await ApplicationVersionSample.RunSampleAsync(client, servicePrincipalID,
        new ApplicationVersionSample.ApplicationVersionSampleInput
        {
            ContainerImage = "<YOUR_CONTAINER_IMAGE>",
            ContainerRegistryUsername = "<YOUR_CONTAINER_REGISTRY_USERNAME>",
            ContainerRegistryPassword = "<YOUR_CONTAINER_REGISTRY_PASSWORD>"
        }
    );

    // certificateサンプルの実行
    await CertificateSample.RunSampleAsync(client, servicePrincipalID,
        new CertificateSample.CertificateSampleInput
        {
            CertificatePem = "<YOUR_CERTIFICATE_PEM>",
            PrivatekeyPem = "<YOUR_PRIVATEKEY_PEM>"
        }
    );

    // limitサンプルの実行
    await LimitSample.RunSampleAsync(client);

    // service_classサンプルの実行
    await ServiceClassSample.RunSampleAsync(client);
}
catch(Error ex)
{
    Console.WriteLine("-----[APIエラーが発生しました]-----");
    Console.WriteLine($"ステータスコード: {ex.Status}");
    Console.WriteLine($"タイトル: {ex.Title}");
}
catch(Exception ex)
{
    Console.WriteLine("-----[エラーが発生しました]-----");
    Console.WriteLine(ex.Message);   
}
