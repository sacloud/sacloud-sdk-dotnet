using Sacloud.Sdk.IAM.ScimConfigurations;
using Sacloud.Sdk.IAM.ScimConfigurations.Item;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class UserProvisioningSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("ユーザープロビジョニングのサンプルを開始します");

        // -------------------------------------------------------
        // POST ユーザープロビジョニングを作成する
        Console.WriteLine("POST /scim-configurations | ユーザープロビジョニングを作成する");
        var createResponse = await client.ScimConfigurations.PostAsync(new ScimConfigurationsPostRequestBody
        {
            Name = "sdk-sample-scim-configuration"
        });
        var scimConfigurationId = createResponse.Id
            ?? throw new InvalidOperationException("ユーザープロビジョニングの作成に失敗しました");
        Console.WriteLine($"  - 作成したユーザープロビジョニングID: {scimConfigurationId}");
        Console.WriteLine($"    - Name: {createResponse.Name}");
        Console.WriteLine($"    - BaseUrl: {createResponse.BaseUrl}");
        Console.WriteLine($"    - CreatedAt: {createResponse.CreatedAt}");
        Console.WriteLine($"    - UpdatedAt: {createResponse.UpdatedAt}");

        // -------------------------------------------------------
        // GET ユーザープロビジョニング一覧を取得する
        Console.WriteLine("GET /scim-configurations | ユーザープロビジョニング一覧を取得する");
        var listResponse = await client.ScimConfigurations.GetAsync(x =>
        {
            x.QueryParameters.Page = 1;
        });
        Console.WriteLine($"  - 取得した件数: {listResponse.Count}");
        foreach (var item in listResponse.Items)
        {
            Console.WriteLine($"    - Id: {item.Id}");
            Console.WriteLine($"      - Name: {item.Name}");
            Console.WriteLine($"      - BaseUrl: {item.BaseUrl}");
            Console.WriteLine($"      - CreatedAt: {item.CreatedAt}");
            Console.WriteLine($"      - UpdatedAt: {item.UpdatedAt}");
        }
        Console.WriteLine($"Count: {listResponse.Count}");
        Console.WriteLine($"Next: {listResponse.Next}");
        Console.WriteLine($"Previous: {listResponse.Previous}");

        // -------------------------------------------------------
        // GET 指定したユーザープロビジョニングを取得する
        Console.WriteLine($"GET /scim-configurations/{scimConfigurationId} | 指定したユーザープロビジョニングを取得する");
        var getResponse = await client.ScimConfigurations[scimConfigurationId].GetAsync();
        Console.WriteLine($"  - 取得したユーザープロビジョニングID: {getResponse.Id}");
        Console.WriteLine($"    - Name: {getResponse.Name}");
        Console.WriteLine($"    - BaseUrl: {getResponse.BaseUrl}");
        Console.WriteLine($"    - CreatedAt: {getResponse.CreatedAt}");
        Console.WriteLine($"    - UpdatedAt: {getResponse.UpdatedAt}");

        // -------------------------------------------------------
        // PUT 指定したユーザープロビジョニングを更新する
        Console.WriteLine($"PUT /scim-configurations/{scimConfigurationId} | 指定したユーザープロビジョニングを更新する");
        var updateResponse = await client.ScimConfigurations[scimConfigurationId].PutAsync(new ScimConfigurationsPutRequestBody
        {
            Name = "sdk-sample-scim-configuration-updated"
        });
        Console.WriteLine($"  - 更新したユーザープロビジョニングID: {updateResponse.Id}");
        Console.WriteLine($"    - Name: {updateResponse.Name}");

        // -------------------------------------------------------
        // POST 指定したユーザープロビジョニングのシークレットトークンを再発行する
        Console.WriteLine($"POST /scim-configurations/{scimConfigurationId}/regenerate-token | シークレットトークンを再発行する");
        var regenerateTokenResponse = await client.ScimConfigurations[scimConfigurationId].RegenerateToken.PostAsync();
        Console.WriteLine($"  - トークンの再発行に成功しました (トークンは非表示にしています)");

        // -------------------------------------------------------
        // DELETE 指定したユーザープロビジョニングを削除する
        Console.WriteLine($"DELETE /scim-configurations/{scimConfigurationId} | 指定したユーザープロビジョニングを削除する");
        await client.ScimConfigurations[scimConfigurationId].DeleteAsync();
        Console.WriteLine($"  - ユーザープロビジョニングを削除しました (レスポンスボディ無し)");

        Console.WriteLine("ユーザープロビジョニングのサンプルの実行が完了しました");
    }
}
