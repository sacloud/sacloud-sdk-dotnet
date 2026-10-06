namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class ClusterSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET サイト一覧の取得
    public async Task GetClusterAsync()
    {
        Console.WriteLine("\nGET /clusters | サイト一覧の取得\n");

        var response = await client.Clusters.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var clusters = response?.Data
            ?? throw new InvalidOperationException("サイト一覧(data)が null です。");
        foreach (var cluster in clusters)
        {
            Console.WriteLine($"- ID: {cluster.Id}");
            Console.WriteLine($"  ApiZone: {string.Join(", ", cluster.ApiZone ?? [])}");
            Console.WriteLine($"  ControlPanelUrl: {cluster.ControlPanelUrl}");
            Console.WriteLine($"  DisplayNameEnUs: {cluster.DisplayNameEnUs}");
            Console.WriteLine($"  DisplayNameJa: {cluster.DisplayNameJa}");
            Console.WriteLine($"  DisplayName: {cluster.DisplayName}");
            Console.WriteLine($"  DisplayOrder: {cluster.DisplayOrder}");
            Console.WriteLine($"  EndpointBase: {cluster.EndpointBase}");
            Console.WriteLine($"  Region: {cluster.Region}");
            Console.WriteLine($"  S3Endpoint: {cluster.S3Endpoint}");
            Console.WriteLine($"  S3EndpointForControlPanel: {cluster.S3EndpointForControlPanel}");
            Console.WriteLine($"  StorageZone: {string.Join(", ", cluster.StorageZone)}");
            Console.WriteLine($"  PlanFamily: {cluster.PlanFamily}");
        }
    }

    // GET サイトの取得
    public async Task GetClusterItemAsync()
    {
        Console.WriteLine("\nGET /clusters/{clusterId} | サイトの取得\n");

        var clusterId = ReadRequiredValue("ClusterId");
        if (clusterId is null) return;

        var response = await client.Clusters[clusterId].GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var cluster = response?.Data
            ?? throw new InvalidOperationException("サイト情報(data)が null です。");
        Console.WriteLine($"ID: {cluster.Id}");
        Console.WriteLine($"ApiZone: {string.Join(", ", cluster.ApiZone ?? [])}");
        Console.WriteLine($"ControlPanelUrl: {cluster.ControlPanelUrl}");
        Console.WriteLine($"DisplayNameEnUs: {cluster.DisplayNameEnUs}");
        Console.WriteLine($"DisplayNameJa: {cluster.DisplayNameJa}");
        Console.WriteLine($"DisplayName: {cluster.DisplayName}");
        Console.WriteLine($"DisplayOrder: {cluster.DisplayOrder}");
        Console.WriteLine($"EndpointBase: {cluster.EndpointBase}");
        Console.WriteLine($"Region: {cluster.Region}");
        Console.WriteLine($"S3Endpoint: {cluster.S3Endpoint}");
        Console.WriteLine($"S3EndpointForControlPanel: {cluster.S3EndpointForControlPanel}");
        Console.WriteLine($"StorageZone: {string.Join(", ", cluster.StorageZone)}");
        Console.WriteLine($"PlanFamily: {cluster.PlanFamily}");
    }
}
