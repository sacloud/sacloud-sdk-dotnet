namespace Sacloud.Sdk.IAM.Samples.Samples;

class IdPolicySample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("組織IDポリシーのサンプルを実行します");

        // -------------------------------------------------------
        // GET 組織IDポリシーを取得する
        Console.WriteLine("GET /organization-id-policy | 組織IDポリシーを取得する");
        var getIdPolicyResponse = await client.OrganizationIdPolicy.GetAsync();
        Console.WriteLine($"  - 取得したバインディング数: {getIdPolicyResponse.Bindings.Count}");

        // -------------------------------------------------------
        // PUT 組織IDポリシーを更新する
        Console.WriteLine("PUT /organization-id-policy | 組織IDポリシーを更新する");
        var updateIdPolicyResponse = await client.OrganizationIdPolicy.PutAsync(
            new OrganizationIdPolicy.OrganizationIdPolicyPutRequestBody
            {
                Bindings = getIdPolicyResponse.Bindings,
            });
        Console.WriteLine($"  - 更新したバインディング数: {updateIdPolicyResponse.Bindings.Count}");

        Console.WriteLine("組織IDポリシーのサンプルの実行が完了しました");
    }
}
