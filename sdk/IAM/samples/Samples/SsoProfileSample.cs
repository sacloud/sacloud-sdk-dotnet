using Sacloud.Sdk.IAM.SsoProfiles;
using Sacloud.Sdk.IAM.SsoProfiles.Item;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class SsoProfileSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("SSOプロファイルのサンプルを開始します");

        // -------------------------------------------------------
        // POST SSOプロファイルを作成する
        Console.WriteLine("POST /sso-profiles | SSOプロファイルを作成する");
        var createResponse = await client.SsoProfiles.PostAsync(new SsoProfilesPostRequestBody
        {
            Name = "sdk-sample-sso-profile",
            Description = "SDKから作成したサンプルSSO",
            IdpEntityId = "https://idp.example.com/sample-entity",
            IdpLoginUrl = "https://idp.example.com/sample-entity/sso/login",
            IdpLogoutUrl = "https://idp.example.com/sample-entity/sso/logout",
            IdpCertificate = "-----BEGIN CERTIFICATE-----\nMIIDqjCCApKgAwIBAgIGAW0example\n-----END CERTIFICATE-----",
        });
        var ssoProfileId = createResponse.Id
            ?? throw new InvalidOperationException("SSOプロファイルの作成に失敗しました");
        Console.WriteLine($"  - 作成したSSOプロファイルID: {ssoProfileId}");
        Console.WriteLine($"    - Name: {createResponse.Name}");

        // -------------------------------------------------------
        // GET 指定したSSOプロファイルを取得する
        Console.WriteLine($"GET /sso-profiles/{ssoProfileId} | 指定したSSOプロファイルを取得する");
        var getResponse = await client.SsoProfiles[ssoProfileId].GetAsync();
        Console.WriteLine($"  - 取得したSSOプロファイルID: {getResponse.Id}");
        Console.WriteLine($"    - Name: {getResponse.Name}");

        // -------------------------------------------------------
        // GET SSOプロファイル一覧を取得する
        Console.WriteLine("GET /sso-profiles | SSOプロファイル一覧を取得する");
        var listResponse = await client.SsoProfiles.GetAsync();
        Console.WriteLine($"  - 取得したプロファイル数: {listResponse.Count}");

        // -------------------------------------------------------
        // PUT 指定したSSOプロファイルを更新する
        Console.WriteLine($"PUT /sso-profiles/{ssoProfileId} | 指定したSSOプロファイルを更新する");
        var updateResponse = await client.SsoProfiles[ssoProfileId].PutAsync(new WithSso_profile_PutRequestBody
        {
            Name = "sdk-updated-sso-profile",
            Description = "更新されたSSO説明",
            IdpEntityId = "https://idp.example.com/sample-entity",
            IdpLoginUrl = "https://idp.example.com/sample-entity/sso/login",
            IdpLogoutUrl = "https://idp.example.com/sample-entity/sso/logout",
            IdpCertificate = "-----BEGIN CERTIFICATE-----\nMIIDqjCCApKgAwIBAgIGAW0example\n-----END CERTIFICATE-----",
        });
        Console.WriteLine($"  - 更新したSSOプロファイルID: {updateResponse.Id}");
        Console.WriteLine($"    - Name: {updateResponse.Name}");

        // -------------------------------------------------------
        // POST 指定したSSOプロファイルを割り当てる
        Console.WriteLine($"POST /sso-profiles/{ssoProfileId}/assign | SSOプロファイルを割り当てる");
        // Note: 組織に割り当てることで、会員配下のユーザすべてに有効化されます
        var assignResponse = await client.SsoProfiles[ssoProfileId].Assign.PostAsync();
        Console.WriteLine($"  - 割り当て完了: {assignResponse.Id}");
        Console.WriteLine($"    - Assigned: {assignResponse.Assigned}");

        // -------------------------------------------------------
        // POST 指定したSSOプロファイルの割り当てを外す
        Console.WriteLine($"POST /sso-profiles/{ssoProfileId}/unassign | SSOプロファイル割り当て解除");
        var unassignResponse = await client.SsoProfiles[ssoProfileId].Unassign.PostAsync();
        Console.WriteLine($"  - 割り当て解除完了: {unassignResponse.Id}");
        Console.WriteLine($"    - Assigned: {unassignResponse.Assigned}");

        // -------------------------------------------------------
        // DELETE 指定したSSOプロファイルを削除する
        Console.WriteLine($"DELETE /sso-profiles/{ssoProfileId} | 指定したSSOプロファイルを削除する");
        await client.SsoProfiles[ssoProfileId].DeleteAsync();
        Console.WriteLine($"  - SSOプロファイルID {ssoProfileId} を削除しました");

        Console.WriteLine("SSOプロファイルのサンプルの実行が完了しました");
    }
}