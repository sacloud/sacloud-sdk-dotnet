using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class CdnSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST CDNサービスリソースの作成
        var body = new NetworkRequestBody
        {
            Location = NetworkRequestBody_location.Japaneast,
            Profile = new FrontDoorProfile
            {
                Level = 1
            },
            Endpoint = new FrontDoorEndpoint
            {
                Route = new FrontDoorRoute
                {
                    Patterns = ["/*"],
                    OriginGroup = new FrontDoorOriginGroup
                    {
                        Origin = new FrontDoorOrigin
                        {
                            HostHeader = "example.com",
                            HostName = "example.com"
                        }
                    }
                }
            }
        };

        var createResponse = await _client.Cdn.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("CDNサービスリソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET CDNサービスリソース一覧の取得
        var listResponse = await _client.Cdn.GetAsync();
        Console.WriteLine("CDNサービスリソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定されたCDNサービスリソースの取得
        var getResponse = await _client.Cdn[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定されたCDNサービスリソース:");
        Console.WriteLine($"Data: {dataJson}, Url: {getResponse.Url}");

        // GET 指定されたCDNサービスリソースのステータス取得
        var statusResponse = await _client.Cdn.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine("指定されたCDNサービスリソースのステータス:");
        Console.WriteLine(
            $"Id: {statusResponse.Id}, Name: {statusResponse.Name}, ProvisioningState: {statusResponse.Properties?.ProvisioningState}, Timestamp: {statusResponse.Properties?.Timestamp}");

        // DELETE 指定されたCDNサービスリソースの削除
        await _client.Cdn[resourceGroupName].DeleteAsync();
        Console.WriteLine("指定されたCDNサービスリソースの削除が開始されました（レスポンスボディなし）");
    }
}