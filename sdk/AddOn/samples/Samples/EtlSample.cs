using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class EtlSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST データETLリソースの作成
        var body = new EtlPostRequestBody
        {
            Location = EtlPostRequestBody_location.Japaneast
        };

        var createResponse = await _client.Analytics.Etl.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("データETLリソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET データETLリソース一覧の取得
        var listResponse = await _client.Analytics.Etl.GetAsync();
        Console.WriteLine("データETLリソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定されたデータETLリソースの取得
        var getResponse = await _client.Analytics.Etl[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定されたデータETLリソース:");
        Console.WriteLine($"Data: {dataJson}, Url: {getResponse.Url}");

        // GET 指定されたデータETLリソースのステータス取得
        var statusResponse = await _client.Analytics.Etl.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine("指定されたデータETLリソースのステータス:");
        Console.WriteLine($"Id: {statusResponse.Id}, Name: {statusResponse.Name}, ProvisioningState: {statusResponse.Properties?.ProvisioningState}, Timestamp: {statusResponse.Properties?.Timestamp}");

        // DELETE 指定されたデータETLリソースの削除
        await _client.Analytics.Etl[resourceGroupName].DeleteAsync();
        Console.WriteLine("指定されたデータETLリソースの削除が開始されました（レスポンスボディなし）");
    }
}
