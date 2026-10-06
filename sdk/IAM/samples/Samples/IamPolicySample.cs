namespace Sacloud.Sdk.IAM.Samples.Samples;

class IamPolicySample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(long projectId, long folderId)
    {
        Console.WriteLine("IAMポリシーのサンプルを実行します");

        // -------------------------------------------------------
        // GET プロジェクトIAMポリシーを取得する
        Console.WriteLine($"GET /projects/{projectId}/iam-policy | プロジェクトIAMポリシーを取得する");
        var getProjectPolicyResponse = await client.Projects[projectId].IamPolicy.GetAsync();
        Console.WriteLine($"  - 取得したバインディング数: {getProjectPolicyResponse.Bindings.Count}");

        // -------------------------------------------------------
        // PUT プロジェクトIAMポリシーを更新する
        Console.WriteLine($"PUT /projects/{projectId}/iam-policy | プロジェクトIAMポリシーを更新する");
        var updateProjectPolicyResponse = await client.Projects[projectId].IamPolicy.PutAsync(new Projects.Item.IamPolicy.IamPolicyPutRequestBody
        {
            Bindings = getProjectPolicyResponse.Bindings,
        });
        Console.WriteLine($"  - 更新したバインディング数: {updateProjectPolicyResponse.Bindings.Count}");

        // -------------------------------------------------------
        // GET フォルダIAMポリシーを取得する
        Console.WriteLine($"GET /folders/{folderId}/iam-policy | フォルダIAMポリシーを取得する");
        var getFolderPolicyResponse = await client.Folders[folderId].IamPolicy.GetAsync();
        Console.WriteLine($"  - 取得したバインディング数: {getFolderPolicyResponse.Bindings.Count}");

        // -------------------------------------------------------
        // PUT フォルダIAMポリシーを更新する
        Console.WriteLine($"PUT /folders/{folderId}/iam-policy | フォルダIAMポリシーを更新する");
        var updateFolderPolicyResponse = await client.Folders[folderId].IamPolicy.PutAsync(new Folders.Item.IamPolicy.IamPolicyPutRequestBody
        {
            Bindings = getFolderPolicyResponse.Bindings,
        });
        Console.WriteLine($"  - 更新したバインディング数: {updateFolderPolicyResponse.Bindings.Count}");

        // -------------------------------------------------------
        // GET 組織IAMポリシーを取得する
        Console.WriteLine("GET /organization-iam-policy | 組織IAMポリシーを取得する");
        var getOrgPolicyResponse = await client.OrganizationIamPolicy.GetAsync();
        Console.WriteLine($"  - 取得したバインディング数: {getOrgPolicyResponse.Bindings.Count}");

        // -------------------------------------------------------
        // PUT 組織IAMポリシーを更新する
        Console.WriteLine("PUT /organization-iam-policy | 組織IAMポリシーを更新する");
        var updateOrgPolicyResponse = await client.OrganizationIamPolicy.PutAsync(new OrganizationIamPolicy.OrganizationIamPolicyPutRequestBody
        {
            Bindings = getOrgPolicyResponse.Bindings,
        });
        Console.WriteLine($"  - 更新したバインディング数: {updateOrgPolicyResponse.Bindings.Count}");

        Console.WriteLine("IAMポリシーのサンプルの実行が完了しました");
    }
}
