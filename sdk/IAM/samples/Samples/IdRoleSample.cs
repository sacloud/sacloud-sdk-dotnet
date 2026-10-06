namespace Sacloud.Sdk.IAM.Samples.Samples;

class IdRoleSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("IDロールのサンプルを実行します");

        // -------------------------------------------------------
        // GET IDロール一覧を取得する
        Console.WriteLine("GET /id-roles | IDロール一覧を取得する");
        var listIdRolesResponse = await client.IdRoles.GetAsync();
        Console.WriteLine($"  - 取得したIDロール数: {listIdRolesResponse.Items.Count}");

        // -------------------------------------------------------
        // GET 指定したIDロールを取得する
        var getIdRoleResponse = await client.IdRoles[listIdRolesResponse.Items[0].Id].GetAsync();
        Console.WriteLine($"GET /id-roles/{getIdRoleResponse.Id} | 指定したIDロールを取得する");
        Console.WriteLine($"  - 取得したIDロールID: {getIdRoleResponse.Id}");
        Console.WriteLine($"    - Name: {getIdRoleResponse.Name}");

        Console.WriteLine("IDロールのサンプルの実行が完了しました");
    }
}
