using Sacloud.Sdk.IAM.Compat.ApiKeys;
using Sacloud.Sdk.IAM.Compat.ApiKeys.Item;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class ProjectApiKeySample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(long projectId)
    {
        Console.WriteLine("プロジェクトAPIキーのサンプルを実行します");

        // -------------------------------------------------------
        // POST APIキーを作成する
        Console.WriteLine("POST /compat/api-keys | APIキーを作成する");
        var createApiKeyResponse = await client.Compat.ApiKeys.PostAsync(new ApiKeysPostRequestBody
        {
            ProjectId = projectId,
            Name = "sample-api-key",
            Description = "SDKから作成したサンプルAPIキー",
            IamRoles = ["resource-viewer"],
        });
        var apiKeyId = createApiKeyResponse.Id
            ?? throw new InvalidOperationException("APIキーの作成に失敗しました");
        Console.WriteLine($"  - 作成したAPIキーID: {apiKeyId}");
        Console.WriteLine($"    - Name: {createApiKeyResponse.Name}");
        Console.WriteLine($"    - AccessToken: {createApiKeyResponse.AccessToken}");

        // -------------------------------------------------------
        // GET 指定したAPIキーを取得する
        Console.WriteLine($"GET /compat/api-keys/{apiKeyId} | 指定したAPIキーを取得する");
        var getApiKeyResponse = await client.Compat.ApiKeys[apiKeyId].GetAsync();
        Console.WriteLine($"  - 取得したAPIキーID: {getApiKeyResponse.Id}");
        Console.WriteLine($"    - Name: {getApiKeyResponse.Name}");

        // -------------------------------------------------------
        // GET APIキー一覧を取得する
        Console.WriteLine("GET /compat/api-keys | APIキー一覧を取得する");
        var listResponse = await client.Compat.ApiKeys.GetAsync();
        Console.WriteLine($"  - APIキー数: {listResponse.Count}");

        // -------------------------------------------------------
        // PUT 指定したAPIキーを更新する
        Console.WriteLine($"PUT /compat/api-keys/{apiKeyId} | 指定したAPIキーを更新する");
        var updateApiKeyResponse = await client.Compat.ApiKeys[apiKeyId].PutAsync(
        new WithApikey_PutRequestBody
        {
            Name = getApiKeyResponse.Name,
            Description = getApiKeyResponse.Description,
            IamRoles = getApiKeyResponse.IamRoles,
        });
        Console.WriteLine($"  - 更新したAPIキーID: {updateApiKeyResponse.Id}");
        Console.WriteLine($"    - Name: {updateApiKeyResponse.Name}");

        // -------------------------------------------------------
        // DELETE 指定したAPIキーを削除する
        Console.WriteLine($"DELETE /compat/api-keys/{apiKeyId} | 指定したAPIキーを削除する");
        await client.Compat.ApiKeys[apiKeyId].DeleteAsync();
        Console.WriteLine($"  - APIキーを削除しました: {apiKeyId}");

        Console.WriteLine("プロジェクトAPIキーのサンプルの実行が完了しました");
    }
}
