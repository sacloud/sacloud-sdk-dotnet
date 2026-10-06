using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.Organization;
using Sacloud.Sdk.IAM.OrganizationServicePolicy;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class OrganizationSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("組織のサンプルを実行します");

        // -------------------------------------------------------
        // GET 組織を取得する
        Console.WriteLine("GET /organization | 組織を取得する");
        var getOrgResponse = await client.Organization.GetAsync();
        Console.WriteLine($"  - 組織ID: {getOrgResponse.Id}");
        Console.WriteLine($"    - Name: {getOrgResponse.Name}");

        // -------------------------------------------------------
        // PUT 組織を更新する
        Console.WriteLine("PUT /organization | 組織を更新する");
        var updateOrgResponse = await client.Organization.PutAsync(new OrganizationPutRequestBody
        {
            Name = getOrgResponse.Name,
        });
        Console.WriteLine($"  - 更新した組織ID: {updateOrgResponse.Id}");
        Console.WriteLine($"    - Name: {updateOrgResponse.Name}");

        // -------------------------------------------------------
        // GET 組織のサービスポリシーを構成するルールを取得する
        Console.WriteLine("GET /organization-service-policy | サービスポリシールールを取得する");
        var getPolicyResponse = await client.OrganizationServicePolicy.GetAsync();
        var rules = getPolicyResponse.Rules
            ?? throw new InvalidOperationException("サービスポリシールールの取得に失敗しました");
        Console.WriteLine($"  - ルール数: {rules.Count}");

        // -------------------------------------------------------
        // PUT 組織のサービスポリシーを更新する

        // 状態確認 → 無効なら有効化
        var statusResponse = await client.ServicePolicyStatus.GetAsync();
        var wasEnabled = statusResponse.Enabled ?? false;
        if (!wasEnabled)
        {
            await client.EnableServicePolicy.PostAsync();
        }

        try
        {
            Console.WriteLine("PUT /organization-service-policy | サービスポリシールールを更新する");
            var updatePolicyResponse = await client.OrganizationServicePolicy.PutAsync(new OrganizationServicePolicyPutRequestBody
            {
                Rules = [.. rules.Select(r => new Rule
                {
                    Code = r.Code,
                    DryRunSpec = r.DryRunSpec,
                    Spec = r.Spec,
                    IsActive = r.IsActive,
                    IsDryRun = r.IsDryRun,
                })],
            });
            Console.WriteLine($"  - ルール数: {updatePolicyResponse.Rules.Count}");
        }
        finally
        {
            // サービスポリシーを元の状態に戻す
            if (!wasEnabled)
            {
                await client.DisableServicePolicy.PostAsync();
                Console.WriteLine("サービスポリシーを無効化しました");
            }
        }

        Console.WriteLine("組織のサンプルの実行が完了しました");
    }
}
