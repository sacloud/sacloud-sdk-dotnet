using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class AiSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST AI機能リソースの作成
        var body = new AiRequestBody
        {
            Location = AiRequestBody_location.Japaneast,
            Sku = 1
        };
        var createResponse = await _client.Ai.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("AI機能リソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET AI機能リソース一覧の取得
        var listResponse = await _client.Ai.GetAsync();
        Console.WriteLine("AI機能リソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定されたAI機能リソースの取得
        var getResponse = await _client.Ai[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定されたAI機能リソース:");
        Console.WriteLine($"Data: {dataJson}, URL: {getResponse.Url}");

        // GET 指定されたAI機能リソースのステータス取得
        var getStatusResponse = await _client.Ai.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine($"指定されたAI機能リソースのステータス:");
        Console.WriteLine($"DeploymentID: {getStatusResponse.Id}, DeploymentName: {getStatusResponse.Name}");

        // DELETE 指定されたAI機能リソースの削除
        await _client.Ai[resourceGroupName].DeleteAsync();
        Console.WriteLine($"指定されたAI機能リソースの削除が開始されました（レスポンスボディなし）");
    }
}
