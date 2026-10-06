using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class ServiceRouteAuthorizationSample(APIGatewayClient client)
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(Guid serviceId, Guid routeId, Guid groupId)
    {
        // PUT 指定したサービスのルートの認可情報を更新する
        Console.WriteLine("\nPUT /services/{serviceId}/routes/{routeId}/authorization | Route認可情報更新API");
        await client.Services[serviceId].Routes[routeId].Authorization.PutAsync(new RouteAuthorizationDetail
        {
            RouteAuthorizationDetailMember2 = new RouteAuthorizationDetailMember2
            {
                IsACLEnabled = true,
                Groups =
                [
                    new RouteAuthorization
                    {
                        Id = groupId,
                        Enabled = true,
                        Name = "service-sample-access-group"
                    }
                ]
            }
        });

        Console.WriteLine("指定したルートの認可情報の更新が完了しました。(レスポンスボディ無し)");

        // GET 指定したサービスのルートの認可情報を取得する
        Console.WriteLine("\nGET /services/{serviceId}/routes/{routeId}/authorization | Route認可情報取得API");
        var getRouteAuthorizationResponse = await client.Services[serviceId].Routes[routeId].Authorization.GetAsync();
        var routeAuthorization = getRouteAuthorizationResponse.Apigw.RouteAuthorization;

        Console.WriteLine("指定したルートの認可情報の取得が完了しました。");
        Console.WriteLine("  - RouteAuthorization:");
        if (routeAuthorization is null)
        {
            Console.WriteLine("    - (null)");
            return;
        }
        Console.WriteLine($"    - IsACLEnabled: {routeAuthorization.IsACLEnabled.FormatNullable()}");
        Console.WriteLine("    - Groups:");
        if (routeAuthorization.Groups is null || routeAuthorization.Groups.Count <= 0)
        {
            Console.WriteLine("      - (null)");
            return;
        }
        foreach (var group in routeAuthorization.Groups)
        {
            Console.WriteLine($"      - Id: {group.Id.FormatNullable()}");
            Console.WriteLine($"        - Name: {group.Name.FormatNullable()}");
            Console.WriteLine($"        - Enabled: {group.Enabled.FormatNullable()}");
        }
    }
}
