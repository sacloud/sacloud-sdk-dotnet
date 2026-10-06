using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class DataLakeSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST データレイクリソースの作成
        var body = new DatalakePostRequestBody
        {
            Location = DatalakePostRequestBody_location.Japaneast,
            Performance = 1,
            Redundancy = 1
        };

        var createResponse = await _client.Analytics.Datalake.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("データレイクリソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET データレイクリソース一覧の取得
        var listResponse = await _client.Analytics.Datalake.GetAsync();
        Console.WriteLine("データレイクリソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定されたデータレイクリソースの取得
        var getResponse = await _client.Analytics.Datalake[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定されたデータレイクリソース:");
        Console.WriteLine($"Data: {dataJson}, Url: {getResponse.Url}");

        // GET 指定されたデータレイクリソースのステータス取得
        var statusResponse = await _client.Analytics.Datalake.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine("指定されたデータレイクリソースのステータス:");
        Console.WriteLine($"Id: {statusResponse.Id}, Name: {statusResponse.Name}, ProvisioningState: {statusResponse.Properties?.ProvisioningState}, Timestamp: {statusResponse.Properties?.Timestamp}");

        // DELETE 指定されたデータレイクリソースの削除
        await _client.Analytics.Datalake[resourceGroupName].DeleteAsync();
        Console.WriteLine("指定されたデータレイクリソースの削除が開始されました（レスポンスボディなし）");
    }
}
