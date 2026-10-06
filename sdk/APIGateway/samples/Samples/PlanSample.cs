using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class PlanSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public string Description { get; } = "プランカテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\nプランのサンプルを実行します");
        try
        {
            // サンプルの実行
            Console.WriteLine("\n---------[プランサンプル]--------");

            // GET 料金プラン一覧を取得する
            Console.WriteLine("\nGET /plans | プラン一覧照会API");
            var listPlansResponse = await client.Plans.GetAsync();

            Console.WriteLine("料金プラン一覧の取得が完了しました。");
            Console.WriteLine("  - Plans:");
            if (listPlansResponse.Apigw.Plans is null || listPlansResponse.Apigw.Plans.Count <= 0)
            {
                Console.WriteLine("    - (null)");
            }
            else
            {
                foreach (var plan in listPlansResponse.Apigw.Plans)
                {
                    Console.WriteLine($"    - Id: {plan.Id.FormatNullable()}");
                    Console.WriteLine($"      - Name: {plan.Name.FormatNullable()}");
                    Console.WriteLine($"      - Description: {plan.Description.FormatNullable()}");
                    Console.WriteLine($"      - Price: {plan.Price.FormatNullable()}");
                    Console.WriteLine($"      - MaxServices: {plan.MaxServices.FormatNullable()}");
                    Console.WriteLine($"      - MaxRequests: {plan.MaxRequests.FormatNullable()}");
                    Console.WriteLine($"      - MaxRequestsUnit: {plan.MaxRequestsUnit.FormatNullable()}");
                    Console.WriteLine($"      - CreatedAt: {plan.CreatedAt.FormatNullable()}");
                    Console.WriteLine($"      - UpdatedAt: {plan.UpdatedAt.FormatNullable()}");
                    Console.WriteLine("      - Overage:");
                    if (plan.Overage is null)
                    {
                        Console.WriteLine("        - (null)");
                    }
                    else
                    {
                        Console.WriteLine($"        - UnitPrice: {plan.Overage.UnitPrice.FormatNullable()}");
                        Console.WriteLine($"        - UnitRequests: {plan.Overage.UnitRequests.FormatNullable()}");
                    }
                }
            }
        }
        finally
        {
            Console.WriteLine("\nプランのサンプルの実行が完了しました");
        }
    }
};
