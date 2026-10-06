using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class DwhSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST データウェアハウスリソースの作成
        var body = new DatawarehousePostRequestBody
        {
            Location = DatawarehousePostRequestBody_location.Japaneast
        };

        var createResponse = await _client.Analytics.Dwh.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("データウェアハウスリソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET データウェアハウスリソース一覧の取得
        var listResponse = await _client.Analytics.Dwh.GetAsync();
        Console.WriteLine("データウェアハウスリソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定されたデータウェアハウスリソースの取得
        var getResponse = await _client.Analytics.Dwh[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定されたデータウェアハウスリソース:");
        Console.WriteLine($"Data: {dataJson}, Url: {getResponse.Url}");

        // GET 指定されたデータウェアハウスリソースのステータス取得
        var statusResponse = await _client.Analytics.Dwh.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine("指定されたデータウェアハウスリソースのステータス:");
        Console.WriteLine($"Id: {statusResponse.Id}, Name: {statusResponse.Name}, ProvisioningState: {statusResponse.Properties?.ProvisioningState}, Timestamp: {statusResponse.Properties?.Timestamp}");

        // DELETE 指定されたデータウェアハウスリソースの削除
        await _client.Analytics.Dwh[resourceGroupName].DeleteAsync();
        Console.WriteLine("指定されたデータウェアハウスリソースの削除が開始されました（レスポンスボディなし）");
    }
}
