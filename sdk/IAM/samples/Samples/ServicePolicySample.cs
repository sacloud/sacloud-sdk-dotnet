namespace Sacloud.Sdk.IAM.Samples.Samples;

class ServicePolicySample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("サービスポリシーのサンプルを実行します");

        // -------------------------------------------------------
        // GET サービスポリシーの有効状態を取得する
        Console.WriteLine("GET /service-policy-status | サービスポリシーの有効状態を取得する");
        var statusResponse = await client.ServicePolicyStatus.GetAsync();
        Console.WriteLine($"  - Enabled: {statusResponse.Enabled}");

        var wasEnabled = statusResponse.Enabled ?? false;

        // -------------------------------------------------------
        // GET ルールテンプレートの一覧を取得する
        Console.WriteLine("GET /service-policy-rule-templates | ルールテンプレートの一覧を取得する");
        var templatesResponse = await client.ServicePolicyRuleTemplates.GetAsync(x =>
        {
            x.QueryParameters.Page = 1;
        });
        Console.WriteLine($"  - テンプレート数: {templatesResponse.Count}");
        if (templatesResponse.Items is not null && templatesResponse.Items.Count > 0)
        {
            foreach (var template in templatesResponse.Items)
            {
                Console.WriteLine($"    - {template.Code}");
                Console.WriteLine($"      - Name: {template.Name}");
            }
        }
        Console.WriteLine($"  - Count: {templatesResponse.Count}");
        Console.WriteLine($"  - Next: {templatesResponse.Next}");
        Console.WriteLine($"  - Previous: {templatesResponse.Previous}");

        // -------------------------------------------------------
        // POST サービスポリシーを有効化する
        Console.WriteLine("POST /enable-service-policy | サービスポリシーを有効化する");
        await client.EnableServicePolicy.PostAsync();
        Console.WriteLine("  - サービスポリシーを有効化しました");

        // -------------------------------------------------------
        // POST サービスポリシーを無効化する
        Console.WriteLine("POST /disable-service-policy | サービスポリシーを無効化する");
        await client.DisableServicePolicy.PostAsync();
        Console.WriteLine("  - サービスポリシーを無効化しました");

        // -------------------------------------------------------
        // CLEANUP サービスポリシーを元の状態に戻す
        if (wasEnabled)
        {
            Console.WriteLine("CLEANUP サービスポリシーを元の状態に戻す");
            await client.EnableServicePolicy.PostAsync();
            Console.WriteLine("  - サービスポリシーを有効化しました");
        }

        Console.WriteLine("サービスポリシーのサンプルの実行が完了しました");
    }
}