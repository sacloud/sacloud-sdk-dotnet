namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class PlansSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET 利用可能なプランの一覧取得
    public async Task GetPlansAsync()
    {
        Console.WriteLine("\nGET /plans | 利用可能なプランの一覧取得\n");

        var response = await client.Plans.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var plans = response?.Data
            ?? throw new InvalidOperationException("利用可能なプランの一覧(data)が null です。");
        foreach (var plan in plans)
        {
            Console.WriteLine($"- Type: {plan.Type}");
            Console.WriteLine($"  ServiceClassPath: {plan.ServiceClassPath}");
            Console.WriteLine($"  ClusterID: {plan.ClusterId}");
            Console.WriteLine($"  CapacityGib: {plan.CapacityGib}");
            Console.WriteLine($"  Fee:");
            if (plan.Fee is not null)
            {
                Console.WriteLine($"    Daily: {plan.Fee.Daily}");
                Console.WriteLine($"    ForMonth: {plan.Fee.ForMonth}");
                Console.WriteLine($"    Hourly: {plan.Fee.Hourly}");
                Console.WriteLine($"    Monthly: {plan.Fee.Monthly}");
            }
        }
    }
}
