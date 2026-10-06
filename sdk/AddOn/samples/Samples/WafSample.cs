using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class WafSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST WAFサービスリソースの作成
        var body = new WafRequestBody
        {
            Location = WafRequestBody_location.Japaneast,
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

        var createResponse = await _client.Security.Waf.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("WAFサービスリソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET WAFサービスリソース一覧の取得
        var listResponse = await _client.Security.Waf.GetAsync();
        Console.WriteLine("WAFサービスリソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定されたWAFサービスリソースの取得
        var getResponse = await _client.Security.Waf[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定されたWAFサービスリソース:");
        Console.WriteLine($"Data: {dataJson}, Url: {getResponse.Url}");

        // GET 指定されたWAFサービスリソースのステータス取得
        var statusResponse = await _client.Security.Waf.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine("指定されたWAFサービスリソースのステータス:");
        Console.WriteLine($"Id: {statusResponse.Id}, Name: {statusResponse.Name}, ProvisioningState: {statusResponse.Properties?.ProvisioningState}, Timestamp: {statusResponse.Properties?.Timestamp}");

        // DELETE 指定されたWAFサービスリソースの削除
        await _client.Security.Waf[resourceGroupName].DeleteAsync();
        Console.WriteLine("指定されたWAFサービスリソースの削除が開始されました（レスポンスボディなし）");
    }
}
