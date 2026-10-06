namespace Sacloud.Sdk.AppRunDedicated.Samples;

static class ServiceClassSample
{
    public static async Task RunSampleAsync(AppRunDedicatedClient client)
    {
        // ------------------------------
        // [サービスクラス]
        // ------------------------------

        // GET ロードバランササービスクラス一覧取得
        var listLoadBalancerServiceClassesResponse = await client.Service_classes.Lb.GetAsync();
        var loadBalancerServiceClasses = listLoadBalancerServiceClassesResponse.LbServiceClasses
            ?? throw new InvalidOperationException("LbServiceClassesがNullです");
        Console.WriteLine($"ロードバランササービスクラスを取得しました: {loadBalancerServiceClasses.Count}件");
        foreach (var serviceClass in loadBalancerServiceClasses)
        {
            Console.WriteLine($"- {serviceClass.Name} (ノード数: {serviceClass.NodeCount}, パス: {serviceClass.Path})");
        }

        // GET ワーカノードサービスクラス一覧取得
        var listWorkerNodeServiceClassesResponse = await client.Service_classes.Worker.GetAsync();
        var workerNodeServiceClasses = listWorkerNodeServiceClassesResponse.WorkerServiceClasses
            ?? throw new InvalidOperationException("WorkerServiceClassesがNullです");
        Console.WriteLine($"ワーカノードサービスクラスを取得しました: {workerNodeServiceClasses.Count}件");
        foreach (var serviceClass in workerNodeServiceClasses)
        {
            Console.WriteLine($"- {serviceClass.Name} (パス: {serviceClass.Path})");
        }
    }
}