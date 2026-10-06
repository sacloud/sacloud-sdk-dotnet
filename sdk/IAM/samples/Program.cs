using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.IAM;
using Sacloud.Sdk.IAM.Samples.Helpers;
using Sacloud.Sdk.IAM.Samples.Samples;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;

// サービスプリンシパルのリソースID
var servicePrincipalResourceId = "<YOUR_SERVICE_PRINCIPAL_RESOURCE_ID>";
// サービスプリンシパルキーのKID
var kid = "<YOUR_KID>";
// サービスプリンシパルキーの秘密鍵（PEM形式）
var privateKeyPem = "<YOUR_PRIVATE_KEY_PEM>";

// ロガーファクトリーの設定
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Information);
});

// JWTの生成
var jwt = JsonWebTokenHelper.GenerateJsonWebToken(servicePrincipalResourceId, kid, privateKeyPem);

// アクセストークンの取得
var accessToken = (await AccessTokenHelper.GetAccessTokenAsync(jwt, loggerFactory: loggerFactory)).AccessToken;

// 認証プロバイダの設定
var authProvider = new ApiKeyAuthenticationProvider($"Bearer {accessToken}", "Authorization", ApiKeyAuthenticationProvider.KeyLocation.Header);
// HTTPクライアントの設定
using var adapter = new HttpClientRequestAdapter(authProvider);
// IAMクライアントの初期化
IAMClient client = new(adapter, loggerFactory);

// サンプルコードの実行
try
{
    await new IamRoleSample(client).RunAsync();

    /*
    Samples/ 配下に API ドキュメントの各カテゴリに対応したサンプルコードクラスがあります。
    動作を確認したいAPIのサンプルコードクラスを呼び出してください。

    long projectId = <PROJECT_ID_FOR_SAMPLE_EXECUTION>;
    long folderId = <FOLDER_ID_FOR_SAMPLE_EXECUTION>;
    long userId = <USER_ID_FOR_SAMPLE_EXECUTION>;
    string servicePrincipalPublicKey = "<SERVICE_PRINCIPAL_PUBLIC_KEY_FOR_SAMPLE_EXECUTION>";
    string servicePrincipalPrivateKey = "<SERVICE_PRINCIPAL_PRIVATE_KEY_FOR_SAMPLE_EXECUTION>";

    await new UserSample(client).RunAsync();
    await new AuthSample(client).RunAsync();
    await new FolderSample(client).RunAsync();
    await new GroupSample(client).RunAsync(userId);
    await new IamPolicySample(client).RunAsync(projectId, folderId);
    await new IamRoleSample(client).RunAsync();
    await new IdPolicySample(client).RunAsync();
    await new IdRoleSample(client).RunAsync();
    await new OrganizationSample(client).RunAsync();
    await new ProjectApiKeySample(client).RunAsync(projectId);
    await new ServicePolicySample(client).RunAsync();
    await new ServicePrincipalSample(client).RunAsync(projectId, servicePrincipalPublicKey, servicePrincipalPrivateKey);
    await new SsoProfileSample(client).RunAsync();
    await new ProjectSample(client).RunAsync(folderId);
    await new User2FASample(client).RunAsync(userId);
    await new UserProvisioningSample(client).RunAsync();
    */
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
