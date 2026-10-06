using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class SearchSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST 検索機能リソースの作成
        var body = new SearchPostRequestBody
        {
            Location = SearchPostRequestBody_location.Japaneast,
            PartitionCount = 1,
            ReplicaCount = 1,
            Sku = 1
        };

        var createResponse = await _client.Analytics.Search.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("検索機能リソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET 検索機能リソース一覧の取得
        var listResponse = await _client.Analytics.Search.GetAsync();
        Console.WriteLine("検索機能リソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定された検索機能リソースの取得
        var getResponse = await _client.Analytics.Search[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定された検索機能リソース:");
        Console.WriteLine($"Data: {dataJson}, Url: {getResponse.Url}");

        // GET 指定された検索機能リソースのステータス取得
        var statusResponse = await _client.Analytics.Search.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine("指定された検索機能リソースのステータス:");
        Console.WriteLine($"Id: {statusResponse.Id}, Name: {statusResponse.Name}, ProvisioningState: {statusResponse.Properties?.ProvisioningState}, Timestamp: {statusResponse.Properties?.Timestamp}");

        // DELETE 指定された検索機能リソースの削除
        await _client.Analytics.Search[resourceGroupName].DeleteAsync();
        Console.WriteLine("指定された検索機能リソースの削除が開始されました（レスポンスボディなし）");
    }
}
