using Microsoft.Kiota.Serialization;
using Sacloud.Sdk.AddOn.Models;

namespace Sacloud.Sdk.AddOn.Samples.Samples;

public class DdosSample(AddOnClient client)
{
    private readonly AddOnClient _client = client;

    public async Task RunAsync()
    {
        // POST DDoS対策サービスリソースの作成
        var body = new DdosRequestBody
        {
            Location = DdosRequestBody_location.Japaneast,
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

        var createResponse = await _client.Security.Ddos.PostAsync(body);
        var resourceGroupName = createResponse.ResourceGroupName;
        var deploymentName = createResponse.DeploymentName;
        Console.WriteLine("DDoS対策サービスリソースの作成情報:");
        Console.WriteLine($"ResourceGroupName: {createResponse.ResourceGroupName}, DeploymentName: {createResponse.DeploymentName}");

        // GET DDoS対策サービスリソース一覧の取得
        var listResponse = await _client.Security.Ddos.GetAsync();
        Console.WriteLine("DDoS対策サービスリソース一覧:");
        foreach (var resource in listResponse.Resources)
        {
            Console.WriteLine($"ResourceGroupName: {resource.Id.ResourceGroupName}, Location: {resource.Id.Location.Name}");
        }

        // GET 指定されたDDoS対策サービスリソースの取得
        var getResponse = await _client.Security.Ddos[resourceGroupName].GetAsync();
        var dataJson = await getResponse.Data.SerializeAsJsonStringAsync();
        Console.WriteLine("指定されたDDoS対策サービスリソース:");
        Console.WriteLine($"Data: {dataJson}, Url: {getResponse.Url}");

        // GET 指定されたDDoS対策サービスリソースのステータス取得
        var statusResponse = await _client.Security.Ddos.Status[resourceGroupName][deploymentName].GetAsync();
        Console.WriteLine("指定されたDDoS対策サービスリソースのステータス:");
        Console.WriteLine($"Id: {statusResponse.Id}, Name: {statusResponse.Name}, ProvisioningState: {statusResponse.Properties?.ProvisioningState}, Timestamp: {statusResponse.Properties?.Timestamp}");

        // DELETE 指定されたDDoS対策サービスリソースの削除
        await _client.Security.Ddos[resourceGroupName].DeleteAsync();
        Console.WriteLine("指定されたDDoS対策サービスリソースの削除が開始されました（レスポンスボディなし）");
    }
}
