namespace Sacloud.Sdk.IAM.Samples.Samples;

class IamRoleSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("IAMロールのサンプルを実行します");

        // -------------------------------------------------------
        // GET IAMロール一覧を取得する
        Console.WriteLine("GET /iam-roles | IAMロール一覧を取得する");
        var listIamRolesResponse = await client.IamRoles.GetAsync();
        Console.WriteLine($"  - 取得したIAMロール数: {listIamRolesResponse.Items.Count}");

        // -------------------------------------------------------
        // GET 指定したIAMロールを取得する
        var getIamRoleResponse = await client.IamRoles[listIamRolesResponse.Items[0].Id].GetAsync();
        Console.WriteLine($"GET /iam-roles/{getIamRoleResponse.Id} | 指定したIAMロールを取得する");
        Console.WriteLine($"  - 取得したIAMロールID: {getIamRoleResponse.Id}");
        Console.WriteLine($"    - Name: {getIamRoleResponse.Name}");
        Console.WriteLine($"    - LowestGrantableResource: {getIamRoleResponse.LowestGrantableResource}");

        Console.WriteLine("IAMロールのサンプルの実行が完了しました");
    }
}
