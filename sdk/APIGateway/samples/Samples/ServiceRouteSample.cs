using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class ServiceRouteSample(APIGatewayClient client, Guid serviceId, string domainName)
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private readonly Guid serviceId = serviceId;
    private readonly string domainName = domainName;
    private Guid RouteId { get; set; } = Guid.Empty;

    public async Task<Guid> RunAsync()
    {
        // POST 指定したサービスにルートを登録する
        Console.WriteLine("\nPOST /services/{serviceId}/routes | Route登録API");
        var createRouteResponse = await client.Services[serviceId].Routes.PostAsync(new RouteDetail
        {
            Name = "sample-route",
            Tags = ["Sample", "CreatedByAPI", "ServiceRouteSample"],
            Protocols = Route_protocols.HttpHttps,
            Methods =
            [
                HTTPMethod.GET,
            ],
            Hosts = [domainName],
            Path = "/sample/route",
            HttpsRedirectStatusCode = 308,
            RegexPriority = 10,
            StripPath = false,
            PreserveHost = true,
            RequestBuffering = false,
            ResponseBuffering = false,
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Protocols = IpRestrictionConfig_protocols.Https,
                RestrictedBy = IpRestrictionConfig_restrictedBy.AllowIps,
                Ips = ["0.0.0.0"]
            }
        });
        var createdRoute = createRouteResponse.Apigw.Route;
        RouteId = createdRoute.Id
            ?? throw new NullReferenceException("Route登録APIのレスポンスにルートIDが含まれていません。");

        Console.WriteLine("指定したサービスへのルートの登録が完了しました。");
        WriteRouteDetail(createdRoute, 2);

        // GET 指定したサービスのルート一覧を取得する
        Console.WriteLine("\nGET /services/{serviceId}/routes | Service Route一覧照会API");
        var listRoutesResponse = await client.Services[serviceId].Routes.GetAsync();

        Console.WriteLine("指定したサービスのルート一覧の取得が完了しました。");
        Console.WriteLine("  - Routes:");
        if (listRoutesResponse.Apigw.Routes is null || listRoutesResponse.Apigw.Routes.Count <= 0)
        {
            Console.WriteLine("    - (null)");
        }
        else
        {
            foreach (var route in listRoutesResponse.Apigw.Routes)
            {
                WriteRoute(route, 4);
            }
        }

        // GET 指定したサービスのルート詳細情報を取得する
        Console.WriteLine("\nGET /services/{serviceId}/routes/{routeId} | Route詳細照会API");
        var getRouteResponse = await client.Services[serviceId].Routes[RouteId].GetAsync();

        Console.WriteLine("指定したルートの詳細情報の取得が完了しました。");
        WriteRouteDetail(getRouteResponse.Apigw.Route, 2);

        // PUT 指定したサービスのルートを更新する
        Console.WriteLine("\nPUT /services/{serviceId}/routes/{routeId} | Route更新API");
        await client.Services[serviceId].Routes[RouteId].PutAsync(new RouteDetail
        {
            Name = $"updated-sample-route",
            Tags = ["Sample", "UpdatedByAPI", "ServiceRouteSample"],
            Protocols = Route_protocols.Https,
            Methods = [HTTPMethod.HEAD],
            Hosts = [domainName],
            Path = "/sample/route/updated",
            HttpsRedirectStatusCode = 426,
            RegexPriority = 1,
            StripPath = true,
            PreserveHost = false,
            RequestBuffering = true,
            ResponseBuffering = true,
            IpRestrictionConfig = new IpRestrictionConfig
            {
                Protocols = IpRestrictionConfig_protocols.Https,
                RestrictedBy = IpRestrictionConfig_restrictedBy.DenyIps,
                Ips = ["0.0.0.0"]
            }
        });

        Console.WriteLine("指定したルートの更新が完了しました。(レスポンスボディ無し)");

        return RouteId;
    }

    public async Task DeleteAsync()
    {
        if (RouteId != Guid.Empty)
        {
            Console.WriteLine("\nDELETE /services/{serviceId}/routes/{routeId} | Route削除API");
            await client.Services[serviceId].Routes[RouteId].DeleteAsync();
            Console.WriteLine("指定したルートの削除が完了しました。(レスポンスボディ無し)");
        }
    }

    private void WriteRoute(Route route, int indent = 2)
    {
        Console.WriteIndentedLine($"- Id: {route.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- ServiceId: {route.ServiceId.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Name: {route.Name.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Path: {route.Path.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Protocols: {route.Protocols.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Host: {route.Host.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- PreserveHost: {route.PreserveHost.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- StripPath: {route.StripPath.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- RequestBuffering: {route.RequestBuffering.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- ResponseBuffering: {route.ResponseBuffering.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- HttpsRedirectStatusCode: {route.HttpsRedirectStatusCode.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- RegexPriority: {route.RegexPriority.FormatNullable()}", indent + 2);

        Console.WriteIndentedLine("- Tags:", indent + 2);
        if (route.Tags is null || route.Tags.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            foreach (var tag in route.Tags)
            {
                Console.WriteIndentedLine($"- {tag.FormatNullable()}", indent + 4);
            }
        }

        Console.WriteIndentedLine("- Hosts:", indent + 2);
        if (route.Hosts is null || route.Hosts.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            foreach (var host in route.Hosts)
            {
                Console.WriteIndentedLine($"- {host.FormatNullable()}", indent + 4);
            }
        }

        Console.WriteIndentedLine("- Methods:", indent + 2);
        if (route.Methods is null || route.Methods.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            foreach (var method in route.Methods)
            {
                Console.WriteIndentedLine($"- {method.FormatNullable()}", indent + 4);
            }
        }
    }

    private void WriteRouteDetail(RouteDetail route, int indent = 2)
    {
        WriteRoute(route, indent);
        Console.WriteIndentedLine("- IpRestrictionConfig:", indent + 2);
        if (route.IpRestrictionConfig is null)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
            return;
        }

        Console.WriteIndentedLine($"- Protocols: {route.IpRestrictionConfig.Protocols.FormatNullable()}", indent + 4);
        Console.WriteIndentedLine($"- RestrictedBy: {route.IpRestrictionConfig.RestrictedBy.FormatNullable()}", indent + 4);
        Console.WriteIndentedLine("- Ips:", indent + 4);
        if (route.IpRestrictionConfig.Ips is null || route.IpRestrictionConfig.Ips.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 6);
            return;
        }

        foreach (var ip in route.IpRestrictionConfig.Ips)
        {
            Console.WriteIndentedLine($"- {ip.FormatNullable()}", indent + 6);
        }
    }
}
