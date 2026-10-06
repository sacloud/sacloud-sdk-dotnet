using Sacloud.Sdk.IAM.ServicePrincipals;
using Sacloud.Sdk.IAM.ServicePrincipals.Item.UploadKey;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.Samples.Helpers;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class ServicePrincipalSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(long projectId, string publicKeyPem, string privateKeyPem)
    {
        Console.WriteLine("サービスプリンシパルのサンプルを実行します");

        // -------------------------------------------------------
        // POST サービスプリンシパルを作成する
        Console.WriteLine("POST /service-principals | サービスプリンシパルを作成する");
        var createServicePrincipalResponse = await client.ServicePrincipals.PostAsync(new ServicePrincipalsPostRequestBody
        {
            ProjectId = projectId,
            Name = "sdk-sample-service-principal",
            Description = "SDKから作成したサンプルサービスプリンシパル",
        });
        var servicePrincipalId = createServicePrincipalResponse.Id
            ?? throw new InvalidOperationException("サービスプリンシパルの作成に失敗しました");
        Console.WriteLine($"  - 作成したサービスプリンシパルID: {servicePrincipalId}");
        Console.WriteLine($"    - Name: {createServicePrincipalResponse.Name}");

        // -------------------------------------------------------
        // GET サービスプリンシパル一覧を取得する
        Console.WriteLine("GET /service-principals | サービスプリンシパル一覧を取得する");
        var listResponse = await client.ServicePrincipals.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.ProjectId = projectId;
        });
        Console.WriteLine($"  - サービスプリンシパル数: {listResponse.Count}");

        // -------------------------------------------------------
        // GET 指定したサービスプリンシパルを取得する
        Console.WriteLine($"GET /service-principals/{servicePrincipalId} | 指定したサービスプリンシパルを取得する");
        var getServicePrincipalResponse = await client.ServicePrincipals[servicePrincipalId].GetAsync();
        Console.WriteLine($"  - 取得したサービスプリンシパルID: {getServicePrincipalResponse.Id}");
        Console.WriteLine($"    - Name: {getServicePrincipalResponse.Name}");
        Console.WriteLine($"    - Description: {getServicePrincipalResponse.Description}");

        // -------------------------------------------------------
        // PUT 指定したサービスプリンシパルを更新する
        Console.WriteLine($"PUT /service-principals/{servicePrincipalId} | 指定したサービスプリンシパルを更新する");
        var updateServicePrincipalResponse = await client.ServicePrincipals[servicePrincipalId].PutAsync(
            new ServicePrincipals.Item.WithService_principal_PutRequestBody
            {
                Name = getServicePrincipalResponse.Name,
                Description = getServicePrincipalResponse.Description,
            });
        Console.WriteLine($"  - 更新したサービスプリンシパルID: {updateServicePrincipalResponse.Id}");
        Console.WriteLine($"    - Name: {updateServicePrincipalResponse.Name}");

        // -------------------------------------------------------
        // POST ユーザ生成の鍵をサービスプリンシパルキーとして登録する
        Console.WriteLine($"POST /service-principals/{servicePrincipalId}/upload-key | ユーザ生成の鍵をサービスプリンシパルキーとして登録する");
        var uploadKeyResponse = await client.ServicePrincipals[servicePrincipalId].UploadKey.PostAsync(new UploadKeyPostRequestBody
        {
            PublicKey = publicKeyPem,
        });
        var servicePrincipalKeyId = uploadKeyResponse.Id
            ?? throw new InvalidOperationException("サービスプリンシパルキーの登録に失敗しました");
        Console.WriteLine($"  - 登録したサービスプリンシパルキーID: {servicePrincipalKeyId}");
        Console.WriteLine($"    - Kid: {uploadKeyResponse.Kid}");
        Console.WriteLine($"    - Status: {uploadKeyResponse.Status}");
        Console.WriteLine($"    - KeyExpiresAt: {uploadKeyResponse.KeyExpiresAt}");

        // -------------------------------------------------------
        // POST 指定したサービスプリンシパルキーを無効化する
        Console.WriteLine($"POST /service-principals/{servicePrincipalId}/keys/{servicePrincipalKeyId}/disable | 指定したサービスプリンシパルキーを無効化する");
        var disableServicePrincipalKeyResponse = await client.ServicePrincipals[servicePrincipalId].Keys[servicePrincipalKeyId].Disable.PostAsync();
        Console.WriteLine($"  - サービスプリンシパルキーを無効化しました: {disableServicePrincipalKeyResponse.Status}");
        Console.WriteLine($"    - KeyExpiresAt: {disableServicePrincipalKeyResponse.KeyExpiresAt}");

        // -------------------------------------------------------
        // POST 指定したサービスプリンシパルキーを有効化する
        Console.WriteLine($"POST /service-principals/{servicePrincipalId}/keys/{servicePrincipalKeyId}/enable | 指定したサービスプリンシパルキーを有効化する");
        var enableServicePrincipalKeyResponse = await client.ServicePrincipals[servicePrincipalId].Keys[servicePrincipalKeyId].Enable.PostAsync();
        Console.WriteLine($"  - サービスプリンシパルキーを有効化しました: {enableServicePrincipalKeyResponse.Status}");
        Console.WriteLine($"    - KeyExpiresAt: {enableServicePrincipalKeyResponse.KeyExpiresAt}");

        // -------------------------------------------------------
        // GET サービスプリンシパルキー一覧を取得する
        Console.WriteLine($"GET /service-principals/{servicePrincipalId}/keys | サービスプリンシパルキー一覧を取得する");
        var listKeysResponse = await client.ServicePrincipals[servicePrincipalId].Keys.GetAsync();
        Console.WriteLine($"  - キー数: {listKeysResponse.Count}");

        // -------------------------------------------------------
        // POST RFC 7523 (JWT Bearer Token Grant) に基づき、署名済みJWTを使って一時的なサービスプリンシパルトークンを取得する
        Console.WriteLine($"Post /service-principals/oauth2/token | RFC 7523 (JWT Bearer Token Grant) に基づき、署名済みJWTを使って一時的なサービスプリンシパルトークンを取得する");
        var createTokenResponse = await client.ServicePrincipals.Oauth2.Token.PostAsync(new ServicePrincipalJWTGrantRequest()
        {
            GrantType = ServicePrincipalJWTGrantRequest_grant_type.UrnIetfParamsOauthGrantTypeJwtBearer,
            Assertion = JsonWebTokenHelper.GenerateJsonWebToken(
                servicePrincipalId.ToString(),
                uploadKeyResponse.Kid,
                privateKeyPem
            ),
        });
        Console.WriteLine($"  - サービスプリンシパルトークンの取得に成功しました（サービスプリンシパルトークンは非表示にしています）");

        // -------------------------------------------------------
        // DELETE 指定したサービスプリンシパルキーを削除する
        Console.WriteLine($"DELETE /service-principals/{servicePrincipalId}/keys/{servicePrincipalKeyId} | 指定したサービスプリンシパルキーを削除する");
        await client.ServicePrincipals[servicePrincipalId].Keys[servicePrincipalKeyId].DeleteAsync();
        Console.WriteLine($"  - サービスプリンシパルキーを削除しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // DELETE 指定したサービスプリンシパルを削除する
        Console.WriteLine($"DELETE /service-principals/{servicePrincipalId} | 指定したサービスプリンシパルを削除する");
        await client.ServicePrincipals[servicePrincipalId].DeleteAsync();
        Console.WriteLine($"  - サービスプリンシパルを削除しました: {servicePrincipalId}");

        Console.WriteLine("サービスプリンシパルのサンプルの実行が完了しました");
    }
}
